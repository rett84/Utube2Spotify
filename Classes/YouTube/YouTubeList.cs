using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayListCreator_FW.Classes.YouTube
{
    public class YouTubeList
    {
        public List<string> CreateList(string playlistYTurl, string apiKeyYT, System.Web.UI.WebControls.CustomValidator Validator)
        {

            string playlistYTurltrim = "www.youtube.com/playlist?list=";
            int found = playlistYTurl.IndexOf(playlistYTurltrim);
            if (found == -1)
            {
                Validator.IsValid = false;
                Validator.ErrorMessage = "Please enter a valid YouTube playlist link.";
                return null;
            }

            Validator.IsValid = true;
            Validator.ErrorMessage = string.Empty;
            string playlistYTid = playlistYTurl.Substring(found + playlistYTurltrim.Length);


            //Get a list of the videos in the Youtube playlist
            List<GetPlaylist.YouTubeItem> videos = GetPlaylist.YouTubeHelper.GetPlaylistVideos(apiKeyYT, playlistYTid);
            List<String> PlaylistYT = new List<String>();

            foreach (var video in videos)
            {
                PlaylistYT.Add(video.Title);
            }

            return PlaylistYT;
        }
    }
}