using MusicSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.apiService
{
    public class DBWorker
    {
        public MyDbContext dbContext = new MyDbContext();

        public List<Artist> GetAuthorsFromDB(int numPage, string reqest = null)
        {
            List<Artist> artists = new List<Artist>();
            if (reqest != null)
                artists.AddRange(dbContext.Artists.Where(item => item.Name.Contains(reqest) || reqest.Contains(item.Name)).ToList());
            else
                artists.AddRange(dbContext.Artists.ToList());
            return artists;
        }

        public List<Album> GetAlbumsFromDB(string author, int numPage)
        {
            List<Album> albums = new List<Album>();
            albums.AddRange(dbContext.Albums.Where(item => item.ArtistAlbum.Contains(author) || author.Contains(item.ArtistAlbum)).ToList());
            return albums;
        }

        public List<Track> GetTracksFromDB(string author, string album, int numPage, int limit = 25)
        {
            List <Track> tracks = new List<Track>();
            if (album != null)
                tracks.AddRange(dbContext.Tracks
                    .Where(item => item.Id < numPage * limit && item.Album.Contains(album) && item.Artist.Contains(author))
                    .ToList());
            else
                tracks.AddRange(dbContext.Tracks
               .Where(item => (item.Album.Contains(author)
               || item.Artist.Contains(author)
               || author.Contains(item.Artist)
               || author.Contains(item.Album)))
               .ToList());
            return tracks;
        }
    }
}