using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Azure.Storage.Sas;
using ConsoleAppStorageQueue.Options;

namespace ConsoleAppStorageQueue
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var o = Microsoft.Extensions.Options.Options.Create(new QueueSenderOptions
            {
                TenantId = "020b0cf3-d6b2-464e-9b2d-45e124244428",
                ClientId = "1d6309c9-3e22-42d8-bb59-3341a6f8632b",
                ClientSecret = "bvrCA5AXaNI_9v3j2rMQ8jrL4f_Zaf.t.0",
                QueueUri = new Uri("https://stefsa.queue.core.windows.net/example-q")
            }); ;

            var client = new QueueSender(o);
            var messageId = await client.SendAsync(new { });
            Console.WriteLine(messageId);
            return;

            Console.WriteLine("credentialQueueApi");
            var credentialQueueApi = new ClientSecretCredential("020b0cf3-d6b2-464e-9b2d-45e124244428", "1d6309c9-3e22-42d8-bb59-3341a6f8632b", "bvrCA5AXaNI_9v3j2rMQ8jrL4f_Zaf.t.0");
            var client1 = new QueueClient(new Uri("https://stefsa.queue.core.windows.net/example-q"), credentialQueueApi);
            client1.SendMessage("credentialQueueApi" + DateTime.UtcNow);


            var s = new StorageSharedKeyCredential("stefsa", "K4HEyKrlX3Bv5kI7Xua73+7hgMdU+X1dFXeNzxEZkN1Y6cAGQsWaeAdJMZhAzYTRXT2eR79TUlhb17x9QpbkXg==");
            var client3 = new QueueClient(new Uri("https://stefsa.queue.core.windows.net/example-q"), s);
            client3.SendMessage("StorageSharedKeyCredential" + DateTime.UtcNow);

            // Create a SAS token that's valid for one hour.
            var sasBuilder = new QueueSasBuilder
            {
                QueueName = "example-q",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Specify read permissions for the SAS.
            sasBuilder.SetPermissions(QueueSasPermissions.Add);

            // Use the key to get the SAS token.
            string sasToken = sasBuilder.ToSasQueryParameters(s).ToString();

            // Construct the full URI, including the SAS token.
            UriBuilder fullUri = new UriBuilder
            {
                Scheme = "https",
                Host = "stefsa.queue.core.windows.net",
                Path = "example-q",
                Query = sasToken
            };

            var q4 = new QueueClient(fullUri.Uri, null);
            q4.SendMessage("QueueSasBuilder " + DateTime.UtcNow);

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
