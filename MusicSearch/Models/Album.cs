using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Album
    {
        public string ArtistAlbum { get; set; }
        public string Name { get; set; }
        public int Playcount { get; set; }
        public string Mbid { get; set; }
        public string Url { get; set; }
        public string ImageSmall { get; set; }
        public string ImageMedium { get; set; }
        public string ImageLarge { get; set; }
    }
}