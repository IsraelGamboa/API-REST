using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_REST.Context;
using API_REST.Models;

namespace API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ClassAppDbContext _context;

        public LibrosController(ClassAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Libros  Obtener todos los libros registrados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libros>>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

        // GET: api/Libros/5  Obtener libro por ID aplicacion para comprar
     /* [HttpGet("{id}")]
        public async Task<ActionResult<Libros>> GetLibros(int id)
        {
            var libros = await _context.Libros.FindAsync(id);

            if (libros == null)
            {
                return NotFound();
            }

            return libros;
        } */

        // POST: api/Libros Registrar Libros a la BD
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Libros>> PostLibros(Libros libros)
        {
            _context.Libros.Add(libros);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibros", new { id = libros.Id }, libros);
        }

        // DELETE: api/Libros/5  Eliminar libro de la BD
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibros(int id)
        {
            var libros = await _context.Libros.FindAsync(id);
            if (libros == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libros);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método Comprar Libro
        [HttpPost("{id}/comprar")]
        public async Task<ActionResult<object>> ComprarLibro(int id, [FromBody] int cantidad)
        {
            // Buscar el libro por ID
            var libro = await _context.Libros.FindAsync(id);

            // Verificar si el libro existe
            if (libro == null)
            {
                return NotFound(new { Message = "Libro no encontrado" });
            }

            // Verificar si la cantidad es válida
            if (cantidad <= 0)
            {
                return BadRequest(new { Message = "La cantidad debe ser mayor a 0" });
            }

            // Calcular el subtotal
            var subtotal = libro.Price * cantidad; // Suponiendo que 'Precio' es una propiedad de 'Libros'

            // Calcular el IVA (16.5%)
            var iva = subtotal * 0.16;

            // Calcular el total
            var total = subtotal + iva;

            // Devolver los datos del libro, la cantidad, el subtotal, el IVA y el total
            var resultado = new
            {
                Libro = libro,
                Cantidad = cantidad,
                Subtotal = subtotal,
                IVA = iva,
                Total = total
            };

            return Ok(resultado);
        }



        private bool LibrosExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
