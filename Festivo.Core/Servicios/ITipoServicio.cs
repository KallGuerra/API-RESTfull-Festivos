using Festivo.Dominio.Entidades;


namespace Festivo.Core.Servicios
{
    public interface ITipoServicio
    {
        Task<IEnumerable<Tipo>> ObtenerTodos();
        Task<Tipo> Obtener(int Id);
        Task<IEnumerable<Tipo>> Buscar(string Dato);
        Task<Tipo> Agregar(Tipo festivo);
        Task<Tipo> Modificar(Tipo festivo);
        Task<bool> Eliminar(int Id);
    }
}
