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


            (int,int) genres = PreferingGenres();


            return stackPanel;
        }


        private static (int,int) PreferingGenres()
        {
            Dictionary<int,int> keyValuePairsMovie = new Dictionary<int,int>();
            Dictionary<int,int> keyValuePairsTvShow = new Dictionary<int, int>();

            foreach (var element in Library.titles)
            {
            }


            int topKey1 = topTwo[0].Key;
            int topKey2 = topTwo[1].Key;

            return (topKey1, topKey2);
        }





        private static void PreferingDates()
        {

        }

        private static void ScaleOfELements()
        {

        }
    }
}
