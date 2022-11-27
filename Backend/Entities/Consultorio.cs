using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Entities
{
    [Table("Consultorio")]
    public partial class Consultorio
    {
        public Consultorio()
        {
            ReservaConsultorios = new HashSet<ReservaConsultorio>();
        }

        [Key]
        public int Id { get; set; }
        public int Numero { get; set; }

        [InverseProperty("IdConsultorioNavigation")]
        public virtual ICollection<ReservaConsultorio> ReservaConsultorios { get; set; }
    }
}
