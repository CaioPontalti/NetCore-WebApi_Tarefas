
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasAPI.DTO
{
    public class UsuarioDTO
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage ="Informe um e-mail válido")]
        public string Email { get; set; }
       
        [Required]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Os campo de Password e ConfirPassword não estão iguais.")]
        public string ConfirPassword { get; set; }
    }
}
