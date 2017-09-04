using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicSearch.apiService;

namespace MusicSearch.Controllers
{
    public class HomeController : MyService
    {
        public ActionResult Index()
        {
            getTopOfAuthors(1);
            ViewBag.authors = authors;
            return View();
        }
       
        public ActionResult Albums(string author="")
        {
            if (author != "")
            {
                getTopAlbums(1, author);
                ViewBag.albums = albums;
            }
            return View();
        }

        public ActionResult About()
        {
            getTopAlbums(1,"");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}