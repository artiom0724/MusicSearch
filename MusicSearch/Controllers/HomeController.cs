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
        public ActionResult Index(int ? author)
        {
            int numPage = author ?? 1;
            if(Request.IsAjaxRequest())
            {
                return PartialView("_Items", myService.TopAuthorsForView(numPage));
            }
            return View(myService.TopAuthorsForView(numPage));
        }
       
        public ActionResult Albums(int? numPage,string author = "")
        {
            int tempPage = numPage ?? 1;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Albums", myService.TopAlbumsForView(author,tempPage));
            }
            return View(myService.TopAlbumsForView(author,tempPage));
        }

        public ActionResult Tracks(string album, string author)
        {            
            return View(myService.TracksOfAlbum(author, album));
        }

        public ActionResult Search(int? numPage, string reqest = "")
        {

            if (reqest != "")
            {
                int tempPage = numPage ?? 1;
                var searchresult = new SearchResult()
                {
                    Authors = myService.SearchArtists(reqest, tempPage),
                    Albums = myService.SearchAlbums(reqest, tempPage),
                    Tracks = myService.SearchTracks(reqest, tempPage),
                    SearchReqest = reqest
                };
                var reqestModel = new List<string> {reqest};
                
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Search", searchresult);
                }
                return View(searchresult);
            }
            return View();
        }
    }
}