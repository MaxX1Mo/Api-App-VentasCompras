using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App_VentasCompras.Data;
using App_VentasCompras.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using App_VentasCompras.DTOs;

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
        [Authorize(Roles = "Admin,Usuario")]
        //[AllowAnonymous]
        [Route("lista")]
        public async Task<ActionResult<List<CategoriaDTO>>> Get()
        {
            var listaDTO = new List<CategoriaDTO>();
            var listaDB = await _context.Categorias.ToListAsync();


            foreach (var item in listaDB)
            {
                listaDTO.Add(new CategoriaDTO
                {
                    IDCategoria = item.IDCategoria,
                    Nombre = item.Nombre,
                });
            }
            return Ok(listaDTO);
        }
        #endregion

        #region Buscar por ID
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        [Route("buscar/{id}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoriaDTO = new CategoriaDTO();
            var categoriaDB = await _context.Categorias.Where(c => c.IDCategoria == id).FirstOrDefaultAsync();
            if (categoriaDB == null)
            {
                return NotFound();
            }
            categoriaDTO.IDCategoria = id;
            categoriaDTO.Nombre = categoriaDB.Nombre;

            return Ok(categoriaDTO);
        }
        #endregion


        #region Crear
        [HttpPost]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("crear")]
        public async Task<ActionResult<CategoriaDTO>> Crear(CategoriaDTO categoriaDTO)
        {
            if (string.IsNullOrWhiteSpace(categoriaDTO.Nombre))
            {
                return BadRequest("El nombre de la categoría es obligatorio.");
            }

            var categoriaExistente = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == categoriaDTO.Nombre.ToLower());

            if (categoriaExistente != null)
            {
                return BadRequest("Ya existe una categoría con ese nombre.");
            }

            var categoriaDB = new Categoria
            {
                Nombre = categoriaDTO.Nombre
            };
            await _context.Categorias.AddAsync(categoriaDB);
            await _context.SaveChangesAsync();

            return Ok("Categoría creada exitosamente.");

        }
        #endregion

        #region Editar
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("editar/{id}")]
        public async Task<ActionResult<CategoriaDTO>> Editar(int id, CategoriaDTO categoriaDTO)
        {

            var categoriaDB = await _context.Categorias
                .Where(c => c.IDCategoria == id).FirstOrDefaultAsync();

            if (categoriaDB == null)
            {
                return NotFound("Categoria no encontrada.");
            }

            categoriaDB.Nombre = categoriaDTO.Nombre;

            await _context.SaveChangesAsync();
            return Ok("Categoria modificada");
        }
        #endregion

        #region Eliminar
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("eliminar/{id}")]
        public async Task<ActionResult<CategoriaDTO>> Eliminar(int id)
        {
            var categoriaDB = await _context.Categorias.FindAsync(id);

            if (categoriaDB is null)
            {
                return NotFound("Categoria no encontrada");
            }

            _context.Categorias.Remove(categoriaDB);

            await _context.SaveChangesAsync();

            return Ok("Categoria eliminada");
        }
        #endregion


    }
}
