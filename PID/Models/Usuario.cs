using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PID.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [StringLength(20)]
        public string Cargo { get; set; } 

        [StringLength(100)]
        public string Departamento { get; set; }

        [StringLength(250)]
        public string FotoUrl { get; set; } 

        [StringLength(50)]
        public string NivelPermissao { get; set; }

        public bool Ativo { get; set; } = true; 

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? UltimoLogin { get; set; }

    }
}
