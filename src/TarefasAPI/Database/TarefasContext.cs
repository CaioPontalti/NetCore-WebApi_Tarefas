using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarefasAPI.Models;

namespace TarefasAPI.Database
{
    public class TarefasContext : IdentityDbContext<ApplicationUser>
    {
        public TarefasContext(DbContextOptions<TarefasContext> opt) : base(opt)
        {
                
        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
