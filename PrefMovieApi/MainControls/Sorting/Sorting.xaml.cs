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
using PrefMovieApi.MainControls.Sorting;
using PrefMovieApi.Setup;

namespace PrefMovieApi
{
    public partial class Sorting : UserControl
    {
        // RandomSelector object
        private RandomSelector randomSelector = new RandomSelector();

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
                    (randomSelector.arrowsAsButtons,
                    randomSelector.isFilmSorting,
                    randomSelector.isTvShowsSorting,
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

            randomSelector.isFilmSorting = false;
            randomSelector.isTvShowsSorting = false;
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));

            Genre.IsEnabled = false;
            Genre.SelectedItem = null;

            foreach (var keys in randomSelector.arrowsAsButtons.Keys.ToList())
            {
                randomSelector.arrowsAsButtons[keys] = false;
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

            try
            {
                randomSelector = new RandomSelector(this);

                Config.logger.Log(LogLevel.Info, $"Random Sorting parameters:" +
                    $"\n{nameof(randomSelector.isFilmSorting)} {randomSelector.isFilmSorting}, {nameof(randomSelector.isTvShowsSorting)} {randomSelector.isTvShowsSorting}" +
                    $"\n{nameof(Genre)} {Genre.SelectedItem}" +
                    $"\nRelaseDateUpButton {randomSelector.arrowsAsButtons["RelaseDateUpButton"]}, RelaseDateDownButton {randomSelector.arrowsAsButtons["RelaseDateDownButton"]}" +
                    $"\nVoteAverageUpButton {randomSelector.arrowsAsButtons["VoteAverageUpButton"]},VoteAverageDownButton {randomSelector.arrowsAsButtons["VoteAverageDownButton"]}" +
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
            randomSelector.isFilmSorting = true;
            randomSelector.isTvShowsSorting = false;

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
            randomSelector.isFilmSorting = false;
            randomSelector.isTvShowsSorting = true;

            TvShowButton.Background = new SolidColorBrush(Colors.Gray);
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox();
        }

        /// <summary>
        /// Loading combo box with genres
        /// </summary>
        /// <param name="isItFilm"></param>
        public void LoadingComboBox(bool isItFilm = false)
        {
            Config.logger.Log(LogLevel.Info, "Loading combobox with items");

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
        }

        /// <summary>
        /// Selecting how many stars will be filled
        /// </summary>
        /// <param name="count">number of star</param>
        public void HighlightStars(int count)
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
            if (randomSelector.arrowsAsButtons["RelaseDateDownButton"])
            {
                randomSelector.arrowsAsButtons["RelaseDateDownButton"] = false;
            }
            RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            RelaseDateDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            randomSelector.arrowsAsButtons["RelaseDateUpButton"] = true;
        }
        private void RelaseDateClickDown(object sender, RoutedEventArgs e)
        {
            if (randomSelector.arrowsAsButtons["RelaseDateUpButton"])
            {
                randomSelector.arrowsAsButtons["RelaseDateUpButton"] = false;
            }
            RelaseDateDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            randomSelector.arrowsAsButtons["RelaseDateDownButton"] = true;
        }
        private void AverageVoteClickUp(object sender, RoutedEventArgs e)
        {
            if (randomSelector.arrowsAsButtons["VoteAverageDownButton"])
            {
                randomSelector.arrowsAsButtons["VoteAverageDownButton"] = false;
            }
            VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            randomSelector.arrowsAsButtons["VoteAverageUpButton"] = true;
        }
        private void AverageVoteClickDown(object sender, RoutedEventArgs e)
        {
            if (randomSelector.arrowsAsButtons["VoteAverageUpButton"])
            {
                randomSelector.arrowsAsButtons["VoteAverageUpButton"] = false;
            }
            VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            randomSelector.arrowsAsButtons["VoteAverageDownButton"] = true;
        }
        private void MouseLeaveUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!randomSelector.arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_UP);
            }
        }
        private void MouseEnterUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!randomSelector.arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.UP);
            }
        }
        private void MouseLeaveDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!randomSelector.arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_DOWN);
            }
        }
        private void MouseEnterDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!randomSelector.arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            }
        }
        private void MenuMouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if ((button.Name == "FilmButton" && randomSelector.isFilmSorting) || (button.Name == "TvShowButton" && randomSelector.isTvShowsSorting))
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
