using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExtendableEnums.OData.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace ExtendableEnums.SimpleOData.Client.UnitTests
{
    public class TestingHost : IDisposable
    {
        private bool hasDisposed;
        private IWebHost host;

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

        public static TestingHost Instance { get; set; }

        public string Address
        {
            get
            {
                return host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IWebHost> GetNewWebHost()
        {
            if (host != null)
            {
                await host.StopAsync().ConfigureAwait(false);
                if (host != null)
                {
                    host.Dispose();
                }
            }

            GetNewWebHostInternal();
            DataContext.ResetData();
            return host;
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
                .Parent // bin folder
                .Parent // project folder
                .Parent.FullName;  // solution folder

            var result = Path.GetFullPath(Path.Combine(solutionRoot, path));
            return result;
        }

        private void GetNewWebHostInternal()
        {
            host = new WebHostBuilder()
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .UseKestrel()
                .UseContentRoot(GetSolutionRelativeContentRoot(SolutionRelativeRootPath))
                .UseStartup(StartupType)
                .Build();

            host.Start();
        }
    }
}
