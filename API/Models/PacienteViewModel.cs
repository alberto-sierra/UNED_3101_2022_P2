using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace _3101_proyecto1.Api.Models
{
    public partial class PacienteViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Identificacion { get; set; } = null!;

        [Required]
        [RegularExpression(@"[0-9]+$")]
        public string Telefono { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string NombreCompleto { get; set; } = null!;
    }
}
