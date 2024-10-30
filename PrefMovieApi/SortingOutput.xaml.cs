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
        private void SetStackPanel<T>(int loopCount, IEnumerable<T> values)
        {
            var listOfElements = values.ToList();
            MainStackPanelForProposal.Height += 320;

            // Lack of titles in list
            if (listOfElements.Count == 0)
            {
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
                /*
                 * For na polowe (max 5)
                 * 
                 * Stack panel -> Horizontal
                 * for na 2??? ( pprzemyslenie dodawanie 2 rzeczy jesli jest tylko jedna
                 * utworzenie stack panele z infomracjami
                 * dodanie do stack panela
                 * dodanie do glownego
                */

            }
        }

    }
}
