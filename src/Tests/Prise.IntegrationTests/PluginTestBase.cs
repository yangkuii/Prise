using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Prise.IntegrationTests
{
    public class PluginTestBase
    {
        protected readonly HttpClient _client;
        protected readonly AppHostWebApplicationFactory _factory;

        protected PluginTestBase(
                 AppHostWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            // Run tests locally
            //_client = new HttpClient();
            //_client.BaseAddress = new Uri("https://localhost:5001");
        }

        protected async Task<T> Post<T>(HttpClient client, string pluginType, string endpoint, object content)
        {
            client.DefaultRequestHeaders.Add("PluginType", pluginType);
            var response = await client.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                                                "application/json"));
            if (!response.IsSuccessStatusCode)
                throw new Exception("Result was not success!");

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected async Task<string> GetRaw(HttpClient client, string pluginType, string endpoint)
        {
            client.DefaultRequestHeaders.Add("PluginType", pluginType);
            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Result was not success!");

            return await response.Content.ReadAsStringAsync();
        }
    }
}