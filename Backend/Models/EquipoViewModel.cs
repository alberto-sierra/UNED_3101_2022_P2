using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.Models
{
    public partial class EquipoViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre del Equipo")]
        public string Nombre { get; set; } = null!;

        [Required]
        public int IdEspecialidad { get; set; }

        [Display(Name = "Especialidad")]
        public string NombreEspecialidad { get; set; }

        [NotMapped]
        public List<SelectListItem> ListaEspecialidad { get; set; }
    }
}
