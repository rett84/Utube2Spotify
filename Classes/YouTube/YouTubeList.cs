using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayListCreator_FW.Classes.YouTube
{
    public class YouTubeList
    {
        static string apiKeyYT = PlayListCreator_FW.Properties.Settings.Default.apiKeyYT;
        public static List<string> PlayList(string playlisturl)
        {

            string playlistYTid = ValidateYoutubeURL.ValidateYoutubePlaylist(playlisturl);

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
    }
}