using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NewShoreAIR.Api.Client
{
    public abstract class BaseClient : IBaseClient
    {
        public string BearerToken { get; private set; }

        public void SetBearerToken(string token)
        {
            BearerToken = token;
        }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            // SET THE BEARER AUTH TOKEN

            if (!string.IsNullOrEmpty(BearerToken))
            {
                if(BearerToken.StartsWith("bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    msg.Headers.Add("Authorization", new List<string>() { BearerToken });
                }
                else
                {
                    msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
                }
            }
            
            return Task.FromResult(msg);
        }
    }
}