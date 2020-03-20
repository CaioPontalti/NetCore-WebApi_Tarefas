using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarefasAPI.Database;
using TarefasAPI.Interfaces;
using TarefasAPI.Repositorios;

namespace TarefasAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TarefasContext>(opt =>
            {
                opt.UseSqlServer(_configuration.GetConnectionString("conn"));
            });

            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
