
using Festivo.Dominio.Dto;
using Festivo.Dominio.Entidades;



namespace Festivo.Core.Servicios
{
    public interface IFestivoServicio
    {
        Task<IEnumerable<Feestivo>> ObtenerTodos();
        Task<Feestivo> Obtener(int Id);
        Task<IEnumerable<Feestivo>> Buscar(int tipo, string dato);
        Task<Feestivo> Agregar(Feestivo festivo);
        Task<Feestivo> Modificar(Feestivo festivo);
        Task<bool> Eliminar(int Id);
        Task<IEnumerable<FestivoCalculadoDto>> ObtenerFestivosPorAno(int ano);
        Task<bool> EsFestivo(int dia, int mes, int ano);
    }
}
