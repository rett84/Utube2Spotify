using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace PlayListCreator_FW.Classes
{
    public class GetSpotifyToken
    {
        public static dynamic GetSpotifyAccessToken(string clientId, string clientSecret)
        {
            var client = new HttpClient();

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            var requestBody = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
            {
                Content = new FormUrlEncodedContent(requestBody)
            };

            var response = client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(content);
            return result;
        }
    }
}