
using App_VentasCompras.Data;
using App_VentasCompras.DTOs;
using App_VentasCompras.Utils;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App_VentasCompras.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]//permite a todos
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly Seguridad _seguridad;

        public LoginController(AppDBContext context, Seguridad seguridad)
        {
            _seguridad = seguridad;
            _context = context;
        }

        [HttpPost]
        [Route("acceso")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email == loginDTO.Email && u.Password == _seguridad.encriptarSHA256(loginDTO.Password))
                .FirstOrDefaultAsync();

            if (usuario == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _seguridad.generarJWT(usuario) });
        }
    }
}
