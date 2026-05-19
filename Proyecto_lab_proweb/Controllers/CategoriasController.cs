using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.Models;

namespace Proyecto_lab_proweb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
            .Include(c => c.Productos)
            .ToListAsync();
            return View(categorias);
        }

        //[HttpGet]
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
             .Include(c => c.Productos)
                  .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Categoría creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Categoría actualizada exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExiste(categoria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
             .Include(c => c.Productos)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var categoria = await _context.Categorias
                 .Include(c => c.Productos)
              .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria != null)
            {
                if (categoria.Productos.Any())
                {
                    TempData["Error"] = "No se puede eliminar la categoría porque tiene productos asociados";
                    return RedirectToAction(nameof(Index));
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Categoría eliminada exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExiste(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
