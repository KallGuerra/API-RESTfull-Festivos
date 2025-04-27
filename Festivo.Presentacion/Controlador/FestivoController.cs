using Festivo.Core.Servicios;
using Festivo.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Festivo.Presentacion.Controlador
{
    [ApiController]
    [Route("api/festivos")]
    public class FestivoController : ControllerBase
    {
        private readonly IFestivoServicio servicio;

        public FestivoController(IFestivoServicio servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet("listar")]
        public async Task<IEnumerable<Feestivo>> ObtenerTodos()
        {
            return await servicio.ObtenerTodos();
        }

        [HttpGet("obtener/{Id}")]
        public async Task<Feestivo> Obtener(int Id)
        {
            return await servicio.Obtener(Id);
        }

        [HttpGet("buscar/{tipo}/{dato}")]
        public async Task<ActionResult<IEnumerable<Feestivo>>> Buscar(int tipo, string dato)
        {
            var resultado = await servicio.Buscar(tipo, dato);
            if (!resultado.Any())
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost("agregar")]
        public async Task<Feestivo> Agregar([FromBody] Feestivo seleccion)
        {
            return await servicio.Agregar(seleccion);
        }

        [HttpPut("modificar")]
        public async Task<Feestivo> Modificar([FromBody] Feestivo seleccion)
        {
            return await servicio.Modificar(seleccion);
        }

        [HttpDelete("eliminar/{Id}")]
        public async Task<bool> Eliminar(int Id)
        {
            return await servicio.Eliminar(Id);
        }
    }
}
