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

        public CarpinteriaController()
        {

            servicio = new Servicio();

            Presupuesto nuevo = new Presupuesto();
        }

        [HttpGet("/listarProductos")]
        public IActionResult GetAllProdctos()
        {
            try
            {
                return Ok(servicio.ObtenerProductos());
            }
            catch (Exception)
            {

                return BadRequest("No se encontro los productos");
            }
        }

        [HttpPost("/nuevoPresupuesto")]
        public IActionResult PostPresupuesto(Presupuesto nuevo)
        {
            try
            {

                return Ok(servicio.CrearPresupuesto(nuevo));
            }
            catch (Exception)
            {

                return BadRequest("No se pudo crear el presupuesto.");
            }
        }

        [HttpDelete("/{id}")]
        public IActionResult PostPresupuesto(int id)
        {
            try
            {
                return Ok(servicio.BorrarPresupuesto(id));
            }
            catch (Exception)
            {

                return BadRequest("No se pudo borrar.");
            }
        }
    }
}
