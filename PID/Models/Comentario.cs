using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PID.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Texto { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Relacionamento com Desenvolvimento
        public int DesenvolvimentoId { get; set; }
        public Desenvolvimento Desenvolvimento { get; set; }

        // Relacionamento com Usuario
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

}
