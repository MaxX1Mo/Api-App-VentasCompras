using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_VentasCompras.Data;
using App_VentasCompras.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace App_VentasCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CategoriaController(AppDBContext context)
        {
            _context = context;
        }


        #region Lista
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }
        #endregion

        #region Buscar por ID
        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
        #endregion


        #region Crear
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("crear")]
        public async Task<ActionResult<Categoria>> Crear(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return BadRequest("El nombre de la categoría es obligatorio.");
            }

            var categoriaExistente = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == categoria.Nombre.ToLower());

            if (categoriaExistente != null)
            {
                return BadRequest("Ya existe una categoría con ese nombre.");
            }

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return Ok("Categoría creada exitosamente.");

        }
        #endregion

        #region Editar
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("editar/{id}")]
        public async Task<IActionResult> Editar(int id, Categoria categoria)
        {
            if (id != categoria.IDCategoria)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.IDCategoria == id);
        }

    }
}
