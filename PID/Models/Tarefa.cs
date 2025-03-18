using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class Tarefa
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Desenvolvimento")]
        public int IdDesenvolvimento { get; set; }
        public Desenvolvimento Desenvolvimento { get; set; }

        [Required]
        public string Descricao { get; set; }

        public string Status { get; set; }
        public string Prioridade { get; set; }

        public DateTime Data { get; set; }
    }
}
