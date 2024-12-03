using Newtonsoft.Json;
using PrefMovieApi.Setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Config.logger.Log(LogLevel.Info, "SerializeLibrary Activated");
            if (!(Library.titles.Count == 0 || Library.titles is null))
            {
                Config.logger.Log(LogLevel.Info, "Library is NOT empty");
                string jsonSerialize = JsonConvert.SerializeObject(Library.titles);
                File.WriteAllText(SetupPaths.PATH_TO_JSON, jsonSerialize);
            }
            else
            {
                Config.logger.Log(LogLevel.Warn, "Library is empty");
            }
        }

        /// <summary>
        /// Deserialize file into list
        /// </summary>
        /// <returns>List of strings</returns>
        public List<ElementParameters> DeserializeLibrary()
        {
            Config.logger.Log(LogLevel.Info, "DeserializeLibrary Activated");
            if (File.Exists(SetupPaths.PATH_TO_JSON))
            {
                Config.logger.Log(LogLevel.Info, "Reading file");
                string readerFile = File.ReadAllText(SetupPaths.PATH_TO_JSON);
                return JsonConvert.DeserializeObject<List<ElementParameters>>(readerFile);
            }
            else
            {
                Config.logger.Log(LogLevel.Warn, "File doesn't exist");
                return new List<ElementParameters>();
            }
        }
    }
}
