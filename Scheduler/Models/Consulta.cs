using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class Consulta
    {
        [Key]
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        public User Medico { get; set; }
        public User Paciente { get; set; }
    }
}
