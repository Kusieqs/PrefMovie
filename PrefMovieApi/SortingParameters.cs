using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace PrefMovieApi
{
    public class SortingParameters
    {
        public Dictionary<string, bool> ArrowsAsButtons { get; } // Dicitonary of button which is true
        public bool IsFilmSorting { get; }
        public bool IsTvShowsSorting { get; }
        public string Genre { get; }
        public int SelectedStars { get; }
        public DateTime? DateFrom { get; }
        public DateTime? DateTo { get; }
        public SortingParameters(Dictionary<string,bool> arrowsAsButtons, bool isFilmSorting, bool isTvShowsSorting,
            object genre, int selectedStars, DateTime? dateFrom, DateTime? dateTo)
        {
            ArrowsAsButtons = arrowsAsButtons;
            IsFilmSorting = isFilmSorting;
            IsTvShowsSorting = isTvShowsSorting;

            if (genre != null)
            {
                Genre = genre.ToString();
            }
            else
            {
                Genre = null;
            }

            SelectedStars = selectedStars;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public string ConvertGenre(Type type)
        {
            string convertGenre;
            if(type == typeof(MoviesGenre))
            {
                convertGenre = Genre.Replace(' ', '_');
            }
            else
            {
                convertGenre = Genre.Replace(' ', '_').Replace("and", "AND");
            }
            return convertGenre;
        }

    }
}
