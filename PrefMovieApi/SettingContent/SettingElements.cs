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
using PrefMovieApi.Setup;
using System.Collections;
using TMDbLib.Client;
using System.Windows.Forms;

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
        /// Setting StackPanel with diffrent season movies and tvShows to main window
        /// </summary>
        /// <param name="mainStackPanel"></param>
        /// <returns></returns>
        public static StackPanel SeasonPrefering(StackPanel mainStackPanel)
        {
            Config.logger.Log(LogLevel.Info, "SeasonPrefering method activated");

            var currentSeason = SettingSeasonByDate();

            var tvShows = Config.client.DiscoverTvShowsAsync()
                .WhereVoteAverageIsAtLeast(7) 
                .WhereGenresInclude(GetGenres(currentSeason, false))
                .Query().Result;

            var movies = Config.client.DiscoverMoviesAsync()
                .WhereVoteAverageIsAtLeast(7)
                .IncludeWithAllOfGenre(GetGenres(currentSeason, true))
                .Query().Result;


            var randomTvShows = CheckSameId(tvShows,4);
            var randomMovies = CheckSameId(movies,4);

            return mainStackPanel = ElementInfo.SetInformationToStackPanel(mainStackPanel, randomMovies as IEnumerable<SearchMovie>, randomTvShows as IEnumerable<SearchTv>);
        }

        /// <summary>
        /// Checking same id of elements.
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="elements">Collection of elements</param>
        /// <returns>list of elements</returns>
        private static IEnumerable<dynamic> CheckSameId<T>(SearchContainer<T> elements, int count = 8) where T : class
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
                } while (list.Count < count);

                return list.AsEnumerable<T>();
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Setting season name, choosing by date
        /// </summary>
        /// <returns>Season enum</returns>
        private static SeasonName? SettingSeasonByDate()
        {
            DateTime date = DateTime.Now;

            var seasons = new List<(DateTime Start, DateTime End, SeasonName Name)>
            {   (new DateTime(date.Year, 1, 1), new DateTime(date.Year, 1, 2), SeasonName.NewYear),
                (new DateTime(date.Year, 1, 3), new DateTime(date.Year, 2, 12), SeasonName.Carnival),
                (new DateTime(date.Year, 2, 13), new DateTime(date.Year, 2, 16), SeasonName.Valentine),
                (new DateTime(date.Year, 2, 17), new DateTime(date.Year, 3, 15), SeasonName.Winter),
                (new DateTime(date.Year, 3, 16), new DateTime(date.Year, 4, 25), SeasonName.Easter),
                (new DateTime(date.Year, 4, 26), new DateTime(date.Year, 6, 20), SeasonName.Spring),
                (new DateTime(date.Year, 6, 21), new DateTime(date.Year, 8, 20), SeasonName.Summer),
                (new DateTime(date.Year, 8, 21), new DateTime(date.Year, 9, 22), SeasonName.BackToSchool),
                (new DateTime(date.Year, 9, 23), new DateTime(date.Year, 10, 25), SeasonName.Autumn),
                (new DateTime(date.Year, 10, 26), new DateTime(date.Year, 11, 3), SeasonName.Halloween),
                (new DateTime(date.Year, 11, 4), new DateTime(date.Year, 12, 3), SeasonName.Autumn),
                (new DateTime(date.Year, 12, 4), new DateTime(date.Year, 12, 27), SeasonName.Christmas),
                (new DateTime(date.Year, 12, 28), new DateTime(date.Year, 12, 31), SeasonName.NewYear),
            };

            foreach (var season in seasons)
            {
                if (date >= season.Start && date <= season.End)
                {
                    return season.Name;
                }
            }

            Config.logger.Log(LogLevel.Error, "Lack of date");
            return null;
        }

        /// <summary>
        /// Choosing which list will be added to sorting by genres
        /// </summary>
        /// <param name="season"></param>
        /// <param name="isItMovie"></param>
        /// <returns>List of ints of genres</returns>
        private static IEnumerable<int> GetGenres(SeasonName? season, bool isItMovie)
        {
            if(isItMovie)
            {
                switch (season)
                {
                    case SeasonName.NewYear:
                        return new List<int>() { 10770 };
                    case SeasonName.Carnival:
                        return new List<int>() { 28, 12, 16, 14 };
                    case SeasonName.Valentine:
                        return new List<int>() { 10749 };
                    case SeasonName.Winter:
                        return new List<int>() { 28, 53, 80 };
                    case SeasonName.Spring:
                        return new List<int>() { 12, 16, 9648, 37 };
                    case SeasonName.Summer:
                        return new List<int>() { 12, 16 };
                    case SeasonName.BackToSchool:
                        return new List<int>() { 12, 35 };
                    case SeasonName.Autumn:
                        return new List<int>() { 10749, 14, 80 };
                    case SeasonName.Halloween:
                        return new List<int>() { 27 };
                    case SeasonName.Christmas:
                    case SeasonName.Easter:
                        return new List<int>() { 35, 16, 10751 };
                    default:
                        Config.logger.Log(LogLevel.Error, "List of genres is empty");
                        return null;
                }
            }
            else
            {
                switch (season)
                {
                    case SeasonName.NewYear:
                        return new List<int>() { 10763, 10762 };
                    case SeasonName.Carnival:
                        return new List<int>() { 10762, 10759, 16, 35 };
                    case SeasonName.Valentine:
                        return new List<int>() { 18 };
                    case SeasonName.Winter:
                        return new List<int>() { 18, 80, 99 };
                    case SeasonName.Spring:
                        return new List<int>() { 16, 35, 10763 };
                    case SeasonName.Summer:
                        return new List<int>() { 16, 10759, 10764 };
                    case SeasonName.BackToSchool:
                        return new List<int>() { 10762, 10764 };
                    case SeasonName.Autumn:
                        return new List<int>() { 18, 80 };
                    case SeasonName.Halloween:
                        return new List<int>() { 80 };
                    case SeasonName.Easter:
                    case SeasonName.Christmas:
                        return new List<int>() { 10751, 16, 35 };
                    default:
                        Config.logger.Log(LogLevel.Error, "List of genres is empty");
                        return null;
                }
            }
        }
    }
}
