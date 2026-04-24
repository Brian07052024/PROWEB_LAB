using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_lab_proweb.Data;
using Proyecto_lab_proweb.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar cadena de conexion a SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar ASP.NET Core Identity
builder.Services.AddIdentity<UsuarioAplicacion, IdentityRole>(options =>
{
    // Configuracion de contrasenas
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Configuracion de usuario
    options.User.RequireUniqueEmail = true;

    // Configuracion de inicio de sesion
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configurar cookies de autenticacion
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login";
    options.LogoutPath = "/Cuenta/Logout";
    options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
 options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Inicializar base de datos con datos de prueba
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<UsuarioAplicacion>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.InicializarAsync(context, userManager, roleManager);
    }
  catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
 logger.LogError(ex, "Error al inicializar la base de datos");
    }
}

// Configurar el pipeline de HTTP
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
