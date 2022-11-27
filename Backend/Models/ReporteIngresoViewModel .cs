using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.Models
{
    public partial class ReporteIngresoViewModel
    {

        [Display(Name = "Número de Citas")]
        public int CitasCantidad { get; set; }

        [Display(Name = "Ingreso Total")]
        public decimal IngresoTotal { get; set; }

    }
}
