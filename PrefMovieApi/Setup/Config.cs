using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TMDbLib.Client;

namespace PrefMovieApi.Setup
{
    internal class Config
    {
        // Api Key
        public const string API_KEY_TO_TMDB = "";

        // base url to posters
        public const string BASE_URL = "https://image.tmdb.org/t/p/w500";

        // Interface as logger
        public static ILogger logger = new FileLogger();

        // List of every logs messages
        public static List<(LogLevel, string)> loggerMessages = new List<(LogLevel, string)>();

        // Client object
        public static TMDbClient client = null;

        // Special object to create jsonFile
        public static JsonFile jsonFile = new JsonFile();

        // List to control which window is open
        public static List<string> existingWindows = new List<string>();

        // Style for infomration about element
        public static Style styleThemeOfElement;

        // Style for button
        public static Style styleForButton;

        // Style for poster button
        public static Style styleForPosterButton;

        // Style for theme textblock in general
        public static Style styleForThemeGeneral;

        // Style for button of refresh
        public static Style styleRefreshButton;

        // Special List to read movies
        public static List<ElementParameters> IdForMovie = new List<ElementParameters>();

        // Special dicitionary to setting buttons
        public static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
    }
}
