using Festivo.Core.Repositorios;
using Festivo.Core.Servicios;
using Festivo.Dominio.Entidades;

namespace Festivo.Aplicacion.Servicios
{
    public class FestivoServicio : IFestivoServicio
    {
        private readonly IFestivoRepositorio repositorio;

        public FestivoServicio(IFestivoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<Feestivo> Agregar(Feestivo festivo)
        {
            return await repositorio.Agregar(festivo);
        }

        public async Task<IEnumerable<Feestivo>> Buscar(int tipo, string dato)
        {
            return (IEnumerable<Feestivo>)await repositorio.Buscar(tipo, dato);
        }

        public async Task<bool> Eliminar(int Id)
        {
            return await repositorio.Eliminar(Id);
        }

        public async Task<Feestivo> Modificar(Feestivo festivo)
        {
            return await repositorio.Modificar(festivo);
        }

        public async Task<Feestivo> Obtener(int Id)
        {
            return await repositorio.Obtener(Id);
        }

        public async Task<IEnumerable<Feestivo>> ObtenerTodos()
        {
            return await repositorio.ObtenerTodos();
        }
    }
}
