using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Entities
{
    public partial class Citum
    {
        [Key]
        public int Id { get; set; }
        public long IdPaciente { get; set; }
        public int IdReserva { get; set; }
        [Column(TypeName = "money")]
        public decimal PrecioConsulta { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }

        [ForeignKey("IdPaciente")]
        [InverseProperty("Cita")]
        public virtual Paciente IdPacienteNavigation { get; set; } = null!;
        [ForeignKey("IdReserva")]
        [InverseProperty("Cita")]
        public virtual ReservaConsultorio IdReservaNavigation { get; set; } = null!;
    }
}
