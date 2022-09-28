using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;

namespace DracXGG.Providers
{
    [Extension("DracExtServiceProvider", configurationSection: "DracExtServiceProvider")]
    public class DracExtServiceProvider : IExtensionConfigProvider
    {
        public DracExtServiceProvider(ServiceOperationsProvider serviceOperationsProvider, DracExtServiceOperationProvider operationProvider)
        {
            serviceOperationsProvider.RegisterService(serviceName: DracExtServiceOperationProvider.ServiceName, serviceOperationsProviderId: DracExtServiceOperationProvider.ServiceId, serviceOperationsProviderInstance: operationProvider);
        }
        public void Initialize(ExtensionConfigContext context)
        {
        }
    }
}
