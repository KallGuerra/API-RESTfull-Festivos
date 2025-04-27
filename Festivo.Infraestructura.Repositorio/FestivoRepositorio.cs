using Festivo.Core.Repositorios;
using Festivo.Dominio.Entidades;
using Festivo.Infraestructura.Persistencia;
using Festivo.Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Festivo.Infraestructura.Repositorio
{
    public class FestivoRepositorio : IFestivoRepositorio
    {
        private readonly FestivoContext context;

        public FestivoRepositorio(FestivoContext context)
        {
            this.context = context;
        }

        public async Task<Feestivo> Agregar(Feestivo festivo)
        {
            context.festivos.Add(festivo);
            await context.SaveChangesAsync();
            return festivo;
        }

        public async Task<IEnumerable<Feestivo>> Buscar(int tipo, string dato)
        {
            dato = dato.ToLower();

            return await context.festivos
                .Include(f => f.Tipo) // Importante: incluir el Tipo
                .Where(f =>
                    (tipo == 0 && f.Nombre.ToLower().Contains(dato)) ||  // Buscar por Nombre
                    (tipo == 1 && f.Tipo.Nombre.ToLower().Contains(dato)) || // Buscar por Tipo
                    (tipo == 2 && f.Mes.ToString() == dato) ||             // Buscar por Mes (en número)
                    (tipo == 3 && f.Dia.ToString() == dato))               // Buscar por Día
                .ToArrayAsync();
        }

        public async Task<bool> Eliminar(int Id)
        {
            var FestivoExistente = await context.festivos.FindAsync(Id);
            if (FestivoExistente == null)
            {
                return false;
            }
            try
            {
                context.festivos.Remove(FestivoExistente);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public async Task<Feestivo> Modificar(Feestivo festivo)
        {
            var FestivoExistente = await context.festivos.FindAsync(festivo.Id);
            if (FestivoExistente == null)
            {
                return null;
            }
            context.Entry(FestivoExistente).CurrentValues.SetValues(festivo);
            return await context.festivos.FindAsync(festivo.Id);
        }


        public async Task<Feestivo> Obtener(int Id)
        {
            return await context.festivos.FindAsync(Id);
        }

        public async Task<IEnumerable<Feestivo>> ObtenerTodos()
        {
            return await context.festivos.ToArrayAsync();
        }
    }
}
