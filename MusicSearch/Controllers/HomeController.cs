using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicSearch.apiService;

namespace MusicSearch.Controllers
{
    public class HomeController : Controller
    {
        public MyService myService;

        public HomeController()
        {
            myService = new MyService();
        }
        public ActionResult Index()
        {            
            return View(myService.TopAuthorsForView());
        }
       
        public ActionResult Albums(string author = "")
        {            
            return View(myService.TopAlbumsForView(author));
        }

        public ActionResult Tracks(string album, string author)
        {            
            return View(myService.TracksOfAlbum(author, album));
        }
    }
}