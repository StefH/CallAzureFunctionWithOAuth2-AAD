using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Azure;

namespace ConsoleAppStorageQueue
{
    internal class TokenCredentialFactory
    {
        private Lazy<TokenCredential> l = new Lazy<TokenCredential>(new ManagedIdentityCredential());


        private ConcurrentDictionary<string, TokenCredential> d = new ConcurrentDictionary<string, TokenCredential>();

        public TokenCredential GetTokenCredential(string clientId)
        {
            // l.IsValueCreated

            return null;// new ClientSecretCredential(queueSenderOptions.TenantId, queueSenderOptions.ClientId, queueSenderOptions.ClientSecret);
        }
    }
}