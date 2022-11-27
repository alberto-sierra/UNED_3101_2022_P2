using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace _3101_proyecto1.Models
{
	public class ReporteCitaViewModel
	{
        [Display(Name = "Nombre del Paciente")]
        public string NombrePaciente { get; set; }

        [Display(Name = "Hora de la Cita")]
        [DisplayFormat(DataFormatString = "{0:hh':'mm}")]
        public DateTime HoraCita { get; set; }

        [Display(Name = "Nombre del Especialista")]
        public string NombreEspecialista { get; set; }

        [Display(Name = "Costo")]
        public decimal Costo { get; set; }
	}
}

