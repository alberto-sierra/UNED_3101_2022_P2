using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.Models
{
    public partial class ReservaConsultorioViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Especialista")]
        public int IdEspecialista { get; set; }

        [Required]
        [Display(Name = "Consultorio")]
        public int IdConsultorio { get; set; }

        [Required]
        [Display(Name = "Equipo Médico")]
        public int IdEquipo { get; set; }

        [Required]
        [Display(Name = "Hora de Inicio")]
        [DisplayFormat(DataFormatString = "{0:hh':'mm}")]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [Display(Name = "Hora Final")]
        public TimeSpan HoraFinal { get; set; }

        [Required]
        [Display(Name = "Día de la Semana")]
        [RegularExpression(@"[0-7]{1}$")]
        public byte DiaSemana { get; set; }

        [Required]
        [Display(Name = "Disponible")]
        public bool Disponible { get; set; }

        [NotMapped]
        public List<SelectListItem> ListaItems { get; set; }

        [NotMapped]
        [Display(Name = "Hora")]
        [RegularExpression(@"[0-9]{4}$")]
        public string HoraSeleccionada { get; set; }

        [NotMapped]
        [Display(Name = "Especialista")]
        public string NombreEspecialista { get; set; }

        [NotMapped]
        [Display(Name = "Consultorio")]
        public string NumeroConsultorio { get; set; }

        [NotMapped]
        [Display(Name = "Equipo Médico")]
        public string NombreEquipo { get; set; }

    }
}
