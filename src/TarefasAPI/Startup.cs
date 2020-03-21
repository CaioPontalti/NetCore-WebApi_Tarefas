using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TarefasAPI.Database;
using TarefasAPI.Interfaces;
using TarefasAPI.Models;
using TarefasAPI.Repositorios;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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
            //desabilita o retorno automatico da validação da model, para personalizar as msgs de retorno de erro no response.
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<TarefasContext>(opt =>
            {
                opt.UseSqlServer(_configuration.GetConnectionString("conn"));
            });

            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Toda autenticação é para gerar token
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //Todo request verifica se está autenticado
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; //apenas https
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["AppSettings:ValidoEm"],
                    ValidIssuer = _configuration["AppSettings:Emissor"],
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build()
                );
            });


            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TarefasContext>();


            //Opção caso o Authorize não esteja funcionando  
            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication(); // this one first
            app.UseMvc();
        }
    }
}
