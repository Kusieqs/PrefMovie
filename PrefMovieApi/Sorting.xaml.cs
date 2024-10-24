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

        private int selectedStars = 0;

        public Sorting()
        {
            MainWindow.logger.Log(LogLevel.Info, "Sorting content was loaded");
            InitializeComponent();
            Genre.IsEnabled = false;
        }

        private void FilmSelectorClick(object sender, RoutedEventArgs e)
        {
            FilmButton.Background = new SolidColorBrush(Colors.Gray);
            TvShowButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox(true);
        }

        private void TvShowSelectorClick(object sender, RoutedEventArgs e)
        {
            TvShowButton.Background = new SolidColorBrush(Colors.Gray);
            FilmButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            LoadingComboBox();

            // TODO: Metoda do zmienienia ComboBoxa z danymi itemami
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
            RelaseDateUpButton.Content = CreatingImage.SettingImage("Images/fillUp.png");
            RelaseDateDownButton.Content = CreatingImage.SettingImage("Images/down.png");
            arrowsAsButtons["RelaseDateUpButton"] = true;
        }
        private void RelaseDateClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["RelaseDateUpButton"])
            {
                arrowsAsButtons["RelaseDateUpButton"] = false;
            }
            RelaseDateDownButton.Content = CreatingImage.SettingImage("Images/fillDown.png");
            RelaseDateUpButton.Content = CreatingImage.SettingImage("Images/up.png");
            arrowsAsButtons["RelaseDateDownButton"] = true;
        }
        private void AverageVoteClickUp(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageDownButton"])
            {
                arrowsAsButtons["VoteAverageDownButton"] = false;
            }
            VoteAverageUpButton.Content = CreatingImage.SettingImage("Images/fillUp.png");
            VoteAverageDownButton.Content = CreatingImage.SettingImage("Images/down.png");
            arrowsAsButtons["VoteAverageUpButton"] = true;
        }
        private void AverageVoteClickDown(object sender, RoutedEventArgs e)
        {
            if (arrowsAsButtons["VoteAverageUpButton"])
            {
                arrowsAsButtons["VoteAverageUpButton"] = false;
            }
            VoteAverageDownButton.Content = CreatingImage.SettingImage("Images/fillDown.png");
            VoteAverageUpButton.Content = CreatingImage.SettingImage("Images/up.png");
            arrowsAsButtons["VoteAverageDownButton"] = true;
        }


        private void MouseLeaveUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("Images/up.png");
            }
        }
        private void MouseEnterUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("Images/fillUp.png");
            }
        }
        private void MouseLeaveDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("Images/down.png");
            }
        }
        private void MouseEnterDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (!arrowsAsButtons[button.Name])
            {
                button.Content = CreatingImage.SettingImage("Images/fillDown.png");
            }
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
                ResetStars();
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
                    starButton.Content = CreatingImage.SettingImage("Images/star.png");
                    starButton.Width = 30;
                    starButton.Height = 30;
                }
                else
                {
                    starButton.Content = CreatingImage.SettingImage("Images/emptyStar.png");
                    starButton.Width = 30;
                    starButton.Height = 30;
                }
            }
        }

        /*
        private void ResetStars()
        {
            for (int i = 1; i <= 5; i++)
            {
                var starButton = FindName($"Star{i}") as Button;
                starButton.Foreground = Brushes.Gray;
            }
        }
        */
    }
}
