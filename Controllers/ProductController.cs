using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers
{
    [Route("admin/productos")]
    public class ProductController : Controller
    {
        // Listar productos con filtros
        [HttpGet("")]
        public IActionResult Index(string? q, string? category, string? status)
        {
            var productos = FakeDatabase.Instance.Products.AsQueryable();

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(q))
            {
                productos = productos.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por categoría
            if (!string.IsNullOrWhiteSpace(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                productos = productos.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por estado
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals("active", StringComparison.OrdinalIgnoreCase))
                {
                    productos = productos.Where(p => p.IsActive);
                }
                else if (status.Equals("inactive", StringComparison.OrdinalIgnoreCase))
                {
                    productos = productos.Where(p => !p.IsActive);
                }
            }

            ViewBag.Query = q ?? string.Empty;
            ViewBag.Category = category ?? "all";
            ViewBag.Status = status ?? "all";
            ViewBag.CategoriasActivas = GetActiveCategories();

            return View(productos.ToList());
        }

        // Formulario crear
        [HttpGet("crear")]
        public IActionResult Create()
        {
            ViewBag.CategoriasActivas = GetActiveCategories();
            return View(new Product());
        }

        // Guardar nuevo producto
        [HttpPost("crear")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoriasActivas = GetActiveCategories();
                return View(model);
            }

            if (IsDuplicateName(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Ya existe un producto con ese nombre.");
                ViewBag.CategoriasActivas = GetActiveCategories();
                return View(model);
            }

            model.Sizes = Request.Form["SizesString"].ToString().Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            model.Id = FakeDatabase.Instance.NextProductId++.ToString();
            model.IsActive = true;
            FakeDatabase.Instance.Products.Add(model);

            TempData["Exito"] = $"Producto '{model.Name}' creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Formulario editar
        [HttpGet("editar/{id}")]
        public IActionResult Edit(string id)
        {
            var producto = GetProductById(id);
            if (producto == null)
                return NotFound();

            ViewBag.CategoriasActivas = GetActiveCategories();
            ViewBag.SizesString = string.Join(", ", producto.Sizes ?? Array.Empty<string>());
            return View(producto);
        }

        // Guardar cambios
        [HttpPost("editar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoriasActivas = GetActiveCategories();
                ViewBag.SizesString = Request.Form["SizesString"].ToString();
                return View(model);
            }

            if (IsDuplicateName(model.Name, id))
            {
                ModelState.AddModelError(nameof(model.Name), "Ya existe un producto con ese nombre.");
                ViewBag.CategoriasActivas = GetActiveCategories();
                ViewBag.SizesString = Request.Form["SizesString"].ToString();
                return View(model);
            }

            var producto = GetProductById(id);
            if (producto == null)
                return NotFound();

            model.Sizes = Request.Form["SizesString"].ToString().Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            producto.Name = model.Name;
            producto.Description = model.Description;
            producto.Price = model.Price;
            producto.Category = model.Category;
            producto.Subcategory = model.Subcategory;
            producto.ImageUrl = model.ImageUrl;
            producto.Material = model.Material;
            producto.Gsm = model.Gsm;
            producto.Sizes = model.Sizes;
            producto.B2BMinQty = model.B2BMinQty;

            TempData["Exito"] = $"Producto '{producto.Name}' actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // Soft delete — cambia IsActive a false
        [HttpPost("eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var producto = GetProductById(id);
            if (producto == null)
                return NotFound();

            producto.IsActive = false;
            TempData["Exito"] = $"Producto '{producto.Name}' desactivado.";
            return RedirectToAction(nameof(Index));
        }

        // Reactivar
        [HttpPost("activar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(string id)
        {
            var producto = GetProductById(id);
            if (producto == null)
                return NotFound();

            producto.IsActive = true;
            TempData["Exito"] = $"Producto '{producto.Name}' reactivado.";
            return RedirectToAction(nameof(Index));
        }

        private static bool IsDuplicateName(string name, string excludeId = "")
        {
            return FakeDatabase.Instance.Products
                .Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                          && p.Id != excludeId);
        }

        private static Product? GetProductById(string id)
        {
            return FakeDatabase.Instance.Products.FirstOrDefault(p => p.Id == id);
        }

        private static List<Categoria> GetActiveCategories()
        {
            return FakeDatabase.Instance.Categorias.Where(c => c.Activo).ToList();
        }
    }
}