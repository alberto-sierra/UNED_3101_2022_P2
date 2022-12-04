using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Models
{
    public partial class ReporteConsultorioViewModel
    {

        [Display(Name = "Especialidad")]
        public string NombreEspecialidad { get; set; }

        [Display(Name = "Consultorio")]
        public string NumeroConsultorio { get; set; }

    }
}
