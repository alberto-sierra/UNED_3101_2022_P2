using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace _3101_proyecto1.Models
{
    public partial class EspecialidadViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre de la Especialidad")]
        public string Nombre { get; set; } = null!;
    }
}
