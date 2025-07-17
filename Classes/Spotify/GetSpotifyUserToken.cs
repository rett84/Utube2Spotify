using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

using Newtonsoft.Json;

namespace PlayListCreator_FW.Classes.Spotify
{
    public class GetSpotifyUserToken
    {
        public static dynamic GetUserToken(string clientID, string clientSecret, string code, bool ismobile)
        {

            bool isDebug = false;
            #if DEBUG
             isDebug = true;
            #endif
            string appUri = string.Empty;//must match Spotify
            if (isDebug)
            {
                if (ismobile)
                {
                    appUri = "http://127.0.0.1:44303/Pages/GenfromYTMobile/";
                }
                else
                {
                    appUri = "http://127.0.0.1:44303/Pages/GenfromYT/";
                }
            }
            else
            {
                if (ismobile)
                {
                    appUri = "https://utube2spotify.azurewebsites.net/Pages/GenfromYTMobile/";

                }
                else
                {
                    appUri = "https://utube2spotify.azurewebsites.net/Pages/GenfromYT/";
                }
            }


            ////Close Redirect Tab.
            //string html = "<html><body><script>window.close();</script>Authorization complete. You can close this tab.</body></html>";
            //byte[] buffer = Encoding.UTF8.GetBytes(html);
            //context.Response.ContentLength64 = buffer.Length;
            //context.Response.ContentType = "text/html";
            //context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            //context.Response.OutputStream.Close();

            //Add parameters to end-point to request Token
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", appUri }
            };

            //POST Request
            var client = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientID}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("https://accounts.spotify.com/api/token", content).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            dynamic tokenData = JsonConvert.DeserializeObject(json);

            //Access Token

            dynamic result = tokenData;
            //if (tokenData.access_token == null)
            //{
            //    result = tokenData.error_description;
            //}
           // string result = tokenData.
            //string refreshToken = tokenData.refresh_token;

            return result;
        }
    }
}