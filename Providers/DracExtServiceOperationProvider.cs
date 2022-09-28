using Microsoft.Azure.Workflows.ServiceProviders.Abstractions;
using Microsoft.WindowsAzure.ResourceStack.Common.Collections;
using Microsoft.WindowsAzure.ResourceStack.Common.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DracXGG.Providers
{
    [ServiceOperationsProvider(Id = DracExtServiceOperationProvider.ServiceId, Name = DracExtServiceOperationProvider.ServiceName)]
    public class DracExtServiceOperationProvider : IServiceOperationsProvider
    {
        public const string ServiceName = "DracExt";
        public const string ServiceId = "/serviceProviders/DracExt";
        private readonly List<ServiceOperation> serviceOperationList;
        private readonly InsensitiveDictionary<ServiceOperation> apiOperationList;

        public DracExtServiceOperationProvider()
        {
            serviceOperationList = new List<ServiceOperation>();
            apiOperationList = new InsensitiveDictionary<ServiceOperation>();

            this.apiOperationList.AddRange(new InsensitiveDictionary<ServiceOperation>
            {
                { "SHA256Encode", Sha256ApiOperationManifest.Operation }
            });

            this.apiOperationList.AddRange(new InsensitiveDictionary<ServiceOperation>
            {
                { "Generate Workspace Signature", SignGeneratorApiOperationManifest.Operation }
            });

            this.serviceOperationList.AddRange(new List<ServiceOperation>
            {
                { Sha256ApiOperationManifest.Operation.CloneWithManifest(Sha256ApiOperationManifest.operationManifest)},
                { SignGeneratorApiOperationManifest.Operation.CloneWithManifest(SignGeneratorApiOperationManifest.operationManifest)}
            });
        }

        string IServiceOperationsProvider.GetBindingConnectionInformation(string operationId, InsensitiveDictionary<JToken> connectionParameters)
        {
            return ServiceOperationsProviderUtilities.GetRequiredParameterValue(serviceId: ServiceId, operationId: operationId, parameterName: "connectionString", parameters: connectionParameters)?.ToValue<string>();
        }

        IEnumerable<ServiceOperation> IServiceOperationsProvider.GetOperations(bool expandManifest)
        {
            return expandManifest ? serviceOperationList : GetApiOperations();
        }

        ServiceOperationApi IServiceOperationsProvider.GetService()
        {
            return DracExtApiOperationsDataProvider.GetServiceOperationApi();
        }

        Task<ServiceOperationResponse> IServiceOperationsProvider.InvokeOperation(string operationId, InsensitiveDictionary<JToken> connectionParameters, ServiceOperationRequest serviceOperationRequest)
        {
            if (operationId == "Generate Workspace Signature")
            {
                //string result = String.Empty;

                try
                {
                    string workspaceID = serviceOperationRequest.Parameters["WorkspaceID"].ToString();
                    string dateString = serviceOperationRequest.Parameters["DateString"].ToString();
                    string payload = serviceOperationRequest.Parameters["Payload"].ToString();
                    string key = serviceOperationRequest.Parameters["Key"].ToString();

                    byte[] jsonBytes = Encoding.UTF8.GetBytes(payload);
                    string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + dateString + "\n/api/logs";

                    var encoding = new System.Text.ASCIIEncoding();
                    byte[] keyByte = Convert.FromBase64String(key);
                    byte[] messageBytes = encoding.GetBytes(stringToHash);

                    string sign = "No Signature Generated";

                    using (var hmacsha256 = new HMACSHA256(keyByte))
                    {
                        byte[] hash = hmacsha256.ComputeHash(messageBytes);
                        sign = "SharedKey " + workspaceID + ":" + Convert.ToBase64String(hash);
                    }

                    string url = "https://" + workspaceID + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Log-Type", "WVDTest");
                    client.DefaultRequestHeaders.Add("Authorization", sign);
                    client.DefaultRequestHeaders.Add("x-ms-date", dateString);

                    System.Net.Http.HttpContent httpContent = new StringContent(payload, Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    Task<System.Net.Http.HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

                    System.Net.Http.HttpContent responseContent = response.Result.Content;
                    string result = responseContent.ReadAsStringAsync().Result;

                    return Task.FromResult(new ServiceOperationResponse(JObject.FromObject(new { Signature = result, ContentLength = jsonBytes.Length.ToString() }), System.Net.HttpStatusCode.OK));
                }
                catch (Exception ex)
                {
                    return Task.FromResult(new ServiceOperationResponse(JObject.FromObject(new { Signature = ex.Message }), System.Net.HttpStatusCode.OK));
                }
            }

            return Task.FromResult(new ServiceOperationResponse(JObject.FromObject(new { Status = "response" }), System.Net.HttpStatusCode.Created));
        }

        private IEnumerable<ServiceOperation> GetApiOperations()
        {
            return this.apiOperationList.Values;
        }
    }
}
