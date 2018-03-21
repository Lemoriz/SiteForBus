using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkJunior.Mod;

namespace WorkJunior
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("serverconf.json");
          

            services.AddDbContext<DBookingTickets>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builderw =>
                {
                    builderw.AllowCredentials();
                    builderw.AllowAnyOrigin();
                    builderw.AllowAnyMethod();
                    builderw.AllowAnyHeader();
                });

            });

            services.AddSession(o =>
            {
             o.Cookie.HttpOnly = false;
                o.IdleTimeout = TimeSpan.FromMinutes(100);
            //o.Cookie.Expiration = TimeSpan.FromDays(1231);
            
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            //logger.Add

            app.UseCors("AllowAllOrigin");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{action}");

            });
        }
    }
}
