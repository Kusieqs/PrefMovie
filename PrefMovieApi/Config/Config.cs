using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        // Style for button
        public static Style styleForButton;

        // Style for poster button
        public static Style styleForPosterButton;

        // Special List to read movies
        public static List<ElementParameters> IdForMovie = new List<ElementParameters>();

        // Special dicitionary to setting buttons
        public static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
    }
}
