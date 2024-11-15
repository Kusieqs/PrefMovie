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
using System.Windows.Shapes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace PrefMovieApi
{
    /// <summary>
    /// Logika interakcji dla klasy DetailInformation.xaml
    /// </summary>
    public partial class DetailInformation : Window
    {
        private readonly ElementParameters element;
        public DetailInformation(ElementParameters element)
        {
            MainWindow.logger.Log(LogLevel.Info, $"Open new window to search {element.Title}");
            InitializeComponent();
            this.element = element;
            MessageBox.Show($"{element.MainId}");
            /*
             * Trzeba sprawdzic jak wyszukac dany film/serial po ID (MainId)
             * Rozdzielic to co to jest
             * Usadowic elementy oraz ogarnac opis filmu
             */
        }
    }
}

