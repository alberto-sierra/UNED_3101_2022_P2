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
    public class EspecialistaController : Controller
    {
        private readonly citasContext _context;

        public EspecialistaController(citasContext context)
        {
            _context = context;
        }

        private List<SelectListItem> getDropDown()
        {
            List<SelectListItem> dropDownList = new List<SelectListItem>();

            if (_context.Especialista != null)
            {
                dropDownList = _context.Especialidades.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                }).ToList();

            }

            return dropDownList;
        }

        // GET: Especialista
        public async Task<IActionResult> Index()
        {
            if (_context.Especialista != null)
            {
                return View(await _context.Especialista
                    .Include(x => x.IdEspecialidadNavigation)
                    .Select(x => new EspecialistumViewModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        PrecioConsulta = x.PrecioConsulta,
                        IdEspecialidad = x.IdEspecialidad,
                        NombreEspecialidad = x.IdEspecialidadNavigation.Nombre
                    })
                .ToListAsync());
            }
            else
            {
                return Problem("Entity set 'citasContext.Equipo'  is null.");
            }
        }

        // GET: Especialista/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var especialistumViewModel = await _context.Especialista
                .Include(x => x.IdEspecialidadNavigation)
                .Select(x => new EspecialistumViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Identificacion = x.Identificacion,
                    PrecioConsulta = x.PrecioConsulta,
                    IdEspecialidad = x.IdEspecialidad,
                    NombreEspecialidad = x.IdEspecialidadNavigation.Nombre
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (especialistumViewModel == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }


            return View(especialistumViewModel);
        }

        // GET: Especialista/Create
        public IActionResult Create()
        {
            var especialistumViewModel = new EspecialistumViewModel
            {
                ListaEspecialidad = getDropDown()
            };
            return View(especialistumViewModel);
        }

        // POST: Especialista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Identificacion,PrecioConsulta,IdEspecialidad")] EspecialistumViewModel especialistumViewModel)
        {
            ModelState.Remove("NombreEspecialidad");
            ModelState.Remove("ListaEspecialidad");
            if (ModelState.IsValid)
            {
                var especialista = new Especialistum
                {
                    Nombre = especialistumViewModel.Nombre,
                    Identificacion = especialistumViewModel.Identificacion,
                    PrecioConsulta = especialistumViewModel.PrecioConsulta,
                    IdEspecialidad = especialistumViewModel.IdEspecialidad
                };
                _context.Add(especialista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especialistumViewModel);
        }

        // GET: Especialista/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var especialistumViewModel = await _context.Especialista
                .Include(x => x.IdEspecialidadNavigation)
                .Select(x => new EspecialistumViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Identificacion = x.Identificacion,
                    IdEspecialidad = x.IdEspecialidad,
                    PrecioConsulta = x.PrecioConsulta,
                    NombreEspecialidad = x.IdEspecialidadNavigation.Nombre
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (especialistumViewModel == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            especialistumViewModel.ListaEspecialidad = getDropDown();

            return View(especialistumViewModel);
        }

        // POST: Especialista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Identificacion,PrecioConsulta,IdEspecialidad")] EspecialistumViewModel especialistumViewModel)
        {
            if (id != especialistumViewModel.Id)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.Remove("NombreEspecialidad");
            ModelState.Remove("ListaEspecialidad");
            if (ModelState.IsValid)
            {
                var especialista = new Especialistum
                {
                    Id = especialistumViewModel.Id,
                    Nombre = especialistumViewModel.Nombre,
                    Identificacion = especialistumViewModel.Identificacion,
                    PrecioConsulta = especialistumViewModel.PrecioConsulta,
                    IdEspecialidad = especialistumViewModel.IdEspecialidad
                };

                try
                {
                    _context.Update(especialista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecialistumViewModelExists(especialistumViewModel.Id))
                    {
                        ViewBag.mensaje = "Especialista no encontrado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            especialistumViewModel.ListaEspecialidad = getDropDown();

            return View(especialistumViewModel);
        }

        // GET: Especialista/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Especialista == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var especialista = await _context.Especialista
                .FirstOrDefaultAsync(m => m.Id == id);
            if (especialista == null)
            {
                ViewBag.mensaje = "Especialista no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var especialistumViewModel = new EspecialistumViewModel
            {
                Id = especialista.Id,
                Nombre = especialista.Nombre,
                Identificacion = especialista.Identificacion,
                PrecioConsulta = especialista.PrecioConsulta
            };

            return View(especialistumViewModel);
        }

        // POST: Especialista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Especialista == null)
            {
                return Problem("Entity set 'citasContext.EspecialistumViewModel'  is null.");
            }
            var especialista = await _context.Especialista.FindAsync(id);
            if (especialista != null)
            {
                _context.Especialista.Remove(especialista);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EspecialistumViewModelExists(int id)
        {
          return (_context.Especialista?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
