using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSearch.apiService
{
    public class TaskSetter
    {
        public string offlinePath;
        public string pathToSearcher;
        public int interval;

        public TaskSetter(string _offlinePath = @"C:\Users\a.zubel\Music",
            string _pathToSearcher = @"C:/Users/a.zubel/MusicSearch/mySearcher/bin/Debug/mySearcher.exe",
            int _interval = 5)
        {
            offlinePath = _offlinePath;                      
            pathToSearcher = _pathToSearcher;                    
            interval = _interval;
        }

        public void CreateTaskRunner()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "NewTask for search Music";
                TimeTrigger timeTrigger = new TimeTrigger();
                timeTrigger.StartBoundary = DateTime.Now;
                timeTrigger.Repetition.Interval = TimeSpan.FromMinutes(interval);
                td.Triggers.Add(timeTrigger);

                td.Actions.Add(new ExecAction(pathToSearcher, offlinePath));
                ts.RootFolder.RegisterTaskDefinition(@"TestMyTasck", td);
            }
        }
    }
}