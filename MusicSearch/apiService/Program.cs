using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSearch
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
