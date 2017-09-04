using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Author
    {
        public string name { get; set; }
        public int listeners { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public int streamable { get; set; }
        public string imageSmall { get; set; }
        public string imageMedium { get; set; }
        public string imageLarge { get; set; }
        public string imageExtralarge { get; set; }
        public string imageMega { get; set; }
    }
}