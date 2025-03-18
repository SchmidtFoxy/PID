using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class Custo
    {
        [Key]
        public int Id { get; set; }

        [Required]  // O ID é obrigatório
        [ForeignKey("Dispendio")]
        public int IdDispendio { get; set; }

        public Dispendio? Dispendio { get; set; }  // Torna a navegação opcional

        [Required]
        public string Descricao { get; set; }

        [Required]  // O ID é obrigatório
        [ForeignKey("Desenvolvimento")]
        public int IdDesenvolvimento { get; set; }

        public Desenvolvimento? Desenvolvimento { get; set; }  // Torna a navegação opcional

        [Required]
        public decimal Valor { get; set; }  // Mudança de float para decimal para evitar perda de precisão

        [Required]
        public DateTime Data { get; set; }
    }
}
