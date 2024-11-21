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
        public readonly Dictionary<string, bool> ArrowsAsButtons;// Dicitonary of button which is true
        public readonly bool IsFilmSorting;
        public readonly bool IsTvShowsSorting;
        public readonly string Genre;
        public readonly int SelectedStars;
        public readonly DateTime? DateFrom;
        public readonly DateTime? DateTo;
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
