using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _3101_proyecto1.Entities;
using _3101_proyecto1.Models;

namespace Backend.Controllers
{
    public class ConsultorioController : Controller
    {
        private readonly citasContext _context;

        public ConsultorioController(citasContext context)
        {
            _context = context;
        }

        // GET: Consultorio
        public async Task<IActionResult> Index()
        {
              return _context.Consultorios != null ? 
                          View(await _context.Consultorios
                          .Select(x => new ConsultorioViewModel
                          {
                              Id = x.Id,
                              Numero = x.Numero
                          })
                          .ToListAsync()) :
                          Problem("Entity set 'citasContext.Consultorios'  is null.");
        }

        // GET: Consultorio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultorio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero")] ConsultorioViewModel consultorioViewModel)
        {
            if (ModelState.IsValid)
            {
                var consultorio = new Consultorio
                {
                    Numero = consultorioViewModel.Numero
                };
                _context.Add(consultorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consultorioViewModel);
        }

        // GET: Consultorio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                ViewBag.mensaje = "Consultorio no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var consultorio = await _context.Consultorios.FindAsync(id);
            
            if (consultorio == null)
            {
                ViewBag.mensaje = "Consultorio no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var consultorioViewModel = new ConsultorioViewModel
            {
                Id = consultorio.Id,
                Numero = consultorio.Numero
            };

            return View(consultorioViewModel);
        }

        // POST: Consultorio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero")] ConsultorioViewModel consultorioViewModel)
        {
            if (id != consultorioViewModel.Id)
            {
                ViewBag.mensaje = "Consultorio no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var consultorio = new Consultorio
                {
                    Id = consultorioViewModel.Id,
                    Numero = consultorioViewModel.Numero
                };

                try
                {
                    _context.Update(consultorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultorioViewModelExists(consultorioViewModel.Id))
                    {
                        ViewBag.mensaje = "Consultorio no encontrado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(consultorioViewModel);
        }

        // GET: Consultorio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Consultorios == null)
            {
                ViewBag.mensaje = "Consultorio no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var consultorio = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultorio == null)
            {
                ViewBag.mensaje = "Consultorio no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var consultorioViewModel = new ConsultorioViewModel
            {
                Id = consultorio.Id,
                Numero = consultorio.Numero
            };

            return View(consultorioViewModel);
        }

        // POST: Consultorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Consultorios == null)
            {
                return Problem("Entity set 'citasContext.ConsultorioViewModel'  is null.");
            }
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio != null)
            {
                _context.Consultorios.Remove(consultorio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultorioViewModelExists(int id)
        {
          return (_context.Consultorios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
