using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace TaskApplicationApiIntegrationTests
{
    public static class IntegrationTestsHelper
    {
        public static async Task<string> GetAccessToken(string auth0TokenRequestBody)
        {
            var client = new RestClient("https://dev-1ytqfri14i4zv6zm.us.auth0.com/oauth/token");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", auth0TokenRequestBody, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);

            return response.Content;
        }

        public static async Task SetAuthorizationHeader(RestRequest request, string auth0TokenRequestBody)
        {
            var tokenJson = await GetAccessToken(auth0TokenRequestBody);
            var token = JsonConvert.DeserializeObject<Token>(tokenJson);
            request.AddHeader("authorization", $"Bearer {token.access_token}");
        }

        internal class Token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }
    }
}
