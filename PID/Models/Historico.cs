using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class Historico
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Desenvolvimento")]
        public int IdDesenvolvimento { get; set; }
        public Desenvolvimento Desenvolvimento { get; set; }

        public string Categoria { get; set; }

        [Required]
        public string Descricao { get; set; }

        public DateTime Data { get; set; }

        public string Responsavel { get; set; }
    }
}
