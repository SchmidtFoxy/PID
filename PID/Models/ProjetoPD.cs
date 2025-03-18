using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class ProjetoPD
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Ano { get; set; }

        // Chave estrangeira para Desenvolvimento
        [Required]
        public int IdDesenvolvimento { get; set; }
        [ForeignKey("IdDesenvolvimento")]
        public Desenvolvimento? Desenvolvimento { get; set; }

        // Chave estrangeira para Dispendio
        [Required]
        public int IdDispendio { get; set; }
        [ForeignKey("IdDispendio")]
        public Dispendio? Dispendio { get; set; }

        public bool ProjetoFinep { get; set; }
        public bool ProjetoLeiBem { get; set; }
    }
}
