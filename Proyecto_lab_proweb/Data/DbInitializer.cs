using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Models;

namespace Proyecto_lab_proweb.Data
{
    public static class DbInitializer
    {
        public static async Task InicializarAsync(
       ApplicationDbContext context,
              UserManager<UsuarioAplicacion> userManager,
         RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Usuario"))
            {
                await roleManager.CreateAsync(new IdentityRole("Usuario"));
            }

            if (!await userManager.Users.AnyAsync())
            {
                var adminUser = new UsuarioAplicacion
                {
                    UserName = "admin@inventario.com",
                    Email = "admin@inventario.com",
                    EmailConfirmed = true,
                    NombreCompleto = "Administrador del Sistema"
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var normalUser = new UsuarioAplicacion
                {
                    UserName = "usuario@inventario.com",
                    Email = "usuario@inventario.com",
                    EmailConfirmed = true,
                    NombreCompleto = "Usuario Normal"
                };

                await userManager.CreateAsync(normalUser, "Usuario123!");
                await userManager.AddToRoleAsync(normalUser, "Usuario");
            }

            if (!context.Categorias.Any())
            {
                var categorias = new[]
                {
                   new Categoria { Nombre = "Electrónica", Descripcion = "Dispositivos electrónicos y accesorios" },
                   new Categoria { Nombre = "Ropa", Descripcion = "Prendas de vestir y accesorios de moda" },
                   new Categoria { Nombre = "Alimentos", Descripcion = "Productos alimenticios y bebidas" },
                   new Categoria { Nombre = "Muebles", Descripcion = "Muebles para hogar y oficina" },
                   new Categoria { Nombre = "Libros", Descripcion = "Libros y material de lectura" }
                };

                context.Categorias.AddRange(categorias);
                await context.SaveChangesAsync();
            }

            if (!context.Proveedores.Any())
            {
                var proveedores = new[]
                {
                            new Proveedor { Nombre = "TecnoSuministros S.A.", Email = "contacto@tecnosuministros.com", Telefono = "555-0101" },
                            new Proveedor { Nombre = "Distribuidora Global", Email = "ventas@distglobal.com", Telefono = "555-0102" },
                            new Proveedor { Nombre = "Importaciones Directas", Email = "info@importdirectas.com", Telefono = "555-0103" },
                            new Proveedor { Nombre = "Suministros Rápidos", Email = "pedidos@sumrapidos.com", Telefono = "555-0104" }
                };

                context.Proveedores.AddRange(proveedores);
                await context.SaveChangesAsync();
            }

            if (!context.Productos.Any())
            {
                var categorias = await context.Categorias.ToListAsync();
                var proveedores = await context.Proveedores.ToListAsync();

                if (categorias.Any() && proveedores.Any())
                {
                    var productos = new[]
             {
                          new Producto { Nombre = "Laptop HP", Descripcion = "Laptop HP Core i5 8GB RAM", Precio = 799.99m, Stock = 15, CategoriaId = categorias[0].Id, ProveedorId = proveedores[0].Id },
                           new Producto { Nombre = "Mouse Inalámbrico", Descripcion = "Mouse óptico inalámbrico", Precio = 25.99m, Stock = 50, CategoriaId = categorias[0].Id, ProveedorId = proveedores[0].Id },
                            new Producto { Nombre = "Camiseta Básica", Descripcion = "Camiseta de algodón 100%", Precio = 15.99m, Stock = 100, CategoriaId = categorias[1].Id, ProveedorId = proveedores[1].Id },
                          new Producto { Nombre = "Pantalón Jeans", Descripcion = "Pantalón jeans azul clásico", Precio = 45.99m, Stock = 60, CategoriaId = categorias[1].Id, ProveedorId = proveedores[1].Id },
                              new Producto { Nombre = "Café Premium", Descripcion = "Café molido 500g", Precio = 12.99m, Stock = 200, CategoriaId = categorias[2].Id, ProveedorId = proveedores[2].Id },
                       new Producto { Nombre = "Galletas de Chocolate", Descripcion = "Paquete de galletas 300g", Precio = 4.99m, Stock = 150, CategoriaId = categorias[2].Id, ProveedorId = proveedores[2].Id },
                       new Producto { Nombre = "Escritorio de Oficina", Descripcion = "Escritorio moderno 120x60cm", Precio = 299.99m, Stock = 10, CategoriaId = categorias[3].Id, ProveedorId = proveedores[3].Id },
                                  new Producto { Nombre = "Silla Ergonómica", Descripcion = "Silla de oficina con soporte lumbar", Precio = 189.99m, Stock = 25, CategoriaId = categorias[3].Id, ProveedorId = proveedores[3].Id },
                       new Producto { Nombre = "Novela Bestseller", Descripcion = "Última novela del autor famoso", Precio = 19.99m, Stock = 40, CategoriaId = categorias[4].Id, ProveedorId = proveedores[1].Id },
                          new Producto { Nombre = "Manual de Programación", Descripcion = "Guía completa de C# y .NET", Precio = 49.99m, Stock = 30, CategoriaId = categorias[4].Id, ProveedorId = proveedores[1].Id }
                    };

                    context.Productos.AddRange(productos);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}