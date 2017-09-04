using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Track
    {
        public string name { get; set; }
        public int duration { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public int streamable { get; set; }
        public string imageSmall { get; set; }
        public string imageMedium { get; set; }
        public string imageLarge { get; set; }
    }
}