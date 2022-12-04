using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
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

        [StringLength(100)]
        [Display(Name = "Numero de Activo")]
        public string? Activo { get; set; }

        [StringLength(100)]
        [Display(Name = "Numero de Serie")]
        public string? Serie { get; set; }

        [StringLength(100)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Fecha de Compra")]
        [DataType(DataType.Date)]
        public DateTime? FechaCompra { get; set; }

        [Display(Name = "Especialidad")]
        public string NombreEspecialidad { get; set; }

        [NotMapped]
        public List<SelectListItem> ListaEspecialidad { get; set; }
    }
}
