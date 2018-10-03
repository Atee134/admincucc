using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Services;
using Ag.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ag.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<AgDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Ag"), o => o.MigrationsAssembly("Ag.Domain")), ServiceLifetime.Scoped);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIncomeService, IncomeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();
            }

            //   app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
