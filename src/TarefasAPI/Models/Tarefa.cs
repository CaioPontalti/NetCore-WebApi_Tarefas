using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasAPI.Models
{
    public class Tarefa
    {
        [Key]
        public int IdTarefaApi { get; set; }

        public int IdTarefaApp { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [Required]
        public string Titulo { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime DataHora { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string Local { get; set; }

        [Column(TypeName = "VARCHAR(1000)")]
        public string Descricao { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Tipo { get; set; }

        public bool Concluido { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime Criado { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime Atualizado { get; set; }

        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
