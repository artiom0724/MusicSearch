using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using MusicSearch.Models;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System;

namespace MusicSearch.apiService
{
    public class MyService 
    {
        private string reqestUrl ;
        private string api_key ;
        Dictionary<string, string> methods;
        Dictionary<string, string> nodes;

        public List<Author> authors;
        public List<Album> albums;
        public List<Track> tracks;

        public void LastFmStart()
        {
            methods = new Dictionary<string, string>();
            methods.Add("GetTopArtists", "geo.gettopartists");
            methods.Add("GetTopAlbumsOfArtist", "artist.gettopalbums");
            methods.Add("GetTracksOfAlbum", "album.getinfo");
            methods.Add("SearchArtists","artist.search");
            methods.Add("SearchAlbums", "album.search");
            methods.Add("SearchTracks", "track.search");

            nodes = new Dictionary<string, string>();
            nodes.Add("artist", "artist");
            nodes.Add("album", "album");
            nodes.Add("track", "track");

            reqestUrl = "http://ws.audioscrobbler.com/2.0/";
            api_key = "4b256082800180d4f07db1a324cca7b8";
        }

        public MyService()
        {
            LastFmStart();
            authors = new List<Author>();
            albums = new List<Album>();
            tracks = new List<Track>();
        }

        public string SetOptions(string method, int numPage, string country, string artist, string album, int limit = 24)
        {
            var webClient = new WebClient();
            webClient.QueryString.Add("method", method);
           
            webClient.QueryString.Add("page", numPage.ToString());
            webClient.QueryString.Add("api_key", api_key);

            if (artist != null)
                webClient.QueryString.Add("artist", artist);
            if (country!=null)
                webClient.QueryString.Add("country", country);
            if(album!=null)
                webClient.QueryString.Add("album", album);
            if (album == null)              
                webClient.QueryString.Add("limit", limit.ToString());

            string returnString;
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            return returnString;
        }

        public string ReqestForNode(XElement node, string findNode, string [] attribute = null, string [] attributeValue = null)
        {
            var tempNode = node.Descendants(findNode);
            
            if (attribute == null)
                return tempNode.First().Value;
            else
            {             
                return tempNode.ToList().Where((attr) => ForeachAttributes(attr,attribute,attributeValue)).First().Value;
            }
        }

        public bool ForeachAttributes(XElement attr,string[] attribute = null, string[] attributeValue = null)
        {
            for (int i = 0; i < attribute.Length; i++)
                if (attr.Attribute(attribute[i]).Value != attributeValue[i])
                    return false;
            return true;
        }

        public void ReqestMethod(XDocument document, string typeNode)
        {
            var nodesOfTypeNodes = document.Descendants(typeNode);
            switch (typeNode)
            {
                case "artist":
                    foreach (var node in nodesOfTypeNodes)
                        ReqestForArtists(node);
                    break;
                case "album":
                    foreach (var node in nodesOfTypeNodes)
                        ReqestForAlbums(node);
                    break;
                case "track":
                    foreach (var node in nodesOfTypeNodes)
                        ReqestForTracks(node);
                    break;
            }

        }

        public void GetTopOfAuthors(int numPage, string country="belarus")
        {           
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTopArtists"], numPage, country, null, null));
            ReqestMethod(document, nodes["artist"]);              
        }

        public void ReqestForArtists(XElement node)
        {
            
            Author author = new Author();
            author.ImageLarge = ReqestForNode(node, "image",new string[]{ "size"}, new string[] { "large" });
            author.Name =EncodingFromUTF8toWin1251((ReqestForNode(node, "name"))) ;
            author.Listeners = int.Parse(ReqestForNode(node, "listeners"));
            authors.Add(author);
        }

        public void GetTopAlbums(int numPage, string artist)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTopAlbumsOfArtist"], numPage, null, artist, null));
            ReqestMethod(document, nodes["album"]);
        }

        public void ReqestForAlbums(XElement node)
        {
            Album album = new Album();
            album.Name = EncodingFromUTF8toWin1251(ReqestForNode(node, "name"));
            album.ImageLarge = ReqestForNode(node, "image", new string[] { "size" }, new string[] { "large" });
            album.Playcount = int.Parse(ReqestForNode(node, "playcount"));
            album.ArtistAlbum = EncodingFromUTF8toWin1251(ReqestForNode(node.Descendants("artist").First(), "name"));
            albums.Add(album);
        }

        public void GetTopTracksOfAlbum(int numPage, string artist, string album)
        {                    
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTracksOfAlbum"], numPage, null, artist, album));
            ReqestMethod(document, "track");
        }

        public void ReqestForTracks(XElement node)
        {
            Track track = new Track();
            track.Name = EncodingFromUTF8toWin1251(ReqestForNode(node, "name"));
            track.Duration = int.Parse(ReqestForNode(node, "duration"));
            tracks.Add(track);
        }

        public List<Author> TopAuthorsForView(int numPage)
        {
            GetTopOfAuthors(numPage);
            return authors;
        }

        public List<Album> TopAlbumsForView(string author, int numpage = 1)
        {
            if (author != "")
            {
                GetTopAlbums(numpage, author);
                return albums;
            }
            return null;
        }

        public List<Track> TracksOfAlbum(string author, string album, int numPage = 1)
        {
            if (album != "" && author != "")
            {
                GetTopTracksOfAlbum(numPage, author, album);
                return tracks;
            }
            return null;
        }

        public List<Author> SearchArtists()
        {
            return null;
        }

        public List<Album> SearchAlbums()
        {
            return null;
        }


        public List<Track> SearchTracks()
        {
            return null;
        }
       
        public string EncodingFromUTF8toWin1251(string encodeElement)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(encodeElement);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }
    }
}