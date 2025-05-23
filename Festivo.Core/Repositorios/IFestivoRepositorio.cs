﻿using Festivo.Dominio.Entidades;
using Festivo.Dominio.Dto;

namespace Festivo.Core.Repositorios
{
    public interface IFestivoRepositorio
    {
        Task<IEnumerable<Feestivo>> ObtenerTodos();

        Task<Feestivo> Obtener(int Id);

        Task<IEnumerable<Feestivo>> Buscar(int tipo, string dato);

        Task<Feestivo> Agregar(Feestivo festivo);

        Task<Feestivo> Modificar(Feestivo festivo);

        Task<bool> Eliminar(int Id);
        Task<IEnumerable<Feestivo>> ObtenerTodosConTipoAsync();
    }
}
