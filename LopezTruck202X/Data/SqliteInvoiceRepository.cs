using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using LopezTruck202X.Models;
using Microsoft.Data.Sqlite;

namespace LopezTruck202X.Data;

public sealed class SqliteInvoiceRepository
{
    private readonly SqliteDatabase _database;

    public SqliteInvoiceRepository(SqliteDatabase database)
    {
        _database = database;
    }

    public async Task InitializeAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Customers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                Address TEXT,
                City TEXT,
                State TEXT,
                Phone TEXT,
                IsActive INTEGER NOT NULL DEFAULT 1
            );

            CREATE TABLE IF NOT EXISTS Invoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number INTEGER NOT NULL,
                Date TEXT NOT NULL,
                CheckNumber TEXT,
                CustomerId INTEGER NOT NULL,
                Subtotal REAL NOT NULL,
                Advance REAL NOT NULL,
                Total REAL NOT NULL,
                FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
            );

            CREATE TABLE IF NOT EXISTS InvoiceLines (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                InvoiceId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Company TEXT,
                Origin TEXT,
                Destination TEXT,
                Dispatch TEXT,
                Empties TEXT,
                Fb TEXT,
                Amount REAL NOT NULL,
                FOREIGN KEY(InvoiceId) REFERENCES Invoices(Id)
            );

            CREATE TABLE IF NOT EXISTS Origins (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Destinations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Companies (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Prices (
                CompanyId INTEGER NOT NULL,
                OriginId INTEGER NOT NULL,
                DestinationId INTEGER NOT NULL,
                Amount REAL NOT NULL,
                PRIMARY KEY (CompanyId, OriginId, DestinationId),
                FOREIGN KEY(CompanyId) REFERENCES Companies(Id),
                FOREIGN KEY(OriginId) REFERENCES Origins(Id),
                FOREIGN KEY(DestinationId) REFERENCES Destinations(Id)
            );
            """;
        await command.ExecuteNonQueryAsync();

        await EnsureCustomerIsActiveColumnAsync(connection);
        await EnsureInvoiceNumberIsNotUniqueAsync(connection);
    }

    public async Task<int> GetNextInvoiceNumberAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COALESCE(MAX(Number), 0) + 1 FROM Invoices";
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<IReadOnlyList<InvoiceSummary>> GetInvoicesAsync(
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        string? customerName)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        var filters = new List<string>();

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            filters.Add("Customers.Name LIKE $customerName");
            command.Parameters.AddWithValue("$customerName", $"%{customerName.Trim()}%");
        }

        if (startDate.HasValue)
        {
            var start = startDate.Value.Date;
            filters.Add("Invoices.Date >= $startDate");
            command.Parameters.AddWithValue("$startDate", start.ToString("O", CultureInfo.InvariantCulture));
        }

        if (endDate.HasValue)
        {
            var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
            filters.Add("Invoices.Date <= $endDate");
            command.Parameters.AddWithValue("$endDate", end.ToString("O", CultureInfo.InvariantCulture));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : string.Empty;
        command.CommandText = $"""
            SELECT Invoices.Id,
                   Invoices.Number,
                   Invoices.Date,
                   Customers.Name,
                   Invoices.Total
            FROM Invoices
            JOIN Customers ON Customers.Id = Invoices.CustomerId
            {whereClause}
            ORDER BY Invoices.Date DESC, Invoices.Number DESC;
            """;

        await using var reader = await command.ExecuteReaderAsync();
        var results = new List<InvoiceSummary>();

        while (await reader.ReadAsync())
        {
            results.Add(new InvoiceSummary
            {
                Id = reader.GetInt32(0),
                Number = reader.GetInt32(1),
                Date = DateTimeOffset.Parse(reader.GetString(2), CultureInfo.InvariantCulture),
                CustomerName = reader.GetString(3),
                Total = Convert.ToDecimal(reader.GetDouble(4))
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<Customer>> GetCustomersAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Name, Address, City, State, Phone
            FROM Customers
            WHERE IsActive = 1
            ORDER BY Name;
            """;
        await using var reader = await command.ExecuteReaderAsync();

        var results = new List<Customer>();
        while (await reader.ReadAsync())
        {
            results.Add(new Customer
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                City = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                State = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Phone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            });
        }

        return results;
    }

    public async Task<Invoice?> GetInvoiceAsync(int invoiceId)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var headerCommand = connection.CreateCommand();
        headerCommand.CommandText = """
            SELECT Invoices.Id,
                   Invoices.Number,
                   Invoices.Date,
                   Invoices.CheckNumber,
                   Invoices.Subtotal,
                   Invoices.Advance,
                   Invoices.Total,
                   Customers.Id,
                   Customers.Name,
                   Customers.Address,
                   Customers.City,
                   Customers.State,
                   Customers.Phone
            FROM Invoices
            JOIN Customers ON Customers.Id = Invoices.CustomerId
            WHERE Invoices.Id = $invoiceId;
            """;
        headerCommand.Parameters.AddWithValue("$invoiceId", invoiceId);

        await using var reader = await headerCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var invoice = new Invoice
        {
            Id = reader.GetInt32(0),
            Number = reader.GetInt32(1),
            Date = DateTimeOffset.Parse(reader.GetString(2), CultureInfo.InvariantCulture),
            CheckNumber = reader.GetString(3),
            Subtotal = Convert.ToDecimal(reader.GetDouble(4)),
            Advance = Convert.ToDecimal(reader.GetDouble(5)),
            Total = Convert.ToDecimal(reader.GetDouble(6)),
            Customer = new Customer
            {
                Id = reader.GetInt32(7),
                Name = reader.GetString(8),
                Address = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                City = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                State = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                Phone = reader.IsDBNull(12) ? string.Empty : reader.GetString(12)
            }
        };

        var lineCommand = connection.CreateCommand();
        lineCommand.CommandText = """
            SELECT Date, Company, Origin, Destination, Dispatch, Empties, Fb, Amount
            FROM InvoiceLines
            WHERE InvoiceId = $invoiceId
            ORDER BY Date;
            """;
        lineCommand.Parameters.AddWithValue("$invoiceId", invoiceId);

        await using var lineReader = await lineCommand.ExecuteReaderAsync();
        while (await lineReader.ReadAsync())
        {
            invoice.Lines.Add(new InvoiceLine
            {
                Date = DateTimeOffset.Parse(lineReader.GetString(0), CultureInfo.InvariantCulture),
                Company = lineReader.IsDBNull(1) ? string.Empty : lineReader.GetString(1),
                From = lineReader.IsDBNull(2) ? string.Empty : lineReader.GetString(2),
                To = lineReader.IsDBNull(3) ? string.Empty : lineReader.GetString(3),
                Dispatch = lineReader.IsDBNull(4) ? string.Empty : lineReader.GetString(4),
                Empties = lineReader.IsDBNull(5) ? string.Empty : lineReader.GetString(5),
                Fb = lineReader.IsDBNull(6) ? string.Empty : lineReader.GetString(6),
                Amount = Convert.ToDecimal(lineReader.GetDouble(7))
            });
        }

        return invoice;
    }

    public async Task SaveInvoiceAsync(Invoice invoice)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        if (await IsCustomerInactiveAsync(connection, invoice.Customer.Name))
        {
            throw new InvalidOperationException("Customer is inactive.");
        }

        await using var transaction = await connection.BeginTransactionAsync();

        var customerId = await UpsertCustomerAsync(connection, invoice.Customer);

        var invoiceId = await UpsertInvoiceAsync(connection, invoice, customerId);

        var deleteLines = connection.CreateCommand();
        deleteLines.CommandText = "DELETE FROM InvoiceLines WHERE InvoiceId = $invoiceId";
        deleteLines.Parameters.AddWithValue("$invoiceId", invoiceId);
        await deleteLines.ExecuteNonQueryAsync();

        foreach (var line in invoice.Lines)
        {
            var lineCommand = connection.CreateCommand();
            lineCommand.CommandText = """
                INSERT INTO InvoiceLines (
                    InvoiceId, Date, Company, Origin, Destination, Dispatch, Empties, Fb, Amount
                ) VALUES (
                    $invoiceId, $date, $company, $origin, $destination, $dispatch, $empties, $fb, $amount
                );
                """;
            lineCommand.Parameters.AddWithValue("$invoiceId", invoiceId);
            lineCommand.Parameters.AddWithValue("$date", line.Date.ToString("O"));
            lineCommand.Parameters.AddWithValue("$company", line.Company);
            lineCommand.Parameters.AddWithValue("$origin", line.From);
            lineCommand.Parameters.AddWithValue("$destination", line.To);
            lineCommand.Parameters.AddWithValue("$dispatch", line.Dispatch);
            lineCommand.Parameters.AddWithValue("$empties", line.Empties);
            lineCommand.Parameters.AddWithValue("$fb", line.Fb);
            lineCommand.Parameters.AddWithValue("$amount", line.Amount);
            await lineCommand.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }

    private static async Task<int> UpsertCustomerAsync(SqliteConnection connection, Customer customer)
    {
        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = "SELECT Id FROM Customers WHERE Name = $name AND IsActive = 1";
        selectCommand.Parameters.AddWithValue("$name", customer.Name);
        var existingId = await selectCommand.ExecuteScalarAsync();

        if (existingId is not null)
        {
            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = """
                UPDATE Customers
                SET Address = $address,
                    City = $city,
                    State = $state,
                    Phone = $phone
                WHERE Id = $id;
                """;
            updateCommand.Parameters.AddWithValue("$address", customer.Address);
            updateCommand.Parameters.AddWithValue("$city", customer.City);
            updateCommand.Parameters.AddWithValue("$state", customer.State);
            updateCommand.Parameters.AddWithValue("$phone", customer.Phone);
            updateCommand.Parameters.AddWithValue("$id", (long)existingId);
            await updateCommand.ExecuteNonQueryAsync();
            return Convert.ToInt32(existingId);
        }

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Customers (Name, Address, City, State, Phone)
            VALUES ($name, $address, $city, $state, $phone);
            SELECT last_insert_rowid();
            """;
        insertCommand.Parameters.AddWithValue("$name", customer.Name);
        insertCommand.Parameters.AddWithValue("$address", customer.Address);
        insertCommand.Parameters.AddWithValue("$city", customer.City);
        insertCommand.Parameters.AddWithValue("$state", customer.State);
        insertCommand.Parameters.AddWithValue("$phone", customer.Phone);
        var newId = await insertCommand.ExecuteScalarAsync();
        return Convert.ToInt32(newId);
    }

    private static async Task<int> UpsertInvoiceAsync(SqliteConnection connection, Invoice invoice, int customerId)
    {
        if (invoice.Id > 0)
        {
            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = """
                UPDATE Invoices
                SET Number = $number,
                    Date = $date,
                    CheckNumber = $check,
                    CustomerId = $customerId,
                    Subtotal = $subtotal,
                    Advance = $advance,
                    Total = $total
                WHERE Id = $id;
                """;
            updateCommand.Parameters.AddWithValue("$number", invoice.Number);
            updateCommand.Parameters.AddWithValue("$date", invoice.Date.ToString("O"));
            updateCommand.Parameters.AddWithValue("$check", invoice.CheckNumber);
            updateCommand.Parameters.AddWithValue("$customerId", customerId);
            updateCommand.Parameters.AddWithValue("$subtotal", invoice.Subtotal);
            updateCommand.Parameters.AddWithValue("$advance", invoice.Advance);
            updateCommand.Parameters.AddWithValue("$total", invoice.Total);
            updateCommand.Parameters.AddWithValue("$id", invoice.Id);
            await updateCommand.ExecuteNonQueryAsync();
            return invoice.Id;
        }

        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = "SELECT Id FROM Invoices WHERE Number = $number";
        selectCommand.Parameters.AddWithValue("$number", invoice.Number);
        var existingId = await selectCommand.ExecuteScalarAsync();

        if (existingId is not null)
        {
            var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = """
                UPDATE Invoices
                SET Date = $date,
                    CheckNumber = $check,
                    CustomerId = $customerId,
                    Subtotal = $subtotal,
                    Advance = $advance,
                    Total = $total
                WHERE Id = $id;
                """;
            updateCommand.Parameters.AddWithValue("$date", invoice.Date.ToString("O"));
            updateCommand.Parameters.AddWithValue("$check", invoice.CheckNumber);
            updateCommand.Parameters.AddWithValue("$customerId", customerId);
            updateCommand.Parameters.AddWithValue("$subtotal", invoice.Subtotal);
            updateCommand.Parameters.AddWithValue("$advance", invoice.Advance);
            updateCommand.Parameters.AddWithValue("$total", invoice.Total);
            updateCommand.Parameters.AddWithValue("$id", (long)existingId);
            await updateCommand.ExecuteNonQueryAsync();
            return Convert.ToInt32(existingId);
        }

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Invoices (Number, Date, CheckNumber, CustomerId, Subtotal, Advance, Total)
            VALUES ($number, $date, $check, $customerId, $subtotal, $advance, $total);
            SELECT last_insert_rowid();
            """;
        insertCommand.Parameters.AddWithValue("$number", invoice.Number);
        insertCommand.Parameters.AddWithValue("$date", invoice.Date.ToString("O"));
        insertCommand.Parameters.AddWithValue("$check", invoice.CheckNumber);
        insertCommand.Parameters.AddWithValue("$customerId", customerId);
        insertCommand.Parameters.AddWithValue("$subtotal", invoice.Subtotal);
        insertCommand.Parameters.AddWithValue("$advance", invoice.Advance);
        insertCommand.Parameters.AddWithValue("$total", invoice.Total);
        var newId = await insertCommand.ExecuteScalarAsync();
        return Convert.ToInt32(newId);
    }

    private static async Task EnsureInvoiceNumberIsNotUniqueAsync(SqliteConnection connection)
    {
        var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT sql FROM sqlite_master WHERE type = 'table' AND name = 'Invoices';";
        var tableSql = await checkCommand.ExecuteScalarAsync() as string;

        if (string.IsNullOrWhiteSpace(tableSql) || !tableSql.Contains("Number INTEGER NOT NULL UNIQUE", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var pragmaCommand = connection.CreateCommand();
        pragmaCommand.CommandText = "PRAGMA foreign_keys = OFF;";
        await pragmaCommand.ExecuteNonQueryAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        var rebuildCommand = connection.CreateCommand();
        rebuildCommand.CommandText = """
            CREATE TABLE Invoices_new (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number INTEGER NOT NULL,
                Date TEXT NOT NULL,
                CheckNumber TEXT,
                CustomerId INTEGER NOT NULL,
                Subtotal REAL NOT NULL,
                Advance REAL NOT NULL,
                Total REAL NOT NULL,
                FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
            );

            INSERT INTO Invoices_new (Id, Number, Date, CheckNumber, CustomerId, Subtotal, Advance, Total)
            SELECT Id, Number, Date, CheckNumber, CustomerId, Subtotal, Advance, Total
            FROM Invoices;

            DROP TABLE Invoices;
            ALTER TABLE Invoices_new RENAME TO Invoices;
            """;
        await rebuildCommand.ExecuteNonQueryAsync();

        await transaction.CommitAsync();

        var enableForeignKeysCommand = connection.CreateCommand();
        enableForeignKeysCommand.CommandText = "PRAGMA foreign_keys = ON;";
        await enableForeignKeysCommand.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyList<Origin>> GetOriginsAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Origins ORDER BY Name";
        await using var reader = await command.ExecuteReaderAsync();

        var results = new List<Origin>();
        while (await reader.ReadAsync())
        {
            results.Add(new Origin
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<Destination>> GetDestinationsAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Destinations ORDER BY Name";
        await using var reader = await command.ExecuteReaderAsync();

        var results = new List<Destination>();
        while (await reader.ReadAsync())
        {
            results.Add(new Destination
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return results;
    }

    public async Task<int> AddOriginAsync(string name)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Origins (Name)
            VALUES ($name);
            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", name);
        var newId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(newId);
    }

    public async Task<IReadOnlyList<Company>> GetCompaniesAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Companies ORDER BY Name";
        await using var reader = await command.ExecuteReaderAsync();

        var results = new List<Company>();
        while (await reader.ReadAsync())
        {
            results.Add(new Company
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return results;
    }

    public async Task<int> AddCompanyAsync(string name)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Companies (Name)
            VALUES ($name);
            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", name);
        var newId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(newId);
    }

    public async Task<int> AddDestinationAsync(string name)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Destinations (Name)
            VALUES ($name);
            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", name);
        var newId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(newId);
    }

    public async Task UpdateOriginAsync(Origin origin)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Origins
            SET Name = $name
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$name", origin.Name);
        command.Parameters.AddWithValue("$id", origin.Id);
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateCompanyAsync(Company company)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Companies
            SET Name = $name
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$name", company.Name);
        command.Parameters.AddWithValue("$id", company.Id);
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateDestinationAsync(Destination destination)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Destinations
            SET Name = $name
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$name", destination.Name);
        command.Parameters.AddWithValue("$id", destination.Id);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteCompanyAsync(int companyId)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM Companies
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", companyId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Customers
            SET IsActive = 0
            WHERE Id = $id
              AND IsActive = 1;
            """;
        command.Parameters.AddWithValue("$id", customerId);
        var deleted = await command.ExecuteNonQueryAsync();
        return deleted > 0;
    }

    public async Task<bool> IsCustomerInactiveAsync(string name)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();
        return await IsCustomerInactiveAsync(connection, name);
    }

    private static async Task<bool> IsCustomerInactiveAsync(SqliteConnection connection, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var command = connection.CreateCommand();
        command.CommandText = "SELECT IsActive FROM Customers WHERE Name = $name;";
        command.Parameters.AddWithValue("$name", name.Trim());
        var result = await command.ExecuteScalarAsync();
        return result is not null && Convert.ToInt32(result) == 0;
    }

    private static async Task EnsureCustomerIsActiveColumnAsync(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info(Customers);";
        await using var reader = await command.ExecuteReaderAsync();
        var hasIsActive = false;
        while (await reader.ReadAsync())
        {
            if (string.Equals(reader.GetString(1), "IsActive", StringComparison.OrdinalIgnoreCase))
            {
                hasIsActive = true;
                break;
            }
        }

        if (hasIsActive)
        {
            return;
        }

        var alterCommand = connection.CreateCommand();
        alterCommand.CommandText = "ALTER TABLE Customers ADD COLUMN IsActive INTEGER NOT NULL DEFAULT 1;";
        await alterCommand.ExecuteNonQueryAsync();
    }

    public async Task<double?> GetPriceAmountAsync(int companyId, int originId, int destinationId)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Amount
            FROM Prices
            WHERE CompanyId = $companyId
              AND OriginId = $originId
              AND DestinationId = $destinationId;
            """;
        command.Parameters.AddWithValue("$companyId", companyId);
        command.Parameters.AddWithValue("$originId", originId);
        command.Parameters.AddWithValue("$destinationId", destinationId);
        var result = await command.ExecuteScalarAsync();
        return result is null ? null : Convert.ToDouble(result);
    }

    public async Task<IReadOnlyList<Price>> GetPricesAsync()
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Prices.CompanyId,
                   Prices.OriginId,
                   Prices.DestinationId,
                   Prices.Amount,
                   Companies.Name,
                   Origins.Name,
                   Destinations.Name
            FROM Prices
            JOIN Companies ON Companies.Id = Prices.CompanyId
            JOIN Origins ON Origins.Id = Prices.OriginId
            JOIN Destinations ON Destinations.Id = Prices.DestinationId
            ORDER BY Companies.Name, Origins.Name, Destinations.Name;
            """;
        await using var reader = await command.ExecuteReaderAsync();

        var results = new List<Price>();
        while (await reader.ReadAsync())
        {
            results.Add(new Price
            {
                CompanyId = reader.GetInt32(0),
                OriginId = reader.GetInt32(1),
                DestinationId = reader.GetInt32(2),
                Amount = reader.GetDouble(3),
                CompanyName = reader.GetString(4),
                OriginName = reader.GetString(5),
                DestinationName = reader.GetString(6)
            });
        }

        return results;
    }

    public async Task UpsertPriceAsync(Price price)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Prices (CompanyId, OriginId, DestinationId, Amount)
            VALUES ($companyId, $originId, $destinationId, $amount)
            ON CONFLICT(CompanyId, OriginId, DestinationId)
            DO UPDATE SET Amount = excluded.Amount;
            """;
        command.Parameters.AddWithValue("$companyId", price.CompanyId);
        command.Parameters.AddWithValue("$originId", price.OriginId);
        command.Parameters.AddWithValue("$destinationId", price.DestinationId);
        command.Parameters.AddWithValue("$amount", price.Amount);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeletePriceAsync(int companyId, int originId, int destinationId)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM Prices
            WHERE CompanyId = $companyId
              AND OriginId = $originId
              AND DestinationId = $destinationId;
            """;
        command.Parameters.AddWithValue("$companyId", companyId);
        command.Parameters.AddWithValue("$originId", originId);
        command.Parameters.AddWithValue("$destinationId", destinationId);
        await command.ExecuteNonQueryAsync();
    }
}
