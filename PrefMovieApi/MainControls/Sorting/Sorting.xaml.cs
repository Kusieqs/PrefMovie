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
using PrefMovieApi.Setup;

namespace PrefMovieApi
{
    public partial class Sorting : UserControl
    {
        // Dictionary for infomration about sorting method
        private Dictionary<string, bool> arrowsAsButtons = new Dictionary<string, bool>()
        {
            ["RelaseDateUpButton"] = false,
            ["RelaseDateDownButton"] = false,
            ["VoteAverageUpButton"] = false,
            ["VoteAverageDownButton"] = false,
        };

        // Control for film sorting method
        private bool isFilmSorting = false;

        // Control for TvShow sorting method
        private bool isTvShowsSorting = false;

        // Count of selected stars
        private int selectedStars = 0;

        // content for searching
        private ContentControl mainContent; 

        public Sorting(ContentControl content)
        {
            Config.logger.Log(LogLevel.Info, "Sorting content was loaded");
            InitializeComponent();
            Genre.IsEnabled = false;
            mainContent = content;
        }

        /// <summary>
        /// Searching by sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchClick(object sender, RoutedEventArgs e)
        {
            Config.logger.Log(LogLevel.Info, "SearchClick activated");
            try
            {
                Config.buttons.Clear();
                SortingParameters sortingParameters = new SortingParameters
                    (arrowsAsButtons,
                    isFilmSorting,
                    isTvShowsSorting,
                    Genre?.SelectedItem,
                    selectedStars,
                    DateFrom?.SelectedDate,
                    DateTo?.SelectedDate);

                mainContent.Content = new SortingOutput(sortingParameters);
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, $"SearchClick: {ex.Message}");
            }
        }

        /// <summary>
        /// Cleaning sorting infomration for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSortingButton(object sender = null, RoutedEventArgs e = null)
        {
            Config.logger.Log(LogLevel.Info, "Clearing all sorting information");

            isFilmSorting = false;
            isTvShowsSorting = false;
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));

            Genre.IsEnabled = false;
            Genre.SelectedItem = null;

            foreach (var keys in arrowsAsButtons.Keys.ToList())
            {
                arrowsAsButtons[keys] = false;
            }
            RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            RelaseDateDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);

            selectedStars = 0;
            for (int i = 1; i <= 5; i++)
            {
                var starButton = FindName($"Star{i}") as Button;
                starButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_STAR);
            }

            DateFrom.SelectedDate = null;
            DateTo.SelectedDate = null;
        }

        /// <summary>
        /// Random selector information about sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomSelectorButton(object sender, RoutedEventArgs e)
        {
            // Cleaning sorting buttons
            ClearSortingButton();
            Random random = new Random();

            try
            {
                int number = random.Next(0, 3);
                if (number == 1)
                {
                    // Choosing film for random main sector
                    isFilmSorting = true;
                    FilmButton.Background = new SolidColorBrush(Colors.Gray);

                    // Loading combo box with genres 
                    LoadingComboBox(true);
                    int enumCount = Enum.GetNames(typeof(MoviesGenre)).Length;
                    int selectorGenre = random.Next(-1, enumCount);
                    if (selectorGenre == -1)
                    {
                        Genre.SelectedItem = null;
                    }
                    else
                    {
                        Genre.SelectedItem = Genre.Items[selectorGenre];
                    }
                }
                else if (number == 2)
                {
                    // Choosing TvShows for random main sector
                    isTvShowsSorting = true;
                    TvShowButton.Background = new SolidColorBrush(Colors.Gray);

                    // Loading combo box with genres 
                    LoadingComboBox();
                    int enumCount = Enum.GetNames(typeof(TvShowsGenre)).Length;
                    int selectorGenre = random.Next(-1, enumCount);
                    if (selectorGenre == -1)
                    {
                        Genre.SelectedItem = null;
                    }
                    else
                    {
                        Genre.SelectedItem = Genre.Items[selectorGenre];
                    }
                }

                // Choosing random sorting of relase date 
                number = random.Next(0, 3);
                if (number == 1)
                {
                    // Random sorting for ascending date
                    arrowsAsButtons["RelaseDateUpButton"] = true;
                    RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
                }
                else if (number == 2)
                {
                    // Random sorting for descending date
                    arrowsAsButtons["RelaseDateDownButton"] = true;
                    RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
                }

                // Choosing random sorting of average vote
                number = random.Next(0, 3);
                if (number == 1)
                {
                    // Random sorting for ascending date
                    arrowsAsButtons["VoteAverageUpButton"] = true;
                    VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
                }
                else if (number == 2)
                {
                    // Random sorting for descending date
                    arrowsAsButtons["VoteAverageDownButton"] = true;
                    VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
                }

                // Choosing how many stars will be fill
                selectedStars = random.Next(0, 6);
                HighlightStars(selectedStars);

                // Choosing date from
                bool isFromRelase = random.Next(0, 2) == 1;
                int fromYear = 0;
                int fromMonth = 0;
                if (isFromRelase)
                {
                    fromYear = random.Next(1980, DateTime.Now.Year + 1);
                    if (fromYear == DateTime.Now.Year)
                    {
                        fromMonth = random.Next(1, DateTime.Now.Month + 1);
                    }
                    else
                    {
                        fromMonth = random.Next(1, 13);
                    }

                    DateFrom.SelectedDate = new DateTime(fromYear, fromMonth, 1);
                }

                // Choosing date to
                bool isToRelase = random.Next(0, 2) == 1;
                int toYear;
                int toMonth;
                if (isToRelase)
                {
                    int selectToYear = isFromRelase == true ? fromYear : 1980;
                    toYear = random.Next(selectToYear, DateTime.Now.Year + 1);

                    int selectToMonth = isFromRelase == true ? fromMonth : 1;

                    if (fromYear == DateTime.Now.Year)
                    {
                        toMonth = random.Next(selectToMonth, DateTime.Now.Month + 1);
                    }
                    else
                    {
                        toMonth = random.Next(1, 13);
                    }

                    DateTo.SelectedDate = new DateTime(toYear, toMonth, 1);
                }

                Config.logger.Log(LogLevel.Info, $"Random Sorting parameters:" +
                    $"\n{nameof(isFilmSorting)} {isFilmSorting}, {nameof(isTvShowsSorting)} {isTvShowsSorting}" +
                    $"\n{nameof(Genre)} {Genre.SelectedItem}" +
                    $"\nRelaseDateUpButton {arrowsAsButtons["RelaseDateUpButton"]}, RelaseDateDownButton {arrowsAsButtons["RelaseDateDownButton"]}" +
                    $"\nVoteAverageUpButton {arrowsAsButtons["VoteAverageUpButton"]},VoteAverageDownButton {arrowsAsButtons["VoteAverageDownButton"]}" +
                    $"\nDate from {DateFrom.SelectedDate}, Date to {DateTo.SelectedDate}");
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.Message);
            }

        }

        /// <summary>
        /// Selecting only films to sorting feature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilmSelectorClick(object sender, RoutedEventArgs e)
        {
            isFilmSorting = true;
            isTvShowsSorting = false;

            FilmButton.Background = new SolidColorBrush(Colors.Gray);
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox(true);
        }

        /// <summary>
        /// Selecting only TvShows to sorting feature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvShowSelectorClick(object sender, RoutedEventArgs e)
        {
            isFilmSorting = false;
            isTvShowsSorting = true;

            TvShowButton.Background = new SolidColorBrush(Colors.Gray);
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox();
        }

        /// <summary>
        /// Loading combo box with genres
        /// </summary>
        /// <param name="isItFilm"></param>
        private void LoadingComboBox(bool isItFilm = false)
        {
            Config.logger.Log(LogLevel.Info, "Loading combobox with items");

            // TODO: Optymalizacja metody
            Genre.Items.Clear();
            Genre.IsEnabled = true;

            if (isItFilm)
            {
                List<string> genres = Enum.GetNames(typeof(MoviesGenre)).ToList();
                foreach (string genre in genres)
                {
                    Genre.Items.Add(genre.Replace('_', ' '));
                }
            }
            else
            {
                List<string> genres = Enum.GetNames(typeof(TvShowsGenre)).ToList();
                foreach (string genre in genres)
                {
                    Genre.Items.Add(genre.Replace('_', ' ').Replace("AND", "and"));
                }
            }

            if (Genre.Items.Count == 0)
            {
                Config.logger.Log(LogLevel.Warn, "Empty Combobox");
            }
        }

        /// <summary>
        /// Selecting how many stars will be filled
        /// </summary>
        /// <param name="count">number of star</param>
        private void HighlightStars(int count)
        {
            Config.logger.Log(LogLevel.Info, "Filling stars as buttons");

            if (count == 0)
            {
                Config.logger.Log(LogLevel.Warn, "Number of stars is 0");
                for (int i = 1; i <= 5; i++)
                {
                    var starButton = FindName($"Star{i}") as Button;
                    starButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_WHITE_STAR, 30);
                }
            }
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    var starButton = FindName($"Star{i}") as Button;
                    if (i <= count)
                    {
                        starButton.Content = CreatingImage.SettingImage(SetupPaths.WHITE_STAR, 30);
                    }
                    else
                    {
                        starButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_WHITE_STAR, 30);
                    }
                }
            }
        }

        #region Filling and unfilling diffrent buttons
        private void RelaseDateClickUp(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["RelaseDateDownButton"])
            {
                arrowsAsButtons["RelaseDateDownButton"] = false;
            }
            RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            RelaseDateDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            arrowsAsButtons["RelaseDateUpButton"] = true;
        }
        private void RelaseDateClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["RelaseDateUpButton"])
            {
                arrowsAsButtons["RelaseDateUpButton"] = false;
            }
            RelaseDateDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            arrowsAsButtons["RelaseDateDownButton"] = true;
        }
        private void AverageVoteClickUp(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageDownButton"])
            {
                arrowsAsButtons["VoteAverageDownButton"] = false;
            }
            VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            arrowsAsButtons["VoteAverageUpButton"] = true;
        }
        private void AverageVoteClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageUpButton"])
            {
                arrowsAsButtons["VoteAverageUpButton"] = false;
            }
            VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            arrowsAsButtons["VoteAverageDownButton"] = true;
        }
        private void MouseLeaveUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            }
        }
        private void MouseEnterUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.UP);
            }
        }
        private void MouseLeaveDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            }
        }
        private void MouseEnterDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            }
        }
        private void MenuMouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if ((button.Name == "FilmButton" && isFilmSorting) || (button.Name == "TvShowButton" && isTvShowsSorting))
            {
                button.Background = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            }

        }
        private void MenuMouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Gray);
        }
        private void StarMouseEnter(object sender, MouseEventArgs e)
        {
            var button = sender as Button;
            int starIndex = int.Parse(button.Tag.ToString());
            HighlightStars(starIndex);
        }
        private void StarMouseLeave(object sender, MouseEventArgs e)
        {
            HighlightStars(selectedStars != 0 ? selectedStars : 0);
        }
        private void StarClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            selectedStars = int.Parse(button.Tag.ToString());
            HighlightStars(selectedStars);
        }
        #endregion
    }
}
