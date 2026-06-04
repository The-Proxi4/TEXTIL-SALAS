using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext? _db;

        public AdminController(ApplicationDbContext? db = null)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var model = CreateDashboardViewModel();
            return View(model);
        }

        [HttpGet("reports")]
        public IActionResult Reports(string reportType = "sales", DateTime? startDate = null, DateTime? endDate = null)
        {
            // simple reuse of ReportViewModel used by ReportsController
            var vm = new textil_salas.ViewModels.ReportViewModel
            {
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate
            };

            if (reportType == "products")
            {
                vm.Products = textil_salas.Models.FakeDatabase.Instance.Products;
            }
            else if (_db != null)
            {
                var ordersQuery = _db.Orders.AsQueryable();
                if (startDate.HasValue)
                    ordersQuery = ordersQuery.Where(o => o.CreatedAt >= startDate.Value);
                if (endDate.HasValue)
                    ordersQuery = ordersQuery.Where(o => o.CreatedAt <= endDate.Value.Date.AddDays(1).AddTicks(-1));

                vm.Orders = ordersQuery.OrderByDescending(o => o.CreatedAt).Take(100).ToList();
            }

            return View("Reports/Index", vm);
        }

        private AdminDashboardViewModel CreateDashboardViewModel()
        {
            return new AdminDashboardViewModel
            {
                TotalProducts = 12,
                TotalOrders = 18,
                TotalCustomers = 9,
                TotalRevenue = 12850m,
                Orders = new()
                {
                    new() { Id = 1045, Customer = "Corporación Salas", Total = 1550m, Status = "Pendiente", CreatedAt = DateTime.Today.AddDays(-1) },
                    new() { Id = 1044, Customer = "Hotel Sol Andino", Total = 3200m, Status = "Entregado", CreatedAt = DateTime.Today.AddDays(-3) },
                    new() { Id = 1043, Customer = "Spa Nativa", Total = 890m, Status = "Enviado", CreatedAt = DateTime.Today.AddDays(-4) },
                },
                Quotes = new()
                {
                    new() { Id = 201, Company = "Hotel Sol Andino", Contact = "Mónica Pérez", Status = "Pendiente", Summary = "100 toallas de baño + 50 batas" },
                    new() { Id = 202, Company = "Spa Nativa", Contact = "Luis Chávez", Status = "Aprobada", Summary = "40 batas bordadas" },
                    new() { Id = 203, Company = "Gym Force", Contact = "Andrea León", Status = "Revisar", Summary = "120 toallas de mano" },
                }
            };
        }
    }
}
