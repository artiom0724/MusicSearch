using MusicSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace MusicSearch.apiService
{
    public class OnlineWorker
    {
        private string reqestUrl;
        private string api_key;
        public Dictionary<string, string> methods;
        public Dictionary<string, string> nodes;

        public List<Artist> artists;
        public List<Album> albums;
        public List<Track> tracks;

        public void LastFmStart()
        {
            methods = new Dictionary<string, string>
            {
                { "GetTopArtists", "geo.gettopartists" },
                { "GetTopAlbumsOfArtist", "artist.gettopalbums" },
                { "GetTracksOfAlbum", "album.getinfo" },
                { "SearchArtists", "artist.search" },
                { "SearchAlbums", "album.search" },
                { "SearchTracks", "track.search" },
                { "GetInfoTrack", "track.getInfo" }
            };

            nodes = new Dictionary<string, string>
            {
                { "artist", "artist" },
                { "album", "album" },
                { "track", "track" },
                {"trackInfo","trackInfo" }
            };

            reqestUrl = "http://ws.audioscrobbler.com/2.0/";
            api_key = "4b256082800180d4f07db1a324cca7b8";
        }

        public OnlineWorker()
        {
            artists = new List<Artist>();
            albums = new List<Album>();
            tracks = new List<Track>();
            LastFmStart();
        }

        public string SetOptions(
            string method, 
            int numPage = 0, 
            string country = null, 
            string artist = null, 
            string album = null,
            string track = null,
            int limit = 24)
        {
            var webClient = new WebClient();
            webClient.QueryString.Add("method", method);
            webClient.QueryString.Add("api_key", api_key);

            if (numPage != 0)
                webClient.QueryString.Add("page", numPage.ToString());           
            if (limit != 0)
                webClient.QueryString.Add("limit", limit.ToString());
            if (artist != null)
                webClient.QueryString.Add("artist", artist);
            if (country != null)
                webClient.QueryString.Add("country", country);
            if (album != null)
                webClient.QueryString.Add("album", album);
            if (track != null)
                webClient.QueryString.Add("track", track);

            string returnString;

            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            return returnString;
        }

        public void ReqestMethod(XDocument document, string typeNode, bool reqestSearch = false)
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
                        ReqestForAlbums(node, reqestSearch);
                    break;
                case "track":
                    foreach (var node in nodesOfTypeNodes)
                        ReqestForTracks(node, reqestSearch);
                    break;
            }
        }

        public string ReqestForNode(XElement node, string findNode, string[] attribute = null, string[] attributeValue = null)
        {
            var tempNode = node.Descendants(findNode);
            if (attribute == null)
                return tempNode.First().Value;
            else
            {
                return tempNode.ToList().Where((attr) => ForeachAttributes(attr, attribute, attributeValue)).First().Value;
            }
        }

        public bool ForeachAttributes(XElement attr, string[] attribute = null, string[] attributeValue = null)
        {
            for (int i = 0; i < attribute.Length; i++)
                if (attr.Attribute(attribute[i]).Value != attributeValue[i])
                    return false;
            return true;
        }

        public string EncodingFromUTF8toWin1251(string encodeElement)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(encodeElement);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }

        public string EncodingFromWin1251ToUTF8(string encodeElement)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] win1251Bytes = utf8.GetBytes(encodeElement);
            byte[] utf8Bytes = Encoding.Convert(win1251, utf8, win1251Bytes);
            return utf8.GetString(utf8Bytes);
        }

        public List<Artist> TopAuthorsForView(int numPage)
        {
            artists.Clear();
            GetTopOfAuthors(numPage);
            return artists;
        }

        public void GetTopOfAuthors(int numPage, string country = "belarus")
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTopArtists"], numPage, country, null, null));
            ReqestMethod(document, nodes["artist"]);
        }

        public void ReqestForArtists(XElement node)
        {
            Artist artist = new Artist
            {
                ImageLarge = ReqestForNode(node, "image", new string[] { "size" }, new string[] { "large" }),
                Name = EncodingFromUTF8toWin1251((ReqestForNode(node, "name"))),
                Listeners = int.Parse(ReqestForNode(node, "listeners"))
            };
            artists.Add(artist);
        }

        public List<Album> TopAlbumsForView(string author, int numPage = 1)
        {
            albums.Clear();
            if (author != "")
            {
                GetTopAlbums(numPage, author);
                return albums;
            }
            return null;
        }

        public void GetTopAlbums(int numPage, string artist)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTopAlbumsOfArtist"], numPage, null, artist, null));
            ReqestMethod(document, nodes["album"]);
        }

        public void ReqestForAlbums(XElement node, bool reqestSearch = false)
        {
            Album album = new Album();
            album.Name = EncodingFromUTF8toWin1251(ReqestForNode(node, "name"));
            album.ImageLarge = ReqestForNode(node, "image", new string[] { "size" }, new string[] { "large" });
            album.Playcount = int.Parse(ReqestForNode(node, "playcount"));
            if (!reqestSearch)
                album.ArtistAlbum = EncodingFromUTF8toWin1251(ReqestForNode(node.Descendants("artist").First(), "name"));
            else
                album.ArtistAlbum = EncodingFromUTF8toWin1251(ReqestForNode(node, "artist"));
            albums.Add(album);
        }

        public List<Track> TracksOfAlbum(string author, string album, int numPage = 1)
        {
            albums.Clear();
            if (album != "" && author != "")
            {
                GetTopTracksOfAlbum(numPage, author, album);
                return tracks;
            }
            return null;
        }

        public void GetTopTracksOfAlbum(int numPage, string artist, string album)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetTracksOfAlbum"], numPage, null, artist, album));
            ReqestMethod(document, "track");
        }

        public void ReqestForTracks(XElement node, bool reqestSearch = false)
        {
            Track track = new Track
            {
                Name = EncodingFromUTF8toWin1251(ReqestForNode(node, "name")),
                Artist = EncodingFromUTF8toWin1251(ReqestForNode(node.Descendants("artist").First(), "name")),
                Listeners = 0
            };
            if (!reqestSearch)
                track.Duration = int.Parse(ReqestForNode(node, "duration"));
            else
            {
                track.ImageLarge = ReqestForNode(node, "image", new string[] { "size" }, new string[] { "large" });
                track.Listeners = int.Parse(ReqestForNode(node, "listeners"));
            }
            tracks.Add(track);
        }

        public void SearchArtistsMehod(string reqest, int numPage)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["SearchArtists"], numPage, null, reqest, null).Replace("opensearch:", ""));
            ReqestMethod(document, "artist");
        }

        public void SearchAlbumsMethod(string reqest, int numPage)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["SearchAlbums"], numPage, null, null, reqest).Replace("opensearch:", "").Replace("streamable", "playcount"));
            ReqestMethod(document, "album", true);
        }

        public void SearchTracksMethod(string reqest, int numPage)
        {
            XDocument document = XDocument.Parse(
                SetOptions(methods["SearchTracks"], numPage, null, null, null, reqest, 24).Replace("opensearch:", ""));
            ReqestMethod(document, "track", true);
        }

        public List<Artist> SearchArtists(string reqest, int numPage = 1)
        {
            artists.Clear();
            SearchArtistsMehod(reqest, numPage);
            return artists;
        }

        public List<Album> SearchAlbums(string reqest, int numPage = 1)
        {
            albums.Clear();
            SearchAlbumsMethod(reqest, numPage);
            return albums;
        }

        public List<Track> SearchTracks(string reqest, int numPage = 1)
        {
            tracks.Clear();
            SearchTracksMethod(reqest, numPage);
            return tracks;
        }

        public Track InfoTrack(string Artist, string Name)
        {
            tracks.Clear();
            XDocument document = XDocument.Parse(
                SetOptions(methods["GetInfoTrack"], 0, null, Artist, null, Name, 0));
            Track track = new Track()
            {
                Album = EncodingFromUTF8toWin1251(document.Descendants("album").First().Descendants("title").First().Value),
                ImageLarge = ReqestForNode(document.Descendants("track").First(), "image", new string[] { "size" }, new string[] { "large" }),
                Listeners = int.Parse(ReqestForNode(document.Descendants("track").First(), "listeners"))
            };
            return track;
        }
    }
}