namespace textil_salas.Models;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<OrderSummary> Orders { get; set; } = [];
    public List<QuoteRow> Quotes { get; set; } = [];
}

public class QuoteRow
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}
