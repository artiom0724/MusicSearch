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
        public MyService myService;

        public HomeController()
        {
            myService = new MyService();
        }

        public ActionResult Index(int ? author, string onOff = "Online")
        {
            ViewBag.reqest = "Search";
            int numPage = author ?? 1;
            myService.SetOnlineOffline(onOff);
            AllArtists allArtists = new AllArtists();
            allArtists.OnlineArtists = myService.TopAuthorsForView(numPage);
            if (numPage == 1)
            {
                myService.SetOnlineOffline("Offline");
                allArtists.LocalArtists = myService.TopAuthorsForView(numPage);
            }
            
            if(Request.IsAjaxRequest() && numPage!=1)
            {
                return PartialView("_Items", allArtists.OnlineArtists);
            }
            return View(allArtists);
        }
       
        public ActionResult Albums(int? numPage,string author = "", string onOff = "Online")
        {
            myService.SetOnlineOffline(onOff);
            int tempPage = numPage ?? 1;
            AllAlbums allAlbums = new AllAlbums();
            allAlbums.OnlineAlbums = myService.TopAlbumsForView(author, tempPage);
            if (numPage == 1)
            {
                myService.SetOnlineOffline("Offline");
                allAlbums.LocalAlbums = myService.TopAlbumsForView(author, tempPage);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Albums", allAlbums.OnlineAlbums);
            }
            return View(allAlbums);
        }

        public ActionResult Tracks(string album, string author, string onOff = "Online")
        {
            List<Track> tracks = new List<Track>();
            myService.SetOnlineOffline("Offline");
            tracks.AddRange(myService.TracksOfAlbum(author, album));
            myService.SetOnlineOffline("Online");
            tracks.AddRange(myService.TracksOfAlbum(author, album));
            return View(tracks);
        }

        public ActionResult Search(int? numPage, string reqest = "", string onOff = "Online")
        {
            myService.SetOnlineOffline(onOff);
            if (reqest != "" )
            {
                ViewBag.reqest = reqest;
                int tempPage = numPage ?? 1;
                var searchresult = new SearchResult()
                {
                    Authors = myService.SearchArtists(reqest, tempPage),
                    Albums = myService.SearchAlbums(reqest, tempPage),
                    Tracks = myService.SearchTracks(reqest, tempPage),
                    SearchReqest = reqest
                };

                if (Request.IsAjaxRequest() && numPage!=null)
                {
                    return PartialView("Search", searchresult);
                }
                return View(searchresult);
            }           
            return View();          
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