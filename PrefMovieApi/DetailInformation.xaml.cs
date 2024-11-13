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
        private readonly ElementParameters element;
        public DetailInformation(ElementParameters element)
        {
            MainWindow.logger.Log(LogLevel.Info, $"Open new window to search {element.Title}");
            InitializeComponent();
            this.element = element;
            _ = LoadDetails();
        }


        private async Task LoadDetails()
        {
            // Pobranie listy filmów
            List<SearchMovie> movies = await GetMovieByTitle();

            SearchMovie movie = movies
                .Where(x => x.ReleaseDate == Config.IdForMovie.Where(y => y.Title == element.Title).FirstOrDefault().Date)
                .FirstOrDefault();
        }

        public async Task<List<SearchMovie>> GetMovieByTitle()
        {
            var searchResults = await GeneralInfo.client.SearchMovieAsync(element.Title);
            List<SearchMovie> movies = new List<SearchMovie>();

            if (searchResults.Results.Count > 0)
            {
                foreach (var movie in searchResults.Results)
                {
                    if (string.Equals(movie.Title, element.Title, StringComparison.OrdinalIgnoreCase))
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
                    if (string.Equals(tvShow.Name, element.Title, StringComparison.OrdinalIgnoreCase))
                    {
                        return tvShow;
                    }
                }
            }
            return null; 
        }
    }

}

