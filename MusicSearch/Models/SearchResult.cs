using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class SearchResult
    {
        public List<Artist> Authors { get; set; }
        public List<Album> Albums { get; set; }
        public List<Track> Tracks { get; set; }
        public string SearchReqest { get; set; }

    }
}