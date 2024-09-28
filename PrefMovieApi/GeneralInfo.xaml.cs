using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Movies;

namespace PrefMovieApi
{
    /// <summary>
    /// Logika interakcji dla klasy GeneralInfo.xaml
    /// </summary>

    public partial class GeneralInfo : UserControl
    {
        // Api Key
        const string API_KEY_TO_TMDB = "";

        // client object
        TMDbClient client = null;

        // special control
        public static bool isReload = true;

        public GeneralInfo()
        {
            InitializeComponent();
            try
            {
                // Connect to API TMDb
                client = new TMDbClient(API_KEY_TO_TMDB);
                client.DefaultLanguage = "en";


                // Checking that the API key is correct
                var testRequest = client.GetMovieAsync(550).Result;
                if(testRequest == null)
                {
                    MainWindow.logger.Log(LogLevel.Error, "Test request is null");
                    throw new FormatException();
                }

                MainWindow.logger.Log(LogLevel.Info, "Api is correct");

                // Setting movies
                if (isReload)
                {
                    MainWindow.logger.Log(LogLevel.Info, "isReload = true");
                    isReload = false;

                    // TODO: Uzycie delegatu 
                    SetMainWindoOfMovies();
                }
                else
                {
                    MainWindow.logger.Log(LogLevel.Warn, "isReload = false");
                }

            }
            catch(Exception)
            {
                MessageBox.Show("Crticial Error!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                Window window = Window.GetWindow(this);
                // TODO: Zamkniecie okna glownego
            }
        }

        /// <summary>
        /// Setting list of prefering movies in main screen 
        /// </summary>
        public void SetMainWindoOfMovies()
        {
            TheNewOnceMovies = SettingMovies.TheNewOnceMovies();
            TheBestMovies = SettingMovies.TheBestMovies();
            TheNewOnceSeries = SettingMovies.TheNewOnceSeries();
            TheBestSeries = SettingMovies.TheBestSeries();
            Preferences = SettingMovies.Preferences();
        }

    }
}
