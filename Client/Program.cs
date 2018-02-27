using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            getToken();
            Console.ReadKey();
        }

        static async void getToken()
        {
            // discover endpoints from metadata
            DiscoveryResponse disco = null;
            string url = "http://localhost:5000";
            //url = "http://10.202.203.29:5000";
            url = "http://10.15.4.155:5000";
            url = "https://centerbmdwtest.staff.xdf.cn";
            //url = "https://localhost";

            //if (url.Contains("https"))
            //{
            //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //    //ServicePointManager.Expect100Continue = false;
            //}
            //using (var dc = new DiscoveryClient(url))
            //{
            //    dc.Policy.RequireHttps = false;
            //    disco = await dc.GetAsync();
            //}

            disco = await DiscoveryClient.GetAsync(url);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "test", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync(url+"/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
