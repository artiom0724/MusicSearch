using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicSearch.apiService;
using System.Text.RegularExpressions;

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
    }
}