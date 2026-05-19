using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.Models;
using Proyecto_lab_proweb.ViewModels;

namespace Proyecto_lab_proweb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosController : Controllerz
    {
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 10;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? busqueda, int? categoriaId, int pagina = 1)
        {
            var consulta = _context.Productos
                .Include(p => p.Categoria)
        .Include(p => p.Proveedor)
           .AsQueryable();

            // Aplicar filtro de busqueda
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(p =>
                      p.Nombre.Contains(busqueda) ||
                      (p.Descripcion != null && p.Descripcion.Contains(busqueda)));
            }

            // Aplicar filtro de categoria
            if (categoriaId.HasValue)
            {
                consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
            }

            // Contar total de registros
            var totalRegistros = await consulta.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)RegistrosPorPagina);

            // Aplicar paginacion
            var productos = await consulta
            .OrderBy(p => p.Nombre)
            .Skip((pagina - 1) * RegistrosPorPagina)
            .Take(RegistrosPorPagina)
            .ToListAsync();

            var viewModel = new ProductoListaViewModel
            {
                Productos = productos,
                TerminoBusqueda = busqueda,
                CategoriaFiltro = categoriaId,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros,
                RegistrosPorPagina = RegistrosPorPagina
            };

            // Cargar categorias para el filtro
            ViewData["Categorias"] = new SelectList(_context.Categorias, "Id", "Nombre", categoriaId);

            return View(viewModel);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
    .Include(p => p.Categoria)
         .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult Crear()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Producto creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock,CategoriaId,ProveedorId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Producto actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExiste(producto.Id))
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

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Nombre", producto.ProveedorId);
            return View(producto);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                 .Include(p => p.Categoria)
        .Include(p => p.Proveedor)
              .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Producto eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductoExiste(int id)
        {
            return await _context.Productos.AnyAsync(e => e.Id == id);
        }
    }
}
