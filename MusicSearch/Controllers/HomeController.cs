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
            myService.SetOnlineOffline(onOff);
            int numPage = author ?? 1;
            if(Request.IsAjaxRequest() && numPage!=1)
            {
                return PartialView("_Items", myService.TopAuthorsForView(numPage));
            }
            return View(myService.TopAuthorsForView(numPage));
        }
       
        public ActionResult Albums(int? numPage,string author = "", string onOff = "Online")
        {
            myService.SetOnlineOffline(onOff);
            int tempPage = numPage ?? 1;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Albums", myService.TopAlbumsForView(author,tempPage));
            }
            return View(myService.TopAlbumsForView(author,tempPage));
        }

        public ActionResult Tracks(string album, string author, string onOff = "Online")
        {
            myService.SetOnlineOffline(onOff);
            return View(myService.TracksOfAlbum(author, album));
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