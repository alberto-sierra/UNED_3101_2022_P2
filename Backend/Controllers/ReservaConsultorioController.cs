using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Backend.Models;

namespace Backend.Controllers
{
    public class ReservaConsultorioController : Controller
    {
        private readonly citasContext _context;
        private readonly string[] horasPublico = { "0800", "0830", "0900", "0930", "1000", "1030", "1100", "1130", "1330", "1400", "1430", "1500", "1530", "1600", "1630" };

        public ReservaConsultorioController(citasContext context)
        {
            _context = context;
        }

        // GET: ReservaConsultorio
        public async Task<IActionResult> Index()
        {
            if (_context.ReservaConsultorios != null)
            {
                return View(await _context.ReservaConsultorios
                          .Include(x => x.IdConsultorioNavigation)
                          .Include(x => x.IdEquipoNavigation)
                          .Include(x => x.IdEspecialistaNavigation)
                          .Select(x => new ReservaConsultorioViewModel
                          {
                              Id = x.Id,
                              IdConsultorio = x.IdConsultorio,
                              IdEquipo = x.IdEquipo,
                              IdEspecialista = x.IdEspecialista,
                              DiaSemana = x.DiaSemana,
                              HoraInicio = x.HoraInicio,
                              HoraFinal = x.HoraFinal,
                              Disponible = x.Disponible,
                              NombreEquipo = x.IdEquipoNavigation.Nombre,
                              NombreEspecialista = x.IdEspecialistaNavigation.Nombre,
                              NumeroConsultorio = x.IdConsultorioNavigation.Numero.ToString()
                          })
                          .ToListAsync());
            }
            else
            {
                return Problem("Entity set 'citasContext.ReservaConsultorios'  is null.");
            }
                          
        }

        // GET: ReservaConsultorio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservaConsultorios == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var reservaConsultorio = await _context.ReservaConsultorios
                .Include(x => x.IdConsultorioNavigation)
                .Include(x => x.IdEquipoNavigation)
                .Include(x => x.IdEspecialistaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservaConsultorio == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var reservaConsultorioViewModel = new ReservaConsultorioViewModel
            {
                Id = reservaConsultorio.Id,
                HoraInicio = reservaConsultorio.HoraInicio,
                NombreEquipo = reservaConsultorio.IdEquipoNavigation.Nombre,
                NombreEspecialista = reservaConsultorio.IdEspecialistaNavigation.Nombre,
                NumeroConsultorio = reservaConsultorio.IdConsultorioNavigation.Numero.ToString()
            };

            return View(reservaConsultorioViewModel);
        }

        // GET: ReservaConsultorio/Create
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> dropDownList = new List<SelectListItem>();
            var consultorios = new List<ConsultorioViewModel>();

            if (_context.Consultorios != null)
            {
                dropDownList = await _context.Consultorios
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Numero.ToString()
                }).ToListAsync();
            }
            else
            {
                return Problem("Entity set 'citasContext.Consultorios'  is null.");
            }

            var reservaConsultorioViewModel = new ReservaConsultorioViewModel
            {
                ListaItems = dropDownList
            };

            return View(reservaConsultorioViewModel);
        }

        // POST: ReservaConsultorio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConsultorio,DiaSemana")] ReservaConsultorioViewModel reservaViewModel)
        {

            var horasDisponibles = horasPublico;

            if (_context.ReservaConsultorios != null)
            {

                var reservas = await _context.ReservaConsultorios
                .Where(x => x.IdConsultorio == reservaViewModel.IdConsultorio
                && x.DiaSemana == reservaViewModel.DiaSemana)
                .Select(x => new ReservaConsultorioViewModel
                {
                    IdConsultorio = x.IdConsultorio,
                    HoraInicio = x.HoraInicio
                })
                .ToListAsync();

                foreach (var item in reservas)
                {
                    var horaReserva = item.HoraInicio.Hours.ToString("D2") + item.HoraInicio.Minutes.ToString("D2");
                    horasDisponibles = horasDisponibles.Where(x => x != horaReserva).ToArray();
                }

            }
            else
            {
                return Problem("Entity set 'citasContext.ReservaConsultorios'  is null.");
            }

            var horasDropdown = horasDisponibles.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = $"{x.Substring(0, 2)}:{x.Substring(2)}"
            }).ToList();

            reservaViewModel.ListaItems = horasDropdown;
            var numConsultorio = await _context.Consultorios
                .Where(x => x.Id == reservaViewModel.IdConsultorio)
                .Select(x => x.Numero)
                .FirstAsync();
            reservaViewModel.NumeroConsultorio = numConsultorio.ToString();

            return View("Create2", reservaViewModel);
        }

        // POST: ReservaConsultorio/Create2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("IdConsultorio,DiaSemana,NumeroConsultorio,HoraSeleccionada")] ReservaConsultorioViewModel reservaViewModel)
        {

            try
            {
                var hora = int.Parse(reservaViewModel.HoraSeleccionada.ToString().Substring(0, 2));
                var minuto = int.Parse(reservaViewModel.HoraSeleccionada.ToString().Substring(2));
                var horaReserva = new TimeSpan(hora, minuto, 0);
                reservaViewModel.HoraInicio = horaReserva;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Formato de hora incorrecto.";
                return RedirectToAction(nameof(Create));
            }

            List<SelectListItem> dropDownList = new List<SelectListItem>();
            var horaSeleccionada = reservaViewModel.HoraInicio;

            if (_context.ReservaConsultorios != null)
            {

                var especialistas = await _context.Especialista
                    .Select(x => new
                    {
                        x.Id,
                        x.Nombre
                    })
                    .ToListAsync();
                    
                var reservas = await _context.ReservaConsultorios
                    .Where(x => x.HoraInicio == reservaViewModel.HoraInicio
                    && x.DiaSemana == reservaViewModel.DiaSemana)
                    .Select(x => new
                    {
                        IdEspecialista = x.IdEspecialista
                    })
                    .ToListAsync();


                foreach (var especialista in especialistas)
                {
                    var agregar = true;
                    foreach (var reserva in reservas)
                    {
                        if (reserva.IdEspecialista == especialista.Id)
                        {
                            agregar = false;
                        }
                    }

                    if (agregar)
                    {
                        dropDownList.Add(new SelectListItem
                        {
                            Value = especialista.Id.ToString(),
                            Text = especialista.Nombre
                        });
                    }
                    
                }

                reservaViewModel.ListaItems = dropDownList;
                return View("Create3", reservaViewModel);
            }
            else
            {
                return Problem("Entity set 'citasContext.ReservaConsultorios'  is null.");
            }

        }

        // POST: ReservaConsultorio/Create3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create3([Bind("IdConsultorio,DiaSemana,NumeroConsultorio,HoraInicio,IdEspecialista")] ReservaConsultorioViewModel reservaViewModel)
        {
            if (_context.Equipos != null)
            {

                var equipos = await _context.Equipos
                .Join(_context.Especialista,
                    eq => eq.IdEspecialidad,
                    es => es.IdEspecialidad,
                    (eq, es) => new
                    {
                        Id = eq.Id,
                        Nombre =  eq.Nombre,
                        IdEspecialista = es.Id,
                        NombreEspecialista = es.Nombre
                    }
                )
                .Where(x => x.IdEspecialista == reservaViewModel.IdEspecialista)
                .Distinct()
                .Select(y => new SelectListItem
                {
                    Value = y.Id.ToString(),
                    Text = y.Nombre
                })
                .ToListAsync();

                var nombreEspecialista = await _context.Especialista
                    .Where(x => x.Id == reservaViewModel.IdEspecialista)
                    .Select(x => x.Nombre)
                    .FirstAsync();
                reservaViewModel.NombreEspecialista = nombreEspecialista;

                reservaViewModel.ListaItems = equipos;

                return View("create4", reservaViewModel);
            }
            else
            {
                return Problem("Entity set 'citasContext.ReservaConsultorios'  is null.");
            }

        }

        // POST: ReservaConsultorio/Create4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create4([Bind("IdConsultorio,DiaSemana,HoraInicio,IdEspecialista,IdEquipo")] ReservaConsultorioViewModel reservaViewModel)
        {
            ModelState.Remove("ListaItems");
            ModelState.Remove("NombreEquipo");
            ModelState.Remove("NombreEspecialista");
            ModelState.Remove("HoraSeleccionada");
            ModelState.Remove("NumeroConsultorio");
            if (ModelState.IsValid)
            {
                var reservaConsultorio = new ReservaConsultorio
                {
                    IdConsultorio = reservaViewModel.IdConsultorio,
                    IdEquipo = reservaViewModel.IdEquipo,
                    IdEspecialista = reservaViewModel.IdEspecialista,
                    HoraInicio = reservaViewModel.HoraInicio,
                    HoraFinal = reservaViewModel.HoraInicio.Add(new TimeSpan(0, 30, 0)),
                    DiaSemana = ((byte)reservaViewModel.DiaSemana),
                    Disponible = true,
                };
                _context.Add(reservaConsultorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reservaViewModel);
        }

        // GET: ReservaConsultorio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservaConsultorios == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var reservaConsultorioViewModel = await _context.ReservaConsultorios.FindAsync(id);
            if (reservaConsultorioViewModel == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(reservaConsultorioViewModel);
        }

        // GET: ReservaConsultorio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservaConsultorios == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var reservaConsultorio = await _context.ReservaConsultorios
                .Include(x => x.IdConsultorioNavigation)
                .Include(x => x.IdEspecialistaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservaConsultorio == null)
            {
                ViewBag.mensaje = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var reservaConsultorioViewModel = new ReservaConsultorioViewModel
            {
                Id = reservaConsultorio.Id,
                NombreEspecialista = reservaConsultorio.IdEspecialistaNavigation.Nombre,
                NumeroConsultorio = reservaConsultorio.IdConsultorioNavigation.Numero.ToString(),
                HoraInicio = reservaConsultorio.HoraInicio
            };

            return View(reservaConsultorioViewModel);
        }

        // POST: ReservaConsultorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservaConsultorios == null)
            {
                return Problem("Entity set 'citasContext.ReservaConsultorioViewModel'  is null.");
            }
            var reservaConsultorioViewModel = await _context.ReservaConsultorios.FindAsync(id);
            if (reservaConsultorioViewModel != null)
            {
                _context.ReservaConsultorios.Remove(reservaConsultorioViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaConsultorioViewModelExists(int id)
        {
          return (_context.ReservaConsultorios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
