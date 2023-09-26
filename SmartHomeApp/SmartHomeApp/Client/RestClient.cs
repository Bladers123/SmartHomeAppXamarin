using Newtonsoft.Json;
using Org.Apache.Http.Client;
using SmartHomeApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHomeApp.Client
{
    class RestClient : IDisposable
    {
        private HttpClient httpClient;

        public RestClient()
        {
            this.httpClient = PreparedClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(5);
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

        /// <summary>
        /// Return null if failure connection to server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetFromServerAsync(string url, CancellationToken token = default)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync(url, token);   
                
            }
            catch (HttpRequestException)
            {
                //await Shell.Current.DisplayAlert("Fehler", "Test", "OK");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine("The request was timed out. " + ex.ToString());
                return null;
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("Operation was cancelled: " + ex.ToString());
                return null;
            }         
            catch (Exception ex)
            {
                Debug.WriteLine("Operation was cancelled: " + ex.ToString());
                return null;
            }



            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        public void Dispose()
        {
            // httpClient?.Dispose();          
        }
    }
}
