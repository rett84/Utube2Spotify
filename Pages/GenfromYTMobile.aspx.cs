using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PlayListCreator_FW.Classes;
using PlayListCreator_FW.Classes.Spotify;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace PlayListCreator_FW.Pages
{
    public partial class GenfromYTMobile : System.Web.UI.Page
    {
        private string apiKeyYT = "AIzaSyBGkNKBhpwrSkK9E5pQvkOz4kYB57vPA9M";
        string clientIDSpotify = "474bbf92a71d4424bcc40f9d392fce43";
        string clientSecretSpotify = "3b4859f4df4546c2b09a542bbf95aba8";
        private static bool ismobile = true;

        public GenfromYTMobile()
        {


        }

        protected void Page_Load(object sender, EventArgs e)
        {


            string code = Request.QueryString["code"];

            var context = GlobalHost.ConnectionManager.GetHubContext<UpdateCount>();
            string connectionID = string.Empty;
            if (Session["connectionIDSigR"] != null)
                connectionID = Session["connectionIDSigR"].ToString();

            if (!IsPostBack && Request.QueryString["code"] != null)
            {
                if (Session["YoutubePlaylistURL"] == null)
                {
                    lblCountSongsInserted.Text = $"The Youtube playlist contains " +
                     $"no songs";
                    lblCountSongsInserted.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                if (Session["SpotifyPlaylistURL"] == null)
                {
                    lblCountSongsInserted.Text = "Spotify Playlist Empty or not Valid";
                    lblCountSongsInserted.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                List<string> SongsListYT = YouTubeList(Session["YoutubePlaylistURL"].ToString());
                string playlistSpotifyid = ValidateSpotifyPlaylist(Session["SpotifyPlaylistURL"].ToString());

                // List<String> PlaylistSpotify = new List<String>();
                int countAdded = 0;

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
                    //PlaylistSpotify.Add(trackUri);
                    success = AddURItoSpotifyPlaylist.AddTrack((string)tokenUser.access_token, trackUri, playlistSpotifyid);
                    if (success)
                        countAdded++;
                    context.Clients.Client(connectionID).updateCount(countAdded, SongsListYT.Count);
                }
                lblCountSongsInserted.Visible = true;
                lblCountSongsInserted.ForeColor = System.Drawing.Color.Green;
                lblCountSongsInserted.Text = $"The Youtube playlist contains " +
                    $"{SongsListYT.Count} songs and {countAdded} songs were found on Spotify and added";
            }
        }

        protected void ButtonCSV_Click(object sender, EventArgs e)
        {
            lblCountSongsInserted.Text = "";
            GenCSVfromList csvexporter = new GenCSVfromList();
            if (YouTubeList(txtPlaylistYT.Text) != null)
                csvexporter.ExportListToCsv(YouTubeList(txtPlaylistYT.Text), HttpContext.Current);
        }

        protected void ButtontoSpotify_Click(object sender, EventArgs e)
        {
            if (ValidateYoutubePlaylist(txtPlaylistYT.Text) == null ||
                ValidateSpotifyPlaylist(txtPlayListSpotify.Text) == null)
            {
                return;
            }

            Session["YoutubePlaylistURL"] = txtPlaylistYT.Text;
            Session["SpotifyPlaylistURL"] = txtPlayListSpotify.Text;
            Session["connectionIDSigR"] = hfConnectionId.Value;
            lblCountSongsInserted.ForeColor = System.Drawing.Color.Black;
            
            GetSpotifyAuthCode();
        }

        private List<string> YouTubeList(string playlisturl)
        {

            string playlistYTid = ValidateYoutubePlaylist(playlisturl);

            if (playlistYTid == null)
                return null;

            //Get a list of the videos in the Youtube playlist
            List<GetPlaylist.YouTubeItem> videos = GetPlaylist.YouTubeHelper.GetPlaylistVideos(apiKeyYT, playlistYTid);
            List<String> PlaylistYT = new List<String>();

            foreach (var video in videos)
            {
                PlaylistYT.Add(video.Title);
            }

            return PlaylistYT;
        }

        private void GetSpotifyAuthCode()
        {
            bool isDebug = false;
            #if DEBUG
               isDebug = true;
            #endif
            string appUri = string.Empty; //must match in Spotify's dashboard
            if (isDebug)
            {
                appUri = "http://127.0.0.1:44303/Pages/GenfromYTMobile/";
            }
            else
            {

                appUri = "https://utube2spotify.azurewebsites.net/Pages/GenfromYTMobile/";

            }

            //Request Authorization from User to Manipulate Playlist
            string scope = "playlist-modify-public playlist-modify-private";
            string authUrl = $"https://accounts.spotify.com/authorize?client_id={clientIDSpotify}&response_type=code&redirect_uri={Uri.EscapeDataString(appUri)}&scope={Uri.EscapeDataString(scope)}";

            Response.Redirect(authUrl);

        }

        private string ValidateYoutubePlaylist(string playlisturl)
        {

            string playlisturltrim = "youtube.com/playlist?list=";

            if (string.IsNullOrEmpty(playlisturl))
            {
                ValidateYTLink.IsValid = false;
                ValidateYTLink.ErrorMessage = "Please enter a valid YouTube playlist link.";
                return null;
            }

            int found = playlisturl.IndexOf(playlisturltrim);
            if (found == -1)
            {
                ValidateYTLink.IsValid = false;
                ValidateYTLink.ErrorMessage = "Please enter a valid YouTube playlist link.";
                return null;
            }

            ValidateYTLink.IsValid = true;
            ValidateYTLink.ErrorMessage = string.Empty;
            string playlistid = playlisturl.Substring(found + playlisturltrim.Length);

            return playlistid;
        }

        private string ValidateSpotifyPlaylist(string playlisturl)
        {

            string playlisturltrim = "https://open.spotify.com/playlist/";

            if (string.IsNullOrEmpty(playlisturl))
            {
                ValidateSpotifyLink.IsValid = false;
                ValidateSpotifyLink.ErrorMessage = "Please enter a valid Spotify playlist link.";
                return null;
            }

            int found = playlisturl.IndexOf(playlisturltrim);
            if (found == -1)
            {
                ValidateSpotifyLink.IsValid = false;
                ValidateSpotifyLink.ErrorMessage = "Please enter a valid Spotify playlist link.";
                return null;
            }

            ValidateSpotifyLink.IsValid = true;
            ValidateSpotifyLink.ErrorMessage = string.Empty;

            string playlistid = playlisturl.Split(new[] { "/playlist/" }, StringSplitOptions.None)[1].Split('?')[0];

            return playlistid;
        }

    }
}