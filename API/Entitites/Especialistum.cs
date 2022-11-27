using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Api.Entities
{
    [Index("Identificacion", Name = "UQ__Especial__D6F931E5DC511E7C", IsUnique = true)]
    public partial class Especialistum
    {
        public Especialistum()
        {
            ReservaConsultorios = new HashSet<ReservaConsultorio>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [StringLength(9)]
        [Unicode(false)]
        public string Identificacion { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal PrecioConsulta { get; set; }
        public int IdEspecialidad { get; set; }

        [ForeignKey("IdEspecialidad")]
        [InverseProperty("Especialista")]
        public virtual Especialidad IdEspecialidadNavigation { get; set; } = null!;
        [InverseProperty("IdEspecialistaNavigation")]
        public virtual ICollection<ReservaConsultorio> ReservaConsultorios { get; set; }
    }
}
