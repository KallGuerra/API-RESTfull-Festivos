
using Festivo.Core.Repositorios;
using Festivo.Core.Servicios;
using Festivo.Dominio.Dto;
using Festivo.Dominio.Entidades;
using System.Security.AccessControl;

namespace Festivo.Aplicacion.Servicios
{
    public class FestivoServicio : IFestivoServicio
    {
        private readonly IFestivoRepositorio repositorio;

        public FestivoServicio(IFestivoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        
        private DateTime CalcularDomingoPascua(int ano)
        {
            // Algoritmo de Butcher/Meeus/Jones para el domingo de Pascua Gregoriano
            int a = ano % 19;
            int b = ano / 100;
            int c = ano % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int mes = (h + l - 7 * m + 114) / 31; // 3 para Marzo, 4 para Abril
            int dia = ((h + l - 7 * m + 114) % 31) + 1;
            return new DateTime(ano, mes, dia);
        }


        private DateTime TrasladarALunes(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Monday)
            {
                return fecha;
            }
            int diasParaLunes = ((int)DayOfWeek.Monday - (int)fecha.DayOfWeek + 7) % 7;
            return fecha.AddDays(diasParaLunes);
        }


        public async Task<IEnumerable<FestivoCalculadoDto>> ObtenerFestivosPorAno(int ano)
        {
            // 1. Pedir al repositorio todos los festivos "base" (como están en la DB)
            //    Es importante que .ObtenerTodosConTipoAsync() también traiga la info del "Tipo" de festivo.
            var festivosDesdeDb = await repositorio.ObtenerTodosConTipoAsync();

            if (festivosDesdeDb == null || !festivosDesdeDb.Any())
            {
                return Enumerable.Empty<FestivoCalculadoDto>(); // Si no hay datos, devuelve lista vacía
            }

            var festivosCalculados = new List<FestivoCalculadoDto>(); // Lista para guardar los resultados
            DateTime domingoPascua = CalcularDomingoPascua(ano); // Calcular Pascua una vez para el año

            foreach (var festivoDb in festivosDesdeDb) // Procesar cada festivo de la DB
            {
                DateTime? fechaFestivoActual = null; // Usamos DateTime? porque podría no calcularse una fecha
                string nombreFestivo = festivoDb.Nombre;

                if (festivoDb.Tipo == null) // Seguridad: verificar que el Tipo se cargó
                {
                    Console.WriteLine($"Advertencia: Festivo '{nombreFestivo}' (ID: {festivoDb.Id}) no tiene Tipo cargado. IdTipo: {festivoDb.IdTipo}. Se omite.");
                    continue; // Saltar al siguiente festivo
                }
                int tipoId = festivoDb.IdTipo; // Usamos el IdTipo para el switch

                try // Intentar calcular la fecha, puede haber errores (ej. 29 feb en año no bisiesto)
                {
                    switch (tipoId)
                    {
                        case 1: // Fijo (ej. Año Nuevo)
                            fechaFestivoActual = new DateTime(ano, festivoDb.Mes, festivoDb.Dia);
                            break;
                        case 2: // Fijo que se traslada a lunes (ej. Santos Reyes)
                            DateTime fechaBaseTipo2 = new DateTime(ano, festivoDb.Mes, festivoDb.Dia);
                            fechaFestivoActual = TrasladarALunes(fechaBaseTipo2);
                            break;
                        case 3: // Basado en Pascua (ej. Jueves Santo)
                            if (festivoDb.DiasPascua.HasValue) // DiasPascua es int? (nullable)
                            {
                                fechaFestivoActual = domingoPascua.AddDays(festivoDb.DiasPascua.Value);
                            }
                            break;
                        case 4: // Basado en Pascua y se traslada a lunes (ej. Ascensión del Señor)
                            if (festivoDb.DiasPascua.HasValue)
                            {
                                DateTime fechaBaseTipo4 = domingoPascua.AddDays(festivoDb.DiasPascua.Value);
                                fechaFestivoActual = TrasladarALunes(fechaBaseTipo4);
                            }
                            break;
                        default: // Tipo desconocido
                            Console.WriteLine($"Advertencia: Tipo de festivo desconocido (IdTipo: {tipoId}) para '{nombreFestivo}'. Se omite.");
                            break;
                    }

                    if (fechaFestivoActual.HasValue) // Si se pudo calcular una fecha
                    {
                        festivosCalculados.Add(new FestivoCalculadoDto
                        {
                            Nombre = nombreFestivo,
                            Fecha = fechaFestivoActual.Value
                        });
                    }
                }
                catch (ArgumentOutOfRangeException ex) // Error común: fecha inválida como 30 de febrero
                {
                    Console.WriteLine($"Error de fecha para '{nombreFestivo}' (Tipo: {tipoId}, Año: {ano}): {ex.Message}. Se omite.");
                }
                catch (Exception ex) // Otro error inesperado
                {
                    Console.WriteLine($"Error procesando '{nombreFestivo}': {ex.Message}. Se omite.");
                }
            }
            // Devolver la lista de festivos calculados, ordenados por fecha
            return festivosCalculados.OrderBy(f => f.Fecha).ToList();
        }

        public async Task<bool> EsFestivo(int dia, int mes, int ano)
        {
            DateTime fechaAVerificar;
            try
            {
                fechaAVerificar = new DateTime(ano, mes, dia); // Construir la fecha a verificar
            }
            catch (ArgumentOutOfRangeException)
            {
                return false; // Fecha inválida (ej. 30/02) no es festivo
            }

            // Obtener todos los festivos calculados para ese año
            var festivosDelAno = await ObtenerFestivosPorAno(ano);
            // Verificar si alguna fecha festiva coincide con la fecha a verificar
            return festivosDelAno.Any(f => f.Fecha.Date == fechaAVerificar.Date);
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
