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
using TMDbLib.Objects.TvShows;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

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

            // Taking 8 random films 
            var randomMovies = movies.Results.OrderBy(x => random.Next()).Take(8);

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

            // Taking 8 random films 
            var randomMovies = movies.Results.OrderBy(x => random.Next()).Take(8);

            // setting main panel to application
            return mainStackPanel = SetInformationToStackPanel(mainStackPanel, randomMovies);
        }

        /// <summary>
        /// Setting StackPanel with diffrent latest tvShows to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal tvShows</returns>
        public static StackPanel TheLatestSeries(StackPanel mainStackPanel)
        {
            MainWindow.logger.Log(LogLevel.Info, "TheLatestSeries method activated");

            // Setting date for -3 months ago
            DateTime today = DateTime.Today;
            DateTime threeMonthsAgo = today.AddMonths(-3);

            // The new once movies from last 3 months
            var tvShows = GeneralInfo.client.DiscoverTvShowsAsync()
                .WhereFirstAirDateIsAfter(threeMonthsAgo)
                .WhereFirstAirDateIsBefore(today)
                .Query().Result;

            // Taking 8 random tvShows 
            var randomTvShows = tvShows.Results.OrderBy(x => random.Next()).Take(8);

            return mainStackPanel = SetInformationToStackPanel(mainStackPanel, randomTvShows);
        }

        /// <summary>
        /// Setting StackPanel with diffrent the best tvShows to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal tvShows</returns>
        public static StackPanel TheBestSeries(StackPanel mainStackPanel)
        {
            MainWindow.logger.Log(LogLevel.Info, "TheBestSeries method activated");

            // setting tvShows with the best average vote
            var tvShows = GeneralInfo.client.DiscoverTvShowsAsync()
                .WhereVoteAverageIsAtLeast(8)
                .Query().Result;

            // Taking 8 random tvShows
            var randomTvShows = tvShows.Results.OrderBy(x => random.Next()).Take(8);

            return mainStackPanel = SetInformationToStackPanel(mainStackPanel, randomTvShows);
        }

        /// <summary>
        /// Setting poster as image to application
        /// </summary>
        /// <param name="randomMoviesOrTvShows">Object of SearchMovie or SearchTv instance</param>
        /// <returns>BitmapImage object</returns>
        private static BitmapImage SetPoster(dynamic randomMoviesOrTvShows)
        {
            string posterUrl = BASE_URL + randomMoviesOrTvShows.PosterPath;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(posterUrl, UriKind.Absolute);
            image.DecodePixelWidth = 200;
            image.EndInit();

            return image;
        }

        /// <summary>
        /// Setting information about movie.
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where it will be</param>
        /// <param name="randomMoviesOrTvShows">IEnumerable<dynamic> with diffrent class</param>
        /// <returns>Stack panel with inputed information about movies</returns>
        private static StackPanel SetInformationToStackPanel(StackPanel mainStackPanel, IEnumerable<dynamic> randomMoviesOrTvShows)
        {
            MainWindow.logger.Log(LogLevel.Info, "SetInformationToStackPanel activated");

            foreach (var movieOrTvShow in randomMoviesOrTvShows)
            {
                // Setting stack panel for poster and informations 
                StackPanel itemStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(20, 0, 30, 0),
                    Width = 400
                };
                
                // Setting poster to posterBrush
                var posterBrush = new ImageBrush
                {
                    ImageSource = SetPoster(movieOrTvShow), 
                    Stretch = Stretch.UniformToFill 
                };

                // Create a Rectangle to display the image with rounded corners
                Rectangle posterRectangle = new Rectangle
                {
                    RadiusX = 10, 
                    RadiusY = 10,
                    Width = 200,
                    Fill = posterBrush 
                };

                // Average rate
                TextBlock averageRate = new TextBlock()
                {
                    Text = movieOrTvShow.VoteAverage.ToString("N1"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.DemiBold,
                    FontSize = 15
                };

                // Border for textblock
                Border averageBorder = new Border()
                {
                    CornerRadius = new CornerRadius(10),
                    Child = averageRate,
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0,0,7,7)
                };

                // Grid for poster with average vote
                Grid posterGrid = new Grid();
                posterGrid.Children.Add(posterRectangle);
                posterGrid.Children.Add(averageBorder);

                // Add the bordered poster to the item stack panel
                itemStackPanel.Children.Add(posterGrid);

                // Setting stack panel for information of movie
                StackPanel informationMovie = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(20,10,0,0)
                };

                // Setting information of movie
                for (int i = 0; i < 4; i++)
                {
                    double fontSize = i == 0 ? 22 : 19;
                    TextBlock text = new TextBlock()
                    {
                        Style = GeneralInfo.styleThemeOfElement,
                        FontSize = fontSize,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    switch (i)
                    {
                        // Title
                        case 0:
                            text.Text = movieOrTvShow is SearchMovie ? $"{movieOrTvShow.Title}" : $"{movieOrTvShow.Name}";
                            break;
                        // Release Date
                        case 1:
                            text.Text = movieOrTvShow is SearchMovie ? $"{movieOrTvShow.ReleaseDate.Year}" : $"{movieOrTvShow.FirstAirDate.Year}";
                            break;
                        // Genre
                        case 2:
                            List<int> genreId = movieOrTvShow.GenreIds;

                            if (movieOrTvShow is SearchMovie)
                            {
                                foreach (var genre in genreId)
                                {
                                    var genreName = (MoviesGenre)genre;
                                    string changedGenre = genreName.ToString().Replace('_', ' ') + Environment.NewLine;
                                    text.Text += changedGenre;
                                }
                            }
                            else
                            {
                                foreach (var genre in genreId)
                                {
                                    var genreName = (TvShowsGenre)genre;
                                    string changedGenre = genreName.ToString().Replace("AND", "&").Replace('_', ' ') + Environment.NewLine;
                                    text.Text += changedGenre;
                                }
                            }
                            break;
                    }

                    if(string.IsNullOrEmpty(text.Text))
                    {
                        string nameOfTheme = i == 0 ? "Title" : i == 1 ? "Average Vote" : i == 2 ? "Date Relase" : "Genre";
                        MainWindow.logger.Log(LogLevel.Warn, $"Text is empty for: {nameOfTheme} {nameof(randomMoviesOrTvShows)}");
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
