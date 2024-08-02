using CarpinteriaBackend.dominio;
using CarpinteriaBackend.Servicios;
using CarpinteriaBackend.Servicios.Implementacion;
using CarpinteriaBackend.Servicios.Interfaz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarpinteriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarpinteriaController : ControllerBase
    {
        private IServicio servicio;
        Presupuesto nuevo;

        public CarpinteriaController()
        {

            servicio = new Servicio();

            nuevo = new Presupuesto();
        }

        [HttpGet("/listarProductos")]
        public IActionResult GetAllProdctos()
        {
            try
            {
                var productos = servicio.ObtenerProductos();

                if (productos == null || !productos.Any())
                {
                    return NotFound("No se encontraron productos.");
                }

                return Ok(productos);
            }
            catch (Exception)
            {
                return BadRequest("No se pudo obtener los productos.");
            }
        }

        [HttpPost("/nuevoPresupuesto")]
        public IActionResult PostPresupuesto(Presupuesto nuevo)
        {
            if (nuevo == null)
            {
                return BadRequest("El presupuesto es nulo.");
            }

            if (nuevo.Detalles == null || !nuevo.Detalles.Any())
            {
                return BadRequest("El presupuesto debe tener al menos un detalle.");
            }

            try
            {
                bool resultado = servicio.CrearPresupuesto(nuevo);
                if (resultado)
                {
                    return Ok("Presupuesto creado con éxito.");
                }
                else
                {
                    return BadRequest("No se pudo crear el presupuesto.");
                }
            }
            catch (Exception ex)
            {
                // Log del error ex si es necesario
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("/{id}")]
        public IActionResult PostPresupuesto(int id)
        {
            Presupuesto pBorrar = servicio.ObtenerPresupuestoPorNro(id);

            if (pBorrar == null)
            {
                return NotFound("Presupuesto no encontrado.");
            }

            // Validar que el presupuesto no esté marcado para ser eliminado
            if (pBorrar.fechaBaja != null)
            {
                return BadRequest("El presupuesto ya ha sido dado de baja.");
            }

            // Borrar el presupuesto
            bool resultado = servicio.BorrarPresupuesto(id);

            if (resultado)
            {
                return Ok("Presupuesto borrado exitosamente.");
            }
            else
            {
                return BadRequest("No se pudo borrar el presupuesto.");
            }
        }

        [HttpPut("/actualizarPresupuesto")]
        public IActionResult PutPresupuesto(int id,Presupuesto nuevo)
        {

            Presupuesto pActualizar = servicio.ObtenerPresupuestoPorNro(id);

            if (pActualizar == null)
            {
                return NotFound("Presupuesto no encontrado.");
            }




            // Actualizar los campos del presupuesto existente con los datos del nuevo
            pActualizar.PresupuestoNro = id;
            pActualizar.Fecha = nuevo.Fecha;
            pActualizar.Cliente = nuevo.Cliente;
            pActualizar.Descuento = nuevo.Descuento;
            pActualizar.CostoMO = nuevo.CostoMO; // Asegúrate de recalcular el total
            pActualizar.fechaBaja = nuevo.fechaBaja;

            pActualizar.Detalles.Clear();
            foreach (DetallePresupuesto detalle in nuevo.Detalles)
            {
                pActualizar.Detalles.Add(detalle);
            }

            // Llamar al servicio para actualizar en la base de datos
            bool resultado = servicio.ActualizarPresupuesto(pActualizar);

            if (resultado)
            {
                return Ok("Presupuesto actualizado con éxito.");
            }
            else
            {
                return StatusCode(500, "Error al actualizar el presupuesto.");
            }
        }
    }
}
