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
        public readonly Dictionary<string, bool> ArrowsAsButtons;
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
            Genre = genre?.ToString(); 
            SelectedStars = selectedStars;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        /// <summary>
        /// Converting genre into enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Correct enum genre</returns>
        public string ConvertGenre(Type type) => type == typeof(MoviesGenre) ? 
            Genre.Replace(' ', '_') : Genre.Replace(' ', '_').Replace("and", "AND");

    }
}
