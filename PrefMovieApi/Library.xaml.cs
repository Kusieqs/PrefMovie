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
    /// Logika interakcji dla klasy Library.xaml
    /// </summary>
    public partial class Library : UserControl
    {
        public static List<string> titles = new List<string>();
        public static JsonFile jsonFile = new JsonFile();
        public Library()
        {
            InitializeComponent();

            titles = jsonFile.DeserializeLibrary();
            if(titles.Count > 0 )
            {
                MainWindow.logger.Log(LogLevel.Info, "In file is more than 0 titles");
                SettingStackPanelLibrary();
            }
        }
        public void SettingStackPanelLibrary()
        {
            MainWindow.logger.Log(LogLevel.Info, "Setting Library of items activated");
            foreach( var item in titles )
            {
                // Creating item for stackpanel
                TextBlock title = new TextBlock()
                {
                    Text = item,
                };

                // Adding item to stackpanel
                FavoriteMovies.Children.Add(title);
            }
        }
    }
}
