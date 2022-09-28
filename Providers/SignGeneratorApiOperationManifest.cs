using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Swagger.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracXGG.Providers
{
    public static class SignGeneratorApiOperationManifest
    {
        internal const string OperationId = "Generate Workspace Signature";
        internal static readonly ServiceOperationManifest operationManifest;

        static SignGeneratorApiOperationManifest()
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
                            "WorkspaceID", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "WorkspaceID",
                                Description = "WorkspaceID"
                            }
                        },
                        {
                            "DateString", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "DateString",
                                Description = "DateString"
                            }
                        },
                        {
                            "Payload", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "Payload",
                                Description = "Payload"
                            }
                        },
                        { 
                            "Key", new SwaggerSchema
                            { 
                                Type = SwaggerSchemaType.String,
                                Title = "SignatureKey",
                                Description = "Workspace Signature Key"
                            }
                        }
                    },
                    Required = new string[] { "WorkspaceID", "DateString", "Payload", "Key" }
                },
                Outputs = new SwaggerSchema
                {
                    Type = SwaggerSchemaType.Object,
                    Properties = new OrdinalDictionary<SwaggerSchema>
                    {
                        {
                            "Body", new SwaggerSchema
                            {
                                Type = SwaggerSchemaType.String,
                                Title = "Body",
                                Description = "Body"
                            }
                        }
                    }
                },
                Connector = DracExtApiOperationsDataProvider.GetServiceOperationApi()
            };
        }

        internal static readonly ServiceOperation Operation = new ServiceOperation()
        {
            Name = "Generate Workspace Signature",
            Id = "Generate Workspace Signature",
            Type = "Generate Workspace Signature",
            Properties = new ServiceOperationProperties()
            {
                Api = DracExtApiOperationsDataProvider.GetServiceOperationApi().GetFlattenedApi(),
                Summary = "Generate Workspace Signature",
                Description = "Generate Workspace Signature",
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
