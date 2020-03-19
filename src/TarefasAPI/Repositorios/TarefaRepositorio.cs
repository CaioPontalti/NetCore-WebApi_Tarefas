using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TarefasAPI.Database;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;

namespace TarefasAPI.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly TarefasContext _context;
        private readonly IHttpContextAccessor _accessor;

        public TarefaRepositorio(TarefasContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            accessor = _accessor;
        }

        public async Task<List<Tarefa>> Restaurar(DateTime dataUltimaSincronizacao)
        {
            if (dataUltimaSincronizacao != null)
                return await Task.FromResult( _context.Tarefas.Where(t => t.Atualizado >= dataUltimaSincronizacao ||
                                             t.Criado >= dataUltimaSincronizacao).ToList() );
            else
                return await Task.FromResult( _context.Tarefas.ToList() );
        }

        public Task Sincronizar(List<Tarefa> tarefas)
        {
            throw new NotImplementedException();
        }
    }
}
