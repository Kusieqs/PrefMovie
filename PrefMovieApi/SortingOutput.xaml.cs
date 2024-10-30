using System;
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

namespace PrefMovieApi
{
    public partial class SortingOutput : UserControl
    {
        public SortingParameters SortingParameters { get; set; }
        public SortingOutput(SortingParameters sortingParameters)
        {
            InitializeComponent();
            SortingParameters = sortingParameters;
            
            if(sortingParameters.IsFilmSorting)
            {
                SetListOfMovies();
            }
            else if(sortingParameters.IsTvShowsSorting)
            {

            }
            else
            {

            }
        }

        private void SetListOfMovies()
        {
            var movies = GeneralInfo.client.DiscoverMoviesAsync();

            if(SortingParameters.ArrowsAsButtons.Any(x => x.Value == true))
            {
                foreach(var arrow in SortingParameters.ArrowsAsButtons)
                {
                    if(arrow.Value == true)
                    {
                        switch(arrow.Key)
                        {
                            case "RelaseDateUpButton":
                                movies = movies.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDate);
                                break;
                            case "RelaseDateDownButton":
                                movies = movies.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDateDesc);
                                break;
                            case "VoteAverageUpButton":
                                movies = movies.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverage);
                                break;
                            case "VoteAverageDownButton":
                                movies = movies.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverageDesc);
                                break;
                        }
                    }
                }
            }

            List<int> listOfGenre = new List<int>() { (int)(MoviesGenre)Enum.Parse(typeof(MoviesGenre), SortingParameters.ConvertGenre(typeof(MoviesGenre)))};
            movies = movies.IncludeWithAllOfGenre(listOfGenre);

            if(SortingParameters.SelectedStars > 0)
            {
                double stars = SortingParameters.SelectedStars * 2;
                movies = movies.WhereVoteAverageIsAtMost(stars);
            }

            if(SortingParameters.DateFrom != null)
            {
                movies = movies.WhereReleaseDateIsAfter(SortingParameters.DateFrom);
            }

            if(SortingParameters.DateTo != null)
            {
                movies = movies.WhereReleaseDateIsBefore(SortingParameters.DateTo);
            }

            Random random = new Random();

            var choosenTitles = movies.Query().Result.Results.OrderBy(x => random.Next()).Take(10);

            SetStackPanel(choosenTitles.Count(), choosenTitles);
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
                    MessageBox.Show(loop.ToString());
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

                            if (indexOfFilm == listOfElements.Count - 1)
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
    }
}
