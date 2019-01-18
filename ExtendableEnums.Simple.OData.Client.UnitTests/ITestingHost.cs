using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ExtendableEnums.SimpleOData.Client.UnitTests
{
    public interface ITestingHost : IDisposable
    {
        string Address { get; }

        Uri BaseODataUrl { get; }

        Task<IWebHost> GetNewWebHost();
    }
}
