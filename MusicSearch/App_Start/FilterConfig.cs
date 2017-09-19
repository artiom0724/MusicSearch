using MusicSearch.apiService;
using System.Web;
using System.Web.Mvc;

namespace MusicSearch
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
