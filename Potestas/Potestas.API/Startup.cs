using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Potestas.API.Mappers;
using Potestas.API.Services;
using Potestas.API.Services.Implementations;
using Potestas.ORM.Plugin.Models;
using Potestas.ORM.Plugin.Storages;
using Potestas.ORM.Plugin.Analizers;

namespace Potestas.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" }));

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddCors();

            string dbConnection = Configuration["Data:ConnectionStrings:ObservationConnection"];
            services.AddDbContext<ObservationContext>(opt => opt.UseSqlServer(dbConnection));

            services.AddSingleton(ConfigureMapper());
            services.AddScoped<DbContext, ObservationContext>();
            services.AddScoped<IEnergyObservationService, EnergyObservationService>();
            services.AddScoped<IEnergyObservationStorage<IEnergyObservation>, DBStorage<IEnergyObservation>>();
            services.AddScoped<IEnergyObservationAnalizer, ORMAnalizer>();
            services.AddScoped<IResearcherService, ResearcherService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));

            app.UseMvc();
        }

        private IMapper ConfigureMapper()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile(new EnergyObservationMappingProfile()));
            return mapperConfig.CreateMapper();
        }
    }
}
