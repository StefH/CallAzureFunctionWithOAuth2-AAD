using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;

namespace ConsoleAppStorageQueue
{
    public class T : TokenCredential
    {
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new AccessToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Imh1Tjk1SXZQZmVocTM0R3pCRFoxR1hHaXJuTSIsImtpZCI6Imh1Tjk1SXZQZmVocTM0R3pCRFoxR1hHaXJuTSJ9.eyJhdWQiOiIxZDYzMDljOS0zZTIyLTQyZDgtYmI1OS0zMzQxYTZmODYzMmIiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8wMjBiMGNmMy1kNmIyLTQ2NGUtOWIyZC00NWUxMjQyNDQ0MjgvIiwiaWF0IjoxNTk1ODc0ODYyLCJuYmYiOjE1OTU4NzQ4NjIsImV4cCI6MTU5NTg3ODc2MiwiYWlvIjoiRTJCZ1lMRFF1LzA1N3VQNVlsdnp5N3RtbnhRUUFBQT0iLCJhcHBpZCI6ImM2NGZlYjhlLTQ1NDUtNGYyYy1hMGRkLTZiNzJhOGQxYThiYiIsImFwcGlkYWNyIjoiMSIsImlkcCI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0LzAyMGIwY2YzLWQ2YjItNDY0ZS05YjJkLTQ1ZTEyNDI0NDQyOC8iLCJvaWQiOiJhOGE4YThlYS03YzczLTQ1MmItYTQ4ZC03ZjgyMDFlNGM0MmUiLCJzdWIiOiJhOGE4YThlYS03YzczLTQ1MmItYTQ4ZC03ZjgyMDFlNGM0MmUiLCJ0aWQiOiIwMjBiMGNmMy1kNmIyLTQ2NGUtOWIyZC00NWUxMjQyNDQ0MjgiLCJ1dGkiOiIzR2dVeTQtMWRreTFyZ3pMMDNKWkFBIiwidmVyIjoiMS4wIn0.1_BLIyW-6CdmhenzNknz6ouZGBNGx9G75ruhRcX_JwOFlyq2W3GffqKPWZ9Yb23OxeE4eSwpyh2UWwEAXbgwGq_NVASNrLw6Tn4KLl2erX6fTM11F56Vp_lqR3Fz3NJA3Hesnso19x_cinDrC_oNV8BTrtM5ltAFHEn7R8oYiHyIAeEB-jBG4lYAwzbtnfpzt-OMFWIIyEhgQcGoQS3cpFn3Fy7M4uDqB4Ca9YdWlxFP31q5D2dJRnMReg_fFz9iVAq6MqUiujlnkRqQzVHgNQiaJ2CZv-YgAxwVBUXQXUCOUf7L_iW49bXe5bX3vpVDuqIAjB_NcPXPnBm3Q0fC0Q", DateTime.Now.AddDays(1));
        }

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new ValueTask<AccessToken>(new AccessToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Imh1Tjk1SXZQZmVocTM0R3pCRFoxR1hHaXJuTSIsImtpZCI6Imh1Tjk1SXZQZmVocTM0R3pCRFoxR1hHaXJuTSJ9.eyJhdWQiOiIxZDYzMDljOS0zZTIyLTQyZDgtYmI1OS0zMzQxYTZmODYzMmIiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8wMjBiMGNmMy1kNmIyLTQ2NGUtOWIyZC00NWUxMjQyNDQ0MjgvIiwiaWF0IjoxNTk1ODc0ODYyLCJuYmYiOjE1OTU4NzQ4NjIsImV4cCI6MTU5NTg3ODc2MiwiYWlvIjoiRTJCZ1lMRFF1LzA1N3VQNVlsdnp5N3RtbnhRUUFBQT0iLCJhcHBpZCI6ImM2NGZlYjhlLTQ1NDUtNGYyYy1hMGRkLTZiNzJhOGQxYThiYiIsImFwcGlkYWNyIjoiMSIsImlkcCI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0LzAyMGIwY2YzLWQ2YjItNDY0ZS05YjJkLTQ1ZTEyNDI0NDQyOC8iLCJvaWQiOiJhOGE4YThlYS03YzczLTQ1MmItYTQ4ZC03ZjgyMDFlNGM0MmUiLCJzdWIiOiJhOGE4YThlYS03YzczLTQ1MmItYTQ4ZC03ZjgyMDFlNGM0MmUiLCJ0aWQiOiIwMjBiMGNmMy1kNmIyLTQ2NGUtOWIyZC00NWUxMjQyNDQ0MjgiLCJ1dGkiOiIzR2dVeTQtMWRreTFyZ3pMMDNKWkFBIiwidmVyIjoiMS4wIn0.1_BLIyW-6CdmhenzNknz6ouZGBNGx9G75ruhRcX_JwOFlyq2W3GffqKPWZ9Yb23OxeE4eSwpyh2UWwEAXbgwGq_NVASNrLw6Tn4KLl2erX6fTM11F56Vp_lqR3Fz3NJA3Hesnso19x_cinDrC_oNV8BTrtM5ltAFHEn7R8oYiHyIAeEB-jBG4lYAwzbtnfpzt-OMFWIIyEhgQcGoQS3cpFn3Fy7M4uDqB4Ca9YdWlxFP31q5D2dJRnMReg_fFz9iVAq6MqUiujlnkRqQzVHgNQiaJ2CZv-YgAxwVBUXQXUCOUf7L_iW49bXe5bX3vpVDuqIAjB_NcPXPnBm3Q0fC0Q", DateTime.Now.AddDays(1)));
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("credentialQueueApi");
            var credentialQueueApi = new ClientSecretCredential("020b0cf3-d6b2-464e-9b2d-45e124244428", "1d6309c9-3e22-42d8-bb59-3341a6f8632b", "bvrCA5AXaNI_9v3j2rMQ8jrL4f_Zaf.t.0");
            var client1 = new QueueClient(new Uri("https://stefsa.queue.core.windows.net/example-q"), credentialQueueApi);
            client1.SendMessage("credentialQueueApi" + DateTime.UtcNow);

            
            //TokenCredential tc = new T();
           // var x = new StorageSharedKeyCredential();

            //StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);

            Console.WriteLine("tc");
            var tc = new ClientSecretCredential("020b0cf3-d6b2-464e-9b2d-45e124244428", "4bd5d21b-1a20-4e86-a06a-b7f8598030b3", "jf864wuqkGDGQHf5uYt--be~5nlc3r-nX8");
            var client2 = new QueueClient(new Uri("https://stefsa.queue.core.windows.net/example-q"), tc);
            client2.SendMessage("tc" + DateTime.UtcNow);
        }
    }
}

/*
 * az login

az ad sp create-for-rbac -n "QueueApi" --role 'Storage Queue Data Contributor' --scope '/subscriptions/2de19637-27a3-42a8-812f-2c2a7f7f935c/resourceGroups/stef-ResourceGroup-WestEuropa/providers/Microsoft.Storage/storageAccounts/stefsa'


Changing "QueueApi" to a valid URI of "http://QueueApi", which is the required format used for service principal names
Creating a role assignment under the scope of "/subscriptions/2de19637-27a3-42a8-812f-2c2a7f7f935c/resourceGroups/stef-ResourceGroup-WestEuropa/providers/Microsoft.Storage/storageAccounts/stefsa"
  Retrying role assignment creation: 1/36
{
  "appId": "1d6309c9-3e22-42d8-bb59-3341a6f8632b",
  "displayName": "QueueApi",
  "name": "http://QueueApi",
  "password": "bvrCA5AXaNI_9v3j2rMQ8jrL4f_Zaf.t.0",
  "tenant": "020b0cf3-d6b2-464e-9b2d-45e124244428"
}
 */
