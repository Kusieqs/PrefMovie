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
        private bool relaseDateUp = false;
        private bool relaseDateDown = false;
        public Sorting()
        {
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
        }


        private void MouseLeaveUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Content = CreatingImage.SettingImage("Images/up.png");
        }
        private void MouseEnterUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Content = CreatingImage.SettingImage("Images/fillUp.png");
        }

        private void MouseLeaveDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Content = CreatingImage.SettingImage("Images/down.png");
        }

        private void MouseEnterDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Content = CreatingImage.SettingImage("Images/fillDown.png");
        }
    }
}
