using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using MusicSearch.Models;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Microsoft.Win32.TaskScheduler;

namespace MusicSearch.apiService
{
    public class MyService : OnlineWorker
    {
        
        public void SearchArtistsMehod(string reqest, int numPage)
        {  
            XDocument document = XDocument.Parse(
                SetOptions(methods["SearchArtists"], numPage, null, reqest, null).Replace("opensearch:",""));
            ReqestMethod(document,"artist");
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
                SetOptions(methods["SearchTracks"], numPage, null, null, null, reqest,24).Replace("opensearch:", ""));
            ReqestMethod(document, "track", true);
        }     

        public List<Artist> SearchArtists(string reqest, int numPage=1)
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
                SetOptions(methods["GetInfoTrack"], 0, null, Artist, null, Name,0));
            ReqestMethod(document, nodes["track"],false);
            return tracks.First();
        }
    }
}