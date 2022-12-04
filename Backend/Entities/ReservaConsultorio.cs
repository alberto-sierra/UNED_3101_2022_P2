using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    [Table("ReservaConsultorio")]
    public partial class ReservaConsultorio
    {
        public ReservaConsultorio()
        {
            Cita = new HashSet<Citum>();
        }

        [Key]
        public int Id { get; set; }
        public int IdEspecialista { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public byte DiaSemana { get; set; }
        public bool Disponible { get; set; }
        public int IdEquipo { get; set; }
        public int IdConsultorio { get; set; }

        [ForeignKey("IdConsultorio")]
        [InverseProperty("ReservaConsultorios")]
        public virtual Consultorio IdConsultorioNavigation { get; set; } = null!;
        [ForeignKey("IdEquipo")]
        [InverseProperty("ReservaConsultorios")]
        public virtual Equipo IdEquipoNavigation { get; set; } = null!;
        [ForeignKey("IdEspecialista")]
        [InverseProperty("ReservaConsultorios")]
        public virtual Especialistum IdEspecialistaNavigation { get; set; } = null!;
        [InverseProperty("IdReservaNavigation")]
        public virtual ICollection<Citum> Cita { get; set; }
    }
}
