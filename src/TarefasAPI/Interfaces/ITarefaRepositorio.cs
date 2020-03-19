using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarefasAPI.Models;

namespace TarefasAPI.Interfaces
{
    public interface ITarefaRepositorio
    {
        Task Sincronizar(List<Tarefa> tarefas);

        Task<List<Tarefa>> Restaurar(DateTime dataUltimaSincronizacao);
    }
}
