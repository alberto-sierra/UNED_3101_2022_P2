using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class EstadisticaModel
	{
		public string Nombre { get; set; }

        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = true)]
        public decimal Porcentaje { get; set; }

		public int Total { get; set; }
    }
}

