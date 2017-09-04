using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using MusicSearch.Models;
using System.Xml.Linq;
using System.Linq;

namespace MusicSearch.apiService
{
    public class MyService : Controller
    {
        public string reqestUrl;
        private string api_key = "4b256082800180d4f07db1a324cca7b8";

        public List<Author> authors;
        public List<Album> albums;
        public List<Track> tracks;

        public MyService()
        {
            reqestUrl = "http://ws.audioscrobbler.com/2.0/";
            authors = new List<Author>();
            albums = new List<Album>();
            tracks = new List<Track>();
        }

        public void setOptions(WebClient webClient,string method, int numPage, string country, string artist, string album)
        {
            webClient.QueryString.Add("method", method);
           
            webClient.QueryString.Add("page", numPage.ToString());
            webClient.QueryString.Add("api_key", api_key);

            if (artist != null)
                webClient.QueryString.Add("artist", artist);
            if (country!=null)
                webClient.QueryString.Add("country", country);
            if(album!=null)
                webClient.QueryString.Add("album", album);
            if (album != null)
                webClient.QueryString.Add("limit", "100");
            else
                webClient.QueryString.Add("limit", "13");
        }

        public void getTopOfAuthors(int numPage, string country="belarus")
        {           
            var webClient = new WebClient();
            setOptions(webClient, "geo.gettopartists", numPage, country, null,null);

            string returnString;        
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            XDocument document = XDocument.Parse(returnString);
            
            for (int i = 0; i <= 8; i++)
            {
                Author author = new Author();
                author.imageLarge = document.Descendants("image").Where(s =>(string) s.Attribute("size")=="large").ElementAt(i).Value;
                author.name = document.Descendants("name").ElementAt(i).Value;
                author.listeners = int.Parse(document.Descendants("listeners").ElementAt(i).Value);
                authors.Add(author);
            }
        }

        public void getTopAlbums(int numPage, string artist)
        {
            var webClient = new WebClient();
            setOptions(webClient, "artist.gettopalbums", numPage, null, artist,null);          

            string returnString;
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
           
            XDocument document = XDocument.Parse(returnString);
            try
            {
                for (int i = 0; i <= 11; i++)
                {
                    Album album = new Album();
                    album.name = document.Descendants("name").ElementAt(i*2).Value;
                    album.imageLarge = document.Descendants("image").Where(s => (string)s.Attribute("size") == "large").ElementAt(i).Value;
                    album.playcount = int.Parse(document.Descendants("playcount").ElementAt(i).Value);
                    albums.Add(album);
                }
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }

        public void getTopTracksOfAlbum(int numPage, string artist, string album)
        {
            var webClient = new WebClient();
            setOptions(webClient, "album.getinfo", numPage, null, artist, album);            

            string returnString;
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            XDocument document = XDocument.Parse(returnString);
            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    Track track = new Track();
                    track.name = document.Descendants("tracks").ElementAt(0).Descendants("name").ElementAt(i * 2).Value;
                    track.duration = int.Parse(document.Descendants("duration").ElementAt(i).Value);
                    tracks.Add(track);
                }
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }
    }
}