using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PlayListCreator_FW
{
    public class GetPlaylist
    {

        public class YouTubeItem
        {
            public string Title { get; set; }
            public string VideoId { get; set; }
        }

        public static class YouTubeHelper
        {
            public static List<YouTubeItem> GetPlaylistVideos(string apiKey, string playlistId)
            {
                var videos = new List<YouTubeItem>();
                string nextPageToken = null;

                using (var client = new HttpClient())
                {

                    do
                    {
                        var url = $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=1000&playlistId={playlistId}&key={apiKey}";

                        if (!string.IsNullOrEmpty(nextPageToken))
                            url += $"&pageToken={nextPageToken}";

                        var response = client.GetStringAsync(url).Result;

                        dynamic json = JsonConvert.DeserializeObject(response);

                        foreach (var item in json.items)
                        {
                            var videoId = (string)item.snippet.resourceId.videoId;
                            var title = (string)item.snippet.title;

                            int index = title.IndexOf("(");
                            if (index >= 0)
                                title = title.Substring(0, index);

                            if(!title.Contains("Private Video"))
                                videos.Add(new YouTubeItem { Title = title,
                                                            VideoId = videoId });
                        }

                        nextPageToken = json.nextPageToken != null ? (string)json.nextPageToken : null;

                    } while (!string.IsNullOrEmpty(nextPageToken));
                }

                return videos;
            }
        }

    }
}