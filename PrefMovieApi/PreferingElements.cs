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

            (IEnumerable<int>,IEnumerable<int>) genres = PreferingGenres();

            isElementExist = true;
            int whichIsBigger = 0;
            double scale = ScaleOfELements(ref whichIsBigger);
            int numberOfElements = 10;
            int movies;
            int tvShows;

            movies = 5;
            tvShows = 5;

            /*
            if (whichIsBigger == 0)
            {
                movies = 5;
                tvShows = 5;
            }
            else if(whichIsBigger == 1) 
            {
                tvShows = (int)Math.Round(scale * numberOfElements);
                MessageBox.Show($"{tvShows} 1");
                movies = numberOfElements - tvShows;
            }
            else
            {
                movies = (int)Math.Round(scale * numberOfElements);
                MessageBox.Show($"{movies} 2");
                tvShows = numberOfElements - movies;
            }
            */

            return stackPanel = SettingElements.PreferingForUser(stackPanel, genres, movies, tvShows);
        }


        private static (IEnumerable<int>,IEnumerable<int>) PreferingGenres()
        {
            int topKey1 = GetGenre(Library.titles.Where(x => x.MediaType == MediaType.Movie).ToList());
            IEnumerable<int> movies = new List<int> { topKey1 };

            int topKey2 = GetGenre(Library.titles.Where(x => x.MediaType == MediaType.TvShow).ToList());
            IEnumerable<int> tvShows = new List<int> { topKey2 };

            return (movies, tvShows);
        }

        private static int GetGenre(List<ElementParameters> parameters)
        {
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
            foreach(var element in parameters)
            {
                foreach(var genres in element.Genres)
                {
                    if(keyValuePairs.ContainsKey(genres))
                    {
                        keyValuePairs[genres]++;
                    }
                    else
                    {
                        keyValuePairs.Add(genres, 1);
                    }
                }
            }

            if (keyValuePairs.Count == 0)
            {
                return 0;
            }
            var order = keyValuePairs.OrderBy(x => x.Value).First().Key;
            return order;
        }

        private static double ScaleOfELements(ref int whichIsBigger)
        {
            int movieScale = Library.titles.Where(x => x.MediaType == MediaType.Movie).Count();
            int tvShowScale = Library.titles.Where(x => x.MediaType == MediaType.TvShow).Count();

            movieScale = movieScale == 0 ? 1: movieScale;
            tvShowScale = tvShowScale == 0 ? 1 : tvShowScale;


            whichIsBigger = movieScale == tvShowScale ? 0 : movieScale > tvShowScale ? 1 : tvShowScale > movieScale ? -1 : 0;

            if (whichIsBigger == 0)
            {
                return 0.5;
            }
            else if (whichIsBigger == 1)
            {
                return movieScale / tvShowScale;
            }
            else
            {
                return tvShowScale / movieScale;
            }
        }
    }
}
