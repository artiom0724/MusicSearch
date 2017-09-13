using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicSearch.Models
{
    public class MyDbContext : DbContext
    {
        //public MyDbContext() : base("DefaultConnection")
        //{ }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }      
        public DbSet<Track> Tracks { get; set; }
    }
}