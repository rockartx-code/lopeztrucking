using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LopezTruck202X.Data;
using Microsoft.Data.Sqlite;

namespace LopezTruck202X.Services;

public sealed class AccessImportService
{
    private readonly SqliteDatabase _database;

    public AccessImportService(SqliteDatabase database)
    {
        _database = database;
    }

    public async Task ImportAsync(string accessPath)
    {
        using var accessConnection = new OleDbConnection(BuildConnectionString(accessPath));
        accessConnection.Open();

        await using var sqliteConnection = _database.CreateConnection();
        await sqliteConnection.OpenAsync();

        await using var transaction = await sqliteConnection.BeginTransactionAsync();

        await ClearSqliteAsync(sqliteConnection);

        var origins = ReadStrings(accessConnection, "SELECT [Place] FROM Origen");
        await InsertNamesAsync(sqliteConnection, "Origins", origins);

        var destinations = ReadStrings(accessConnection, "SELECT [Place] FROM Destiny");
        await InsertNamesAsync(sqliteConnection, "Destinations", destinations);

        var companies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        companies.UnionWith(ReadStrings(accessConnection, "SELECT [Company] FROM Origen"));
        companies.UnionWith(ReadStrings(accessConnection, "SELECT [Company] FROM Precios"));
        companies.UnionWith(ReadStrings(accessConnection, "SELECT [Company] FROM Detail"));
        await InsertNamesAsync(sqliteConnection, "Companies", companies);

        await ImportCustomersAsync(accessConnection, sqliteConnection);

        var originLookup = await LoadNameLookupAsync(sqliteConnection, "Origins");
        var destinationLookup = await LoadNameLookupAsync(sqliteConnection, "Destinations");
        var companyLookup = await LoadNameLookupAsync(sqliteConnection, "Companies");
        var customerLookup = await LoadNameLookupAsync(sqliteConnection, "Customers");

        await ImportPricesAsync(accessConnection, sqliteConnection, companyLookup, originLookup, destinationLookup);

        var invoiceLookup = await ImportInvoicesAsync(accessConnection, sqliteConnection, customerLookup);
        await ImportInvoiceLinesAsync(accessConnection, sqliteConnection, invoiceLookup);

        await transaction.CommitAsync();
    }

    private static string BuildConnectionString(string accessPath)
    {
        var builder = new OleDbConnectionStringBuilder
        {
            Provider = "Microsoft.ACE.OLEDB.12.0",
            DataSource = accessPath
        };
        builder.Add("Persist Security Info", "False");
        return builder.ToString();
    }

    private static IReadOnlyCollection<string> ReadStrings(OleDbConnection connection, string query)
    {
        using var command = connection.CreateCommand();
        command.CommandText = query;
        using var reader = command.ExecuteReader();
        var values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (reader?.Read() == true)
        {
            var value = reader.IsDBNull(0) ? null : reader.GetValue(0)?.ToString();
            if (!string.IsNullOrWhiteSpace(value))
            {
                values.Add(value.Trim());
            }
        }

        return values;
    }

    private static async Task ClearSqliteAsync(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM InvoiceLines;
            DELETE FROM Invoices;
            DELETE FROM Customers;
            DELETE FROM Prices;
            DELETE FROM Origins;
            DELETE FROM Destinations;
            DELETE FROM Companies;
            """;
        await command.ExecuteNonQueryAsync();
    }

    private static async Task InsertNamesAsync(SqliteConnection connection, string table, IEnumerable<string> names)
    {
        foreach (var name in names.Where(name => !string.IsNullOrWhiteSpace(name)))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"INSERT OR IGNORE INTO {table} (Name) VALUES ($name)";
            command.Parameters.AddWithValue("$name", name.Trim());
            await command.ExecuteNonQueryAsync();
        }
    }

    private static async Task ImportCustomersAsync(OleDbConnection access, SqliteConnection sqlite)
    {
        using var command = access.CreateCommand();
        command.CommandText = "SELECT [Name], [Address], [City], [State], [Phone] FROM Costumers";
        using var reader = command.ExecuteReader();

        while (reader?.Read() == true)
        {
            var name = ReadString(reader, 0);
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            var insert = sqlite.CreateCommand();
            insert.CommandText = """
                INSERT OR IGNORE INTO Customers (Name, Address, City, State, Phone)
                VALUES ($name, $address, $city, $state, $phone);
                """;
            insert.Parameters.AddWithValue("$name", name.Trim());
            insert.Parameters.AddWithValue("$address", ReadString(reader, 1) ?? string.Empty);
            insert.Parameters.AddWithValue("$city", ReadString(reader, 2) ?? string.Empty);
            insert.Parameters.AddWithValue("$state", ReadString(reader, 3) ?? string.Empty);
            insert.Parameters.AddWithValue("$phone", ReadString(reader, 4) ?? string.Empty);
            await insert.ExecuteNonQueryAsync();
        }
    }

    private static async Task<Dictionary<string, int>> LoadNameLookupAsync(SqliteConnection connection, string table)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT Id, Name FROM {table}";
        await using var reader = await command.ExecuteReaderAsync();
        var lookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            lookup[name] = id;
        }

        return lookup;
    }

    private static async Task ImportPricesAsync(
        OleDbConnection access,
        SqliteConnection sqlite,
        IDictionary<string, int> companies,
        IDictionary<string, int> origins,
        IDictionary<string, int> destinations)
    {
        using var command = access.CreateCommand();
        command.CommandText = "SELECT [Company], [From], [To], [Amount] FROM Precios";
        using var reader = command.ExecuteReader();

        while (reader?.Read() == true)
        {
            var company = NormalizeName(ReadString(reader, 0));
            var origin = NormalizeName(ReadString(reader, 1));
            var destination = NormalizeName(ReadString(reader, 2));

            var companyId = await EnsureNameAsync(sqlite, companies, "Companies", company);
            var originId = await EnsureNameAsync(sqlite, origins, "Origins", origin);
            var destinationId = await EnsureNameAsync(sqlite, destinations, "Destinations", destination);

            var amount = ReadDouble(reader, 3);
            var insert = sqlite.CreateCommand();
            insert.CommandText = """
                INSERT OR REPLACE INTO Prices (CompanyId, OriginId, DestinationId, Amount)
                VALUES ($companyId, $originId, $destinationId, $amount);
                """;
            insert.Parameters.AddWithValue("$companyId", companyId);
            insert.Parameters.AddWithValue("$originId", originId);
            insert.Parameters.AddWithValue("$destinationId", destinationId);
            insert.Parameters.AddWithValue("$amount", amount);
            await insert.ExecuteNonQueryAsync();
        }
    }

    private static async Task<Dictionary<int, int>> ImportInvoicesAsync(
        OleDbConnection access,
        SqliteConnection sqlite,
        Dictionary<string, int> customers)
    {
        using var command = access.CreateCommand();
        command.CommandText = "SELECT [No], [D], [Check], [Costumer], [Subtotal], [Advance] FROM Invoices";
        using var reader = command.ExecuteReader();
        var invoiceLookup = new Dictionary<int, int>();

        while (reader?.Read() == true)
        {
            var number = ReadInt(reader, 0);
            if (number is null)
            {
                continue;
            }

            var customerName = ReadString(reader, 3);
            if (string.IsNullOrWhiteSpace(customerName))
            {
                continue;
            }

            var customerId = await EnsureCustomerAsync(sqlite, customers, customerName);
            var date = ReadDate(reader, 1);
            var subtotal = ReadDouble(reader, 4);
            var advance = ReadDouble(reader, 5);
            var total = subtotal - advance;

            var insert = sqlite.CreateCommand();
            insert.CommandText = """
                INSERT INTO Invoices (Number, Date, CheckNumber, CustomerId, Subtotal, Advance, Total)
                VALUES ($number, $date, $checkNumber, $customerId, $subtotal, $advance, $total);
                SELECT last_insert_rowid();
                """;
            insert.Parameters.AddWithValue("$number", number.Value);
            insert.Parameters.AddWithValue("$date", date.ToString("O", CultureInfo.InvariantCulture));
            insert.Parameters.AddWithValue("$checkNumber", ReadString(reader, 2) ?? string.Empty);
            insert.Parameters.AddWithValue("$customerId", customerId);
            insert.Parameters.AddWithValue("$subtotal", subtotal);
            insert.Parameters.AddWithValue("$advance", advance);
            insert.Parameters.AddWithValue("$total", total);

            var newId = await insert.ExecuteScalarAsync();
            if (newId is not null)
            {
                invoiceLookup[number.Value] = Convert.ToInt32(newId, CultureInfo.InvariantCulture);
            }
        }

        return invoiceLookup;
    }

    private static async Task ImportInvoiceLinesAsync(
        OleDbConnection access,
        SqliteConnection sqlite,
        IReadOnlyDictionary<int, int> invoiceLookup)
    {
        using var command = access.CreateCommand();
        command.CommandText = "SELECT [Invoice], [Date], [Company], [From], [To], [Dispatch], [emtys], [FB], [Amount] FROM Detail";
        using var reader = command.ExecuteReader();

        while (reader?.Read() == true)
        {
            var invoiceNumber = ReadInt(reader, 0);
            if (invoiceNumber is null || !invoiceLookup.TryGetValue(invoiceNumber.Value, out var invoiceId))
            {
                continue;
            }

            var insert = sqlite.CreateCommand();
            insert.CommandText = """
                INSERT INTO InvoiceLines (
                    InvoiceId, Date, Company, Origin, Destination, Dispatch, Empties, Fb, Amount
                ) VALUES (
                    $invoiceId, $date, $company, $origin, $destination, $dispatch, $empties, $fb, $amount
                );
                """;
            insert.Parameters.AddWithValue("$invoiceId", invoiceId);
            insert.Parameters.AddWithValue("$date", ReadDate(reader, 1).ToString("O", CultureInfo.InvariantCulture));
            insert.Parameters.AddWithValue("$company", ReadString(reader, 2) ?? string.Empty);
            insert.Parameters.AddWithValue("$origin", ReadString(reader, 3) ?? string.Empty);
            insert.Parameters.AddWithValue("$destination", ReadString(reader, 4) ?? string.Empty);
            insert.Parameters.AddWithValue("$dispatch", ReadString(reader, 5) ?? string.Empty);
            insert.Parameters.AddWithValue("$empties", ReadString(reader, 6) ?? string.Empty);
            insert.Parameters.AddWithValue("$fb", ReadString(reader, 7) ?? string.Empty);
            insert.Parameters.AddWithValue("$amount", ReadDouble(reader, 8));
            await insert.ExecuteNonQueryAsync();
        }
    }

    private static async Task<int> EnsureCustomerAsync(
        SqliteConnection connection,
        IDictionary<string, int> lookup,
        string customerName)
    {
        var normalizedName = customerName.Trim();
        if (lookup.TryGetValue(normalizedName, out var existingId))
        {
            return existingId;
        }

        var insert = connection.CreateCommand();
        insert.CommandText = """
            INSERT INTO Customers (Name, Address, City, State, Phone)
            VALUES ($name, '', '', '', '');
            SELECT last_insert_rowid();
            """;
        insert.Parameters.AddWithValue("$name", normalizedName);
        var newId = await insert.ExecuteScalarAsync();
        var id = Convert.ToInt32(newId, CultureInfo.InvariantCulture);
        lookup[normalizedName] = id;
        return id;
    }

    private static async Task<int> EnsureNameAsync(
        SqliteConnection connection,
        IDictionary<string, int> lookup,
        string table,
        string name)
    {
        if (lookup.TryGetValue(name, out var existingId))
        {
            return existingId;
        }

        var insert = connection.CreateCommand();
        insert.CommandText = $"""
            INSERT OR IGNORE INTO {table} (Name)
            VALUES ($name);
            SELECT Id FROM {table} WHERE Name = $name;
            """;
        insert.Parameters.AddWithValue("$name", name);
        var newId = await insert.ExecuteScalarAsync();
        var id = Convert.ToInt32(newId, CultureInfo.InvariantCulture);
        lookup[name] = id;
        return id;
    }

    private static string NormalizeName(string? name)
    {
        return string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
    }

    private static string? ReadString(OleDbDataReader reader, int index)
    {
        return reader.IsDBNull(index) ? null : reader.GetValue(index)?.ToString();
    }

    private static double ReadDouble(OleDbDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
        {
            return 0d;
        }

        return Convert.ToDouble(reader.GetValue(index), CultureInfo.InvariantCulture);
    }

    private static int? ReadInt(OleDbDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
        {
            return null;
        }

        return Convert.ToInt32(reader.GetValue(index), CultureInfo.InvariantCulture);
    }

    private static DateTimeOffset ReadDate(OleDbDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
        {
            return DateTimeOffset.Now;
        }

        return new DateTimeOffset(Convert.ToDateTime(reader.GetValue(index), CultureInfo.InvariantCulture));
    }
}
