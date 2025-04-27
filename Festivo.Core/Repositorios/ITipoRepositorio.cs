using Festivo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festivo.Core.Repositorios
{
    public interface ITipoRepositorio
    {
        Task<IEnumerable<Tipo>> ObtenerTodos();

        Task<Tipo> Obtener(int Id);

        Task<IEnumerable<Tipo>> Buscar(string dato);

        Task<Tipo> Agregar(Tipo festivo);

        Task<Tipo> Modificar(Tipo festivo);

        Task<bool> Eliminar(int Id);
    }
}
