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
            MyTask myTask = new MyTask(args.First());
            myTask.JastDoIt();
        }
    }
}
