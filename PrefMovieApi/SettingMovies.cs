using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace PrefMovieApi
{
    public static class SettingMovies
    {
        // base url to posters
        private const string BASE_URL = "https://image.tmdb.org/t/p/w500";

        // random object
        public static Random random = new Random();

        /// <summary>
        /// Setting StackPanel with diffrent latest movies to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal movies</returns>
        public static StackPanel TheLatestMovies(StackPanel mainStackPanel)
        {
            MainWindow.logger.Log(LogLevel.Info, "TheNewOnceMovies method activated");

            // Setting date for -3 months ago
            DateTime today = DateTime.Today;
            DateTime threeMonthsAgo = today.AddMonths(-3);

            // The new once movies from last 3 months
            var movies = GeneralInfo.client.DiscoverMoviesAsync()
                .WhereReleaseDateIsAfter(threeMonthsAgo)  
                .WhereReleaseDateIsBefore(today)          
                .Query().Result;

            // Taking 5 random films 
            var randomMovies = movies.Results.OrderBy(x => random.Next()).Take(5);

            // setting main panel to application
            return mainStackPanel = SetInformationToStackPanel(mainStackPanel, randomMovies);
        }

        /// <summary>
        /// Setting StackPanel with diffrent the best movies to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal movies</returns>
        public static StackPanel TheBestMovies(StackPanel mainStackPanel)
        {
            MainWindow.logger.Log(LogLevel.Info, "TheBestMovies method activated");

            // setting movies with the best average vote
            var movies = GeneralInfo.client.DiscoverMoviesAsync()
                .WhereVoteAverageIsAtLeast(8)
                .Query().Result;

            // Taking 5 random films 
            var randomMovies = movies.Results.OrderBy(x => random.Next()).Take(5);

            // setting main panel to application
            return mainStackPanel = SetInformationToStackPanel(mainStackPanel, randomMovies);
        }

        public static StackPanel TheLatestSeries()
        {
            return null;
        }

        public static StackPanel TheBestSeries()
        {
            return null;
        }

        public static StackPanel Preferences()
        {
            return null;
        }

        /// <summary>
        /// Setting poster as image to application
        /// </summary>
        /// <param name="movie">Object of movie instance</param>
        /// <returns>BitmapImage object</returns>
        private static BitmapImage SetPoster(SearchMovie movie)
        {
            string posterUrl = BASE_URL + movie.PosterPath;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(posterUrl, UriKind.Absolute);
            image.EndInit();

            return image;
        }

        /// <summary>
        /// Setting information about movie.
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where it will be</param>
        /// <param name="randomMovies">IEnumerable<SearchMovie> with movies</param>
        /// <returns>Stack panel with inputed information about movies</returns>
        private static StackPanel SetInformationToStackPanel(StackPanel mainStackPanel, IEnumerable<SearchMovie> randomMovies)
        {
            foreach (var movie in randomMovies)
            {
                MainWindow.logger.Log(LogLevel.Info, $"Foreach loop activated: {movie.Title}");

                // Setting stack panel for poster and informations 
                StackPanel itemStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                };

                // Adding poster to stack panel
                System.Windows.Controls.Image poster = new System.Windows.Controls.Image();
                poster.Source = SetPoster(movie);
                itemStackPanel.Children.Add(poster);

                // Setting stack panel for information of movie
                StackPanel informationMovie = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                // Setting information of movie
                for (int i = 0; i < 4; i++)
                {
                    TextBlock text = new TextBlock();
                    switch (i)
                    {
                        case 1:
                            text.Text = movie.Title;
                            break;
                        case 2:
                            // TODO: Wywala nam opis obiektu (listy)
                            text.Text = movie.GenreIds.ToString();
                            break;
                        case 3:
                            text.Text = movie.VoteAverage.ToString();
                            break;
                        case 4:
                            // TODO: Ogarniecie 4 danej informacji
                            text.Text = movie.ToString();
                            break;
                    }
                    informationMovie.Children.Add(text);
                }

                itemStackPanel.Children.Add(informationMovie);
                mainStackPanel.Children.Add(itemStackPanel);
            }

            return mainStackPanel;
        }
    }
}
