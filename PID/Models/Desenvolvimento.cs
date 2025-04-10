using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PID.Models
{
    public class Desenvolvimento
    {
        [Key]
        public int IdDesenvolvimento { get; set; }

        [Required]
        public string Classificacao { get; set; } = string.Empty;

        [Required]
        public string Dificuldade { get; set; } = string.Empty;

        [Required]
        public string Produto { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public int ERP { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public bool ProjetoFinep { get; set; }
        public bool ProjetoLeiBem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Custo { get; set; } = 0;

        public string Fase { get; set; } = "Inicial";
        public string Status { get; set; } = "Novo";

        public string Solicitante { get; set; } = string.Empty;

        public string TempoDesenvolvimento
        {
            get
            {
                return (DataFim > DataInicio)
                    ? $"{(DataFim - DataInicio).Days / 30} meses"
                    : "0 meses";
            }
        }

        public ICollection<Custo> Custos { get; set; } = new List<Custo>();

        [Required(ErrorMessage = "Selecione um projeto.")]
        public int ProjetoPDId { get; set; }

        [ForeignKey("ProjetoPDId")]
        public ProjetoPD? ProjetoPD { get; set; }

        // Adicionando os relacionamentos com Comentarios e HistoricoEdicoes
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public ICollection<HistoricoEdicaoDesenvolvimento> HistoricoEdicoes { get; set; } = new List<HistoricoEdicaoDesenvolvimento>();


    }
}
