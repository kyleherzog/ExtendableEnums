using System;
using System.Reflection;
using System.Runtime.Serialization;
using ExtendableEnums.Microsoft.AspNetCore.OData;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace ExtendableEnums.OData.TestHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc(routebuilder =>
            {
                routebuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routebuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.AddExtendableEnum<SampleStatus>();
            // AddProperty(builder.AddComplexType(typeof(SampleStatus)), nameof(SampleStatus.Value));
            builder.EntitySet<SampleBook>("SampleBooks");

            return builder.GetEdmModel();
        }
    }
}
