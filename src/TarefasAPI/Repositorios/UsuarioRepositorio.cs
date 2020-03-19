using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;

namespace TarefasAPI.Repositorios
{
    
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepositorio(UserManager<ApplicationUser> userManage)
        {
            _userManager = userManage;
        }

        public async Task<ApplicationUser> Obter(string email, string senha)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario != null)
                return usuario;
            else
                return new ApplicationUser();

        }
        public async Task Cadastrar(ApplicationUser applicationUser, string senha)
        {
            var result =  await _userManager.CreateAsync(applicationUser, senha);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {

                }
            }
        }        
    }
}
