using MusicSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace MusicSearch.apiService
{
    public class MyTask : MyService
    {     
        private string[] extensionFiles;
        List<string> resultSearchingFiles;

        public MyTask(string path)
        {
            offlinePath = path;
            extensionFiles = new string[] { "mp3", "ogg", "wma", "flac", "aac", "mmf", "amr", "m4a", "m4r", "mp2", "wav" };
        }

        public void StartTimer()
        {
            int num = 0;
            TimerCallback tm = new TimerCallback(JastDoIt);
            Timer timer = new Timer(tm, num, 0, 300000);//300000
        }
     
        public void SetOfflinePath(string path)
        {
            offlinePath = path;
        }

        public void JastDoIt(object obj = null)
        {
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Tracks]");
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Albums]");
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Artists]");

            dbContext.SaveChanges();
            //return;

            resultSearchingFiles = new List<string>();
            resultSearchingFiles.Clear();
            SearchDirectories(new string[] { offlinePath });

            DeleteDeletingMusic();

            ParseInDB();
        }

        public void SearchDirectories(string[] dir)
        {
            foreach (var tempDir in dir)
            {
                SearchFiles(tempDir);
                string[] dirInDir = Directory.GetDirectories(tempDir);
                SearchDirectories(dirInDir);
            }
        }

        public void SearchFiles(string paths)
        {
            foreach (var item in extensionFiles)
            {
                string[] filesInDir = Directory.GetFiles(paths, "*." + item);
                foreach (var file in filesInDir)
                {
                    resultSearchingFiles.Add(file);
                }
            }

        }

        public void DeleteDeletingMusic()
        {
            List<Track> deleted = new List<Track>();
            deleted.AddRange(dbContext.Tracks.ToList().Where(elem => IsNonInDirectory(elem)));
            if (deleted.Count() > 0)
            {
                dbContext.Tracks.RemoveRange(deleted);
                dbContext.SaveChanges();
            }
        }

        public bool IsNonInDirectory(Track track)
        {
            foreach (var item in resultSearchingFiles)
            {
                if (track.Url == item)
                    return false;
            }
            return true;
        }

        public void ParseInDB()
        {
            foreach (var file in resultSearchingFiles)
            {
                ReadOptionsAndUpdata(file);
            }
        }

        public void ReadOptionsAndUpdata(string file)
        {
            var audio = TagLib.File.Create(file);
            Track track = new Track
            {
                Artist = String.Join(",", audio.Tag.Performers),
                Name = audio.Tag.Title,
                Duration = audio.Properties.Duration.Seconds,
                Url = file
            };         
            UpDataTrack(file, track);
            UpDataAlbum(track);
            UpDataArtist(track);           
        }

        public void UpDataTrack(string file, Track track)
        {
            if (dbContext.Tracks.Where(elem => elem.Url == file).Count() == 0)
            {
                if(track.Name.Contains('('))
                {
                    track.Name = track.Name.Substring(0, track.Name.IndexOf('('));
                }
                var updataTrack = InfoTrack(track.Artist,track.Name);
                //track.ImageLarge = updataTrack.ImageLarge;
                //track.Listeners = updataTrack.Listeners;                  
                track.Album = updataTrack.Album;
                dbContext.Tracks.Add(track);
                dbContext.SaveChanges();
            }
        }

        public void UpDataAlbum(Track track)
        {
            var getdataAlbum = SearchAlbums(track.Album).Where(item => item.Name == track.Album);
            if (getdataAlbum.Count() == 0)
                return;
            var updataAlbum = getdataAlbum.First();
            if (dbContext.Albums.Where(elem => elem.Name == updataAlbum.Name).Count() == 0)
            {
                dbContext.Albums.Add(updataAlbum);
                dbContext.SaveChanges();
            }
        }

        public void UpDataArtist(Track track)
        {
            var updataArtist = SearchArtists(track.Artist).First();
            if (dbContext.Artists.Where(elem => elem.Name == updataArtist.Name).Count() == 0)
            {
                dbContext.Artists.Add(updataArtist);
                dbContext.SaveChanges();
            }
        }

    }
}