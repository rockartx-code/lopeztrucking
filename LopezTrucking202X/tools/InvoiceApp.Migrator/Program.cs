using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var options = MigratorOptions.Parse(args);
if (options.ShowHelp)
{
    Console.WriteLine(MigratorOptions.HelpText);
    return;
}

var migrator = new LegacyMigrator(
    options,
    new LegacyCsvReader(),
    new LegacyStringNormalizer(options.CompatibilityMode));

await migrator.RunAsync();

internal sealed class MigratorOptions
{
    public string DataDirectory { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = "Data Source=invoiceapp.db";
    public bool CompatibilityMode { get; init; }
    public string ReportsDirectory { get; init; } = string.Empty;
    public bool ShowHelp { get; init; }

    public static string HelpText =>
        """
        InvoiceApp.Migrator
        Usage:
          InvoiceApp.Migrator --data-dir <path> [--connection <connectionString>] [--reports-dir <path>] [--compatibility]

        Options:
          --data-dir        Directory containing legacy CSV exports.
          --connection      SQLite connection string (default: Data Source=invoiceapp.db).
          --reports-dir     Directory to write conflict reports (default: <data-dir>/reports).
          --compatibility   Preserve legacy string values without normalization.
          --help            Show this help text.
        """;

    public static MigratorOptions Parse(string[] args)
    {
        var options = new MigratorOptions();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg.Equals("--help", StringComparison.OrdinalIgnoreCase) || arg.Equals("-h", StringComparison.OrdinalIgnoreCase))
            {
                return options with { ShowHelp = true };
            }

            if (arg.Equals("--data-dir", StringComparison.OrdinalIgnoreCase))
            {
                options = options with { DataDirectory = GetValue(args, ref i, arg) };
                continue;
            }

            if (arg.Equals("--connection", StringComparison.OrdinalIgnoreCase))
            {
                options = options with { ConnectionString = GetValue(args, ref i, arg) };
                continue;
            }

            if (arg.Equals("--reports-dir", StringComparison.OrdinalIgnoreCase))
            {
                options = options with { ReportsDirectory = GetValue(args, ref i, arg) };
                continue;
            }

            if (arg.Equals("--compatibility", StringComparison.OrdinalIgnoreCase))
            {
                options = options with { CompatibilityMode = true };
            }
        }

        return options;
    }

    private static string GetValue(string[] args, ref int index, string arg)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException($"Missing value for {arg}");
        }

        index++;
        return args[index];
    }
}

internal sealed class LegacyMigrator
{
    private const string InvoicesFile = "Invoices.csv";
    private const string DetailsFile = "Details.csv";
    private const string PlacesFile = "Places.csv";
    private const string CompaniesFile = "Companies.csv";
    private const string PreciosFile = "Precios.csv";

    private readonly MigratorOptions _options;
    private readonly LegacyCsvReader _csvReader;
    private readonly LegacyStringNormalizer _normalizer;

    public LegacyMigrator(
        MigratorOptions options,
        LegacyCsvReader csvReader,
        LegacyStringNormalizer normalizer)
    {
        _options = options;
        _csvReader = csvReader;
        _normalizer = normalizer;
    }

    public async Task RunAsync()
    {
        if (string.IsNullOrWhiteSpace(_options.DataDirectory))
        {
            Console.WriteLine("Missing required --data-dir value.");
            Console.WriteLine(MigratorOptions.HelpText);
            return;
        }

        var reportsDirectory = string.IsNullOrWhiteSpace(_options.ReportsDirectory)
            ? Path.Combine(_options.DataDirectory, "reports")
            : _options.ReportsDirectory;

        Directory.CreateDirectory(reportsDirectory);

        var optionsBuilder = new DbContextOptionsBuilder<InvoiceAppDbContext>()
            .UseSqlite(_options.ConnectionString);

        await using var dbContext = new InvoiceAppDbContext(optionsBuilder.Options);
        await dbContext.Database.MigrateAsync();

        var companies = await LoadCompaniesAsync(dbContext);
        var places = await LoadPlacesAsync(dbContext);
        var invoices = await LoadInvoicesAsync(dbContext, companies);
        await LoadDetailsAsync(dbContext, invoices, companies, places);
        await LoadPreciosAsync(dbContext, companies, places, reportsDirectory);

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Migration completed.");
    }

    private async Task<Dictionary<string, InvoiceApp.Domain.Entities.Company>> LoadCompaniesAsync(
        InvoiceAppDbContext dbContext)
    {
        var companiesPath = Path.Combine(_options.DataDirectory, CompaniesFile);
        if (!File.Exists(companiesPath))
        {
            Console.WriteLine($"Companies CSV not found at {companiesPath}. Skipping.");
            return new Dictionary<string, InvoiceApp.Domain.Entities.Company>(StringComparer.OrdinalIgnoreCase);
        }

        var rows = _csvReader.Read(companiesPath);
        var companies = new Dictionary<string, InvoiceApp.Domain.Entities.Company>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            var name = _normalizer.Normalize(row.GetValue("Name", "Company", "Costumer"));
            if (string.IsNullOrWhiteSpace(name) || companies.ContainsKey(name))
            {
                continue;
            }

            var place = new InvoiceApp.Domain.Entities.Place
            {
                Name = name,
                AddressLine1 = _normalizer.Normalize(row.GetValue("Address", "AddressLine1", "Address 1")),
                AddressLine2 = _normalizer.Normalize(row.GetValue("AddressLine2", "Address 2")),
                City = _normalizer.Normalize(row.GetValue("City")),
                State = _normalizer.Normalize(row.GetValue("State")),
                PostalCode = _normalizer.Normalize(row.GetValue("PostalCode", "Zip", "ZipCode")),
                IsCompany = true
            };

            var company = new InvoiceApp.Domain.Entities.Company
            {
                Name = name,
                Address = place
            };

            await dbContext.Places.AddAsync(place);
            await dbContext.Companies.AddAsync(company);
            companies[name] = company;
        }

        return companies;
    }

    private async Task<Dictionary<string, InvoiceApp.Domain.Entities.Place>> LoadPlacesAsync(
        InvoiceAppDbContext dbContext)
    {
        var placesPath = Path.Combine(_options.DataDirectory, PlacesFile);
        if (!File.Exists(placesPath))
        {
            Console.WriteLine($"Places CSV not found at {placesPath}. Skipping.");
            return new Dictionary<string, InvoiceApp.Domain.Entities.Place>(StringComparer.OrdinalIgnoreCase);
        }

        var rows = _csvReader.Read(placesPath);
        var places = new Dictionary<string, InvoiceApp.Domain.Entities.Place>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            var name = _normalizer.Normalize(row.GetValue("Name", "Place"));
            if (string.IsNullOrWhiteSpace(name) || places.ContainsKey(name))
            {
                continue;
            }

            var place = new InvoiceApp.Domain.Entities.Place
            {
                Name = name,
                AddressLine1 = _normalizer.Normalize(row.GetValue("Address", "AddressLine1", "Address 1")),
                AddressLine2 = _normalizer.Normalize(row.GetValue("AddressLine2", "Address 2")),
                City = _normalizer.Normalize(row.GetValue("City")),
                State = _normalizer.Normalize(row.GetValue("State")),
                PostalCode = _normalizer.Normalize(row.GetValue("PostalCode", "Zip", "ZipCode")),
                IsFrom = row.GetBool("IsFrom", "From"),
                IsTo = row.GetBool("IsTo", "To")
            };

            await dbContext.Places.AddAsync(place);
            places[name] = place;
        }

        return places;
    }

    private async Task<Dictionary<string, InvoiceApp.Domain.Entities.Invoice>> LoadInvoicesAsync(
        InvoiceAppDbContext dbContext,
        Dictionary<string, InvoiceApp.Domain.Entities.Company> companies)
    {
        var invoicesPath = Path.Combine(_options.DataDirectory, InvoicesFile);
        if (!File.Exists(invoicesPath))
        {
            Console.WriteLine($"Invoices CSV not found at {invoicesPath}. Skipping.");
            return new Dictionary<string, InvoiceApp.Domain.Entities.Invoice>(StringComparer.OrdinalIgnoreCase);
        }

        var rows = _csvReader.Read(invoicesPath);
        var invoices = new Dictionary<string, InvoiceApp.Domain.Entities.Invoice>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            var invoiceNumber = _normalizer.Normalize(row.GetValue("No", "Invoice", "InvoiceNumber", "Invoice No"));
            if (string.IsNullOrWhiteSpace(invoiceNumber) || invoices.ContainsKey(invoiceNumber))
            {
                continue;
            }

            var companyName = _normalizer.Normalize(row.GetValue("Costumer", "Customer", "Company", "Name"));
            if (!companies.TryGetValue(companyName, out var company))
            {
                company = await CreateCompanyAsync(dbContext, companies, companyName);
            }

            var invoice = new InvoiceApp.Domain.Entities.Invoice
            {
                InvoiceNumber = invoiceNumber,
                InvoiceDate = ParseDate(row.GetValue("D", "Date", "InvoiceDate")),
                CompanyId = company.Id
            };

            await dbContext.Invoices.AddAsync(invoice);
            invoices[invoiceNumber] = invoice;
        }

        return invoices;
    }

    private async Task LoadDetailsAsync(
        InvoiceAppDbContext dbContext,
        Dictionary<string, InvoiceApp.Domain.Entities.Invoice> invoices,
        Dictionary<string, InvoiceApp.Domain.Entities.Company> companies,
        Dictionary<string, InvoiceApp.Domain.Entities.Place> places)
    {
        var detailsPath = Path.Combine(_options.DataDirectory, DetailsFile);
        if (!File.Exists(detailsPath))
        {
            Console.WriteLine($"Details CSV not found at {detailsPath}. Skipping.");
            return;
        }

        var rows = _csvReader.Read(detailsPath);
        foreach (var row in rows)
        {
            var invoiceNumber = _normalizer.Normalize(row.GetValue("Invoice", "InvoiceNumber", "No"));
            if (!invoices.TryGetValue(invoiceNumber, out var invoice))
            {
                var companyName = _normalizer.Normalize(row.GetValue("Company"));
                if (!companies.TryGetValue(companyName, out var company))
                {
                    company = await CreateCompanyAsync(dbContext, companies, companyName);
                }

                invoice = new InvoiceApp.Domain.Entities.Invoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = ParseDate(row.GetValue("Date")),
                    CompanyId = company.Id
                };

                await dbContext.Invoices.AddAsync(invoice);
                invoices[invoiceNumber] = invoice;
            }

            var description = _normalizer.Normalize(row.GetValue("Dispatch", "Mix", "Description"));
            var amount = row.GetDecimal("Amount", "Subtotal");
            var empties = row.GetInt("Emtys", "Empties");
            var emptyUnitPrice = row.GetDecimal("EmptyUnitPrice", "EmptyPrice", "EmptyRate");
            var amountBase = amount;

            if (empties > 0 && emptyUnitPrice > 0 && amount >= empties * emptyUnitPrice)
            {
                amountBase = amount - (empties * emptyUnitPrice);
            }

            var detailGroup = new InvoiceApp.Domain.Entities.DetailGroup
            {
                Description = string.IsNullOrWhiteSpace(description) ? "Legacy Detail" : description,
                AmountBase = amountBase,
                EmptiesCount = empties
            };

            detailGroup.SnapshotEmptyUnitPrice(emptyUnitPrice);

            await dbContext.DetailGroups.AddAsync(detailGroup);
            dbContext.Entry(detailGroup).Property("InvoiceId").CurrentValue = invoice.Id;
            await dbContext.DetailGroupCompanies.AddAsync(new InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupCompany
            {
                DetailGroupId = detailGroup.Id,
                CompanyId = invoice.CompanyId
            });

            var fromPlace = await GetOrCreatePlaceAsync(dbContext, places, row.GetValue("From"), isFrom: true);
            var toPlace = await GetOrCreatePlaceAsync(dbContext, places, row.GetValue("To"), isTo: true);

            if (fromPlace != null)
            {
                await dbContext.DetailGroupFromPlaces.AddAsync(new InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupFromPlace
                {
                    DetailGroupId = detailGroup.Id,
                    PlaceId = fromPlace.Id
                });
            }

            if (toPlace != null)
            {
                await dbContext.DetailGroupToPlaces.AddAsync(new InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupToPlace
                {
                    DetailGroupId = detailGroup.Id,
                    PlaceId = toPlace.Id
                });
            }

            invoice.DetailGroups.Add(detailGroup);
        }
    }

    private async Task LoadPreciosAsync(
        InvoiceAppDbContext dbContext,
        Dictionary<string, InvoiceApp.Domain.Entities.Company> companies,
        Dictionary<string, InvoiceApp.Domain.Entities.Place> places,
        string reportsDirectory)
    {
        var preciosPath = Path.Combine(_options.DataDirectory, PreciosFile);
        if (!File.Exists(preciosPath))
        {
            Console.WriteLine($"Precios CSV not found at {preciosPath}. Skipping.");
            return;
        }

        var rows = _csvReader.Read(preciosPath);
        var agreements = new Dictionary<string, InvoiceApp.Domain.Entities.PriceAgreement>(StringComparer.OrdinalIgnoreCase);
        var mixAmounts = new Dictionary<string, HashSet<decimal>>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            var companyName = _normalizer.Normalize(row.GetValue("Company", "Costumer", "Customer"));
            if (!companies.TryGetValue(companyName, out var company))
            {
                company = await CreateCompanyAsync(dbContext, companies, companyName);
            }

            var mixName = _normalizer.Normalize(row.GetValue("Mix", "MixName", "Dispatch"));
            if (string.IsNullOrWhiteSpace(mixName))
            {
                var from = _normalizer.Normalize(row.GetValue("From"));
                var to = _normalizer.Normalize(row.GetValue("To"));
                mixName = $"{companyName} | {from} -> {to}";
            }

            var amount = row.GetDecimal("Amount", "BaseRate", "Price");
            var emptyUnitPrice = row.GetDecimal("EmptyUnitPrice", "EmptyPrice", "EmptyRate");

            if (!mixAmounts.TryGetValue(mixName, out var amounts))
            {
                amounts = new HashSet<decimal>();
                mixAmounts[mixName] = amounts;
            }

            amounts.Add(amount);

            var key = $"{company.Id}:{mixName}";
            if (!agreements.TryGetValue(key, out var agreement))
            {
                agreement = new InvoiceApp.Domain.Entities.PriceAgreement
                {
                    CompanyId = company.Id,
                    MixName = mixName,
                    EffectiveDate = ParseDate(row.GetValue("EffectiveDate", "Date", "D"))
                };

                await dbContext.PriceAgreements.AddAsync(agreement);
                agreements[key] = agreement;
            }

            agreement.Items.Add(new InvoiceApp.Domain.Entities.PriceAgreementItem
            {
                ItemType = InvoiceApp.Domain.Entities.ItemType.Company,
                BaseRate = amount,
                EmptyUnitPrice = emptyUnitPrice
            });

            await GetOrCreatePlaceAsync(dbContext, places, row.GetValue("From"), isFrom: true);
            await GetOrCreatePlaceAsync(dbContext, places, row.GetValue("To"), isTo: true);
        }

        await WriteMixConflictReportAsync(mixAmounts, reportsDirectory);
    }

    private async Task<InvoiceApp.Domain.Entities.Company> CreateCompanyAsync(
        InvoiceAppDbContext dbContext,
        Dictionary<string, InvoiceApp.Domain.Entities.Company> companies,
        string companyName)
    {
        var normalizedName = string.IsNullOrWhiteSpace(companyName)
            ? "Legacy Company"
            : companyName;

        if (companies.TryGetValue(normalizedName, out var existing))
        {
            return existing;
        }

        var place = new InvoiceApp.Domain.Entities.Place
        {
            Name = normalizedName,
            AddressLine1 = string.Empty,
            City = string.Empty,
            State = string.Empty,
            PostalCode = string.Empty,
            IsCompany = true
        };

        var company = new InvoiceApp.Domain.Entities.Company
        {
            Name = normalizedName,
            Address = place
        };

        await dbContext.Places.AddAsync(place);
        await dbContext.Companies.AddAsync(company);
        companies[normalizedName] = company;
        return company;
    }

    private async Task<InvoiceApp.Domain.Entities.Place?> GetOrCreatePlaceAsync(
        InvoiceAppDbContext dbContext,
        Dictionary<string, InvoiceApp.Domain.Entities.Place> places,
        string? rawName,
        bool isFrom = false,
        bool isTo = false)
    {
        var name = _normalizer.Normalize(rawName);
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        if (places.TryGetValue(name, out var place))
        {
            if (isFrom)
            {
                place.IsFrom = true;
            }

            if (isTo)
            {
                place.IsTo = true;
            }

            return place;
        }

        place = new InvoiceApp.Domain.Entities.Place
        {
            Name = name,
            AddressLine1 = string.Empty,
            City = string.Empty,
            State = string.Empty,
            PostalCode = string.Empty,
            IsFrom = isFrom,
            IsTo = isTo
        };

        await dbContext.Places.AddAsync(place);
        places[name] = place;
        return place;
    }

    private static DateOnly ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DateOnly.FromDateTime(DateTime.UtcNow);
        }

        if (DateOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
        {
            return dateOnly;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            return DateOnly.FromDateTime(dateTime);
        }

        if (DateTime.TryParse(value, out dateTime))
        {
            return DateOnly.FromDateTime(dateTime);
        }

        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private static async Task WriteMixConflictReportAsync(
        IReadOnlyDictionary<string, HashSet<decimal>> mixAmounts,
        string reportsDirectory)
    {
        var conflicts = mixAmounts
            .Where(entry => entry.Value.Count > 1)
            .Select(entry => new
            {
                MixName = entry.Key,
                Amounts = string.Join(" | ", entry.Value.OrderBy(value => value).Select(value => value.ToString(CultureInfo.InvariantCulture)))
            })
            .OrderBy(entry => entry.MixName)
            .ToList();

        if (conflicts.Count == 0)
        {
            return;
        }

        var reportPath = Path.Combine(reportsDirectory, "mix-conflicts.csv");
        await using var writer = new StreamWriter(reportPath);
        await writer.WriteLineAsync("MixName,Amounts");
        foreach (var conflict in conflicts)
        {
            await writer.WriteLineAsync($"\"{conflict.MixName}\",\"{conflict.Amounts}\"");
        }

        Console.WriteLine($"Mix conflict report written to {reportPath}");
    }
}

internal sealed class LegacyCsvReader
{
    public List<LegacyCsvRow> Read(string path)
    {
        if (!File.Exists(path))
        {
            return new List<LegacyCsvRow>();
        }

        var delimiter = DetectDelimiter(path);
        using var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(path);
        parser.SetDelimiters(delimiter);
        parser.HasFieldsEnclosedInQuotes = true;

        if (parser.EndOfData)
        {
            return new List<LegacyCsvRow>();
        }

        var headers = parser.ReadFields();
        if (headers is null || headers.Length == 0)
        {
            return new List<LegacyCsvRow>();
        }

        var rows = new List<LegacyCsvRow>();
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields is null)
            {
                continue;
            }

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < headers.Length; i++)
            {
                if (i >= fields.Length)
                {
                    break;
                }

                map[headers[i]] = fields[i];
            }

            rows.Add(new LegacyCsvRow(map));
        }

        return rows;
    }

    private static string DetectDelimiter(string path)
    {
        var firstLine = File.ReadLines(path).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(firstLine))
        {
            return ",";
        }

        var commaCount = firstLine.Count(character => character == ',');
        var semicolonCount = firstLine.Count(character => character == ';');
        return semicolonCount > commaCount ? ";" : ",";
    }
}

internal sealed class LegacyCsvRow
{
    private readonly Dictionary<string, string> _values;

    public LegacyCsvRow(Dictionary<string, string> values)
    {
        _values = values;
    }

    public string GetValue(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (_values.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        return string.Empty;
    }

    public int GetInt(params string[] keys)
    {
        var value = GetValue(keys);
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (int.TryParse(value, out parsed))
        {
            return parsed;
        }

        return 0;
    }

    public decimal GetDecimal(params string[] keys)
    {
        var value = GetValue(keys);
        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out parsed))
        {
            return parsed;
        }

        return 0m;
    }

    public bool GetBool(params string[] keys)
    {
        var value = GetValue(keys);
        if (bool.TryParse(value, out var parsed))
        {
            return parsed;
        }

        return value.Equals("yes", StringComparison.OrdinalIgnoreCase)
            || value.Equals("y", StringComparison.OrdinalIgnoreCase)
            || value.Equals("1", StringComparison.OrdinalIgnoreCase)
            || value.Equals("true", StringComparison.OrdinalIgnoreCase)
            || value.Equals("from", StringComparison.OrdinalIgnoreCase)
            || value.Equals("to", StringComparison.OrdinalIgnoreCase);
    }
}

internal sealed class LegacyStringNormalizer
{
    private readonly bool _compatibilityMode;

    public LegacyStringNormalizer(bool compatibilityMode)
    {
        _compatibilityMode = compatibilityMode;
    }

    public string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        if (_compatibilityMode)
        {
            return value;
        }

        var normalized = value.Replace("\r\n", ", ").Replace('\n', ',').Replace('\r', ' ');
        normalized = string.Join(' ', normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return normalized.Trim();
    }
}
