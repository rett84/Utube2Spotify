using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayListCreator_FW.Classes.YouTube
{
    public class ValidateYoutubeURL
    {
        public static string ValidateYoutubePlaylist(string playlisturl)
        {

            string playlisturltrim = "youtube.com/playlist?list=";

            if (string.IsNullOrEmpty(playlisturl))
            {
              
                return null;
            }

            int found = playlisturl.IndexOf(playlisturltrim);
            if (found == -1)
            {
              
                return null;
            }

            string playlistid = playlisturl.Substring(found + playlisturltrim.Length);

            return playlistid;
        }
    }
}