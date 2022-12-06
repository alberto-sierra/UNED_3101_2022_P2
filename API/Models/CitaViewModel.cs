using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace _3101_proyecto1.Api.Models
{
    public partial class CitaViewModel
    {

        [Required]
        public long IdPaciente { get; set; }

        [Required]
        public int IdReserva { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
    }
}
