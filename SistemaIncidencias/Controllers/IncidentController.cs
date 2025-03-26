using System;
using System.Linq;
using System.Web.Mvc;
using SistemaIncidencias.Models;
using SistemaIncidencias.Services;

namespace SistemaIncidencias.Controllers
{
    public class IncidentController : Controller
    {
        private readonly IIncidentService _incidentService;

        // Constructor con inyección de dependencias
        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        // GET: Incident
        public ActionResult Index(
            IncidentStatus? estado,
            IncidentPriority? prioridad,
            int page = 1,
            int pageSize = 10)
        {
            // Obtener incidencias con paginación y filtros
            var incidencias = _incidentService.GetIncidents(estado, prioridad)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Información para la paginación
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalIncidents = _incidentService.GetIncidents().Count();

            // Preparar lista de estados y prioridades para los filtros
            ViewBag.Estados = Enum.GetValues(typeof(IncidentStatus))
                .Cast<IncidentStatus>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            ViewBag.Prioridades = Enum.GetValues(typeof(IncidentPriority))
                .Cast<IncidentPriority>()
                .Select(p => new SelectListItem
                {
                    Value = p.ToString(),
                    Text = p.ToString()
                });

            return View(incidencias);
        }

        // GET: Incident/Detalles/5
        public ActionResult Detalles(int id)
        {
            var incidencia = _incidentService.GetIncidentById(id);

            if (incidencia == null)
            {
                return HttpNotFound();
            }

            return View(incidencia);
        }

        // GET: Incident/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Incident/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Incident incidencia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _incidentService.CrearIncidencia(incidencia);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear la incidencia: " + ex.Message);
                }
            }

            return View(incidencia);
        }

        // GET: Incident/Editar/5
        public ActionResult Editar(int id)
        {
            var incidencia = _incidentService.GetIncidentById(id);

            if (incidencia == null)
            {
                return HttpNotFound();
            }

            return View(incidencia);
        }

        // POST: Incident/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Incident incidencia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _incidentService.ActualizarIncidencia(incidencia);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la incidencia: " + ex.Message);
                }
            }

            return View(incidencia);
        }

        // POST: Incident/AgregarComentario
        [HttpPost]
        public ActionResult AgregarComentario(Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _incidentService.AgregarComentario(comentario);
                    return Json(new { success = true, message = "Comentario agregado exitosamente" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al agregar comentario: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Datos inválidos" });
        }

        // GET: Incident/Estadisticas
        public ActionResult Estadisticas()
        {
            var estadisticas = _incidentService.ObtenerEstadisticas().ToList();
            return View(estadisticas);
        }
    }
}