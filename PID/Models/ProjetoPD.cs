using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class ProjetoPD
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        public int Ano { get; set; }

        public bool ProjetoFinep { get; set; }
        public bool ProjetoLeiBem { get; set; }

        public ICollection<Desenvolvimento> Desenvolvimentos { get; set; } = new List<Desenvolvimento>();

        [Required]
        public int IdDispendio { get; set; }
        [ForeignKey("IdDispendio")]
        public Dispendio? Dispendio { get; set; }
    }
}
