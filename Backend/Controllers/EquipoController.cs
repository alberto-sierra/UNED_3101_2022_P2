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
    public class EquipoController : Controller
    {
        private readonly citasContext _context;

        public EquipoController(citasContext context)
        {
            _context = context;
        }

        private List<SelectListItem> getDropDown()
        {
            List<SelectListItem> dropDownList = new List<SelectListItem>();

            if (_context.Equipos != null)
            {
                dropDownList = _context.Especialidades.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                }).ToList();

            }

            return dropDownList;
        }

        // GET: Equipo
        public async Task<IActionResult> Index()
        {
            if (_context.Equipos != null)
            {
                return View(await _context.Equipos
                    .Include(x => x.IdEspecialidadNavigation)
                    .Select(x => new EquipoViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
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

        // GET: Equipo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var equipoViewModel = await _context.Equipos
                .Include(x => x.IdEspecialidadNavigation)
                .Select(x => new EquipoViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Activo = x.Activo,
                    Serie = x.Serie,
                    Descripcion = x.Descripcion,
                    FechaCompra = x.FechaCompra,
                    IdEspecialidad = x.IdEspecialidad,
                    NombreEspecialidad = x.IdEspecialidadNavigation.Nombre
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipoViewModel == null)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(equipoViewModel);
        }

        // GET: Equipo/Create
        public IActionResult Create()
        {
            var equipoViewModel = new EquipoViewModel
            {
                ListaEspecialidad = getDropDown()
            };
            return View(equipoViewModel);
        }

        // POST: Equipo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,IdEspecialidad,Activo,Serie,Descripcion,FechaCompra")] EquipoViewModel equipoViewModel)
        {
            ModelState.Remove("NombreEspecialidad");
            ModelState.Remove("ListaEspecialidad");
            if (ModelState.IsValid)
            {
                var equipo = new Equipo
                {
                    Nombre = equipoViewModel.Nombre,
                    Activo = equipoViewModel.Activo,
                    Serie = equipoViewModel.Serie,
                    Descripcion = equipoViewModel.Descripcion,
                    FechaCompra = equipoViewModel.FechaCompra,
                    IdEspecialidad = equipoViewModel.IdEspecialidad
                };
                _context.Add(equipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipoViewModel);
        }

        // GET: Equipo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var equipoViewModel = new EquipoViewModel
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                Activo = equipo.Activo,
                Serie = equipo.Serie,
                Descripcion = equipo.Descripcion,
                FechaCompra = equipo.FechaCompra,
                IdEspecialidad = equipo.IdEspecialidad,
                ListaEspecialidad = getDropDown()
            };

            return View(equipoViewModel);
        }

        // POST: Equipo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Activo,Serie,Descripcion,FechaCompra,IdEspecialidad")] EquipoViewModel equipoViewModel)
        {
            if (id != equipoViewModel.Id)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.Remove("NombreEspecialidad");
            ModelState.Remove("ListaEspecialidad");
            if (ModelState.IsValid)
            {
                var equipo = new Equipo
                {
                    Id = equipoViewModel.Id,
                    Nombre = equipoViewModel.Nombre,
                    Activo = equipoViewModel.Activo,
                    Serie = equipoViewModel.Serie,
                    Descripcion = equipoViewModel.Descripcion,
                    FechaCompra = equipoViewModel.FechaCompra,
                    IdEspecialidad = equipoViewModel.IdEspecialidad
                };

                try
                {
                    _context.Update(equipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoViewModelExists(equipoViewModel.Id))
                    {
                        ViewBag.mensaje = "Equipo no encontrado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            equipoViewModel.ListaEspecialidad = getDropDown();

            return View(equipoViewModel);
        }

        // GET: Equipo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
            .FirstOrDefaultAsync(m => m.Id == id);

            if (equipo == null)
            {
                ViewBag.mensaje = "Equipo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var equipoViewModel = new EquipoViewModel
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                Activo = equipo.Activo,
                Serie = equipo.Serie
            };

            return View(equipoViewModel);
        }

        // POST: Equipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipos == null)
            {
                return Problem("Entity set 'citasContext.Equipo'  is null.");
            }
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipoViewModelExists(int id)
        {
          return (_context.Equipos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
