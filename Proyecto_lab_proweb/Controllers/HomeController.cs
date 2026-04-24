using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.Models;
using Proyecto_lab_proweb.ViewModels;

namespace Proyecto_lab_proweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 9;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Si el usuario es admin, redirigir al dashboard
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            // Obtener estadisticas basicas para la pagina de inicio
            ViewData["TotalProductos"] = await _context.Productos.CountAsync();
            ViewData["TotalCategorias"] = await _context.Categorias.CountAsync();

            return View();
        }

        public IActionResult Acerca()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        public async Task<IActionResult> Catalogo(string? busqueda, int? categoriaId, string? ordenar, int pagina = 1)
        {
            var consulta = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Stock > 0) // Solo mostrar productos con stock
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

            // Aplicar ordenamiento
            consulta = ordenar switch
            {
                "precio_asc" => consulta.OrderBy(p => p.Precio),
                "precio_desc" => consulta.OrderByDescending(p => p.Precio),
                "nombre_desc" => consulta.OrderByDescending(p => p.Nombre),
                _ => consulta.OrderBy(p => p.Nombre)
            };

            // Contar total de registros
            var totalRegistros = await consulta.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)RegistrosPorPagina);

            // Aplicar paginacion
            var productos = await consulta
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();

            // Obtener todas las categorias para el filtro
            var categorias = await _context.Categorias.ToListAsync();

            var viewModel = new CatalogoViewModel
            {
                Productos = productos,
                Categorias = categorias,
                TerminoBusqueda = busqueda,
                CategoriaSeleccionada = categoriaId,
                OrdenarPor = ordenar,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros,
                RegistrosPorPagina = RegistrosPorPagina
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
