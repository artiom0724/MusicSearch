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
            topAuthorsForView(1);
            return View();
        }
       
        public ActionResult Albums(string author = "")
        {
            topAlbumsForView(author,1);
            return View();
        }

        public ActionResult Tracks(string album, string author)
        {
            tracksOfAlbum(author, album, 1);
            return View();
        }
    }
}