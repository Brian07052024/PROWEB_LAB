using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.ViewModels;

namespace Proyecto_lab_proweb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalProductos = await _context.Productos.CountAsync(),
                TotalCategorias = await _context.Categorias.CountAsync(),
                TotalProveedores = await _context.Proveedores.CountAsync(),
                ProductosBajoStock = await _context.Productos.CountAsync(p => p.Stock > 0 && p.Stock <= 10),
                ProductosAgotados = await _context.Productos.CountAsync(p => p.Stock == 0),
                ValorTotalInventario = await _context.Productos.SumAsync(p => p.Precio * p.Stock)
            };

            // Obtener productos recientes (ultimos 5)
            viewModel.ProductosRecientes = await _context.Productos
             .Include(p => p.Categoria)
                  .OrderByDescending(p => p.Id)
             .Take(5)
            .Select(p => new ProductoRecienteViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Categoria = p.Categoria != null ? p.Categoria.Nombre : "Sin categoria",
                Precio = p.Precio,
                Stock = p.Stock
            })
          .ToListAsync();

            // Obtener categorias con mas productos
            viewModel.CategoriasMasProductos = await _context.Categorias
          .Include(c => c.Productos)
             .OrderByDescending(c => c.Productos.Count)
    .Take(5)
        .Select(c => new CategoriaEstadisticaViewModel
        {
            Nombre = c.Nombre,
            CantidadProductos = c.Productos.Count,
            ValorTotal = c.Productos.Sum(p => p.Precio * p.Stock)
        })
 .ToListAsync();

            return View(viewModel);
        }
    }
}
