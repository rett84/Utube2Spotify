using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;

namespace PlayListCreator_FW.Classes.Spotify
{
    public class AddURItoSpotifyPlaylist
    {
        public static bool AddTrack(string token, string trackURI, string playlist_id)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(trackURI))
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonBody = JsonConvert.SerializeObject(new
                {
                    uris = new[] { trackURI }
                });

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"https://api.spotify.com/v1/playlists/{playlist_id}/tracks", content).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject(json);

                string jsonString = JsonConvert.SerializeObject(result);

                if (jsonString.IndexOf("snapshot") != -1)
                {
                    success = true;
                }

            }

            return success;
        }
    }
}