using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers
{
    [Route("admin/categorias")]
    public class CategoriaController : Controller
    {
        // CA-1: Listar todas las categorías (activas e inactivas para admin)
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(FakeDatabase.Instance.Categorias);
        }

        // CA-2: Formulario crear
        [HttpGet("crear")]
        public IActionResult Create()
        {
            return View(new Categoria());
        }

        // CA-2: Guardar nueva categoría
        [HttpPost("crear")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (IsDuplicateName(model.Nombre))
            {
                ModelState.AddModelError(nameof(model.Nombre), "Ya existe una categoría con ese nombre.");
                return View(model);
            }

            model.Id = FakeDatabase.Instance.NextCategoriaId++;
            model.Activo = true;
            FakeDatabase.Instance.Categorias.Add(model);

            TempData["Exito"] = $"Categoría '{model.Nombre}' creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // CA-3: Formulario editar
        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            var categoria = GetCategoriaById(id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // CA-3: Guardar cambios
        [HttpPost("editar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (IsDuplicateName(model.Nombre, id))
            {
                ModelState.AddModelError(nameof(model.Nombre), "Ya existe una categoría con ese nombre.");
                return View(model);
            }

            var categoria = GetCategoriaById(id);
            if (categoria == null)
                return NotFound();

            categoria.Nombre = model.Nombre;
            categoria.Descripcion = model.Descripcion;

            TempData["Exito"] = $"Categoría '{categoria.Nombre}' actualizada.";
            return RedirectToAction(nameof(Index));
        }

        // CA-4: Soft delete — cambia Activo a false
        [HttpPost("eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var categoria = GetCategoriaById(id);
            if (categoria == null)
                return NotFound();

            categoria.Activo = false;
            TempData["Exito"] = $"Categoría '{categoria.Nombre}' desactivada.";
            return RedirectToAction(nameof(Index));
        }

        // Reactivar (bonus útil para la demo)
        [HttpPost("activar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            var categoria = GetCategoriaById(id);
            if (categoria == null)
                return NotFound();

            categoria.Activo = true;
            TempData["Exito"] = $"Categoría '{categoria.Nombre}' reactivada.";
            return RedirectToAction(nameof(Index));
        }

        private static bool IsDuplicateName(string nombre, int excludeId = 0)
        {
            return FakeDatabase.Instance.Categorias
                .Any(c => c.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)
                          && c.Id != excludeId);
        }

        private static Categoria? GetCategoriaById(int id)
        {
            return FakeDatabase.Instance.Categorias.FirstOrDefault(c => c.Id == id);
        }
    }
}