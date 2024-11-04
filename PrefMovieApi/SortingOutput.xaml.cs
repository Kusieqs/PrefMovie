using System;
using System.CodeDom;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace PrefMovieApi
{
    public partial class SortingOutput : UserControl
    {
        public SortingParameters SortingParameters { get; set; }
        public SortingOutput(SortingParameters sortingParameters)
        {
            InitializeComponent();
            SortingParameters = sortingParameters;
            IEnumerable<SearchMovieTvBase> choosenTitles = null;

            if (sortingParameters.IsFilmSorting)
            {
                choosenTitles = SettingMovies();
            }
            else if(sortingParameters.IsTvShowsSorting)
            {
                choosenTitles = SettingTvShows();
            }
            else
            {
                choosenTitles = SettingTvShows(5);
                choosenTitles = choosenTitles.Concat(SettingMovies(5));
            }
            SetStackPanel(choosenTitles.Count(), choosenTitles);
        }



        private IEnumerable<SearchMovieTvBase> SettingMovies(int countOfElements = 10)
        {
            DiscoverMovie discoverMovie = SetListOfMovies();
            SetOrdering(discoverMovie);
            SetGenre(discoverMovie);

            if (SortingParameters.SelectedStars > 0)
            {
                SetStars(discoverMovie);
            }

            Random random = new Random();
            SetDate(discoverMovie);
            return discoverMovie.Query().Result.Results.OrderBy(x => random.Next()).Take(countOfElements);
        }
        private IEnumerable<SearchMovieTvBase> SettingTvShows(int countOfElements = 10)
        {
            DiscoverTv discoverTv = SetListOfTvShows();
            SetOrdering(discoverTv);
            SetGenre(discoverTv);

            if (SortingParameters.SelectedStars > 0)
            {
                SetStars(discoverTv);
            }

            Random random = new Random();
            SetDate(discoverTv);
            return discoverTv.Query().Result.Results.OrderBy(x => random.Next()).Take(countOfElements);
        }

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
                if (arrow.Value == true)
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
                if (arrow.Value == true)
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



        /// <summary>
        /// REFACTORING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loopCount"></param>
        /// <param name="values"></param>
        private void SetStackPanel<T>(int loopCount, IEnumerable<T> values)
        {
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
                    MessageBox.Show(loop.ToString()); // 5
                    for (int i = 0; i < loop; i++)
                    {
                        MainStackPanelForProposal.Height += 320;
                        Border elements = new Border()
                        {
                            Height = 300,
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
                                Width = 480,
                                Height = 250
                            };

                            // Grid for poster with average vote and button
                            Grid posterGrid = new Grid();
                            posterGrid.Children.Add(ElementInfo.PosterDiploy(listOfElements[indexOfFilm]));
                            posterGrid.Children.Add(ElementInfo.AverageRateDiploy(listOfElements[indexOfFilm]));

                            // Add the bordered poster to the item stack panel
                            itemStackPanel.Children.Add(posterGrid);

                            // Setting stack panel for information of movie
                            StackPanel informationMovie = new StackPanel()
                            {
                                Orientation = Orientation.Vertical,
                                Margin = new Thickness(20, 10, 0, 0)
                            };

                            // Adding infomration about element
                            informationMovie = ElementInfo.SettingInformationAboutElement(listOfElements[indexOfFilm], values, informationMovie);
                            itemStackPanel.Children.Add(informationMovie);

                            Grid.SetColumn(itemStackPanel, j);
                            gridFor2Films.Children.Add(itemStackPanel);

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
        private DiscoverMovie SetListOfMovies() => GeneralInfo.client.DiscoverMoviesAsync();
        private DiscoverTv SetListOfTvShows() => GeneralInfo.client.DiscoverTvShowsAsync();
    }
}
