using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;

namespace TarefasAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;
        private readonly UserManager<ApplicationUser> _userManager;

        public TarefaController(ITarefaRepositorio tarefaRepositorio, UserManager<ApplicationUser> userManager)
        {
            _tarefaRepositorio = tarefaRepositorio;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("sincronizar"), Authorize]
        public async Task<IActionResult> Sincronizar([FromBody] List<Tarefa> tarefas)
        {
            if (tarefas == null)
                return BadRequest();

            return Ok( await _tarefaRepositorio.Sincronizar(tarefas));
        }

        [Authorize]
        [HttpGet("retorno-model")]
        public async Task<IActionResult> ReturnModel()
        {
            return Ok(await Task.FromResult( new Tarefa()));
        }


        [Authorize]
        [HttpGet("restaurar")]
        public async Task<IActionResult> Restaurar([FromQuery] DateTime data, [FromBody] List<Tarefa> tarefas)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return Ok( await _tarefaRepositorio.Restaurar(user, data));
        }
    }
}