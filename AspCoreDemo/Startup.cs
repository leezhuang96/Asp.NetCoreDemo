using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreDemo.Models;
using AspCoreDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspCoreDemo
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // register service
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddControllers();
            //services.AddMvc();

            services.AddSingleton<IDepartmentService, DepartmentService>();
            services.AddSingleton<IEmployeeService, EmpolyeeService>();

            services.Configure<AspCoreDemoOptions>(configuration.GetSection("AspCoreDemo"));
        }

        // register middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // static files
            app.UseStaticFiles();

            // redirection http request to https request
            app.UseHttpsRedirection();

            // Authentication
            app.UseAuthentication();

            // routing
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                //MVC use routing table
                endpoints.MapControllerRoute("default", "{controller=Department}/{action=Index}/{id?}");
                
                //MVC use controller routing attribute
                endpoints.MapControllers();
            });
        }
    }
}
