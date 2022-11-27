using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3101_proyecto1.FrontEnd.Models
{
	public class PageViewModel
	{
        [Required]
        [Display(Name = "Identificación")]
        public string? Identificacion { get; set; }
    }
}

