using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class SearchResult
    {
        public SearchResult()
        {
            Artists = new List<Artist>();
            Albums = new List<Album>();
            Tracks = new List<Track>();
        }
        public List<Artist> Artists { get; set; }
        public List<Album> Albums { get; set; }
        public List<Track> Tracks { get; set; }
        public string SearchReqest { get; set; }

    }
}