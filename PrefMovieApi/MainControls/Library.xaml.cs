using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
    /// Logika interakcji dla klasy Library.xaml
    /// </summary>
    public partial class Library : UserControl
    {
        public static List<string> titles = new List<string>();
        public static List<string> existingWindows = new List<string>();
        public static JsonFile jsonFile = new JsonFile();
        public Library()
        {
            InitializeComponent();

            titles = jsonFile.DeserializeLibrary();
            if(titles.Count > 0 )
            {
                MainWindow.logger.Log(LogLevel.Info, "In file is more than 0 titles");
                LoadFavoriteMovies();
            }
        }

        /// <summary>
        /// Adding element from list and library
        /// </summary>
        /// <param name="id">id of button</param>
        public void AddingNewElement(string id)
        {
            string title = Config.IdForMovie[id];
            titles.Add(title);

            MainWindow.logger.Log(LogLevel.Info, $"Adding new element to library: {title}");
            LoadFavoriteMovies();
        }

        /// <summary>
        /// Deleting element from list and library
        /// </summary>
        /// <param name="id">id of button</param>
        public void DeletingNewElement(string id)
        {
            string title = Config.IdForMovie[id];
            titles.Remove(title);

            MainWindow.logger.Log(LogLevel.Info, $"Deleting new element to library: {title}");
            LoadFavoriteMovies();
        }

        /// <summary>
        /// Loading library on the right side of window
        /// </summary>
        public void LoadFavoriteMovies()
        {
            FavoriteMovies.Children.Clear();

            foreach(var item in titles)
            {
                // Creating button as title in library
                Button titleButton = new Button()
                {
                    Content = item,
                    Style = FindResource("TitleButton") as Style,
                };
                titleButton.Click += OpenElement;


                // If item is longer than 25 characters than we will add Tooltip
                ToolTip longTitle = null;
                if (item.Length > 25)
                {
                    longTitle = new ToolTip()
                    {
                        Content = item,
                    };
                }
                titleButton.ToolTip = longTitle;
                ToolTipService.SetInitialShowDelay(titleButton, 0);
                FavoriteMovies.Children.Add(titleButton);
            }
            jsonFile.SerializeLibrary();
        }

        public void OpenElement(object sender,  RoutedEventArgs e)
        {
            MainWindow.logger.Log(LogLevel.Info, "Opening new element as window");
            string title = (string)(sender as Button).Content;
            
            if(!existingWindows.Any(x => x == title))
            {
                var informationAboutElement = new DetailInformation(title);
                informationAboutElement.Show();
            }
            else
            {
                MainWindow.logger.Log(LogLevel.Warn, "Window is existing");
            }
        }
    }
}
