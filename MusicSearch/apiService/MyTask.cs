using MusicSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            resultSearchingFiles = new List<string>();
            resultSearchingFiles.Clear();
            SearchDirectories(new string[] { offlinePath });
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
                Album = audio.Tag.Album,
                Duration = audio.Properties.Duration.Seconds,
                Url = file
            };

            //dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Tracks]");
            if (dbContext.Tracks.Where(elem => elem.Url == file).Count() == 0)
            {
                var updataTrack = SearchTracks(track.Artist + " " + track.Name).First();
                track.ImageLarge = updataTrack.ImageLarge;
                track.Listeners = updataTrack.Listeners;
                dbContext.Tracks.Add(track);
                dbContext.SaveChanges();
            }

            var updataAlbum = SearchAlbums(track.Album).Where(item => item.ArtistAlbum == track.Artist).First();
            if (dbContext.Albums.Where(elem => elem.Name == updataAlbum.Name).Count() == 0)
            {
                dbContext.Albums.Add(updataAlbum);
                dbContext.SaveChanges();
            }

            var updataArtist = SearchArtists(track.Artist).First();
            if (dbContext.Artists.Where(elem => elem.Name == updataArtist.Name).Count() == 0)
            {
                dbContext.Artists.Add(updataArtist);
                dbContext.SaveChanges();
            }
        }

    }
}