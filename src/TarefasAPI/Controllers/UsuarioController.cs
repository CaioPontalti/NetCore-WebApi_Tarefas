using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.DTO;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;

namespace TarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("ConfirPassword");
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepositorio.Obter(usuarioDTO.Email, usuarioDTO.Password);
                if (usuario != null)
                {
                    await _signInManager.SignInAsync(usuario, false);
                    return Ok();
                }
                else 
                {
                    return NotFound("Usuário ou Senha inválidos");
                }
            }
            else 
            {
                return UnprocessableEntity(ModelState);
            }
        }

        public async Task<IActionResult> Cadastrar ([FromBody] UsuarioDTO usuario)
        {
            if (!ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.FullName = usuario.Nome;
                user.Email = usuario.Email; ;
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(usuario);
                }
                else 
                {
                    List<string> Erros = new List<string>();
                    foreach (var item in result.Errors)
                        Erros.Add(item.Description);

                    return UnprocessableEntity(Erros);
                }    
            }
            else 
            {
                return UnprocessableEntity(ModelState);
            }
                
        }
    }
}