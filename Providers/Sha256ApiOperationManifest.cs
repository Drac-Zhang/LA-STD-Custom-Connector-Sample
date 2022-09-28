using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracXGG.Providers
{
    public static class Sha256ApiOperationManifest
    {
        internal const string OperationId = "SHA256Encode";
        internal static readonly ServiceOperationManifest operationManifest;

        static Sha256ApiOperationManifest()
        {
            operationManifest = new ServiceOperationManifest()
            {
                ConnectionReference = new ConnectionReferenceFormat
                {
                    ReferenceKeyFormat = ConnectionReferenceKeyFormat.ServiceProvider,
                },
                Settings = new OperationManifestSettings()
                {
                    SecureData = new OperationManifestSettingWithOptions<SecureDataOptions>(),
                    TrackedProperties = new OperationManifestSetting()
                    {
                        Scopes = new OperationScope[] { (OperationScope.Action) }
                    },
                    RetryPolicy = new OperationManifestSetting()
                    {
                        Scopes = new OperationScope[] { OperationScope.Action }
                    }
                },
                InputsLocation = new InputsLocation[]
                {
                    InputsLocation.Inputs,
                    InputsLocation.Parameters
                },
                Inputs = new SwaggerSchema
                {
                    Type = SwaggerSchemaType.Object,
                    Properties = new OrdinalDictionary<SwaggerSchema>
                    {
                        {
                            "content", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "content",
                                Description = "content"
                            }
                        }
                    },
                    Required = new string[] { "content" }
                },
                Outputs = new SwaggerSchema
                {
                    Type = SwaggerSchemaType.Object,
                    Properties = new OrdinalDictionary<SwaggerSchema>
                    {
                        {
                            "body", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "body",
                                Description = "body"
                            }
                        }
                    }
                },
                Connector = DracExtApiOperationsDataProvider.GetServiceOperationApi()
            };
        }

        internal static readonly ServiceOperation Operation = new ServiceOperation()
        {
            Name = "SHA256Encode",
            Id = "SHA256Encode",
            Type = "SHA256Encode",
            Properties = new ServiceOperationProperties()
            {
                Api = DracExtApiOperationsDataProvider.GetServiceOperationApi().GetFlattenedApi(),
                Summary = "Encode with SHA 256",
                Description = "Encode with SHA 256",
                Visibility = Visibility.Important,
                OperationType = OperationType.ServiceProvider,
                BrandColor = DracExtApiOperationsDataProvider.GetServiceOperationApi().Properties.BrandColor,
                IconUri = DracExtApiOperationsDataProvider.GetServiceOperationApi().Properties.IconUri,
                Annotation = new ServiceOperationAnnotation()
                {
                    Status = StatusAnnotation.Preview,
                    Family = "/serviceProviders/DracExt"
                }
            }
        };
    }
}
