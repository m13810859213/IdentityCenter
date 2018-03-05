using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Butterfly.Client.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API
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
            
            var appSettings = Configuration.GetSection("AppSettings");
            string idCenterUrl = "http://localhost:5000";
            if (appSettings != null)
            {
                idCenterUrl = appSettings.GetSection("IDCenterUrl").Value;
            }
            services.AddMvc();
            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                //options.Authority = idCenterUrl;
                //options.RequireHttpsMetadata = false;
                //options.ApiName = "api1";

                //连接https://demo.identityserver.io/ 验证
                options.Authority = "https://demo.identityserver.io/";
                options.ApiName = "api";
            });

            services.AddButterfly(option =>
            {
                option.CollectorUrl = "http://localhost:9618";
                option.Service = "my service";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
