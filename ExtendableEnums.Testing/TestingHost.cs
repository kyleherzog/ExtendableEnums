using System.Net;
using ExtendableEnums.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;

namespace ExtendableEnums.Testing;

public class TestingHost : IDisposable
{
    private bool hasDisposed;
    private IHost? host;

    public TestingHost(Type startupType, string solutionRelativeRootPath, bool deferWebHostCreation = false)
    {
        StartupType = startupType;
        SolutionRelativeRootPath = solutionRelativeRootPath;
        if (!deferWebHostCreation)
        {
            GetNewWebHostInternal();
        }
    }

    ~TestingHost()
    {
        Dispose(false);
    }

    public static TestingHost? Instance { get; set; }

    public string Address
    {
        get
        {
            if (host is null)
            {
                return string.Empty;
            }

            var server = host.Services.GetRequiredService<IServer>();

            var feature = server.Features.Get<IServerAddressesFeature>();
            if (feature is null)
            {
                throw new InvalidOperationException("Unable to find the 'IServerAddressFeature' to read the address.");
            }

            return feature.Addresses.First();
        }
    }

    public Uri BaseODataUrl
    {
        get
        {
            return new Uri($"{Address}/odata");
        }
    }

    public string SolutionRelativeRootPath { get; set; }

    public Type StartupType { get; }

    public static TestingHost GetRequiredInstance()
    {
        if (Instance is null)
        {
            throw new InvalidOperationException("The TestingHost Instance must be set first.");
        }

        return Instance;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task GetNewWebHost()
    {
        if (host is not null)
        {
            await host.StopAsync().ConfigureAwait(false);
            host?.Dispose();
        }

        GetNewWebHostInternal();
        DataContext.ResetData();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!hasDisposed)
        {
            if (disposing)
            {
                host?.Dispose();
            }

            hasDisposed = true;
        }
    }

    private static string GetSolutionRelativeContentRoot(string path)
    {
        var solutionRoot = new DirectoryInfo(PlatformServices.Default.Application.ApplicationBasePath) // netcoreapp#.# folder
            .Parent // Debug or Release folder
            ?.Parent // bin folder
            ?.Parent // project folder
            ?.Parent?.FullName;  // solution folder

        if (solutionRoot is null)
        {
            throw new FileNotFoundException("Unable to find the solution root.");
        }

        var result = Path.GetFullPath(Path.Combine(solutionRoot, path));
        return result;
    }

    private void GetNewWebHostInternal()
    {
        host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 0);
                })
                .UseContentRoot(GetSolutionRelativeContentRoot(SolutionRelativeRootPath))
                .UseStartup(StartupType);
            })

            .Build();

        host.Start();
    }
}