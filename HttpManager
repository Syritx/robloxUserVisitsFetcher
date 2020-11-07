using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace apitest
{
    class HttpManager {

        static HttpClient client = new HttpClient();
        public static async Task<string> CreateHttpRequest(string url) {

            HttpResponseMessage responseMessage = await client.GetAsync(url);
            string httpResponse = await responseMessage.Content.ReadAsStringAsync();
            return httpResponse;
        }
    }
}
