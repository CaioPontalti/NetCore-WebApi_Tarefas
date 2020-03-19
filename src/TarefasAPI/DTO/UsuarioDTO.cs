
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
        [Compare("Password")]
        public string ConfirPassword { get; set; }
    }
}
