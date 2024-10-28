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
    /// <summary>
    /// Logika interakcji dla klasy Sorting.xaml
    /// </summary>
    public partial class Sorting : UserControl
    {
        private Dictionary<string, bool> arrowsAsButtons = new Dictionary<string, bool>()
        {
            ["RelaseDateUpButton"] = false,
            ["RelaseDateDownButton"] = false,
            ["VoteAverageUpButton"] = false,
            ["VoteAverageDownButton"] = false,
        }; // Dictionary for infomration about sorting method

        private bool isFilmSorting = false; // Control for film sorting method
        private bool isTvShowsSorting = false; // Control for TvShow sorting method
        private int selectedStars = 0; // Count of selected stars

        public Sorting()
        {
            MainWindow.logger.Log(LogLevel.Info, "Sorting content was loaded");
            InitializeComponent();
            Genre.IsEnabled = false;
        }

        /// <summary>
        /// Cleaning sorting infomration for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSortingButton(object sender = null, RoutedEventArgs e = null)
        {
            MainWindow.logger.Log(LogLevel.Info, "Clearing all sorting information");
            isFilmSorting = false;
            isTvShowsSorting = false;
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));

            Genre.IsEnabled = false;
            Genre.SelectedItem = null;

            foreach(var keys in arrowsAsButtons.Keys.ToList())
            {
                arrowsAsButtons[keys] = false;
            }
            RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/up.png");
            RelaseDateDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/down.png");
            VoteAverageUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/up.png");
            VoteAverageDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/down.png");

            selectedStars = 0;
            for (int i = 1; i <= 5; i++)
            {
                var starButton = FindName($"Star{i}") as Button;
                starButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/emptyStar.png");
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

            int number = random.Next(0, 3);
            if(number == 1)
            {
                // Choosing film for random main sector
                isFilmSorting = true;
                FilmButton.Background = new SolidColorBrush(Colors.Gray);

                // Loading combo box with genres 
                LoadingComboBox(true);
                int enumCount = Enum.GetNames(typeof(MoviesGenre)).Length;
                int selectorGenre = random.Next(-1, enumCount);
                if(selectorGenre == -1)
                {
                    Genre.SelectedItem = null;
                }
                else
                {
                    Genre.SelectedItem = Genre.Items[selectorGenre];
                }
            }
            else if(number == 2)
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
            number = random.Next(0,3);
            if(number == 1)
            {
                // Random sorting for ascending date
                arrowsAsButtons["RelaseDateUpButton"] = true;
                RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            }
            else if(number == 2)
            {
                // Random sorting for descending date
                arrowsAsButtons["RelaseDateDownButton"] = true;
                RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            }

            // Choosing random sorting of average vote
            number = random.Next(0, 3);
            if (number == 1)
            {
                // Random sorting for ascending date
                arrowsAsButtons["VoteAverageUpButton"] = true;
                VoteAverageUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            }
            else if (number == 2)
            {
                // Random sorting for descending date
                arrowsAsButtons["VoteAverageDownButton"] = true;
                VoteAverageDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            }

            // Choosing how many stars will be fill
            selectedStars = random.Next(0, 6);
            HighlightStars(selectedStars);

            // Choosing date from
            bool isFromRelase = random.Next(0, 2) == 1;
            int fromYear = 0;
            int fromMonth = 0;
            if(isFromRelase)
            {
                fromYear = random.Next(1980, DateTime.Now.Year + 1);
                if(fromYear == DateTime.Now.Year)
                {
                    fromMonth = random.Next(1,DateTime.Now.Month+1);
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

            MainWindow.logger.Log(LogLevel.Info, $"Random Sorting parameters:" +
                $"\n{nameof(isFilmSorting)} {isFilmSorting}, {nameof(isTvShowsSorting)} {isTvShowsSorting}" +
                $"\n{nameof(Genre)} {Genre.SelectedItem}" +
                $"\nRelaseDateUpButton {arrowsAsButtons["RelaseDateUpButton"]}, RelaseDateDownButton {arrowsAsButtons["RelaseDateDownButton"]}" +
                $"\nVoteAverageUpButton {arrowsAsButtons["VoteAverageUpButton"]},VoteAverageDownButton {arrowsAsButtons["VoteAverageDownButton"]}" +
                $"\nDate from {DateFrom.SelectedDate}, Date to {DateTo.SelectedDate}");

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
            MainWindow.logger.Log(LogLevel.Info, "Loading combobox with items");

            // TODO: Optymalizacja metody
            Genre.Items.Clear(); 
            Genre.IsEnabled = true;

            if (isItFilm)
            {
                List<string> genres = Enum.GetNames(typeof(MoviesGenre)).ToList();
                foreach(string genre in genres)
                {
                    Genre.Items.Add(genre.Replace('_', ' '));
                }
            }
            else
            {
                List<string> genres = Enum.GetNames(typeof(TvShowsGenre)).ToList();
                foreach (string genre in genres)
                {
                    Genre.Items.Add(genre.Replace('_', ' ').Replace("AND","and"));
                }
            }

            if(Genre.Items.Count == 0)
            {
                MainWindow.logger.Log(LogLevel.Warn, "Empty Combobox");
            }
        }

        #region Filling and unfilling diffrent buttons
        private void RelaseDateClickUp(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["RelaseDateDownButton"])
            {
                arrowsAsButtons["RelaseDateDownButton"] = false;
            }
            RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            RelaseDateDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/down.png");
            arrowsAsButtons["RelaseDateUpButton"] = true;
        }
        private void RelaseDateClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["RelaseDateUpButton"])
            {
                arrowsAsButtons["RelaseDateUpButton"] = false;
            }
            RelaseDateDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/up.png");
            arrowsAsButtons["RelaseDateDownButton"] = true;
        }
        private void AverageVoteClickUp(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageDownButton"])
            {
                arrowsAsButtons["VoteAverageDownButton"] = false;
            }
            VoteAverageUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            VoteAverageDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/down.png");
            arrowsAsButtons["VoteAverageUpButton"] = true;
        }
        private void AverageVoteClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageUpButton"])
            {
                arrowsAsButtons["VoteAverageUpButton"] = false;
            }
            VoteAverageDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            VoteAverageUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/up.png");
            arrowsAsButtons["VoteAverageDownButton"] = true;
        }
        private void MouseLeaveUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/up.png");
            }
        }
        private void MouseEnterUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            }
        }
        private void MouseLeaveDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/down.png");
            }
        }
        private void MouseEnterDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
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
            if (selectedStars == 0)
            {
            }
            else
            {
                HighlightStars(selectedStars);
            }
        }
        private void StarClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            selectedStars = int.Parse(button.Tag.ToString());
            HighlightStars(selectedStars);
        }
        #endregion

        /// <summary>
        /// Selecting how many stars will be filled
        /// </summary>
        /// <param name="count">number of star</param>
        private void HighlightStars(int count)
        {
            MainWindow.logger.Log(LogLevel.Info, "Filling stars as buttons");

            if(count == 0)
            {
                MainWindow.logger.Log(LogLevel.Warn, "Number of stars is 0");
                for(int i = 1; i <= 5; i++)
                {
                    var starButton = FindName($"Star{i}") as Button;
                    starButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/emptyStar.png");
                }
            }
            else
            {
                for (int i = 5; i >= 1; i--)
                {
                    var starButton = FindName($"Star{i}") as Button;
                    if (i >= count)
                    {
                        starButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/star.png");
                        starButton.Width = 30;
                        starButton.Height = 30;
                    }
                    else
                    {
                        starButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/emptyStar.png");
                        starButton.Width = 30;
                        starButton.Height = 30;
                    }
                }
            }
        }
    }
}
