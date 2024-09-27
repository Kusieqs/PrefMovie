using System;
using System.Collections.Generic;
using System.Globalization;
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
        const string API_KEY_TO_TMDB = "";
        TMDbClient client = null;

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
                    throw new FormatException();
                }

                // TODO: Załadowanie 5/6 list roznymi kategorami filmow ( od preferonwaych po reszte )
            }
            catch(Exception)
            {
                MessageBox.Show("Crticial Error!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                Window window = Window.GetWindow(this);
                // TODO: Zamkniecie okna glownego
            }
        }
        public void SetListOfMovies()
        {
            // TODO: Implement list of movies 
        }

    }
}
