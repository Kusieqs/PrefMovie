using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using PrefMovieApi.Setup;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrefMovieApi
{
    public static class PreferingElements
    {
        public static bool isElementExist = false;

        public static StackPanel StackPanelWithContent()
        {
            StackPanel stackPanel = new StackPanel()
            {
                Name = "Preferd",
                Height = 290,
                Orientation = Orientation.Horizontal,
            };


            /// JSON zrobic tak zeby nam zczytywalo genres i bedzie G


            return stackPanel;
        }


        private static void PreferingGenres()
        {

        }

        private static void PreferingDates()
        {

        }

        private static void ScaleOfELements()
        {

        }
    }
}
