using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using _3101_proyecto1.Entities;

namespace _3101_proyecto1.Models
{
    public partial class PacienteViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = null!;

        [Required]
        [Display(Name = "Número de Teléfono")]
        [RegularExpression(@"[0-9]+$")]
        public string Telefono { get; set; } = null!;

        [Required]
        [Display(Name = "Nombre Completo")]
        [StringLength(100, MinimumLength = 5)]
        public string NombreCompleto { get; set; } = null!;
    }
}
