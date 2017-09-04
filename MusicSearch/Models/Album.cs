using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Album
    {
        public string name { get; set; }
        public int playcount { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public string imageSmall { get; set; }
        public string imageMedium { get; set; }
        public string imageLarge { get; set; }
    }
}