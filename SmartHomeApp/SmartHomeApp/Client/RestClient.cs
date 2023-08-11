using Newtonsoft.Json;
using SmartHomeApp.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHomeApp.Client
{
    class RestClient : IDisposable
    {
        private readonly HttpClient httpClient;

        public RestClient()
        {
            this.httpClient = PreparedClient();
        }

        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(5) };

            return client;
        }

        public async Task<string> GetFromServerAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
