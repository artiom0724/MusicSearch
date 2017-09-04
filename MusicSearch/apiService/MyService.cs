using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using MusicSearch.Models;

namespace MusicSearch.apiService
{
    public class MyService : Controller
    {
        public string reqestUrl;
        private string api_key = "4b256082800180d4f07db1a324cca7b8";

        public List<Author> authors;
        public List<Album> albums;

        public MyService()
        {
            reqestUrl = "http://ws.audioscrobbler.com/2.0/";
            authors = new List<Author>();
            albums = new List<Album>();
        }

        public void getTopOfAuthors(int numPage, string country="belarus")
        {           
            var webClient = new WebClient();
            webClient.QueryString.Add("method", "geo.gettopartists");
            webClient.QueryString.Add("page", numPage.ToString());
            webClient.QueryString.Add("api_key", api_key);
            webClient.QueryString.Add("country", country);
            webClient.QueryString.Add("limit", "9");
            string returnString;        
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(returnString);
            
            for (int i = 0; i <= 8; i++)
            {
                Author author = new Author();
                author.name = doc.GetElementsByTagName("name").Item(i).InnerText;
                author.imageMega = doc.GetElementsByTagName("image").Item(i*5+4).InnerText;
                author.listeners = int.Parse(doc.GetElementsByTagName("listeners").Item(i).InnerText);
                authors.Add(author);
            }
        }

        public void getTopAlbums(int numPage, string author)
        {
            var webClient = new WebClient();
            webClient.QueryString.Add("method", "artist.gettopalbums");
            webClient.QueryString.Add("artist", author);
            webClient.QueryString.Add("page", numPage.ToString());
            webClient.QueryString.Add("api_key", api_key);
            webClient.QueryString.Add("limit", "9");

            string returnString;
            using (webClient)
            {
                returnString = webClient.DownloadString(reqestUrl);
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(returnString);

            for (int i = 0; i <= 8; i++)
            {
                Album album = new Album();
                album.name = doc.GetElementsByTagName("name").Item(i).InnerText;
                album.imageLarge = doc.GetElementsByTagName("image").Item(i * 4 + 3).InnerText;
                album.playcount = int.Parse(doc.GetElementsByTagName("playcount").Item(i).InnerText);
                albums.Add(album);
            }
        }
    }
}