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
    public class PacienteController : Controller
    {
        private readonly citasContext _context;

        public PacienteController(citasContext context)
        {
            _context = context;
        }

        // GET: Paciente
        public async Task<IActionResult> Index()
        {
              return _context.Pacientes != null ? 
                          View(await _context.Pacientes.Select(x => new PacienteViewModel
                          {
                              Id = x.Id,
                              Identificacion = x.Identificacion,
                              NombreCompleto = x.NombreCompleto,
                              Telefono = x.Telefono
                          })
                          .ToListAsync()) :
                          Problem("Entity set 'citasContext.Pacientes'  is null.");
        }

        // GET: Paciente/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var pacienteViewModel = await _context.Pacientes.Select(x => new PacienteViewModel
            {
                Id = x.Id,
                Identificacion = x.Identificacion,
                NombreCompleto = x.NombreCompleto,
                Telefono = x.Telefono
            })
            .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteViewModel == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(pacienteViewModel);
        }

        // GET: Paciente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paciente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identificacion,NombreCompleto,Telefono")] PacienteViewModel pacienteViewModel)
        {
            if (ModelState.IsValid)
            {
                var paciente = new Paciente
                {
                    Identificacion = pacienteViewModel.Identificacion,
                    NombreCompleto = pacienteViewModel.NombreCompleto,
                    Telefono = pacienteViewModel.Telefono
                };
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pacienteViewModel);
        }

        // GET: Paciente/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var paciente = await _context.Pacientes
            .FindAsync(id);
            if (paciente == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var pacienteViewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Identificacion = paciente.Identificacion,
                NombreCompleto = paciente.NombreCompleto,
                Telefono = paciente.Telefono
            };
            return View(pacienteViewModel);
        }

        // POST: Paciente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Identificacion,NombreCompleto,Telefono")] PacienteViewModel pacienteViewModel)
        {
            if (id != pacienteViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var paciente = new Paciente
                {
                    Id = pacienteViewModel.Id,
                    Identificacion = pacienteViewModel.Identificacion,
                    NombreCompleto = pacienteViewModel.NombreCompleto,
                    Telefono = pacienteViewModel.Telefono
                };

                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteViewModelExists(pacienteViewModel.Id))
                    {
                        ViewBag.mensaje = "Error al guardar en base de datos.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pacienteViewModel);
        }

        // GET: Paciente/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                ViewBag.mensaje = "Paciente no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var pacienteViewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Identificacion = paciente.Identificacion,
                NombreCompleto = paciente.NombreCompleto,
                Telefono = paciente.Telefono
            };

            return View(pacienteViewModel);
        }

        // POST: Paciente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'citasContext.Pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            ViewBag.mensaje = "Registro eliminado con éxito";
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteViewModelExists(long id)
        {
          return (_context.Pacientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
