using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceProveedor.Models;
using ServiceProveedor.Data;

namespace ServiceProveedor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorApiController : ControllerBase
    {
        private readonly ProveedorContext dBConexion;
        private readonly List<string> ProductosValidos = new List<string>
        {
            "Tomate", "Pimiento", "Cebolla", "Papa", "Choclo", "Cherry", "Chaucha", "Zapallito", "Aji"
        };

        public ProveedorApiController(ProveedorContext dBConexion)
        {
            this.dBConexion = dBConexion;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores([FromQuery] string? estado = null)
        {
            IQueryable<Proveedor> query = dBConexion.proveedores;

            if (estado?.ToLower() == "activo")
            {
                query = query.Where(p => p.Estado);
            }
            else if (estado?.ToLower() == "inactivo")
            {
                query = query.Where(p => !p.Estado);
            }
            var proveedores = await query.ToListAsync();
            return Ok(proveedores);
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            if (proveedor.ProductosSuministrados == null)
            {
                proveedor.ProductosSuministrados = new List<string>();
            }
            var productosInvalidos = proveedor.ProductosSuministrados
                .Where(p => !ProductosValidos.Contains(p, StringComparer.OrdinalIgnoreCase))
                .ToList();
            if (productosInvalidos.Any())
            {
                return BadRequest($"Los siguientes productos no son válidos: {string.Join(", ", productosInvalidos)}");
            }
            dBConexion.proveedores.Add(proveedor);
            await dBConexion.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProveedor), new { nombre = proveedor.Nombre }, proveedor);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<Proveedor>> GetProveedor([FromQuery] string? nombre = null)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El parámetro 'nombre' es obligatorio.");
            }
            var proveedor = await dBConexion.proveedores.FirstOrDefaultAsync(p => p.Nombre!.ToLower() == nombre.ToLower());
            if (proveedor == null)
            {
                return NotFound("Proveedor no encontrado.");
            }
            return Ok(proveedor);
        }

        [HttpPut]
        public async Task<IActionResult> PutProveedor([FromQuery] string? nombre, [FromBody] Proveedor proveedorActualizado)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El parámetro 'nombre' es obligatorio para actualizar.");
            }
            var existingProveedor = await dBConexion.proveedores.FirstOrDefaultAsync(p => p.Nombre!.ToLower() == nombre.ToLower());
            if (existingProveedor == null)
            {
                return NotFound("Proveedor no encontrado.");
            }
            if (proveedorActualizado.ProductosSuministrados != null)
            {
                var productosInvalidos = proveedorActualizado.ProductosSuministrados
                    .Where(p => !ProductosValidos.Contains(p, StringComparer.OrdinalIgnoreCase))
                    .ToList();
                if (productosInvalidos.Any())
                {
                    return BadRequest($"Los siguientes productos no son válidos: {string.Join(", ", productosInvalidos)}");
                }
            }
            existingProveedor.Nombre = proveedorActualizado.Nombre;
            existingProveedor.Telefono = proveedorActualizado.Telefono;
            existingProveedor.Correo = proveedorActualizado.Correo;
            existingProveedor.Direccion = proveedorActualizado.Direccion;
            existingProveedor.Estado = proveedorActualizado.Estado;
            existingProveedor.ProductosSuministrados = proveedorActualizado.ProductosSuministrados ?? new List<string>();
            await dBConexion.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProveedor([FromQuery] string? nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El parámetro 'nombre' es obligatorio para eliminar.");
            }
            var proveedor = await dBConexion.proveedores.FirstOrDefaultAsync(p => p.Nombre!.ToLower() == nombre.ToLower());
            if (proveedor == null)
            {
                return NotFound("Proveedor no encontrado.");
            }
            proveedor.Estado = false;
            await dBConexion.SaveChangesAsync();
            return NoContent();
        }
    }
}