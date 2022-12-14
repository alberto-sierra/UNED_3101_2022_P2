using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using _3101_proyecto1.FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace Frontend.Controllers
{
    public class CitaController : Controller
    {
        private readonly IConfiguration _config;

        public CitaController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // GET: Cita
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.mensaje = TempData["mensaje"].ToString();
            }

            return View();
        }

        // POST: Cita/List
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(string identificacion)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(_config["ApiURL"] + "/Cita/GetAllByDoc/" + identificacion).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var citumViewModel = JsonConvert.DeserializeObject<List<CitumViewModel>>(responseString);
                    if (citumViewModel != null)
                    {
                        ViewBag.IdPaciente = citumViewModel[0].IdPaciente;
                        if (citumViewModel.Count == 1 && citumViewModel[0].Id == 0)
                        {
                            return View(nameof(List), Array.Empty<CitumViewModel>());
                        }
                        return View(citumViewModel);
                    }
                }

                throw new Exception();
            }
            catch
            {
                TempData["mensaje"] = "Error de comunicaci??n con el API.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Cita/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cita/Create/1
        public ActionResult Create(int id)
        {
            return View(new CitumViewModel { IdPaciente = id, Fecha = DateTime.Today });
        }

        // POST: Cita/Create1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create1([Bind("IdPaciente,Fecha")] CitumViewModel citumViewModel)
        {
            try
            {
                var citasDisponibles = new List<CitumViewModel>();
                var citasProgramadas = new List<CitumViewModel>();
                var nuevaFecha = DateTime.Today;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();

                if (citumViewModel.Fecha.Date > DateTime.Now.Date)
                {
                    response = client.GetAsync(_config["ApiURL"] +
                    "/Cita?idPaciente=" + citumViewModel.IdPaciente +
                    "&Fecha=" + citumViewModel.Fecha).Result;
                }
                else
                {
                    nuevaFecha = (DateTime.Now).AddMinutes(15);
                    citumViewModel.Fecha = nuevaFecha;
                    response = client.GetAsync(_config["ApiURL"] +
                    "/Cita?idPaciente=" + citumViewModel.IdPaciente +
                    "&Fecha=" + nuevaFecha).Result;
                }
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    citasDisponibles = JsonConvert.DeserializeObject<List<CitumViewModel>>(responseString);

                    if (citasDisponibles == null || citasDisponibles.Count == 0)
                    {
                        TempData["mensaje"] = "No hay disponibilidad para nuevas citas.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    throw new Exception();
                }

                var listaItems = citasDisponibles
                    .Select(x => new SelectListItem
                    {
                        Value = x.HoraInicio.ToString(),
                        Text = x.HoraInicio.ToString(@"hh\:mm")
                    })
                    .DistinctBy(x => x.Value)
                    .ToList();

                citumViewModel.ListaItems = listaItems;

                return View(citumViewModel);

            }
            catch
            {
                TempData["mensaje"] = "Error de comunicaci??n con el API.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cita/Create2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create2([Bind("IdPaciente,HoraInicio,Fecha")] CitumViewModel citumViewModel)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(_config["ApiURL"] +
                    "/Cita?idPaciente=" + citumViewModel.IdPaciente +
                    "&Fecha=" + citumViewModel.Fecha.Add(citumViewModel.HoraInicio)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var citumViewModelList = JsonConvert.DeserializeObject<List<CitumViewModel>>(responseString);
                    if (citumViewModelList == null || citumViewModelList.Count == 0)
                    {
                        TempData["mensaje"] = "No hay disponibilidad para nuevas citas.";
                        return RedirectToAction(nameof(Index));
                    }

                    var listaItems = citumViewModelList
                        .Where(x => x.HoraInicio == citumViewModel.HoraInicio)
                        .Select(x => new SelectListItem
                        {
                            Value = x.IdEspecialidad.ToString(),
                            Text = $"{x.Especialidad}"
                        })
                        .DistinctBy(x => x.Value)
                        .ToList();

                    citumViewModel.ListaItems = listaItems;
                    return View(citumViewModel);
                }

                throw new Exception();
            }
            catch
            {
                TempData["mensaje"] = "Error de comunicaci??n con el API.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cita/Create3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create3([Bind("IdPaciente,HoraInicio,Fecha,IdEspecialidad")] CitumViewModel citumViewModel)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(_config["ApiURL"] +
                    "/Cita?idPaciente=" + citumViewModel.IdPaciente +
                    "&Fecha=" + citumViewModel.Fecha.Add(citumViewModel.HoraInicio)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var citumViewModelList = JsonConvert.DeserializeObject<List<CitumViewModel>>(responseString);
                    if (citumViewModelList == null || citumViewModelList.Count == 0)
                    {
                        TempData["mensaje"] = "No hay disponibilidad para nuevas citas.";
                        return RedirectToAction(nameof(Index));
                    }

                    var listaItems = citumViewModelList
                        .Where(x => x.HoraInicio == citumViewModel.HoraInicio
                        && x.IdEspecialidad == citumViewModel.IdEspecialidad)
                        .Select(x => new SelectListItem
                        {
                            Value = x.IdReserva.ToString(),
                            Text = x.NombreEspecialista
                        })
                        .DistinctBy(x => x.Value)
                        .ToList();

                    var especialidad = citumViewModelList
                        .Where(x => x.HoraInicio == citumViewModel.HoraInicio
                        && x.IdEspecialidad == citumViewModel.IdEspecialidad)
                        .DistinctBy(x => x.Especialidad)
                        .Select(x => x.Especialidad)
                        .First();

                    citumViewModel.ListaItems = listaItems;
                    citumViewModel.Especialidad = especialidad;
                    return View(citumViewModel);
                }

                throw new Exception();
            }
            catch
            {
                TempData["mensaje"] = "Error de comunicaci??n con el API.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cita/Create4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create4([Bind("IdPaciente,Fecha,IdReserva")] CitumViewModel citumViewModel)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.PostAsJsonAsync<CitumViewModel>(_config["ApiURL"] + "/Cita", citumViewModel).Result;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var citumViewModelList = JsonConvert.DeserializeObject<CitumViewModel>(responseString);
                    if (citumViewModelList != null)
                    {
                        TempData["mensaje"] = "Cita registrada con ??xito.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                throw new Exception();
            }
            catch
            {
                TempData["mensaje"] = "Error de comunicaci??n con el API.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Cita/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(_config["ApiURL"] + "/Cita/" + id).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var citumViewModel = JsonConvert.DeserializeObject<CitumViewModel>(responseString);
                    if (citumViewModel == null)
                    {
                        ViewBag.error = "Cita no existe.";
                        return View();
                    }

                    return View(citumViewModel);
                }

                throw new Exception();
            }
            catch
            {
                ViewBag.error = "Error de comunicaci??n con el API.";
                return View();
            }
        }

        // POST: Cita/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.DeleteAsync(_config["ApiURL"] + "/Cita/" + id).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var deleteStatus = JsonConvert.DeserializeObject(responseString);
                    if (deleteStatus != null)
                    {
                        TempData["mensaje"] = "Cita eliminada con ??xito.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                throw new Exception();
            }
            catch
            {
                TempData["mensaje"] = "Error de borrado. Verifique la existencia de la cita.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}