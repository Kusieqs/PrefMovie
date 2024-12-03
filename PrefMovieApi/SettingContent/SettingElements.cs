using System;
using System.Collections.Generic;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using TMDbLib.Objects.General;

namespace PrefMovieApi
{
    public static class SettingElements
    {

        // random object
        
        private readonly static Random random = new Random();

        /// <summary>
        /// Setting StackPanel with diffrent latest movies to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal movies</returns>
        public static StackPanel TheLatestMovies(StackPanel mainStackPanel)
        {
            Config.logger.Log(LogLevel.Info, "TheNewOnceMovies method activated");

            // Setting date for -3 months ago
            DateTime today = DateTime.Today;
            DateTime threeMonthsAgo = today.AddMonths(-3);

            // The new once movies from last 3 months
            var movies = Config.client.DiscoverMoviesAsync()
                .WhereReleaseDateIsAfter(threeMonthsAgo)
                .WhereReleaseDateIsBefore(today)
                .Query().Result;

            // Taking 8 random films 
            var randomMovies = CheckSameId(movies);

            // setting main panel to application
            return mainStackPanel = ElementInfo.SetInformationToStackPanel(mainStackPanel, randomMovies);
        }

        /// <summary>
        /// Setting StackPanel with diffrent the best movies to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal movies</returns>
        public static StackPanel TheBestMovies(StackPanel mainStackPanel)
        {
            Config.logger.Log(LogLevel.Info, "TheBestMovies method activated");

            // setting movies with the best average vote
            var movies = Config.client.DiscoverMoviesAsync()
                .WhereVoteAverageIsAtLeast(8)
                .Query().Result;

            // Taking 8 random films 
            var randomMovies = CheckSameId(movies);

            // setting main panel to application
            return mainStackPanel = ElementInfo.SetInformationToStackPanel(mainStackPanel, randomMovies);
        }

        /// <summary>
        /// Setting StackPanel with diffrent latest tvShows to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal tvShows</returns>
        public static StackPanel TheLatestSeries(StackPanel mainStackPanel)
        {
            Config.logger.Log(LogLevel.Info, "TheLatestSeries method activated");

            // Setting date for -3 months ago
            DateTime today = DateTime.Today;
            DateTime threeMonthsAgo = today.AddMonths(-3);

            // The new once movies from last 3 months
            var tvShows = Config.client.DiscoverTvShowsAsync()
                .WhereFirstAirDateIsAfter(threeMonthsAgo)
                .WhereFirstAirDateIsBefore(today)
                .Query().Result;

            // Taking 8 random tvShows 
            var randomTvShows = CheckSameId(tvShows);

            return mainStackPanel = ElementInfo.SetInformationToStackPanel(mainStackPanel, randomTvShows);
        }

        /// <summary>
        /// Setting StackPanel with diffrent the best tvShows to main window
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where diffrent elemnts will input</param>
        /// <returns>Stack panel with proposal tvShows</returns>
        public static StackPanel TheBestSeries(StackPanel mainStackPanel)
        {
            Config.logger.Log(LogLevel.Info, "TheBestSeries method activated");

            // setting tvShows with the best average vote
            var tvShows = Config.client.DiscoverTvShowsAsync()
                .WhereVoteAverageIsAtLeast(8)
                .Query().Result;

            // Taking 8 random tvShows
            var randomTvShows = CheckSameId(tvShows);

            return mainStackPanel = ElementInfo.SetInformationToStackPanel(mainStackPanel, randomTvShows);
        }

        /// <summary>
        /// Checking same id of elements.
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="elements">Collection of elements</param>
        /// <returns>list of elements</returns>
        private static IEnumerable<dynamic> CheckSameId<T>(SearchContainer<T> elements) where T : class
        {
            try
            {
                Config.logger.Log(LogLevel.Info, "Checking elements with same id");
                List<T> list = new List<T>();
                do
                {
                    dynamic element = elements.Results.OrderBy(x => random.Next()).First();

                    if (!Config.buttons.Any(x => x.Key.ToString() == element.Id.ToString()))
                    {
                        list.Add(element);
                        Config.buttons.Add(element.Id.ToString(), null);
                    }
                } while (list.Count < 8);

                return list.AsEnumerable<T>();
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.Message);
                MessageBox.Show("Critical error","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
