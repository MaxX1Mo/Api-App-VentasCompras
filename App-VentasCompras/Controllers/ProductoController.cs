using Microsoft.AspNetCore.Http;
using App_VentasCompras.DTOs;
using App_VentasCompras.Data;
using App_VentasCompras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using App_VentasCompras.Utils;

namespace App_VentasCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ProductoController(AppDBContext context)
        {
            _context = context;
        }

        #region Listado
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("Lista")]
        public async Task<ActionResult<List<ProductoDTO>>> Get()
        {
            var listaDTO = new List<ProductoDTO>();
            var listaDB = await _context.Productos
                .Include(u => u.Usuario)
                .Include(u => u.Usuario)
                   .ThenInclude(p => p.Persona)
                .Include(pv => pv.ProductoVenta)
                .Include(c => c.Categoria)
                .ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new ProductoDTO
                {
                    IDProducto = item.IDProducto,
                    NombreProducto = item.NombreProducto,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio,
                    Imagen = item.Imagen,
                    IDUsuario = item.Usuario.IDUsuario,
                    Username = item.Usuario.Username,
                    Email = item.Usuario.Email,
                    NroCelular = item.Usuario.Persona.NroCelular,
                    NombreCategoria = item.Categoria.Nombre,
                    IDProductoVenta = item.ProductoVenta.IDProductoVenta,
                    Fecha = item.ProductoVenta.Fecha,
                    EstadoProducto = item.ProductoVenta.EstadoProducto,
                    EstadoVenta = item.ProductoVenta.EstadoVenta,
                    Cantidad = item.ProductoVenta.Cantidad,
                });
            }
            return Ok(listaDTO);
        }
        #endregion


        #region Buscar
        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("buscar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Buscar(int id)
        {
            var productoDTO = new ProductoDTO();
            var productoDB = await _context.Productos
                .Include(u => u.Usuario)
                  .ThenInclude(p => p.Persona)
                .Include(pv => pv.ProductoVenta)
                .Include(c => c.Categoria)
                .Where(p => p.IDProducto == id)
                .FirstOrDefaultAsync();
            if (productoDB == null)
            {
                return NotFound();
            }
            productoDTO.IDProducto = id;
            productoDTO.NombreProducto = productoDB.NombreProducto;
            productoDTO.Descripcion = productoDB.Descripcion;
            productoDTO.Precio = productoDB.Precio;
            productoDTO.Imagen = productoDB.Imagen;
            productoDTO.IDUsuario = productoDB.Usuario.IDUsuario;
            productoDTO.NombreCategoria = productoDB.Categoria.Nombre;
            productoDTO.IDProductoVenta = productoDB.ProductoVenta.IDProductoVenta;
            productoDTO.Fecha = productoDB.ProductoVenta.Fecha;
            productoDTO.EstadoProducto = productoDB.ProductoVenta.EstadoProducto;
            productoDTO.EstadoVenta = productoDB.ProductoVenta.EstadoVenta;
            productoDTO.Cantidad = productoDB.ProductoVenta.Cantidad;

            return Ok(productoDTO);
        }
        #endregion

        #region Crear
        [HttpPost]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("crear")]
        public async Task<ActionResult<ProductoDTO>> Crear(ProductoDTO productoDTO)
        {
            
            
            if (string.IsNullOrWhiteSpace(productoDTO.NombreCategoria))
            {
                productoDTO.NombreCategoria = "sin categoria";
            }

            // Buscar o crear la categoría, si no existe la categoria que agrego el usuario
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == productoDTO.NombreCategoria.ToLower());

            if (categoria == null)
            {
                categoria = new Categoria { Nombre = productoDTO.NombreCategoria };
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
            }
            //me traigo el id categoria creado
            productoDTO.IDCategoria = categoria.IDCategoria;

            var productoDB = new Producto
            {
                NombreProducto = productoDTO.NombreProducto,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Imagen = productoDTO.Imagen,
                IDUsuario = productoDTO.IDUsuario,
                IDCategoria = productoDTO.IDCategoria
            };

            if (string.IsNullOrWhiteSpace(productoDTO.NombreProducto) || productoDTO.Precio <= 0)
            {
                return BadRequest("Nombre del producto y precio son obligatorios.");
            }

            await _context.Productos.AddAsync(productoDB);
            await _context.SaveChangesAsync(); // para obtener el ID del producto

            var productoVenta = new ProductoVenta
            {
                Fecha = DateTime.Now,
                EstadoProducto = productoDTO.EstadoProducto,
                EstadoVenta = productoDTO.EstadoVenta,
                Cantidad = productoDTO.Cantidad,
                IDProducto = productoDB.IDProducto
            };

            await _context.ProductoVentas.AddAsync(productoVenta);
            await _context.SaveChangesAsync();

            return Ok("Producto creado correctamente");
        }
        #endregion

        #region Editar
        [HttpPut]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("editar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Editar(int id, ProductoDTO productoDTO)
        {
            var productoDB = await _context.Productos
                .Include(v => v.ProductoVenta)
                .Where(p => p.IDProducto == id).FirstOrDefaultAsync();

            if (productoDB == null)
            {
                return NotFound("Producto no encontrado.");
            }

            // Buscar la categoria por nombre
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == productoDTO.NombreCategoria.ToLower());

            if (categoria == null)
                return BadRequest("La categoría especificada no existe.");


            productoDB.NombreProducto = productoDTO.NombreProducto;
            productoDB.Descripcion = productoDTO.Descripcion;
            productoDB.Precio = productoDTO.Precio;
            productoDB.Imagen = productoDTO.Imagen;
            productoDB.IDCategoria = categoria.IDCategoria;

            if (productoDB.ProductoVenta != null)
            {
                productoDB.ProductoVenta.EstadoProducto = productoDTO.EstadoProducto;
                productoDB.ProductoVenta.EstadoVenta = productoDTO.EstadoVenta;
                productoDB.ProductoVenta.Cantidad = productoDTO.Cantidad;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Producto modificado");
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
        public async Task<ActionResult<ProductoDTO>> Eliminar(int id)
        {
            var productoDB = await _context.Productos.FindAsync(id);

            if (productoDB is null)
            {
                return NotFound("Producto no encontrado");
            }

            _context.Productos.Remove(productoDB);

            await _context.SaveChangesAsync();

            return Ok("Producto eliminado");
        }
        #endregion

        #region Lista de productos por usuario
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("ListaPorUsuario")]
        public async Task<ActionResult<List<ProductoDTO>>> GetListaPorUsuario(int id)
        {
            var listaDTO = new List<ProductoDTO>();
            var listaDB = await _context.Productos
                .Include(u => u.Usuario)
                  .ThenInclude(p => p.Persona)
                .Include(pv => pv.ProductoVenta)
                .Include(c => c.Categoria)
                .Where(u => u.IDUsuario == id)
                .ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new ProductoDTO
                {
                    IDProducto = item.IDProducto,
                    NombreProducto = item.NombreProducto,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio,
                    Imagen = item.Imagen,
                    IDUsuario = item.Usuario.IDUsuario,
                    Username = item.Usuario.Username,
                    Email = item.Usuario.Email,
                    NroCelular = item.Usuario.Persona.NroCelular,
                    NombreCategoria = item.Categoria.Nombre,
                    IDProductoVenta = item.ProductoVenta.IDProductoVenta,
                    Fecha = item.ProductoVenta.Fecha,
                    EstadoProducto = item.ProductoVenta.EstadoProducto,
                    EstadoVenta = item.ProductoVenta.EstadoVenta,
                    Cantidad = item.ProductoVenta.Cantidad,
                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Lista de productos por Categoria
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("ListaPorCategoria")]
        public async Task<ActionResult<List<ProductoDTO>>> GetListaPorCategoria(string categoria)
        {
            var listaDTO = new List<ProductoDTO>();
            var listaDB = await _context.Productos
                .Include(u => u.Usuario)
                 .ThenInclude(p => p.Persona)
                .Include(pv => pv.ProductoVenta)
                .Include(c => c.Categoria)
                .Where(u => u.Categoria.Nombre == categoria)
                .ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new ProductoDTO
                {
                    IDProducto = item.IDProducto,
                    NombreProducto = item.NombreProducto,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio,
                    Imagen = item.Imagen,
                    IDUsuario = item.Usuario.IDUsuario,
                    Username = item.Usuario.Username,
                    Email = item.Usuario.Email,
                    NroCelular = item.Usuario.Persona.NroCelular,
                    NombreCategoria = item.Categoria.Nombre,
                    IDProductoVenta = item.ProductoVenta.IDProductoVenta,
                    Fecha = item.ProductoVenta.Fecha,
                    EstadoProducto = item.ProductoVenta.EstadoProducto,
                    EstadoVenta = item.ProductoVenta.EstadoVenta,
                    Cantidad = item.ProductoVenta.Cantidad,
                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Lista productos por nombre
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("ListaPorNombre")]
        public async Task<ActionResult<List<ProductoDTO>>> BuscarPorNombre(string nombre)
        {
            var productos = await _context.Productos
                .Include(p => p.Usuario)
                    .ThenInclude(u => u.Persona)
                .Include(p => p.ProductoVenta)
                .Include(p => p.Categoria)
                .Where(p => p.NombreProducto.Contains(nombre))
                .ToListAsync();

            if (!productos.Any())
                return NotFound("No se encontraron productos con ese nombre.");

            var listaDTO = productos.Select(item => new ProductoDTO
            {
                IDProducto = item.IDProducto,
                NombreProducto = item.NombreProducto,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Imagen = item.Imagen,
                IDUsuario = item.Usuario.IDUsuario,
                Username = item.Usuario.Username,
                Email = item.Usuario.Email,
                NroCelular = item.Usuario.Persona.NroCelular,
                NombreCategoria = item.Categoria.Nombre,
                IDProductoVenta = item.ProductoVenta.IDProductoVenta,
                Fecha = item.ProductoVenta.Fecha,
                EstadoProducto = item.ProductoVenta.EstadoProducto,
                EstadoVenta = item.ProductoVenta.EstadoVenta,
                Cantidad = item.ProductoVenta.Cantidad
            }).ToList();

            return Ok(listaDTO);
        }

        #endregion

        #region Lista productos por ubicacion
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("ListaPorUbicacion")]
        public async Task<ActionResult<List<ProductoDTO>>> BuscarPorUbicacion(string provincia, string localidad)
        {
            var listaDTO = new List<ProductoDTO>();

            var listaDB = await _context.Productos
                .Include(p => p.Usuario)
                    .ThenInclude(u => u.Persona)
                        .ThenInclude(p => p.Ubicacion)
                .Include(pv => pv.ProductoVenta)
                .Include(c => c.Categoria)
                .Where(p => p.Usuario.Persona.Ubicacion.Provincia == provincia &&
                            p.Usuario.Persona.Ubicacion.Localidad == localidad)
                .ToListAsync();

            foreach (var item in listaDB)
            {
                listaDTO.Add(new ProductoDTO
                {
                    IDProducto = item.IDProducto,
                    NombreProducto = item.NombreProducto,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio,
                    Imagen = item.Imagen,
                    IDUsuario = item.Usuario.IDUsuario,
                    Username = item.Usuario.Username,
                    Email = item.Usuario.Email,
                    NroCelular = item.Usuario.Persona.NroCelular,
                    NombreCategoria = item.Categoria.Nombre,
                    IDProductoVenta = item.ProductoVenta.IDProductoVenta,
                    Fecha = item.ProductoVenta.Fecha,
                    EstadoProducto = item.ProductoVenta.EstadoProducto,
                    EstadoVenta = item.ProductoVenta.EstadoVenta,
                    Cantidad = item.ProductoVenta.Cantidad,
                });
            }

            return Ok(listaDTO);
        }
        #endregion


        /*

       */
    }
}
