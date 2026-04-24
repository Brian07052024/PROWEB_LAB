using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.Models;

namespace Proyecto_lab_proweb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Proveedores
        .Include(p => p.Productos)
            .ToListAsync();
            return View(proveedores);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .Include(p => p.Productos)
      .FirstOrDefaultAsync(m => m.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Id,Nombre,Email,Telefono")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Proveedor creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Email,Telefono")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Proveedor actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExiste(proveedor.Id))
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
            return View(proveedor);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
       .Include(p => p.Productos)
  .FirstOrDefaultAsync(m => m.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var proveedor = await _context.Proveedores
             .Include(p => p.Productos)
    .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor != null)
            {
                if (proveedor.Productos.Any())
                {
                    TempData["Error"] = "No se puede eliminar el proveedor porque tiene productos asociados";
                    return RedirectToAction(nameof(Index));
                }

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Proveedor eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExiste(int id)
        {
            return _context.Proveedores.Any(e => e.Id == id);
        }
    }
}
