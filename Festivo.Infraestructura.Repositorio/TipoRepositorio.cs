using Festivo.Core.Repositorios;
using Festivo.Infraestructura.Persistencia;
using Festivo.Dominio.Entidades;
using Festivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Festivo.Infraestructura.Repositorio
{
    public class TipoRepositorio : ITipoRepositorio
    {
        private readonly FestivoContext context;

        public TipoRepositorio(FestivoContext context)
        {
            this.context = context;
        }

        public async Task<Tipo> Agregar(Tipo festivo)
        {
            context.tipos.Add(festivo);
            await context.SaveChangesAsync();
            return festivo;
        }

        public async Task<IEnumerable<Tipo>> Buscar(string dato)
        {
            dato = dato.ToLower();

            return await context.tipos
                .Where(item => item.Nombre.ToLower().Contains(dato))
                .ToArrayAsync();
        }

        public async Task<bool> Eliminar(int Id)
        {
            var FestivoExistente = await context.tipos.FindAsync(Id);
            if (FestivoExistente == null)
            {
                return false;
            }
            try
            {
                context.tipos.Remove(FestivoExistente);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Tipo> Modificar(Tipo festivo)
        {
            var TipoExistente = await context.tipos.FindAsync(festivo.Id);
            if (TipoExistente == null)
            {
                return null;
            }
            context.Entry(TipoExistente).CurrentValues.SetValues(festivo);
            return await context.tipos.FindAsync(festivo.Id);
        }

        public async Task<Tipo> Obtener(int Id)
        {
            return await context.tipos.FindAsync();
        }

        public async Task<IEnumerable<Tipo>> ObtenerTodos()
        {
            return await context.tipos.ToArrayAsync();
        }
    }
}
