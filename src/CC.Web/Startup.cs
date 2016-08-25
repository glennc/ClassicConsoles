using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CC.Web.Services;

namespace CC.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public IHostingEnvironment Environment {get; private set;}

        public Startup(IHostingEnvironment env)
        {
            Environment = env;
            Configuration = new ConfigurationBuilder()
                                    .SetBasePath(Environment.ContentRootPath)
                                    .AddJsonFile("config.json")
                                    .AddEnvironmentVariables()
                                    .Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WebSettings>(Configuration);

            //Add InMemoryDistributedCache so that when running on my local machine I can still use the
            //cache types.
            services.AddDistributedMemoryCache();
            services.AddMvc();
            //TODO: This could possible be singleton, assuming HTTPClient is fine to be a singleton.
            services.AddTransient<ICatalogService, CatalogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
