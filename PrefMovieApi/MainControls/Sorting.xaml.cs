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
        };

        private bool isFilmSorting = false;
        private bool isTvShowsSorting = false;
        private int selectedStars = 0;

        public Sorting()
        {
            MainWindow.logger.Log(LogLevel.Info, "Sorting content was loaded");
            InitializeComponent();
            Genre.IsEnabled = false;
        }

        private void ClearSortingButton(object sender = null, RoutedEventArgs e = null)
        {
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
        private void RandomSelectorButton(object sender, RoutedEventArgs e)
        {
            ClearSortingButton();
            Random random = new Random();

            int number = random.Next(0, 3);

            if(number == 1)
            {
                isFilmSorting = true;
                FilmButton.Background = new SolidColorBrush(Colors.Gray);
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
                isTvShowsSorting = true;
                TvShowButton.Background = new SolidColorBrush(Colors.Gray);
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

            number = random.Next(0,3);

            if(number == 1)
            {
                arrowsAsButtons["RelaseDateUpButton"] = true;
                RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            }
            else if(number == 2)
            {
                arrowsAsButtons["RelaseDateDownButton"] = true;
                RelaseDateUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            }

            number = random.Next(0, 3);

            if (number == 1)
            {
                arrowsAsButtons["VoteAverageUpButton"] = true;
                VoteAverageUpButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillUp.png");
            }
            else if (number == 2)
            {
                arrowsAsButtons["VoteAverageDownButton"] = true;
                VoteAverageDownButton.Content = CreatingImage.SettingImage("/PrefMovieApi;component/Images/fillDown.png");
            }

            selectedStars = random.Next(0, 6);
            if(selectedStars == 0)
            {
                // TODO: Zresetowanie gwiazdek
            }
            HighlightStars(selectedStars);


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

        }

        private void FilmSelectorClick(object sender, RoutedEventArgs e)
        {
            isFilmSorting = true;
            isTvShowsSorting = false;

            FilmButton.Background = new SolidColorBrush(Colors.Gray);
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox(true);
        }
        private void TvShowSelectorClick(object sender, RoutedEventArgs e)
        {
            isFilmSorting = false;
            isTvShowsSorting = true;

            TvShowButton.Background = new SolidColorBrush(Colors.Gray);
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox();
        }
        private void LoadingComboBox(bool isItFilm = false)
        {
            MainWindow.logger.Log(LogLevel.Info, "Loading combobox with items");

            // TODO: Optymalizacja metody
            Genre.Items.Clear(); 
            Genre.IsEnabled = true;

            if (isItFilm)
            {
                // TODO: Zanotowac
                List<string> genres = Enum.GetNames(typeof(MoviesGenre)).ToList();
                foreach(string genre in genres)
                {
                    Genre.Items.Add(genre.Replace('_', ' '));
                }
            }
            else
            {
                // TODO: Zanotowac
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
            if((button.Name == "FilmButton" && isFilmSorting) || (button.Name == "TvShowButton" && isTvShowsSorting))
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
        private void HighlightStars(int count)
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
