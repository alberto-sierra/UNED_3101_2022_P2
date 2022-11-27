using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _3101_proyecto1.Entities
{
    [Table("Paciente")]
    [Index("Identificacion", Name = "UQ__Paciente__D6F931E57485B694", IsUnique = true)]
    public partial class Paciente
    {
        public Paciente()
        {
            Cita = new HashSet<Citum>();
        }

        [Key]
        public long Id { get; set; }
        [StringLength(9)]
        [Unicode(false)]
        public string Identificacion { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string Telefono { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string NombreCompleto { get; set; } = null!;

        [InverseProperty("IdPacienteNavigation")]
        public virtual ICollection<Citum> Cita { get; set; }
    }
}
