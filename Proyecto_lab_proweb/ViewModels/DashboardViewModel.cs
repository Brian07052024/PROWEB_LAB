namespace Proyecto_lab_proweb.ViewModels
{
    // ViewModel para el dashboard del administrador
    public class DashboardViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalProveedores { get; set; }
        public int ProductosBajoStock { get; set; }
        public decimal ValorTotalInventario { get; set; }
        public int ProductosAgotados { get; set; }

        // Productos recientes
        public List<ProductoRecienteViewModel> ProductosRecientes { get; set; } = new();

        // Categorias con mas productos
        public List<CategoriaEstadisticaViewModel> CategoriasMasProductos { get; set; } = new();
    }

    public class ProductoRecienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class CategoriaEstadisticaViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
