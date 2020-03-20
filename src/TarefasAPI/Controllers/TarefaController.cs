using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;

namespace TarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;
        private readonly UserManager<ApplicationUser> _userManager;

        public TarefaController(ITarefaRepositorio tarefaRepositorio, UserManager<ApplicationUser> userManager)
        {
            _tarefaRepositorio = tarefaRepositorio;
            _userManager = userManager;
        }

        public async Task<IActionResult> Sincronizar([FromBody] List<Tarefa> tarefas)
        {
            return Ok( await _tarefaRepositorio.Sincronizar(tarefas));
        }

        public async Task<IActionResult> Restaurar([FromQuery] DateTime data, [FromBody] List<Tarefa> tarefas)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return Ok( await _tarefaRepositorio.Restaurar(user, data));
        }
    }
}