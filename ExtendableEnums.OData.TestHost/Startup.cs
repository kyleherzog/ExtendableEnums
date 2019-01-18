using System;
using System.Reflection;
using System.Runtime.Serialization;
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
            AddProperty(builder.AddComplexType(typeof(SampleStatus)), nameof(SampleStatus.Value));
            builder.EntitySet<SampleBook>("SampleBooks");

            return builder.GetEdmModel();
        }

        public static StructuralTypeConfiguration AddProperty(StructuralTypeConfiguration config, string propertyName)
        {
            var propertyInfo = config.ClrType.GetProperty(propertyName);
            var property = config.AddProperty(propertyInfo);

            var attribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>(inherit: false);

            if (attribute != null && !String.IsNullOrWhiteSpace(attribute.Name))
            {
                property.Name = attribute.Name;
            }
            else
            {
                var caser = new LowerCamelCaser();
                property.Name = caser.ToLowerCamelCase(propertyInfo.Name);
            }

            return config;
        }
    }
}
