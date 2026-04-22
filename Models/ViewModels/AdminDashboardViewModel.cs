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
