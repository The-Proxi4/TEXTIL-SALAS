using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;
using textil_salas.ViewModels;

namespace textil_salas.Controllers;

public class ReportsController : Controller
{
    private readonly ApplicationDbContext _db;

    public ReportsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(string reportType = "sales", DateTime? startDate = null, DateTime? endDate = null)
    {
        var vm = new ReportViewModel
        {
            ReportType = reportType,
            StartDate = startDate,
            EndDate = endDate
        };

        // normalize dates
        if (vm.EndDate.HasValue)
            vm.EndDate = vm.EndDate.Value.Date.AddDays(1).AddTicks(-1);

        if (vm.ReportType == "products")
        {
            // use fake database for product catalog
            vm.Products = FakeDatabase.Instance.Products;
        }
        else
        {
            // orders / sales
            var ordersQuery = _db.Orders.AsQueryable();

            if (vm.StartDate.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CreatedAt >= vm.StartDate.Value);
            if (vm.EndDate.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CreatedAt <= vm.EndDate.Value);

            vm.Orders = ordersQuery.OrderByDescending(o => o.CreatedAt).Take(100).ToList();
        }

        return View(vm);
    }

    [HttpGet]
    public IActionResult Export(string reportType = "sales", DateTime? startDate = null, DateTime? endDate = null, string format = "csv")
    {
        // normalize end date
        if (endDate.HasValue)
            endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

        var vm = new ReportViewModel
        {
            ReportType = reportType,
            StartDate = startDate,
            EndDate = endDate
        };

        if (reportType == "products")
        {
            vm.Products = FakeDatabase.Instance.Products;
        }
        else
        {
            var ordersQuery = _db.Orders.AsQueryable();
            if (startDate.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                ordersQuery = ordersQuery.Where(o => o.CreatedAt <= endDate.Value);

            vm.Orders = ordersQuery.OrderByDescending(o => o.CreatedAt).ToList();
        }

        if (format?.ToLowerInvariant() == "pdf")
        {
            var doc = new textil_salas.Documents.ReportsPdfDocument(vm);
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var pdfBytes = QuestPDF.Fluent.Document.Create(doc).GeneratePdf();
            var fileName = reportType + "_report_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // default CSV export
        var csv = new System.Text.StringBuilder();
        if (reportType == "products")
        {
            csv.AppendLine("Id,Name,Category,Price,Active");
            foreach (var p in vm.Products ?? new())
                csv.AppendLine($"{p.Id},\"{p.Name}\",\"{p.Category}\",{p.Price},{(p.IsActive ? "1" : "0")}");
        }
        else
        {
            csv.AppendLine("OrderNumber,CreatedAt,UserId,Total,ItemsCount");
            foreach (var o in vm.Orders ?? new())
                csv.AppendLine($"{o.OrderNumber},{o.CreatedAt:O},{o.UserId},{o.Total},{o.Items?.Count ?? 0}");
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        var csvFile = reportType + "_report_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".csv";
        return File(bytes, "text/csv", csvFile);
    }
}
