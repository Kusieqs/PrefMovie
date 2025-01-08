using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PrefMovieApi
{
    public static class PreferingElements
    {
        public static bool isElementExist = false;

        /// <summary>
        /// Creating stack panel with content for special group
        /// </summary>
        /// <returns>Stack panel with content</returns>
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
            return stackPanel = SettingElements.PreferingForUser(stackPanel, genres, 5, 5);
        }


        /// <summary>
        /// Setting genres of elements
        /// </summary>
        /// <returns>Two of IEnumerable collections with id of genres</returns>
        private static (IEnumerable<int>,IEnumerable<int>) PreferingGenres()
        {
            int topKey1 = GetGenre(Library.titles.Where(x => x.MediaType == MediaType.Movie).ToList());
            IEnumerable<int> movies = new List<int> { topKey1 };

            int topKey2 = GetGenre(Library.titles.Where(x => x.MediaType == MediaType.TvShow).ToList());
            IEnumerable<int> tvShows = new List<int> { topKey2 };

            return (movies, tvShows);
        }

        /// <summary>
        /// Getting genres for movies and series
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>Main genre</returns>
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

    }
}
