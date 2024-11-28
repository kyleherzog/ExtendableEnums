using ExtendableEnums.Microsoft.AspNetCore;
using ExtendableEnums.Microsoft.AspNetCore.OData;
using ExtendableEnums.Testing.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ExtendableEnums.TestHost;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder app)
    {
        DataContext.ResetData();

        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine(Configuration);
        services.AddControllers().AddOData(opt =>
        {
            opt.Select().Expand().Filter().OrderBy().SetMaxTop(100).Count();
            opt.AddRouteComponents("odata", GetEdmModel());
        });
        services.AddMvc(options =>
        {
            options.UseExtendableEnumModelBinding();
        });
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.AddExtendableEnum<SampleStatus>();
        builder.EntitySet<SampleBook>("SampleBooks");

        return builder.GetEdmModel();
    }
}