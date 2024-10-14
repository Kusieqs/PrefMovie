using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefMovieApi
{
    public class JsonFile
    {
        /// <summary>
        /// Serialize list to file
        /// </summary>
        /// <param name="listOfMoviesAndTvShows"> List of movies and tv shows</param>
        public void SerializeLibrary(List<string> listOfMoviesAndTvShows)
        {
            MainWindow.logger.Log(LogLevel.Info, "SerializeLibrary Activated");
            if(!(listOfMoviesAndTvShows.Count == 0 || listOfMoviesAndTvShows is null))
            {
                MainWindow.logger.Log(LogLevel.Info, "Library is NOT empty");
                string jsonSerialize = JsonConvert.SerializeObject(listOfMoviesAndTvShows);
                File.WriteAllText("LibraryFile.txt", jsonSerialize);
            }
            else
            {
                MainWindow.logger.Log(LogLevel.Warn, "Library is empty");
            }
        }

        /// <summary>
        /// Deserialize file into list
        /// </summary>
        /// <returns>List of strings</returns>
        public List<string> DeserializeLibrary()
        {
            MainWindow.logger.Log(LogLevel.Info, "DeserializeLibrary Activated");
            if (File.Exists("LibraryFile.txt"))
            {
                MainWindow.logger.Log(LogLevel.Info, "Reading file");
                string readerFile = File.ReadAllText("LibraryFile.txt");
                return JsonConvert.DeserializeObject<List<string>>(readerFile);
            }
            else
            {
                MainWindow.logger.Log(LogLevel.Warn,"File doesn't exist");
                return new List<string>();
            }
        }
    }
}
