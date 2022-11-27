using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Api.Entities
{
    [Table("Especialidad")]
    public partial class Especialidad
    {
        public Especialidad()
        {
            Equipos = new HashSet<Equipo>();
            Especialista = new HashSet<Especialistum>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;

        [InverseProperty("IdEspecialidadNavigation")]
        public virtual ICollection<Equipo> Equipos { get; set; }
        [InverseProperty("IdEspecialidadNavigation")]
        public virtual ICollection<Especialistum> Especialista { get; set; }
    }
}
