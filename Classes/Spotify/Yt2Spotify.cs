using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayListCreator_FW.Classes.Spotify
{
    public class Yt2Spotify
    {
        private string apiKeyYT = PlayListCreator_FW.Properties.Settings.Default.apiKeyYT;
        private string clientIDSpotify = PlayListCreator_FW.Properties.Settings.Default.clientIDSpotify;
        private string clientSecretSpotify = PlayListCreator_FW.Properties.Settings.Default.clientSecretSpotify;


        public void AddtoSpotifyPlaylist(string youtubeurl, string spotifyurl)
        {

            List<string> SongsListYT = YouTubeList(youtubeurl);
            string playlistSpotifyid = ValidateSpotifyPlaylist(spotifyurl);


            dynamic tokenGeneral = GetSpotifyToken.GetSpotifyAccessToken(clientIDSpotify, clientSecretSpotify);
            if (tokenGeneral.access_token == null)
            {
                lblCountSongsInserted.Text = tokenGeneral.error_description;
                return;
            }

            dynamic tokenUser = GetSpotifyUserToken.GetUserToken(clientIDSpotify, clientSecretSpotify, code, ismobile);
            if (tokenUser.access_token == null)
            {
                lblCountSongsInserted.Text = tokenUser.error_description;
                return;
            }


            foreach (var song in SongsListYT)
            {
                bool success = false;
                string trackUri = GetSpotifyURI.SearchSpotifyTrackUri((string)tokenGeneral.access_token, song);
                success = AddURItoSpotifyPlaylist.AddTrack((string)tokenUser.access_token, trackUri, playlistSpotifyid);
                if (success)
                    countAdded++;
                context.Clients.Client(connectionID).updateCount(countAdded, SongsListYT.Count);
            }
        }


    }
}