using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public int Duration { get; set; }
        public int Listeners { get; set; }
        public string Album { get; set; }
        public string ImageLarge { get; set; }
        public string DataMusic { get; set; }
        public string Url { get; set; }
    }
}