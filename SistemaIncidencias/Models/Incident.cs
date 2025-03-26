using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaIncidencias.Models
{
    public enum IncidentStatus
    {
        Abierta,
        EnProgreso,
        Resuelta,
        Cerrada
    }

    public enum IncidentPriority
    {
        Baja,
        Media,
        Alta,
        Critica
    }

    public class Incident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        public IncidentStatus Estado { get; set; }

        public IncidentPriority Prioridad { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaUltimaActualizacion { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioReporteId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Tecnico")]
        public int? TecnicoAsignadoId { get; set; }
        public virtual Tecnico Tecnico { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }
    }

    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Incident> Incidencias { get; set; }
    }

    public class Tecnico
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Incident> Incidencias { get; set; }
    }

    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("Incident")]
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}