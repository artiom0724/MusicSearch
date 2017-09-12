using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        public string ArtistAlbum { get; set; }
        public string Name { get; set; }
        public int Playcount { get; set; }
        public string ImageLarge { get; set; }
    }
}