using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto_lab_proweb.Models;
using Proyecto_lab_proweb.ViewModels;

namespace Proyecto_lab_proweb.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly SignInManager<UsuarioAplicacion> _signInManager;

        public CuentaController(
      UserManager<UsuarioAplicacion> userManager,
            SignInManager<UsuarioAplicacion> signInManager)
        {
 _userManager = userManager;
    _signInManager = signInManager;
        }

        [HttpGet]
     public IActionResult Registro()
        {
       return View();
        }

        [HttpPost]
[ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
      {
      var usuario = new UsuarioAplicacion
           {
         UserName = model.Email,
          Email = model.Email,
         NombreCompleto = model.NombreCompleto
            };

     var resultado = await _userManager.CreateAsync(usuario, model.Password);

    if (resultado.Succeeded)
    {
           await _userManager.AddToRoleAsync(usuario, "Usuario");
      await _signInManager.SignInAsync(usuario, isPersistent: false);
  TempData["Exito"] = "Registro exitoso. Bienvenido!";
   return RedirectToAction("Index", "Home");
   }

            foreach (var error in resultado.Errors)
                {
ModelState.AddModelError(string.Empty, error.Description);
                }
     }

     return View(model);
        }

[HttpGet]
 public IActionResult Login(string? returnUrl = null)
   {
        ViewData["ReturnUrl"] = returnUrl;
    return View();
      }

        [HttpPost]
     [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
  {
     ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
    {
  var resultado = await _signInManager.PasswordSignInAsync(
     model.Email,
                model.Password,
             model.RecordarMe,
  lockoutOnFailure: false);

       if (resultado.Succeeded)
     {
       TempData["Exito"] = "Inicio de sesion exitoso";
         return RedirectToLocal(returnUrl);
   }
              else
            {
           ModelState.AddModelError(string.Empty, "Intento de inicio de sesion invalido");
    }
      }

       return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
   public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
        TempData["Exito"] = "Sesion cerrada correctamente";
            return RedirectToAction("Index", "Home");
        }

  [HttpGet]
    public IActionResult AccesoDenegado()
    {
            return View();
        }

        private IActionResult RedirectToLocal(string? returnUrl)
     {
       if (Url.IsLocalUrl(returnUrl))
       {
   return Redirect(returnUrl);
            }
       else
     {
            return RedirectToAction("Index", "Home");
    }
        }
    }
}
