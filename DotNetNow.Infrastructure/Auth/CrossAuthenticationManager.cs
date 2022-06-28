using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNow.Infrastructure.Auth
{
    public class CrossAuthenticationManager
    {
        private readonly string _authServerUrl;
        private readonly string _key;
        public CrossAuthenticationManager(string key, string authServerUrl)
        {
            _key = key;
            _authServerUrl = authServerUrl;
        }

        public async Task<string> Authenticate(string username)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(new RequestKey
                    {
                        PrivateKey = _key,
                        UserName = username
                    }), Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
                    HttpResponseMessage response = await httpClient.PostAsync($"{_authServerUrl}/account/set", stringContent);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        private class RequestKey
        {
            public string PrivateKey { get; set; }
            public string UserName { get; set; }
        }
    }


}
