using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace PrefMovieApi
{
    /// <summary>
    /// Logika interakcji dla klasy DetailInformation.xaml
    /// </summary>
    public partial class DetailInformation : Window
    {
        private readonly string _title;
        public DetailInformation(string title)
        {
            // TITLE jest pusty
            MainWindow.logger.Log(LogLevel.Info, $"Open new window to search {title}"); 
            InitializeComponent();
            _title = title;
            _ = LoadDetails();
        }


        private async Task LoadDetails()
        {
            // Pobranie listy filmów
            List<SearchMovie> movies = await GetMovieByTitle();
            if (movies.Count > 0)
            {
                MessageBox.Show("Znalezione filmy:");
                foreach (var movie in movies)
                {
                    MessageBox.Show($"Tytuł: {movie.Title}, Data wydania: {movie.ReleaseDate}");
                }
            }
            else
            {
                MessageBox.Show("Nie znaleziono filmów o podanym tytule.");
            }
        }

        public async Task<List<SearchMovie>> GetMovieByTitle()
        {
            var searchResults = await GeneralInfo.client.SearchMovieAsync(_title);
            List<SearchMovie> movies = new List<SearchMovie>();

            if (searchResults.Results.Count > 0)
            {
                foreach (var movie in searchResults.Results)
                {
                    if (string.Equals(movie.Title, _title, StringComparison.OrdinalIgnoreCase))
                    {
                        movies.Add(movie);  
                    }
                }
            }
            return movies; 
        }
        public SearchTv GetTvShowByTitle()
        {
            var searchResults = GeneralInfo.client.DiscoverTvShowsAsync().Query().Result;

            if (searchResults.Results.Count > 0)
            {
                foreach (var tvShow in searchResults.Results)
                {
                    if (string.Equals(tvShow.Name, _title, StringComparison.OrdinalIgnoreCase))
                    {
                        return tvShow;
                    }
                }
            }
            return null; 
        }
    }

}

