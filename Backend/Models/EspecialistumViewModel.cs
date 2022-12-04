using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Models
{
    public partial class EspecialistumViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre del Especialista")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        [Required]
        [Display(Name = "Precio de la Consulta")]
        [Column(TypeName = "money")]
        public decimal PrecioConsulta { get; set; }

        [Required]
        public int IdEspecialidad { get; set; }

        [Display(Name = "Especialidad")]
        public string NombreEspecialidad { get; set; }

        [NotMapped]
        public List<SelectListItem> ListaEspecialidad { get; set; }
    }
}
