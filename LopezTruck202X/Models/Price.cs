namespace LopezTruck202X.Models;

public sealed class Price
{
    public int CompanyId { get; set; }
    public int OriginId { get; set; }
    public int DestinationId { get; set; }
    public double Amount { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string OriginName { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
}
