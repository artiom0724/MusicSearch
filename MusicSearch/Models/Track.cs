﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class Track
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Mbid { get; set; }
        public string Url { get; set; }
        public int Streamable { get; set; }
        public string ImageSmall { get; set; }
        public string ImageMedium { get; set; }
        public string ImageLarge { get; set; }
    }
}