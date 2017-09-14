using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class AllAlbums
    {
        public List<Album> LocalAlbums { get; set; }
        public List<Album> OnlineAlbums { get; set; }
    }
}