using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrefMovieApi
{
    public static class Config
    {
        // Api Key
        public const string API_KEY_TO_TMDB = "";

        // base url to posters
        public const string BASE_URL = "https://image.tmdb.org/t/p/w500";

        // Path to log file
        public const string PATH_TO_LOG = "log.txt";

        // Path to json
        public const string PATH_TO_JSON = "LibraryFile.txt";

        // Style for infomration about element
        public static Style styleThemeOfElement;

        //Style for buttom
        public static Style styleForButton;

        // Special List to read movies
        public static List<ElementParameters> IdForMovie = new List<ElementParameters>();
    }
}
