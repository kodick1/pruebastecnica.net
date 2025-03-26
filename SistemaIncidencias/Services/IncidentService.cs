using System;
using System.Linq;
using SistemaIncidencias.Models;
using SistemaIncidencias.Repositories;

namespace SistemaIncidencias.Services
{
    public interface IIncidentService
    {
        IQueryable<Incident> GetIncidents(
            IncidentStatus? estado = null,
            IncidentPriority? prioridad = null,
            DateTime? fecha = null,
            int? tecnicoId = null);

        Incident GetIncidentById(int id);
        void CrearIncidencia(Incident incidencia);
        void ActualizarIncidencia(Incident incidencia);
        void AgregarComentario(Comentario comentario);
        IQueryable<object> ObtenerEstadisticas();
    }

    public class IncidentService : IIncidentService
    {
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IRepository<Comentario> _comentarioRepository;

        public IncidentService(
            IRepository<Incident> incidentRepository,
            IRepository<Comentario> comentarioRepository)
        {
            _incidentRepository = incidentRepository;
            _comentarioRepository = comentarioRepository;
        }

        public IQueryable<Incident> GetIncidents(
            IncidentStatus? estado = null,
            IncidentPriority? prioridad = null,
            DateTime? fecha = null,
            int? tecnicoId = null)
        {
            var query = _incidentRepository.GetAll();

            if (estado.HasValue)
                query = query.Where(i => i.Estado == estado.Value);

            if (prioridad.HasValue)
                query = query.Where(i => i.Prioridad == prioridad.Value);

            if (fecha.HasValue)
                query = query.Where(i => i.FechaCreacion.Date == fecha.Value.Date);

            if (tecnicoId.HasValue)
                query = query.Where(i => i.TecnicoAsignadoId == tecnicoId.Value);

            return query.OrderByDescending(i => i.FechaCreacion);
        }

        public Incident GetIncidentById(int id)
        {
            return _incidentRepository.GetById(id);
        }

        public void CrearIncidencia(Incident incidencia)
        {
            incidencia.FechaCreacion = DateTime.Now;
            _incidentRepository.Insert(incidencia);
        }

        public void ActualizarIncidencia(Incident incidencia)
        {
            incidencia.FechaUltimaActualizacion = DateTime.Now;
            _incidentRepository.Update(incidencia);
        }

        public void AgregarComentario(Comentario comentario)
        {
            comentario.FechaCreacion = DateTime.Now;
            _comentarioRepository.Insert(comentario);
        }

        public IQueryable<object> ObtenerEstadisticas()
        {
            return _incidentRepository.GetAll()
                .GroupBy(i => new { i.Estado, i.Prioridad })
                .Select(g => new {
                    Estado = g.Key.Estado,
                    Prioridad = g.Key.Prioridad,
                    Cantidad = g.Count()
                });
        }
    }
}