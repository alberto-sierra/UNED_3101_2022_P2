using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace API;

public class Cita
{
    public int Id { get; set; }
    public long IdPaciente { get; set; }
    public int IdReserva { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public TimeSpan HoraInicio { get; set; }

}

