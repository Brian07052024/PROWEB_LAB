using Microsoft.AspNetCore.Identity;

namespace Proyecto_lab_proweb.Models
{
    public class UsuarioAplicacion : IdentityUser
    {
        [PersonalData]
        public string? NombreCompleto { get; set; }
    }
}
