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
using PrefMovieApi.Setup;

namespace PrefMovieApi
{
    public partial class Library : UserControl
    {
        // List of parameters of objects
        public static List<ElementParameters> titles = new List<ElementParameters>();

        public Library()
        {
            InitializeComponent();

            titles = Config.jsonFile.DeserializeLibrary();
            if (titles.Count > 0)
            {
                Config.logger.Log(LogLevel.Info, "In file is more than 0 titles");
                LoadFavoriteMovies();
            }
        }

        /// <summary>
        /// Loading library on the right side of window
        /// </summary>
        public void LoadFavoriteMovies()
        {
            FavoriteMovies.Children.Clear();

            foreach (var item in titles)
            {
                // Creating button as title in library
                Button titleButton = new Button()
                {
                    Content = item.Title,
                    Tag = item.Id,
                    Style = FindResource("TitleButton") as Style,
                };
                titleButton.Click += OpenElement;

                // If item is longer than 25 characters than we will add Tooltip
                ToolTip longTitle = null;
                if (item.Title.Length > 25)
                {
                    longTitle = new ToolTip()
                    {
                        Content = item.Title,
                    };
                }
                titleButton.ToolTip = longTitle;
                ToolTipService.SetInitialShowDelay(titleButton, 0);
                FavoriteMovies.Children.Add(titleButton);
            }
            Config.jsonFile.SerializeLibrary();
        }

        /// <summary>
        /// Adding element from list and library
        /// </summary>
        /// <param name="id">id of button</param>
        public void AddingNewElement(int id)
        {
            ElementParameters element = new ElementParameters(Config.IdForMovie.Where(x => x.Id == id).FirstOrDefault());
            titles.Add(element);

            Config.logger.Log(LogLevel.Info, $"Adding new element to library: {element.Id}");
            LoadFavoriteMovies();
        }

        /// <summary>
        /// Deleting element from list and library
        /// </summary>
        /// <param name="id">id of button</param>
        public void DeletingNewElement(int id)
        {
            /// TODO: Problem z usunieciem danego elementu z listy? nie porównuje mi dobrego elemntu
            ElementParameters element = new ElementParameters(Config.IdForMovie.Where(x => x.Id == id).FirstOrDefault());
            List<ElementParameters> newList = titles.Where(x => x.Id != id).ToList();

            titles = newList.Intersect(titles).ToList();

            Config.logger.Log(LogLevel.Info, $"Deleting new element to library: {element.Id}");
            LoadFavoriteMovies();
        }

        /// <summary>
        /// Opening new window with choosen element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenElement(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                ElementParameters element = titles.Where(x => x.Id == (int)button.Tag).FirstOrDefault();

                if (!Config.existingWindows.Any(x => x == element.Id.ToString()))
                {
                    // Adding control to list
                    Config.existingWindows.Add(element.Id.ToString());

                    DetailInformation detailInformation;
                    if (Config.buttons.TryGetValue(button.Tag.ToString(), out Button value))
                    {
                        Config.logger.Log(LogLevel.Info, "Opening new element as window with element info and button");
                        detailInformation = new DetailInformation(element, value);
                    }
                    else
                    {
                        Config.logger.Log(LogLevel.Info, "Opening new element as window with element info");
                        detailInformation = new DetailInformation(element);
                    }
                    detailInformation.Show();
                }
                else
                {
                    Config.logger.Log(LogLevel.Warn, "Window is existing");
                }
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.Message);
            }
        }
    }
}
