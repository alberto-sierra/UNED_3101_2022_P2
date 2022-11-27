using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.Api.Models
{
    public partial class ReservaConsultorioViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh':'mm}")]
        public TimeSpan HoraInicio { get; set; }

        public string? NombreEspecialista { get; set; }

        public string? NumeroConsultorio { get; set; }

        public string? Especialidad { get; set; }

    }
}