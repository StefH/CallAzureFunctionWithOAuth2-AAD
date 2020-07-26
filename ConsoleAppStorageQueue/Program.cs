using System;
using Azure.Identity;
using Azure.Storage.Queues;

namespace ConsoleAppStorageQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            var tc = new ClientSecretCredential("020b0cf3-d6b2-464e-9b2d-45e124244428", "4bd5d21b-1a20-4e86-a06a-b7f8598030b3", "jf864wuqkGDGQHf5uYt--be~5nlc3r-nX8");
            //var credentialQueueApi = new ClientSecretCredential("020b0cf3-d6b2-464e-9b2d-45e124244428", "1d6309c9-3e22-42d8-bb59-3341a6f8632b", "bvrCA5AXaNI_9v3j2rMQ8jrL4f_Zaf.t.0");
            var client = new QueueClient(new Uri("https://stefsa.queue.core.windows.net/example-q"), tc);

            // this fails with error
            client.SendMessage("msg");
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
