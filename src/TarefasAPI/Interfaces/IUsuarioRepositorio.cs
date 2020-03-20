using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarefasAPI.Models;

namespace TarefasAPI.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<ApplicationUser> Obter(string email, string senha);

        Task<dynamic> Cadastrar(ApplicationUser applicationUser, string senha);
    }
}
