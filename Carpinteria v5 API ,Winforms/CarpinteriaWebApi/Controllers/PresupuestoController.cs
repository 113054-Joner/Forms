using CarpinteriaFront.Entidades;
using CarpinteriaFront.Servicios.Factory;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarpinteriaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresupuestoController : ControllerBase
    {
        private IPresupuestoServicio servicio;

        public PresupuestoController()
        {
            servicio = new ServicerFactory().CrearServicio();
        }

        // GET: api/<PresupuestoController>
        [HttpGet("presupuestos")]
        public IActionResult GetPresupuesto()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista = servicio.ListarProductos();
                return Ok(lista);
            }
            catch (Exception)
            {

                return StatusCode(500, "Error");
            }
        }

        // GET api/<PresupuestoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PresupuestoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PresupuestoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PresupuestoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
