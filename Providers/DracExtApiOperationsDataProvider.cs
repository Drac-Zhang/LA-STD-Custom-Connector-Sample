using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;

namespace DracXGG.Providers
{
    public static class DracExtApiOperationsDataProvider
    {
        public static ServiceOperationApi GetServiceOperationApi()
        {
            return new ServiceOperationApi()
            {
                Name = "DracExt",
                Id = "/serviceProviders/DracExt",
                Type = DesignerApiType.ServiceProvider,
                Properties = new ServiceOperationApiProperties
                {
                    BrandColor = 4287090426,
                    IconUri = new Uri(DracXGG.Properties.Resources.IconUri),
                    Description = "DracExt",
                    DisplayName = "DracExt",
                    Capabilities = new ApiCapability[] {  ApiCapability.Actions },
                    ConnectionParameters = new ConnectionParameters
                    {
                        ConnectionString = new ConnectionStringParameters
                        {
                            Type = ConnectionStringType.SecureString,
                            ParameterSource = ConnectionParameterSource.AppConfiguration,
                            UIDefinition = new UIDefinition
                            {
                                DisplayName = "Connection String",
                                Description = "test connection string",
                                Tooltip = "test connection string",
                                Constraints = new Constraints
                                {
                                    Required = "true"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
