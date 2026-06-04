using System.Collections.Generic;
using textil_salas.Models;

namespace textil_salas.ViewModels;

public class ReportViewModel
{
    public string ReportType { get; set; } = "sales";
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Order>? Orders { get; set; }
    public List<Product>? Products { get; set; }
}
