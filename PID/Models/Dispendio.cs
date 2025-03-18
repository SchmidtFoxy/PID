using System.ComponentModel.DataAnnotations;

namespace PID.Models
{
    public class Dispendio
    {
        [Key]
        public int IdDispendio { get; set; }

        [Required]
        public string Descricao { get; set; }

        public bool ProjetoFinep { get; set; }
        public bool ProjetoLeiBem { get; set; }
    }
}
