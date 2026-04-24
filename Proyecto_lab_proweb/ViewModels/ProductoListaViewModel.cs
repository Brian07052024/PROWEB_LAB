using Proyecto_lab_proweb.Models;

namespace Proyecto_lab_proweb.ViewModels
{
    // ViewModel para lista de productos con paginacion y busqueda
    public class ProductoListaViewModel
    {
        public List<Producto> Productos { get; set; } = new();
        public string? TerminoBusqueda { get; set; }
        public int? CategoriaFiltro { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosPorPagina { get; set; } = 10;

        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }

    // ViewModel para el catalogo publico con paginacion y busqueda
    public class CatalogoViewModel
    {
        public List<Producto> Productos { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public string? TerminoBusqueda { get; set; }
        public int? CategoriaSeleccionada { get; set; }
        public string? OrdenarPor { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosPorPagina { get; set; } = 9;

        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
