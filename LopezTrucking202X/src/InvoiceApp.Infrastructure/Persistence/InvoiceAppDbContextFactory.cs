using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvoiceApp.Infrastructure.Persistence;

public sealed class InvoiceAppDbContextFactory : IDesignTimeDbContextFactory<InvoiceAppDbContext>
{
    public InvoiceAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InvoiceAppDbContext>();
        optionsBuilder.UseSqlite("Data Source=invoiceapp.db");
        return new InvoiceAppDbContext(optionsBuilder.Options);
    }
}
