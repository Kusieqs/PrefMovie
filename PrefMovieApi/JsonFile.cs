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
        public void SerializeLibrary()
        {
            MainWindow.logger.Log(LogLevel.Info, "SerializeLibrary Activated");
            if(!(Library.titles.Count == 0 || Library.titles is null))
            {
                MainWindow.logger.Log(LogLevel.Info, "Library is NOT empty");
                string jsonSerialize = JsonConvert.SerializeObject(Library.titles);
                File.WriteAllText(Config.PATH_TO_JSON, jsonSerialize);
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
            if (File.Exists(Config.PATH_TO_JSON))
            {
                MainWindow.logger.Log(LogLevel.Info, "Reading file");
                string readerFile = File.ReadAllText(Config.PATH_TO_JSON);
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
