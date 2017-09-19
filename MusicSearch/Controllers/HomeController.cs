using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicSearch.apiService;
using System.Text.RegularExpressions;
using MusicSearch.Models;
using System.Diagnostics;

namespace MusicSearch.Controllers
{
    public class HomeController : Controller
    {
        public OnlineWorker onlineWorker;

        public HomeController()
        {
            onlineWorker = new MyService();
        }

        public ActionResult Index(int ? author)
        {
            ViewBag.reqest = "Search";
            int numPage = author ?? 1;           
            List<Artist> artists = new List<Artist>();
            artists.AddRange(onlineWorker.TopAuthorsForView(numPage));
           
            if(Request.IsAjaxRequest() && numPage!=1)
            {
                return PartialView("_Items", artists);
            }
            return View(artists);
        }
       
        public ActionResult Albums(int? numPage,string author = "")
        {
            myService.SetOnlineOffline("Online");
            int tempPage = numPage ?? 1;
            AllAlbums allAlbums = new AllAlbums();
            allAlbums.OnlineAlbums.AddRange(myService.TopAlbumsForView(author, tempPage));
            if (tempPage == 1)
            {
                myService.SetOnlineOffline("Offline");
                allAlbums.LocalAlbums.AddRange(myService.TopAlbumsForView(author, tempPage));
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Albums", allAlbums.OnlineAlbums);
            }
            return View(allAlbums);
        }

        public ActionResult Tracks(string album, string author)
        {
            if (album == null)
                return View();
            List<Track> tracks = new List<Track>();
            myService.SetOnlineOffline("Offline");
            tracks.AddRange(myService.TracksOfAlbum(author, album));
            myService.SetOnlineOffline("Online");
            tracks.AddRange(myService.TracksOfAlbum(author, album));
            return View(tracks);
        }

        public ActionResult Search(int? numPage, string reqest = "")
        {           
            if (reqest != "")
            {
                ViewBag.reqest = reqest;
                int tempPage = numPage ?? 1;
                var searchresult = new SearchResult()
                {
                    SearchReqest = reqest
                };
                if (tempPage == 1)
                {
                    myService.SetOnlineOffline("Online");
                    searchresult.MyOnline.Artists.AddRange(myService.SearchArtists(reqest, tempPage));
                    searchresult.MyOnline.Albums.AddRange(myService.SearchAlbums(reqest, tempPage));
                    searchresult.MyOnline.Tracks.AddRange(myService.SearchTracks(reqest, tempPage));
                }

                myService.SetOnlineOffline("Offline");
                searchresult.MyLocal.Artists.AddRange(myService.SearchArtists(reqest, tempPage));
                searchresult.MyLocal.Albums.AddRange(myService.SearchAlbums(reqest, tempPage));
                searchresult.MyLocal.Tracks.AddRange(myService.SearchTracks(reqest, tempPage));
                                   
                if (Request.IsAjaxRequest() && numPage != null)
                {
                    return PartialView("_SearchReqest", searchresult);
                }
                return View(searchresult);
            }
            return View();
        }

        public ActionResult AddArtists(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;
            myService.SetOnlineOffline("Online");
            List<Artist> tempArtists = new List<Artist>();
            tempArtists.AddRange(myService.SearchArtists(reqest, tempPage));           
            return PartialView("_Items", tempArtists);
        }

        public ActionResult AddAlbums(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;
            myService.SetOnlineOffline("Online");
            List<Album> tempAlbums = new List<Album>();
            tempAlbums.AddRange(myService.SearchAlbums(reqest, tempPage));
            return PartialView("_Albums", tempAlbums);
        }

        public ActionResult AddTracks(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;
            myService.SetOnlineOffline("Online");
            List<Track> tempTracks = new List<Track>();
            tempTracks.AddRange(myService.SearchTracks(reqest, tempPage));
            return PartialView("Tracks", tempTracks);
        }

        public ActionResult MyAudio(string url)
        {
            var file = url;
            return File(file, "audio/mp3");
        }

        public ActionResult SetOnOff(string onOff, string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}