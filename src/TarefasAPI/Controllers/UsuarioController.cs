using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _configuration;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
                                 IConfiguration configuration)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
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
                    return Ok(await GerarToken(usuarioDTO.Email));
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

        [Authorize]
        [HttpGet("logoff")]
        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.FullName = usuario.Nome;
                user.UserName = usuario.Email;
                user.Email = usuario.Email; ;
                var result = await _usuarioRepositorio.Cadastrar(user, usuario.Password);

                if (result.status == true)
                {
                    return Ok();
                }
                else
                {
                    List<string> Erros = new List<string>();
                    foreach (var erro in result.retorno)
                        Erros.Add(erro.Description);

                    return UnprocessableEntity(Erros);
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }

        }

        private async Task<object> GerarToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            List<Claim> claims = new List<Claim>();

            
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); 
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString())); 
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)); 

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identityClaims, 
                Issuer = _configuration["AppSettings:Emissor"],
                Audience = _configuration["AppSettings:ValidoEm"], 
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["AppSettings:ExpiracaoHoras"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return new { Token = encodedToken, Exp = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["AppSettings:ExpiracaoHoras"])) };
        }
    }
}