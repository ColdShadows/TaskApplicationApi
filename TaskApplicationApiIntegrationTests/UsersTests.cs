using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using TaskApplicationApi.Models;

namespace TaskApplicationApiIntegrationTests
{
    public class UsersTests
    {
        private IConfiguration Configuration { get; }

        public UsersTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<UsersTests>();

            Configuration = builder.Build();
        }

        [Fact]
        public async Task Post()
        {
            var user = new User
            {
                FirstName = "First",
                LastName = "Last"
            };

            var userJson = JsonConvert.SerializeObject(user);

            var auth0TokenRequestBody = Configuration["Auth0:TokenRequestBody"];
            var client = new RestClient("https://localhost:44377/api/users");

            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", userJson, ParameterType.RequestBody);
            await IntegrationTestsHelper.SetAuthorizationHeader(request, auth0TokenRequestBody);

            RestResponse response = await client.ExecuteAsync(request);

            response.StatusCode.Should().BeOneOf(System.Net.HttpStatusCode.Created, System.Net.HttpStatusCode.Conflict);
        }    
    }
}
