using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Models;

namespace Proyecto_lab_proweb.Data
{
    public class ApplicationDbContext : IdentityDbContext<UsuarioAplicacion>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Producto>()
            .HasOne(p => p.Proveedor)
            .WithMany(pr => pr.Productos)
            .HasForeignKey(p => p.ProveedorId)
            .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
