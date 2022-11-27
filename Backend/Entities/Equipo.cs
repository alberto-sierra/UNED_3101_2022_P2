using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Entities
{
    [Table("Equipo")]
    public partial class Equipo
    {
        public Equipo()
        {
            ReservaConsultorios = new HashSet<ReservaConsultorio>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        public int IdEspecialidad { get; set; }

        [ForeignKey("IdEspecialidad")]
        [InverseProperty("Equipos")]
        public virtual Especialidad IdEspecialidadNavigation { get; set; } = null!;
        [InverseProperty("IdEquipoNavigation")]
        public virtual ICollection<ReservaConsultorio> ReservaConsultorios { get; set; }
    }
}
