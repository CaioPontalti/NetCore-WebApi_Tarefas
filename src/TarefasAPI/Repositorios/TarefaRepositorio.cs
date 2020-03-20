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
        //private readonly IHttpContextAccessor _accessor;

        public TarefaRepositorio(TarefasContext context)
        {
            _context = context;
            //accessor = _accessor;
        }

        public async Task<List<Tarefa>> Restaurar(ApplicationUser applicationUser, DateTime dataUltimaSincronizacao)
        {
            if (dataUltimaSincronizacao != null)
            {
                return await Task.FromResult(_context.Tarefas.Where(t => t.Atualizado >= dataUltimaSincronizacao ||
                                            t.Criado >= dataUltimaSincronizacao).ToList());
            }
            else
            {
                return await Task.FromResult(_context.Tarefas.ToList());
            }
        }

        public async Task<List<Tarefa>> Sincronizar(List<Tarefa> tarefas)
        {
            var novasTarefas = tarefas.Where(t => t.IdTarefaApi == 0).ToList();
            var tarefasAtualizadasExcluidas = tarefas.Where(t => t.IdTarefaApi != 0).ToList();
            
            if (novasTarefas.Count() > 0)
            {
                foreach (var tarefa in novasTarefas)
                {
                    await _context.Tarefas.AddAsync(tarefa);
                }
            }

            if (tarefasAtualizadasExcluidas.Count() > 0)
            {
                foreach (var tarefa in tarefasAtualizadasExcluidas)
                {
                    _context.Tarefas.Update(tarefa);
                }
            }

            await _context.SaveChangesAsync();

            return await Task.FromResult(novasTarefas.ToList());
        }
    }
}
