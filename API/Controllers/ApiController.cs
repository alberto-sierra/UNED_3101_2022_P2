using Microsoft.AspNetCore.Mvc;
using _3101_proyecto1.Api.Data;
using _3101_proyecto1.Api.Entities;
using _3101_proyecto1.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers;

[ApiController]
[Route("Cita")]
public class CitaController : ControllerBase
{

    private readonly ApiContext _context;

    public CitaController(ApiContext context)
    {
        _context = context;
    }

    [HttpGet("Throw")]
    public IActionResult Throw() =>
    throw new Exception("Internal error.");

    [HttpGet("/Cita/GetAllByDoc/{identificacion}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<CitumViewModel>> GetCitas(string identificacion, bool? idLocal)
    {
        if (identificacion == null)
        {
            return BadRequest(new { error = "Faltan parametros obligatorios" });
        }

        var pacienteId = new long();
        var paciente = new PacienteViewModel();
        paciente = null;
        var citas = new List<CitumViewModel>();


        if (idLocal == true && long.TryParse(identificacion, out pacienteId))
        {
            paciente = _context.Pacientes
            .Where(x => x.Id == pacienteId)
            .Select(x => new PacienteViewModel
            {
                Id = x.Id,
                Identificacion = x.Identificacion
            }).SingleOrDefault();

            citas = _context.Cita
            .Include(x => x.IdPacienteNavigation)
            .Include(x => x.IdReservaNavigation)
            .Where(x => x.IdPacienteNavigation.Id == pacienteId)
            .Select(x => new CitumViewModel
            {
                Id = x.Id,
                IdPaciente = x.IdPaciente,
                IdReserva = x.IdReserva,
                HoraInicio = x.Fecha.TimeOfDay,
                Fecha = x.Fecha,
                NombreEspecialista = x.IdReservaNavigation.IdEspecialistaNavigation.Nombre,
                IdEspecialista = x.IdReservaNavigation.IdEspecialista,
                NombrePaciente = x.IdPacienteNavigation.NombreCompleto,
                Especialidad = x.IdReservaNavigation.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                PrecioConsulta = x.PrecioConsulta
            })
            .ToList();
        }
        else
        {
            paciente = _context.Pacientes
            .Where(x => x.Identificacion == identificacion)
            .Select(x => new PacienteViewModel
            {
                Id = x.Id,
                Identificacion = x.Identificacion
            }).SingleOrDefault();

            citas = _context.Cita
            .Include(x => x.IdPacienteNavigation)
            .Include(x => x.IdReservaNavigation)
            .Where(x => x.IdPacienteNavigation.Identificacion == identificacion)
            .Select(x => new CitumViewModel
            {
                Id = x.Id,
                IdPaciente = x.IdPaciente,
                IdReserva = x.IdReserva,
                HoraInicio = x.Fecha.TimeOfDay,
                Fecha = x.Fecha,
                NombreEspecialista = x.IdReservaNavigation.IdEspecialistaNavigation.Nombre,
                IdEspecialista = x.IdReservaNavigation.IdEspecialista,
                NombrePaciente = x.IdPacienteNavigation.NombreCompleto,
                Especialidad = x.IdReservaNavigation.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                PrecioConsulta = x.PrecioConsulta
            })
            .ToList();
        }

        

        if (citas.Count == 0)
        {
            if (paciente == null)
            {
                return Ok(Array.Empty<string>());
            }
            else
            {
                return Ok(new object[] { new CitumViewModel { Id = 0, IdPaciente = paciente.Id } });
            }
            
        }

        return Ok(citas);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CitumViewModel> GetById(int? id)
    {

        if (id == null)
        {
            return BadRequest(new {error = "Faltan parametros obligatorios"});
        }

        var citumViewModel = _context.Cita
            .Where(x => x.Id == id)
            .Include(x => x.IdPacienteNavigation)
            .Include(x => x.IdReservaNavigation)
            .Select(x => new CitumViewModel
            {
                Id = x.Id,
                NombrePaciente = x.IdPacienteNavigation.NombreCompleto,
                NombreEspecialista = x.IdReservaNavigation.IdEspecialistaNavigation.Nombre,
                IdEspecialista = x.IdReservaNavigation.IdEspecialista,
                HoraInicio = x.Fecha.TimeOfDay,
                Fecha = x.Fecha,
                Especialidad = x.IdReservaNavigation.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                PrecioConsulta = x.IdReservaNavigation.IdEspecialistaNavigation.PrecioConsulta
            })
            .SingleOrDefault();

        if (citumViewModel == null)
        {
            return Ok(new CitumViewModel { });
        }

        return Ok(citumViewModel);
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<CitumViewModel>> GetHora(DateTime? fecha, long? idPaciente)
    {
        if (fecha == null || fecha <= DateTime.Now)
        {
            fecha = DateTime.Now;
        }

        var disponiblePaciente = new List<CitumViewModel>();
        var citasDisponibles = _context.ReservaConsultorios
            .Where(x => x.Disponible == true)
            .Where(x => x.DiaSemana == (int)fecha.Value.DayOfWeek)
            .Include(x => x.IdConsultorioNavigation)
            .Include(x => x.IdEspecialistaNavigation)
            .Select(x => new CitumViewModel
            {
                IdReserva = x.Id,
                HoraInicio = x.HoraInicio,
                Fecha = fecha.Value,
                NombreEspecialista = x.IdEspecialistaNavigation.Nombre,
                IdEspecialista = x.IdEspecialista,
                IdEspecialidad = x.IdEspecialistaNavigation.IdEspecialidad,
                Especialidad = x.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                PrecioConsulta = x.IdEspecialistaNavigation.PrecioConsulta
            })
            .ToList();

        disponiblePaciente = new List<CitumViewModel>(citasDisponibles);

        if (idPaciente != null)
        {
            var citasProgramadas = _context.Cita
            .Where(x => x.IdPaciente == idPaciente)
            .Where(x => x.Fecha >= fecha)
            .Select(x => new CitumViewModel
            {
                Id = x.Id,
                HoraInicio = x.Fecha.TimeOfDay,
                Fecha = x.Fecha,
            })
            .ToList();

            if (citasProgramadas != null)
            {
                // Eliminar horas de citas programadas de disponibles
                foreach (var citaDisponible in citasDisponibles)
                {
                    foreach (var citaProgramada in citasProgramadas)
                    {
                        if (citaProgramada.HoraInicio == citaDisponible.HoraInicio)
                        {
                            disponiblePaciente.Remove(citaDisponible);
                        }
                    }
                }
            }
        }

        if (disponiblePaciente == null)
        {
            return Ok(new object[] { new CitumViewModel { } });
        }

        return Ok(disponiblePaciente);
    }

    [HttpGet("Hora/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<CitumViewModel>> GetHora(int? id)
    {
        var citumViewModel = _context.ReservaConsultorios
            .Where(x => x.Id == id)
            .Include(x => x.IdConsultorioNavigation)
            .Include(x => x.IdEspecialistaNavigation)
            .Select(x => new CitumViewModel
            {
                IdReserva = x.Id,
                HoraInicio = x.HoraInicio,
                NombreEspecialista = x.IdEspecialistaNavigation.Nombre,
                IdEspecialista = x.IdEspecialista,
                IdEspecialidad = x.IdEspecialistaNavigation.IdEspecialidad,
                Especialidad = x.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                PrecioConsulta = x.IdEspecialistaNavigation.PrecioConsulta
            })
            .SingleOrDefault();

        if (citumViewModel == null)
        {
            return Ok();
        }

        return Ok(citumViewModel);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody][Bind("IdPaciente,Fecha,IdReserva")] CitaViewModel citaViewModel)
    {
        if (ModelState.IsValid)
        {

            try
            {
                var paciente = _context.Pacientes
                    .Find(Convert.ToInt64(citaViewModel.IdPaciente));

                if (paciente == null)
                {
                    return BadRequest(new { error = "Id de paciente inválido" });
                }

                var reserva = _context.ReservaConsultorios
                    .Where(x => x.Id == citaViewModel.IdReserva)
                    .Include(x => x.IdEspecialistaNavigation)
                    .Include(x => x.IdEspecialistaNavigation.IdEspecialidadNavigation)
                    .SingleOrDefault();

                if (reserva == null)
                {
                    return BadRequest(new { error = "Especialista no disponible." });
                }

                var nuevaCita = new Citum
                {
                    IdPaciente = citaViewModel.IdPaciente,
                    IdReserva = citaViewModel.IdReserva,
                    PrecioConsulta = reserva.IdEspecialistaNavigation.PrecioConsulta,
                    Fecha = citaViewModel.Fecha + reserva.HoraInicio
                };

                _context.Add(nuevaCita);
                await _context.SaveChangesAsync();

                var citumViewModel = new CitumViewModel
                {
                    Id = nuevaCita.Id,
                    IdPaciente = nuevaCita.IdPaciente,
                    IdReserva = nuevaCita.IdReserva,
                    IdEspecialidad = reserva.IdEspecialistaNavigation.IdEspecialidadNavigation.Id,
                    IdEspecialista = reserva.IdEspecialista,
                    PrecioConsulta = nuevaCita.PrecioConsulta,
                    HoraInicio = reserva.HoraInicio,
                    Fecha = nuevaCita.Fecha,
                    NombreEspecialista = reserva.IdEspecialistaNavigation.Nombre,
                    Especialidad = reserva.IdEspecialistaNavigation.IdEspecialidadNavigation.Nombre,
                    NombrePaciente = nuevaCita.IdPacienteNavigation.NombreCompleto
                };

                return CreatedAtAction(nameof(Create), citumViewModel);
            }
            catch (DbUpdateException err)
            {
                return BadRequest(new { error = err.Message});
            }

        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(int? id)
    {
        {
            if (id == null)
            {
                return BadRequest(new { error = "Faltan parametros obligatorios" });
            }

            var citumViewModel = _context.Cita
                .FirstOrDefault(m => m.Id == id);
            if (citumViewModel == null)
            {
                return Ok(new { deleted = 0 });
            }

            _context.Cita.Remove(citumViewModel);
            _context.SaveChangesAsync();
            return Ok(new { deleted = citumViewModel.Id });
        }
    }


}

[Route("Paciente")]
public class PacienteController : ControllerBase
{
    private readonly ApiContext _context;

    public PacienteController(ApiContext context)
    {
        _context = context;
    }

    [HttpGet("Throw")]
    public IActionResult Throw() =>
    throw new Exception("Internal error.");

    [HttpGet("{identificacion}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PacienteViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetByDoc(string identificacion)
    {
        var paciente = _context.Pacientes
            .FirstOrDefault(m => m.Identificacion == identificacion);

        if (paciente == null)
        {
            return Ok(Array.Empty<string>());
        }

        var pacienteViewModel = new PacienteViewModel
        {
            Id = paciente.Id,
            Identificacion = paciente.Identificacion,
            NombreCompleto = paciente.NombreCompleto,
            Telefono = paciente.Telefono
        };

        return Ok(pacienteViewModel);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody][Bind("Identificacion,NombreCompleto,Telefono")] PacienteViewModel pacienteViewModel)
    {
        if (ModelState.IsValid)
        {
            var paciente = new Paciente
            {
                Identificacion = pacienteViewModel.Identificacion,
                NombreCompleto = pacienteViewModel.NombreCompleto,
                Telefono = pacienteViewModel.Telefono
            };
            try
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                pacienteViewModel.Id = paciente.Id;
                return CreatedAtAction(nameof(Create), pacienteViewModel);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            
        }
        return BadRequest();
    }
}
