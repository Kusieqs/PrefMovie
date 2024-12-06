using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDbLib.Objects.Discover;

namespace PrefMovieApi.MainControls.Sorting
{
    internal static class GettingElements
    {
        public static SortingParameters SortingParameters;
        public static dynamic SetSorting(dynamic discover, SortingParameters sortingParameters)
        {
            SortingParameters = sortingParameters;

            SetGenre(discover);
            SetDate(discover);
            SetOrdering(discover);
            if (sortingParameters.SelectedStars > 0)
            {
                SetStars(discover);
            }
            return discover;
        }
        private static void SetGenre(DiscoverTv discoverTv)
        {
            if (SortingParameters.Genre != null)
            {
                List<int> listOfGenre = new List<int>() { (int)(TvShowsGenre)Enum.Parse(typeof(TvShowsGenre), SortingParameters.ConvertGenre(typeof(TvShowsGenre))) };
                discoverTv = discoverTv.WhereGenresInclude(listOfGenre);
            }
        }
        private static void SetGenre(DiscoverMovie discoverMovie)
        {
            if (SortingParameters.Genre != null)
            {
                List<int> listOfGenre = new List<int>() { (int)(MoviesGenre)Enum.Parse(typeof(MoviesGenre), SortingParameters.ConvertGenre(typeof(MoviesGenre))) };
                discoverMovie = discoverMovie.IncludeWithAllOfGenre(listOfGenre);
            }
        }

        private static void SetStars(DiscoverTv discoverTv)
        {
            double stars = SortingParameters.SelectedStars * 2;
            if(stars == 10)
            {
                stars = 9.9;
            }
            discoverTv = discoverTv.WhereVoteAverageIsAtMost(stars).WhereVoteAverageIsAtLeast(0.1);
        }
        private static void SetStars(DiscoverMovie discoverMovie)
        {
            double stars = SortingParameters.SelectedStars * 2;
            if (stars == 10)
            {
                stars = 9.9;
            }
            discoverMovie = discoverMovie.WhereVoteAverageIsAtMost(stars).WhereVoteAverageIsAtLeast(0.1);
        }

        private static void SetDate(DiscoverTv discoverTv)
        {
            if (SortingParameters.DateFrom.HasValue)
            {
                discoverTv = discoverTv.WhereFirstAirDateIsAfter(SortingParameters.DateFrom.Value);
            }

            if (SortingParameters.DateTo.HasValue)
            {
                discoverTv = discoverTv.WhereFirstAirDateIsBefore(SortingParameters.DateTo.Value);
            }
        }
        private static void SetDate(DiscoverMovie discoverMovie)
        {
            if (SortingParameters.DateFrom.HasValue)
            {
                discoverMovie = discoverMovie.WhereReleaseDateIsAfter(SortingParameters.DateFrom.Value);
            }

            if (SortingParameters.DateTo.HasValue)
            {
                discoverMovie = discoverMovie.WhereReleaseDateIsBefore(SortingParameters.DateTo.Value);
            }
        }

        private static void SetOrdering(DiscoverTv discoverTv)
        {
            foreach (var arrow in SortingParameters.ArrowsAsButtons)
            {
                if (arrow.Value)
                {
                    switch (arrow.Key)
                    {
                        case "RelaseDateUpButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.PrimaryReleaseDate);
                            break;
                        case "RelaseDateDownButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.PrimaryReleaseDateDesc);
                            break;
                        case "VoteAverageUpButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.VoteAverage);
                            break;
                        case "VoteAverageDownButton":
                            discoverTv = discoverTv.OrderBy(DiscoverTvShowSortBy.VoteAverageDesc);
                            break;
                    }
                }
            }
        }
        private static void SetOrdering(DiscoverMovie discoverMovie)
        {
            foreach (var arrow in SortingParameters.ArrowsAsButtons)
            {
                if (arrow.Value)
                {
                    switch (arrow.Key)
                    {
                        case "RelaseDateUpButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDate);
                            break;
                        case "RelaseDateDownButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.ReleaseDateDesc);
                            break;
                        case "VoteAverageUpButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverage);
                            break;
                        case "VoteAverageDownButton":
                            discoverMovie = discoverMovie.OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.VoteAverageDesc);
                            break;
                    }
                }
            }
        }
    }
}
