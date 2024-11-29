using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Search;

namespace PrefMovieApi
{
    public partial class SortingOutput : UserControl
    {

        public readonly SortingParameters SortingParameters; // Object of soritng parameters
        public SortingOutput(SortingParameters sortingParameters)
        {
            MainWindow.logger.Log(LogLevel.Info, $"Sorting of proposed films activated");
            InitializeComponent();
            SortingParameters = sortingParameters;

            // Creating main panel with proposal elements
            CreatingInstantOfList();
        }

        /// <summary>
        /// Creating IEnumerable<SearchMovieTvBase> with proposal elements
        /// </summary>
        private void CreatingInstantOfList()
        {
            // Clearing panel
            MainStackPanelForProposal.Children.Clear();
            MainStackPanelForProposal.Height = 0;

            // Setting list of elements
            IEnumerable<SearchMovieTvBase> choosenTitles = GetChosenTitles();

            // Setting elements to stack panel
            SetStackPanel(choosenTitles.Count(), choosenTitles);
        }

        /// <summary>
        /// Choosing which option user choose
        /// </summary>
        /// <returns>IEnumerable list of proposed elements</returns>
        private IEnumerable<SearchMovieTvBase> GetChosenTitles()
        {
            if (SortingParameters.IsFilmSorting)
            {
                return GetMedia(MediaType.Movie);
            }
            else if (SortingParameters.IsTvShowsSorting)
            {
                return GetMedia(MediaType.TvShow);
            }
            else
            {
                return GetMedia(MediaType.TvShow, 6).Concat(GetMedia(MediaType.Movie, 6));
            }
        }

        /// <summary>
        /// Setting sorting parameters to main list of elements
        /// </summary>
        /// <param name="mediaType">Type of sorting</param>
        /// <param name="countOfElements">Count of elements to display</param>
        /// <returns>IEnumerable list of proposed elements</returns>
        private IEnumerable<SearchMovieTvBase> GetMedia(MediaType mediaType, int countOfElements = 12)
        {
            MainWindow.logger.Log(LogLevel.Info, "Getting media of movie or TvShow");
            try
            {
                Random random = new Random();
                dynamic discover = mediaType == MediaType.Movie ? (dynamic)SetListOfMovies() : SetListOfTvShows();
                SetGenre(discover);
                SetOrdering(discover);

                if (SortingParameters.SelectedStars > 0)
                {
                    SetStars(discover);
                }

                SetDate(discover);
                return ConvertToObject(discover, countOfElements, random);
            }
            catch (Exception ex)
            {
                MainWindow.logger.Log(LogLevel.Error, $"Message: {ex.Message}");
                return null;
            }
        }

        #region Sorting features
        private void SetGenre(DiscoverTv discoverTv)
        {
            if(SortingParameters.Genre != null)
            {
                List<int> listOfGenre = new List<int>() { (int)(TvShowsGenre)Enum.Parse(typeof(TvShowsGenre), SortingParameters.ConvertGenre(typeof(TvShowsGenre))) };
                discoverTv = discoverTv.WhereGenresInclude(listOfGenre);
            }
        }
        private void SetGenre(DiscoverMovie discoverMovie)
        {
            if(SortingParameters.Genre != null)
            {
                List<int> listOfGenre = new List<int>() { (int)(MoviesGenre)Enum.Parse(typeof(MoviesGenre), SortingParameters.ConvertGenre(typeof(MoviesGenre))) };
                discoverMovie = discoverMovie.IncludeWithAllOfGenre(listOfGenre);
            }
        }

        private void SetStars(DiscoverTv discoverTv)
        {
            double stars = SortingParameters.SelectedStars * 2;
            discoverTv = discoverTv.WhereVoteAverageIsAtMost(stars);
        }
        private void SetStars(DiscoverMovie discoverMovie)
        {
            double stars = SortingParameters.SelectedStars * 2;
            discoverMovie = discoverMovie.WhereVoteAverageIsAtMost(stars);
        }

        private void SetDate(DiscoverTv discoverTv)
        {
            if (SortingParameters.DateFrom.HasValue)
            {
                discoverTv = discoverTv.WhereFirstAirDateIsAfter(SortingParameters.DateFrom.Value);
            }

            if (SortingParameters.DateTo.HasValue)
            {
                discoverTv = discoverTv.WhereFirstAirDateIsBefore(SortingParameters.DateTo.Value);
            }
        }
        private void SetDate(DiscoverMovie discoverMovie)
        {
            if (SortingParameters.DateFrom.HasValue)
            {
                discoverMovie = discoverMovie.WhereReleaseDateIsAfter(SortingParameters.DateFrom.Value);
            }

            if (SortingParameters.DateTo.HasValue)
            {
                discoverMovie = discoverMovie.WhereReleaseDateIsBefore(SortingParameters.DateTo.Value);
            }
        }

        private void SetOrdering(DiscoverTv discoverTv)
        {
            foreach (var arrow in SortingParameters.ArrowsAsButtons)
            {
                if (arrow.Value)
                {
                    switch (arrow.Key)
                    {
                        case "RelaseDateUpButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.PrimaryReleaseDate);
                            break;
                        case "RelaseDateDownButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.PrimaryReleaseDateDesc);
                            break;
                        case "VoteAverageUpButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.VoteAverage);
                            break;
                        case "VoteAverageDownButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.VoteAverageDesc);
                            break;
                    }
                }
            }
        }
        private void SetOrdering(DiscoverMovie discoverMovie)
        {
            foreach (var arrow in SortingParameters.ArrowsAsButtons)
            {
                if (arrow.Value)
                {
                    switch (arrow.Key)
                    {
                        case "RelaseDateUpButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDate);
                            break;
                        case "RelaseDateDownButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDateDesc);
                            break;
                        case "VoteAverageUpButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverage);
                            break;
                        case "VoteAverageDownButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverageDesc);
                            break;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// REFACTORING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! (very common metod in elementInfo.cs)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loopCount"></param>
        /// <param name="values"></param>
        private void SetStackPanel<T>(int loopCount, IEnumerable<T> values)
        {
            MainWindow.logger.Log(LogLevel.Info, "Setting elements to window");
            var listOfElements = values.ToList();
            try
            {
                if (listOfElements.Count == 0)
                {
                    MainStackPanelForProposal.Height += 320;
                    MainWindow.logger.Log(LogLevel.Warn, "list of sorting is empty");

                    // Adding special textblock to stackpanel
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = "Lack of titles!",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 35,
                        Foreground = new SolidColorBrush(Colors.White),
                        Margin = new Thickness(0, 140, 0, 0)
                    };
                    MainStackPanelForProposal.Children.Add(textBlock);
                }
                else
                {
                    int loop = (int)Math.Ceiling(loopCount / 2.0);
                    int indexOfFilm = 0;
                    for (int i = 0; i < loop; i++)
                    {
                        MainStackPanelForProposal.Height += 300;
                        Border elements = new Border()
                        {
                            Margin = new Thickness(20, 20, 10, 10)
                        };

                        Grid gridFor2Films = new Grid();
                        gridFor2Films.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        gridFor2Films.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        for (int j = 0; j < 2; j++)
                        {
                            StackPanel itemStackPanel = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(20, 0, 10, 0),
                                Width = 440,
                                Height = 270,
                                HorizontalAlignment = 0 == j ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                            };

                            // Grid for poster with average vote and button
                            Grid posterGrid = new Grid();
                            string idOfElement;
                            posterGrid.Children.Add(ElementInfo.PosterDiploy(out idOfElement,listOfElements[indexOfFilm]));
                            posterGrid.Children.Add(ElementInfo.AverageRateDiploy(listOfElements[indexOfFilm]));

                            dynamic movieOrTV = listOfElements[indexOfFilm];

                            bool isInLibrary = Library.titles.Any(x => x.Id == (listOfElements[indexOfFilm] is SearchMovie 
                            ? movieOrTV.Id : movieOrTV.Id));

                            posterGrid.Children.Add(ElementInfo.FavortieElementDiploy(idOfElement, isInLibrary));

                            // Add the bordered poster to the item stack panel
                            itemStackPanel.Children.Add(posterGrid);

                            // Setting stack panel for information of movie
                            StackPanel informationMovie = new StackPanel()
                            {
                                Orientation = Orientation.Vertical,
                                Margin = new Thickness(20, 10, 0, 0),
                            };

                            // Adding infomration about element
                            informationMovie = ElementInfo.SettingInformationAboutElement(listOfElements[indexOfFilm], values, informationMovie);
                            itemStackPanel.Children.Add(informationMovie);

                            Grid.SetColumn(itemStackPanel, j);
                            gridFor2Films.Children.Add(itemStackPanel);


                            MediaType media = movieOrTV is SearchMovie ? MediaType.Movie : MediaType.TvShow;
                            string title = movieOrTV is SearchMovie ? movieOrTV.Title : movieOrTV.Name;
                            Config.IdForMovie.Add(new ElementParameters(media, movieOrTV.Id, title));

                            ++indexOfFilm;
                            if (indexOfFilm == listOfElements.Count)
                            {
                                break;
                            }
                        }
                        elements.Child = gridFor2Films;
                        MainStackPanelForProposal.Children.Add(elements);
                    }
                }
            
            }
            catch (Exception ex)
            {
                MainWindow.logger.Log(LogLevel.Error, $"Sorting: {ex.Message}");
            }
        }

        /// <summary>
        /// Getting movies as object
        /// </summary>
        /// <returns>Object of movies</returns>
        private DiscoverMovie SetListOfMovies() => GeneralInfo.client.DiscoverMoviesAsync();

        /// <summary>
        /// Getting tvshows as object
        /// </summary>
        /// <returns>Object of tvshows</returns>
        private DiscoverTv SetListOfTvShows() => GeneralInfo.client.DiscoverTvShowsAsync();

        /// <summary>
        /// Taking random elements from object as DiscoverTv and converting into query
        /// </summary>
        /// <param name="discoverTv">Instant of elements</param>
        /// <param name="countOfElements">Number of elements to choose</param>
        /// <param name="random">Random object</param>
        /// <returns></returns>
        public IEnumerable<SearchMovieTvBase> ConvertToObject(DiscoverTv discoverTv, int countOfElements, Random random)
                => discoverTv.Query().Result.Results.OrderBy(x => random.Next()).Take(countOfElements);

        /// <summary>
        /// Taking random elements from object as DiscoverMovie and converting into query
        /// </summary>
        /// <param name="discoverMovie">Instant of elements</param>
        /// <param name="countOfElements">Number of elements to choose</param>
        /// <param name="random">Random object</param>
        /// <returns></returns>
        public IEnumerable<SearchMovieTvBase> ConvertToObject(DiscoverMovie discoverMovie, int countOfElements, Random random)
                => discoverMovie.Query().Result.Results.OrderBy(x => random.Next()).Take(countOfElements);

        /// <summary>
        /// Refresh click of button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            Config.buttons.Clear();
            CreatingInstantOfList();
        }
    }
}
