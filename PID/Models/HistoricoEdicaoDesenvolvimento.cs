using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PID.Models
{
    public class HistoricoEdicaoDesenvolvimento
    {
        public int Id { get; set; }

        public int IdDesenvolvimento { get; set; }
        public Desenvolvimento Desenvolvimento { get; set; }

        public string CampoAlterado { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorAtual { get; set; }

        public DateTime DataAlteracao { get; set; } = DateTime.Now;

        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }


}
