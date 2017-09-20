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
            onlineWorker = new OnlineWorker();
        }

        public ActionResult Index(int ? author)
        {
            ViewBag.reqest = "Search";
            int numPage = author ?? 1;                   
            if(Request.IsAjaxRequest() && numPage!=1)
            {
                return PartialView("_Items", onlineWorker.TopAuthorsForView(numPage));
            }
            return View(onlineWorker.TopAuthorsForView(numPage));
        }
       
        public ActionResult Albums(int? numPage,string author = "")
        {        
            int tempPage = numPage ?? 1;     
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Albums", onlineWorker.TopAlbumsForView(author, tempPage));
            }
            return View(onlineWorker.TopAlbumsForView(author, tempPage));
        }

        public ActionResult Tracks(string album = null, string author = null)
        {
            if (album == null)
                return View();
            return View(onlineWorker.TracksOfAlbum(author, album));
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
                searchresult.Artists.AddRange(onlineWorker.SearchArtists(reqest, tempPage));
                searchresult.Albums.AddRange(onlineWorker.SearchAlbums(reqest, tempPage));
                searchresult.Tracks.AddRange(onlineWorker.SearchTracks(reqest, tempPage));
                return View(searchresult);
            }
            return View();
        }

        public ActionResult AddArtists(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;     
            return PartialView("_Items", onlineWorker.SearchArtists(reqest, tempPage));
        }

        public ActionResult AddAlbums(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;
            return PartialView("_Albums", onlineWorker.SearchAlbums(reqest, tempPage));
        }

        public ActionResult AddTracks(int? numPage, string reqest = "")
        {
            int tempPage = numPage ?? 1;                
            return PartialView("Tracks", onlineWorker.SearchTracks(reqest, tempPage));
        }

        public ActionResult MyAudio(string url)
        {
            var file = url;
            return File(file, "audio/mp3");
        }
    }
}