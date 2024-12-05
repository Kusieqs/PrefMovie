using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PrefMovieApi.Setup;
using TMDbLib.Objects.General;

namespace PrefMovieApi.MainControls.Sorting
{
    internal class RandomSelector
    {
        public bool isFilmSorting { get; set; } = false;
        public bool isTvShowsSorting { get;set; } = false;

        // Dictionary for infomration about sorting method
        private Dictionary<string, bool> arrowsAsButtons = new Dictionary<string, bool>()
        {
            ["RelaseDateUpButton"] = false,
            ["RelaseDateDownButton"] = false,
            ["VoteAverageUpButton"] = false,
            ["VoteAverageDownButton"] = false,
        };

        private PrefMovieApi.Sorting sorting { get; set; }

        public RandomSelector(PrefMovieApi.Sorting sorting) 
        {
            this.sorting = sorting;
            Random random = new Random();
            CheckingMode(random.Next(0, 3));

            int enumCount = 0;
            DownWritingEnums(ref enumCount, random);

            SelectingSortingDate(random);
            SelectingVote(random);

            //Choosing how many stars will be fill
            int selectedStars = random.Next(0, 6);
            sorting.HighlightStars(selectedStars);

            SelectingDate(random);

        }

        private void CheckingMode(int random)
        {
            switch(random)
            {
                case 0: 
                    isFilmSorting = true; 
                    sorting.FilmButton.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case 1: 
                    isTvShowsSorting = true;
                    sorting.TvShowButton.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case 2:
                    break;
            }
        }
        private void DownWritingEnums(ref int enumCount, Random random)
        {
            // Loading combo box
            sorting.LoadingComboBox(isFilmSorting);

            // Selecting item as random
            int selectorGenre = random.Next(-1, enumCount);
            if (selectorGenre == -1)
            {
                sorting.Genre.SelectedItem = null;
            }
            else
            {
                sorting.Genre.SelectedItem = sorting.Genre.Items[selectorGenre];
            }
        }
        private void SelectingSortingDate(Random random)
        {
            int number = random.Next(0, 3);
            if (number == 1)
            {
                // Random sorting for ascending date
                arrowsAsButtons["RelaseDateUpButton"] = true;
                sorting.RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            }
            else if (number == 2)
            {
                // Random sorting for descending date
                arrowsAsButtons["RelaseDateDownButton"] = true;
                sorting.RelaseDateUpButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            }
        }
        private void SelectingVote(Random random)
        {
            // Choosing random sorting of average vote
            int number = random.Next(0, 3);
            if (number == 1)
            {
                // Random sorting for ascending date
                arrowsAsButtons["VoteAverageUpButton"] = true;
                sorting.VoteAverageUpButton.Content = CreatingImage.SettingImage(SetupPaths.UP);
            }
            else if (number == 2)
            {
                // Random sorting for descending date
                arrowsAsButtons["VoteAverageDownButton"] = true;
                sorting.VoteAverageDownButton.Content = CreatingImage.SettingImage(SetupPaths.DOWN);
            }
        }
        private void SelectingDate(Random random)
        {
            bool isFromRelase = random.Next(0, 2) == 1;
            int fromYear = 0;
            int fromMonth = 0;
            if (isFromRelase)
            {
                fromYear = random.Next(1980, DateTime.Now.Year + 1);
                if (fromYear == DateTime.Now.Year)
                {
                    fromMonth = random.Next(1, DateTime.Now.Month + 1);
                }
                else
                {
                    fromMonth = random.Next(1, 13);
                }

                sorting.DateFrom.SelectedDate = new DateTime(fromYear, fromMonth, 1);
            }

            // Choosing date to
            bool isToRelase = random.Next(0, 2) == 1;
            int toYear;
            int toMonth;
            if (isToRelase)
            {
                int selectToYear = isFromRelase == true ? fromYear : 1980;
                toYear = random.Next(selectToYear, DateTime.Now.Year + 1);

                int selectToMonth = isFromRelase == true ? fromMonth : 1;

                if (fromYear == DateTime.Now.Year)
                {
                    toMonth = random.Next(selectToMonth, DateTime.Now.Month + 1);
                }
                else
                {
                    toMonth = random.Next(1, 13);
                }

                sorting.DateTo.SelectedDate = new DateTime(toYear, toMonth, 1);
            }
        }
    }
}
