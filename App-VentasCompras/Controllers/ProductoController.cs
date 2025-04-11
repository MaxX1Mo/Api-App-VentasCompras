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
        [Route("lista")]
        public async Task<ActionResult<List<ProductoDTO>>> Get()
        {
            var listaDTO = new List<ProductoDTO>();
            var listaDB = await _context.Productos
                .Include(u => u.Usuario)
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
        [Route("buscar/{id}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            var productoDTO = new ProductoDTO();
            var productoDB = await _context.Productos
                .Include(u => u.Usuario)
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
            productoDTO.Username = productoDB.Usuario.Username;
            productoDTO.Email = productoDB.Usuario.Email;
            productoDTO.NroCelular = productoDB.Usuario.Persona.NroCelular;
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
        [Authorize(Roles = "Admin,Empleado")]
        [Route("crear")]
        public async Task<ActionResult<ProductoDTO>> Crear(ProductoDTO productoDTO)
        {

            var productoVenta = new ProductoVenta
            {
                Fecha = DateTime.Now,
                EstadoProducto = productoDTO.EstadoProducto,
                EstadoVenta = productoDTO.EstadoVenta,
                Cantidad = productoDTO.Cantidad,
                IDProducto = productoDTO.IDProducto
            };
            var productoDB = new Producto
            {
                NombreProducto = productoDTO.NombreProducto,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Imagen = productoDTO.Imagen,
                IDUsuario = productoDTO.IDUsuario,
                IDCategoria = productoDTO.IDCategoria
            };
            await _context.Productos.AddAsync(productoDB);
            await _context.SaveChangesAsync();
            return Ok("Producto Creado");
        }
        #endregion
        /*
       */
    }
}
