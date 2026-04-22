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
            return View(FakeDatabase.Categorias);
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

            // Validar nombre duplicado
            bool duplicado = FakeDatabase.Categorias
                .Any(c => c.Nombre.Equals(model.Nombre, StringComparison.OrdinalIgnoreCase));

            if (duplicado)
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                return View(model);
            }

            model.Id = FakeDatabase.NextCategoriaId++;
            model.Activo = true;
            FakeDatabase.Categorias.Add(model);

            TempData["Exito"] = $"Categoría '{model.Nombre}' creada correctamente.";
            return RedirectToAction("Index");
        }

        // CA-3: Formulario editar
        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            var cat = FakeDatabase.Categorias.FirstOrDefault(c => c.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        // CA-3: Guardar cambios
        [HttpPost("editar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validar nombre duplicado (ignorar la misma categoría)
            bool duplicado = FakeDatabase.Categorias
                .Any(c => c.Nombre.Equals(model.Nombre, StringComparison.OrdinalIgnoreCase) && c.Id != id);

            if (duplicado)
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                return View(model);
            }

            var cat = FakeDatabase.Categorias.FirstOrDefault(c => c.Id == id);
            if (cat == null) return NotFound();

            cat.Nombre = model.Nombre;
            cat.Descripcion = model.Descripcion;

            TempData["Exito"] = $"Categoría '{cat.Nombre}' actualizada.";
            return RedirectToAction("Index");
        }

        // CA-4: Soft delete — cambia Activo a false
        [HttpPost("eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var cat = FakeDatabase.Categorias.FirstOrDefault(c => c.Id == id);
            if (cat == null) return NotFound();

            cat.Activo = false;

            TempData["Exito"] = $"Categoría '{cat.Nombre}' desactivada.";
            return RedirectToAction("Index");
        }

        // Reactivar (bonus útil para la demo)
        [HttpPost("activar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            var cat = FakeDatabase.Categorias.FirstOrDefault(c => c.Id == id);
            if (cat == null) return NotFound();

            cat.Activo = true;

            TempData["Exito"] = $"Categoría '{cat.Nombre}' reactivada.";
            return RedirectToAction("Index");
        }
    }
}