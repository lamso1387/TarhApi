using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarhApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Routing;
using TarhApi.Services;
using Newtonsoft.Json; 
using TarhApi.Controllers;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using TarhApi.Middleware;

namespace TarhApi
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
            services.AddScoped<UserService>();
            services.AddScoped<ILogger, Logger<DefaultController>>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;//disables asp core automatic bad request
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();//avoid cameCase or Uppercase or any convert just default
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;//avoid loop in db entity load reference foriegn entity (because of Include) BUT not works . use  [JsonIgnore] in properties instead or select new properies to return
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());//convert enum to api in api responce
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;//not result null item in api resporce
                                                                                                            // options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;//set request input to null if request has extra fields

                });
            services.AddDbContext<TarhDb>(builder =>
            {
                builder.UseSqlServer(Configuration[Constants.Setting.TarhDbConfugrationValue]);
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tarh API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger Logger)// IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.WithOrigins("http://localhost", "http://localhost:4200").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseGetRoutesMiddleware(GetRoutes);
            app.UseMiddleware<CustomHandlerMiddleware>();
 
            if (env.IsDevelopment())
            {
         //       app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(GetRoutes);

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tarh API V1");
            });


            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TarhDb>();
                context.Database.Migrate();

            }
        }

        private readonly Action<IRouteBuilder> GetRoutes =
    routes =>
    {
        routes.MapRoute(
        name: "default",
        template: "{controller=deposit}/{action=test}/{id?}");
    };
    }
}
