using Festivo.Core.Repositorios;
using Festivo.Core.Servicios;
using Festivo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Festivo.Aplicacion.Servicios
{
    public class TipoServicio : ITipoServicio
    {
        private readonly ITipoRepositorio repositorio;

        public TipoServicio(ITipoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<Tipo> Agregar(Tipo festivo)
        {
            return await repositorio.Agregar(festivo);
        }

        public async Task<IEnumerable<Tipo>> Buscar(string Dato)
        {
            return (IEnumerable<Tipo>) await repositorio.Buscar(Dato);
        }

        public async Task<bool> Eliminar(int Id)
        {
            return await repositorio.Eliminar(Id);
        }

        public async Task<Tipo> Modificar(Tipo festivo)
        {
            return await repositorio.Modificar(festivo);
        }

        public async Task<Tipo> Obtener(int Id)
        {
            return await repositorio.Obtener(Id);
        }

        public async Task<IEnumerable<Tipo>> ObtenerTodos()
        {
            return await repositorio.ObtenerTodos();
        }
    }
}
