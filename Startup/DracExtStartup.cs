using System;
using DracXGG.Providers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;


[assembly: Microsoft.Azure.WebJobs.Hosting.WebJobsStartup(typeof(DracXGG.Startup.DracExtStartup))]
namespace DracXGG.Startup
{
    public class DracExtStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<DracExtServiceProvider>();
            builder.Services.TryAddSingleton<DracExtServiceOperationProvider>();
        }
    }
}
