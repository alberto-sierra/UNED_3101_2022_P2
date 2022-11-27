using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.FrontEnd.Models
{
    public partial class CitumViewModel
    {
        public int Id { get; set; }

        [Required]
        public long IdPaciente { get; set; }

        [Required]
        public int IdReserva { get; set; }

        [Required]
        [Display(Name = "Hora de Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [Display(Name = "Especialista")]
        public string NombreEspecialista { get; set; }
        public int IdEspecialista { get; set; }

        [Required]
        [Display(Name = "Paciente")]
        public string NombrePaciente { get; set; }

        public int IdEspecialidad { get; set; }

        [Required]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal PrecioConsulta { get; set; }
        public List<SelectListItem> ListaItems { get; set; }
    }
}
