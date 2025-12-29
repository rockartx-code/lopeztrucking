using System;
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
                Phone TEXT
            );

            CREATE TABLE IF NOT EXISTS Invoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number INTEGER NOT NULL UNIQUE,
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
            """;
        await command.ExecuteNonQueryAsync();
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

    public async Task SaveInvoiceAsync(Invoice invoice)
    {
        await using var connection = _database.CreateConnection();
        await connection.OpenAsync();

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
        selectCommand.CommandText = "SELECT Id FROM Customers WHERE Name = $name";
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
}
