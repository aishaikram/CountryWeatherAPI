using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryWeatherAPI.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using CountryWeatherAPI.Services;

namespace CountryWeatherAPI
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //  .AddEnvironmentVariables();


            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //alows to configure formatter for the input/output
            //  .AddMvcOptions(o=>o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            /*
             * //sets the settings of serializer options of json //used Json.NET
            //this sets the json to use the property names as they r defined instead of converting them to lower case

            .AddJsonOptions(o=> 
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            });
            */
            //AI- by default registered with scoped lifetime
            //trusted connection using windows credentials specified in appSettings file used in Development environment mode
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];

            //registers the db context service for the specified database with the services
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            //Transient created for each request, scoped, once for a request, Singledon created on first time 
            //alos can be replaced here with mock repository for testing
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            //AI- added logging to the console in case of error
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseMvc();
            // seed the data
            cityInfoContext.EnsureSeedDataforContext();

            // add mappings into the AutoMapper nuget package
            AutoMapper.Mapper.Initialize(cfg =>
            {
                //map takes return type , input param type
                cfg.CreateMap<Entities.City, Models.CityDTO>();
                cfg.CreateMap<Models.CityForCreationDTO, Entities.City>();
                cfg.CreateMap<Models.CityForUpdateDTO, Entities.City>();
                
            });

            // app.Run((context) =>
            //{
            //    throw new Exception("Aisha's exception");
            //});

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
