using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

namespace PlayListCreator_FW.Classes
{
    public class GetSpotifyURI
    {
        public static string SearchSpotifyTrackUri(string token, string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var url = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=track&limit=1";
            var response = client.GetAsync(url).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(json);
            if (result.tracks.items.Count > 0)
            {
                return result.tracks.items[0].uri;  // e.g., "spotify:track:6habFhsOp2NvshLv26DqMb"
            }

            return null;
        }
    }
}