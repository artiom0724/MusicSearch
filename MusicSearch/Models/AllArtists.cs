using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class AllArtists
    {
        public AllArtists()
        {
            LocalArtists = new List<Artist>();
            OnlineArtists = new List<Artist>();
        }
        public List<Artist> LocalArtists { get; set; }
        public List<Artist> OnlineArtists { get; set; }
    }
}