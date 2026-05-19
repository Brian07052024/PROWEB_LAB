# 4.1 Documentación técnica

# 4.1 Documentación técnica
## Cómo se monta el proyecto
1. Clonar el repositorio.
2. Restaurar dependencias.
3. Configurar la cadena de conexión en `appsettings.json`.
4. Ejecutar migraciones de base de datos.
5. Iniciar la aplicacion.

## Tecnologías utilizadas
- .NET 9
- ASP.NET Core (Razor Pages/MVC)
- Entity Framework Core
- SQL Server (o el motor configurado)
- Bootstrap

## C�mo se ejecuta
```sh
dotnet restore
dotnet ef database update
dotnet run
```
Acceder a `https://localhost:xxxx`.

---

# 4.2 Documentación de usuario

## Funcionalidades
- Autenticación y roles (admin/usuario).
- Gestión de productos.
- Gestión de categorías.
- Gestión de proveedores.
- Catálogo público.
- Dashboard administrativo.

## Cómo se utiliza
- **Crear cuenta**: ir a `Registro`, completar formulario y enviar.
- **Iniciar sesión**: ir a `Login`, ingresar credenciales.
- **Crear producto**: ir a `Productos` → `Crear`, completar datos y guardar.
- **Editar producto**: ir a `Productos` → `Editar`.
- **Eliminar producto**: ir a `Productos` → `Eliminar`.
- **Crear categoría**: ir a `Categorías` → `Crear`.
- **Crear proveedor**: ir a `Proveedores` → `Crear`.
- **Compra** (si aplica en tu versión): seleccionar producto en el catálogo y seguir el flujo definido.

## Requisitos previos
- Contar con usuario activo.
- Para administración: rol `Admin`.
- Base de datos inicializada.
- Acceso previo otorgado por un administrador (si aplica).