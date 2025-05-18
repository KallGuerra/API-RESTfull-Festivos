using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festivo.Dominio.Entidades
{
    [Table("Festivo")]
    public class Feestivo
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Dia")]
        public int Dia { get; set; }

        [Column("Mes")]
        public int Mes { get; set; }

        [Column("Nombre"), StringLength(100)]
        public string Nombre { get; set; } = null;

        [Column("DiasPascua")]
        public int? DiasPascua { get; set; }


        [ForeignKey("IdTipo")]
        public int IdTipo { get; set; }
        public Tipo Tipo { get; set; } 

        
    }
}
