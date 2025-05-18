using Festivo.Core.Repositorios;
using Festivo.Dominio.Dto;
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
            var nuevaEntidad = new Feestivo
            {
                Dia = festivo.Dia,
                Mes = festivo.Mes,
                Nombre = festivo.Nombre,
                DiasPascua = festivo.DiasPascua,
                IdTipo = festivo.IdTipo
            };

            context.festivos.Add(nuevaEntidad);
            await context.SaveChangesAsync();

            festivo.Id = nuevaEntidad.Id; // Asignar el ID generado por la base de datos
            return festivo;
        }

        public async Task<IEnumerable<Feestivo>> Buscar(int tipo, string dato)
        {
            dato = dato.ToLower();

            var resultado = await context.festivos
                .Include(f => f.Tipo) // Asegurarse de incluir la relación con Tipo
                .Where(f =>
                    (tipo == 0 && f.Nombre.ToLower().Contains(dato)) ||  // Buscar por Nombre
                    (tipo == 1 && f.Tipo.Nombre.ToLower().Contains(dato)) || // Buscar por Tipo
                    (tipo == 2 && f.Mes.ToString() == dato) ||             // Buscar por Mes (en número)
                    (tipo == 3 && f.Dia.ToString() == dato))               // Buscar por Día
                .Select(f => new Feestivo
                {
                    Id = f.Id,
                    Dia = f.Dia,
                    Mes = f.Mes,
                    Nombre = f.Nombre,
                    DiasPascua = (int)f.DiasPascua,
                    IdTipo = f.IdTipo
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var festivoExistente = await context.festivos.FindAsync(Id);
            if (festivoExistente == null)
            {
                return false; // Retornar false si no se encuentra el festivo
            }

            try
            {
                context.festivos.Remove(festivoExistente);
                await context.SaveChangesAsync();
                return true; // Retornar true si la eliminación fue exitosa
            }
            catch
            {
                return false; // Retornar false si ocurre algún error
            }
        }

        public async Task<Feestivo> Modificar(Feestivo festivo)
        {
            var festivoExistente = await context.festivos.FindAsync(festivo.Id);
            if (festivoExistente == null)
            {
                return null;
            }

            festivoExistente.Dia = festivo.Dia;
            festivoExistente.Mes = festivo.Mes;
            festivoExistente.Nombre = festivo.Nombre;
            festivoExistente.DiasPascua = festivo.DiasPascua;
            festivoExistente.IdTipo = festivo.IdTipo;

            await context.SaveChangesAsync();

            return festivo;
        }

        public async Task<Feestivo> Obtener(int Id)
        {
            var festivo = await context.festivos.FindAsync(Id);
            if (festivo == null)
            {
                return null;
            }
            return new Feestivo
            {
                Id = festivo.Id,
                Dia = festivo.Dia,
                Mes = festivo.Mes,
                Nombre = festivo.Nombre,
                DiasPascua = (int)festivo.DiasPascua,
                IdTipo = festivo.IdTipo
            };
        }
        

        public async Task<IEnumerable<Feestivo>> ObtenerTodos()
        {
            return await context.festivos
                .Select(f => new Feestivo
                {
                    Id = f.Id,
                    Dia = f.Dia,
                    Mes = f.Mes,
                    Nombre = f.Nombre,
                    DiasPascua = (int)f.DiasPascua,
                    IdTipo = f.IdTipo
                })
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Feestivo>> ObtenerTodosConTipoAsync()
        {
            return await context.Set<Feestivo>() // Otra forma de acceder al DbSet
                                 .Include(f => f.Tipo)
                                 .ToListAsync();
        }
    }
}
