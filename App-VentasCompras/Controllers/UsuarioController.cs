using App_VentasCompras.Data;
using App_VentasCompras.DTOs;
using App_VentasCompras.Models;
using App_VentasCompras.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace App_VentasCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly Seguridad _seguridad;

        public UsuarioController(AppDBContext context, Seguridad seguridad)
        {
            _context = context;
            _seguridad = seguridad;
        }

        #region Listado
        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("lista")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get()
        {
            var listaDTO = new List<UsuarioDTO>();
            var listaDB = await _context.Usuarios
                .Include(u => u.Persona)
                   .ThenInclude(p => p.Ubicacion)
                .Include(s => s.Status)
                   .ThenInclude(u => u.Valoracion)
                .Include(p => p.Productos)
                .ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new UsuarioDTO
                {
                    IDUsuario = item.IDUsuario,
                    Email = item.Email,
                    Username = item.Username,
                    Password = item.Password,
                    Rol = item.Rol,

                    IDPersona = item.Persona.IDPersona,
                    Nombre = item.Persona.Nombre,
                    Apellido = item.Persona.Apellido,
                    NroCelular = item.Persona.NroCelular,

                    Pais = item.Persona.Ubicacion.Pais,
                    Localidad = item.Persona.Ubicacion.Localidad,
                    Provincia = item.Persona.Ubicacion.Provincia,
                    CodigoPostal = item.Persona.Ubicacion.CodigoPostal,
                    Calle = item.Persona.Ubicacion.Calle,
                    NroCalle = item.Persona.Ubicacion.Calle,

                    VentasExitosas = item.Status.VentasExitosas,
                    Bueno = item.Status.Valoracion.Bueno,
                    Regular = item.Status.Valoracion.Regular,
                    Malo = item.Status.Valoracion.Malo,
                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Buscar
        [HttpGet]
        [Authorize(Roles = "Admin, Usuario")]
        //[AllowAnonymous]
        [Route("buscar/{id}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuarioDTO = new UsuarioDTO();
            var usuarioDB = await _context.Usuarios
                .Include(u => u.Persona)
                   .ThenInclude(p => p.Ubicacion)
                .Include(s => s.Status)
                   .ThenInclude(u => u.Valoracion)
                .Include(p => p.Productos)
                .Where(c => c.IDUsuario == id)
                .FirstOrDefaultAsync();

            if (usuarioDB == null)
            {
                return NotFound();
            }

            usuarioDTO.IDUsuario = id;
            usuarioDTO.Email = usuarioDB.Email;
            usuarioDTO.Username = usuarioDB.Username;
            usuarioDTO.Password = usuarioDB.Password;
            usuarioDTO.Rol = usuarioDB.Rol;

            usuarioDTO.IDPersona = usuarioDB.Persona.IDPersona;
            usuarioDTO.Nombre = usuarioDB.Persona.Nombre;
            usuarioDTO.Apellido = usuarioDB.Persona.Apellido;
            usuarioDTO.NroCelular = usuarioDB.Persona.NroCelular;

            usuarioDTO.Pais = usuarioDB.Persona.Ubicacion.Pais;
            usuarioDTO.Localidad = usuarioDB.Persona.Ubicacion.Localidad;
            usuarioDTO.Provincia = usuarioDB.Persona.Ubicacion.Provincia;
            usuarioDTO.CodigoPostal = usuarioDB.Persona.Ubicacion.CodigoPostal;
            usuarioDTO.Calle = usuarioDB.Persona.Ubicacion.Calle;
            usuarioDTO.NroCalle = usuarioDB.Persona.Ubicacion.NroCalle;

            usuarioDTO.VentasExitosas = usuarioDB.Status.VentasExitosas;
            usuarioDTO.Bueno = usuarioDB.Status.Valoracion.Bueno;
            usuarioDTO.Malo = usuarioDB.Status.Valoracion.Malo;
            usuarioDTO.Regular = usuarioDB.Status.Valoracion.Regular;

            return Ok(usuarioDTO);
        }
        #endregion


        #region CREAR
        [HttpPost]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("crear")]
        public async Task<ActionResult<UsuarioDTO>> Crear(UsuarioDTO usuarioDTO)
        {
            var ubicacionDB = new Ubicacion
            {
                Pais = usuarioDTO.Pais,
                Localidad = usuarioDTO.Localidad,
                Provincia = usuarioDTO.Provincia,
                CodigoPostal = usuarioDTO.CodigoPostal,
                Calle = usuarioDTO.Calle,
                NroCalle = usuarioDTO.NroCalle
            };
            var personaDB = new Persona
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                NroCelular = usuarioDTO.NroCelular,
                Ubicacion = ubicacionDB
            };
       
            var valoracionDB = new Valoracion
            {
                Bueno = usuarioDTO.Bueno,
                Malo = usuarioDTO.Malo,
                Regular = usuarioDTO.Regular
            };
            var statusDB = new Status
            {
                VentasExitosas = usuarioDTO.VentasExitosas,
                Valoracion = valoracionDB
            };

            var usuarioDB = new Usuario
            {
                Email = usuarioDTO.Email,
                Username = usuarioDTO.Username,
                Password = _seguridad.encriptarSHA256(usuarioDTO.Password),
                Rol = usuarioDTO.Rol,
                Status = statusDB,
                Persona = personaDB
            };


            await _context.Usuarios.AddAsync(usuarioDB);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado");
        }
        #endregion


        #region CREAR para registro usuario
        [HttpPost]
        [AllowAnonymous]
        [Route("registro")]
        public async Task<ActionResult<UsuarioDTO>> Registro(UsuarioDTO usuarioDTO)
        {
            var ubicacionDB = new Ubicacion
            {
                Pais = usuarioDTO.Pais,
                Localidad = usuarioDTO.Localidad,
                Provincia = usuarioDTO.Provincia,
                CodigoPostal = usuarioDTO.CodigoPostal,
                Calle = usuarioDTO.Calle,
                NroCalle = usuarioDTO.NroCalle
            };
            var personaDB = new Persona
            {
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                NroCelular = usuarioDTO.NroCelular,
                Ubicacion = ubicacionDB
            };

            var valoracionDB = new Valoracion
            {
                Bueno = usuarioDTO.Bueno,
                Malo = usuarioDTO.Malo,
                Regular = usuarioDTO.Regular
            };
            var statusDB = new Status
            {
                VentasExitosas = usuarioDTO.VentasExitosas,
                Valoracion = valoracionDB
            };

            var usuarioDB = new Usuario
            {
                Email = usuarioDTO.Email,
                Username = usuarioDTO.Username,
                Password = _seguridad.encriptarSHA256(usuarioDTO.Password),
                Rol = Roles.Usuario,
                Status = statusDB,
                Persona = personaDB
            };

            await _context.Usuarios.AddAsync(usuarioDB);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado");
        }
        #endregion


        #region EDITAR
        [HttpPut]
        [Authorize(Roles = "Admin, Usuario")]
        //[AllowAnonymous]
        [Route("editar/{id}")]
        public async Task<ActionResult<UsuarioDTO>> Editar(int id, UsuarioEditarDTO usuarioDTO)
        {
            var usuariodb = await _context.Usuarios
                .Include(u => u.Persona)
                   .ThenInclude(p => p.Ubicacion)
                .Include(s => s.Status)
                   .ThenInclude(u => u.Valoracion)
                .FirstOrDefaultAsync(u => u.IDUsuario == id);

            if (usuariodb == null)
            {
                return NotFound("Usuario no encontrado.");
            }


            usuariodb.Email = usuarioDTO.Email;
            usuariodb.Username = usuarioDTO.Username;

            if (!string.IsNullOrEmpty(usuarioDTO.Password))
            {
                usuariodb.Password = _seguridad.encriptarSHA256(usuarioDTO.Password);
            }
            else { 
            }
           // usuariodb.Password = _seguridad.encriptarSHA256(usuarioDTO.Password);
            usuariodb.Rol = usuarioDTO.Rol;

            if (usuariodb.Persona != null)
            {
                usuariodb.Persona.Nombre = usuarioDTO.Nombre;
                usuariodb.Persona.Apellido = usuarioDTO.Apellido;
                usuariodb.Persona.NroCelular = usuarioDTO.NroCelular;
            }
            if (usuariodb.Persona.Ubicacion != null)
            {
                usuariodb.Persona.Ubicacion.Pais = usuarioDTO.Pais;
                usuariodb.Persona.Ubicacion.Localidad = usuarioDTO.Localidad;
                usuariodb.Persona.Ubicacion.Provincia = usuarioDTO.Provincia;
                usuariodb.Persona.Ubicacion.CodigoPostal = usuarioDTO.CodigoPostal;
                usuariodb.Persona.Ubicacion.Calle = usuarioDTO.Calle;
                usuariodb.Persona.Ubicacion.NroCalle = usuarioDTO.NroCalle;

            }
            if(usuariodb.Status != null)
            {
                usuariodb.Status.VentasExitosas = usuarioDTO.VentasExitosas;
            }
            if(usuariodb.Status.Valoracion != null)
            {
                usuariodb.Status.Valoracion.Regular = usuarioDTO.Regular;
                usuariodb.Status.Valoracion.Malo = usuarioDTO.Malo;
                usuariodb.Status.Valoracion.Bueno = usuarioDTO.Bueno;

            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar los datos: {ex.Message}");
            }
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<UsuarioDTO>> Eliminar(int id)
        {
            var usuarioDB = await _context.Usuarios.FindAsync(id);

            if (usuarioDB is null)
            {
                return NotFound("Usuario no encontrado");
            }

            _context.Usuarios.Remove(usuarioDB);

            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado");
        }
        #endregion

    }
}
