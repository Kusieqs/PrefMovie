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
        private readonly Dictionary<string, bool> arrowsAsButtons;
        private readonly int selectedStars;
        private readonly string genre;
        private readonly DateTime from;
        private readonly DateTime to;
        public SortingOutput(Dictionary<string, bool> arrowsAsButtons, bool isFilmSorting, bool isTvShowsSorting, 
            int selectedStars, string genre, DateTime from, DateTime to)
        {
            InitializeComponent();
            this.arrowsAsButtons = arrowsAsButtons;
            this.selectedStars = selectedStars;
            this.genre = genre;
            this.from = from;
            this.to = to;
            
            if(isFilmSorting)
            {
                SetListOfMovies();
            }
            else if(isTvShowsSorting)
            {

            }
            else
            {

            }
        }

        private void SetListOfMovies()
        {
            var movies = GeneralInfo.client.DiscoverMoviesAsync();

            if(arrowsAsButtons.Any(x => x.Value == true))
            {
                foreach(var arrow in arrowsAsButtons)
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

            List<int> listOfGenre = new List<int>() { (int)(MoviesGenre)Enum.Parse(typeof(MoviesGenre), genre) };
            movies = movies.IncludeWithAllOfGenre(listOfGenre);

            if(selectedStars > 0)
            {
                double stars = selectedStars * 2;
                movies = movies.WhereVoteAverageIsAtMost(stars);
            }

            if(from != null)
            {
                movies = movies.WhereReleaseDateIsAfter(from);
            }

            if(to != null)
            {
                movies = movies.WhereReleaseDateIsBefore(to);
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

            // Lack of titles in list
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
                for(int i = 0;  i < loop; i++)
                {
                    MainStackPanelForProposal.Height += 320;
                    Border elements = new Border()
                    {
                        Height = 300,
                    };

                    Grid gridFor2Films = new Grid();
                    gridFor2Films.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) });
                    gridFor2Films.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    for(int j = 0; j < 2; j++)
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
    }
}
