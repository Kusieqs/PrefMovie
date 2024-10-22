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
        public Sorting()
        {
            InitializeComponent();
        }

        private void FilmSelectorClick(object sender, RoutedEventArgs e)
        {
            FilmButton.Background = new SolidColorBrush(Colors.Gray);
        }

        private void TvShowSelectorClick(object sender, RoutedEventArgs e)
        {
            TvShowButton.Background = new SolidColorBrush(Colors.Gray);
        }
    }
}
