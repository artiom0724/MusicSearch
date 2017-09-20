using MusicSearch.apiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mySearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalSearcher localSearcher = new LocalSearcher(@"C:\Users\a.zubel\Music");
            localSearcher.JastDoIt();
        }
    }
}
