using Festivo.Core.Servicios;
using Festivo.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Festivo.Presentacion.Controlador
{
    [ApiController]
    [Route("api/Tipo")]
    public class TipoController : ControllerBase
    {
        private readonly ITipoServicio servicio;

        public TipoController(ITipoServicio servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet("listar")]
        public async Task<IEnumerable<Tipo>> ObtenerTodos()
        {
            return await servicio.ObtenerTodos();
        }

        [HttpGet("obtener/{Id}")]
        public async Task<Tipo> Obtener(int Id)
        {
            return await servicio.Obtener(Id);
        }

        [HttpGet("buscar/{dato}")]
        public async Task<ActionResult<IEnumerable<Tipo>>> Buscar(string dato)
        {
            var resultado = await servicio.Buscar(dato);
            if (!resultado.Any())
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost("agregar")]
        public async Task<Tipo> Agregar([FromBody] Tipo seleccion)
        {
            return await servicio.Agregar(seleccion);
        }

        [HttpPut("modificar")]
        public async Task<Tipo> Modificar([FromBody] Tipo seleccion)
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
