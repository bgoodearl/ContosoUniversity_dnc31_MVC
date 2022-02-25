using System;
//using System.Threading.Tasks;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CUD = ContosoUniversity.DAL;

namespace ContosoUniversity
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
            services.AddControllersWithViews();

            //Inject HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Inject Entity Framework Repository Factory
            string connStr = Configuration["ConnectionStrings:SchoolDbContext"];

            if (string.IsNullOrWhiteSpace(connStr))
            {
                //Logger.LogError("Connection string not configured");
                throw new Exception("Connection String not configured");
            }
            else
            {
                services.AddSingleton<ISchoolRepositoryFactory>(xp => new CUD.Repositories.SchoolRepositoryFactory(connStr));
                services.AddSingleton<ISchoolViewDataRepositoryFactory, CUD.Repositories.SchoolViewDataRepositoryFactory>();
                services.AddSingleton<ISchoolDbContextFactory>(sp => new CUD.SchoolDbContextFactory(connStr));
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
