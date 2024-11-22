using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace PrefMovieApi
{
    /// <summary>
    /// Logika interakcji dla klasy DetailInformation.xaml
    /// </summary>
    public partial class DetailInformation : Window
    {
        private readonly ElementParameters element; // Czy to bedzie potrzebne?
        private bool IsButtonEnter;
        public DetailInformation(ElementParameters element)
        {
            MainWindow.logger.Log(LogLevel.Info, $"Open new window to search {element.Id}");
            this.element = element;
            InitializeComponent();
            LoadButton();

            if (element.MediaType == MediaType.Movie)
            {
                var movie = GeneralInfo.client.GetMovieAsync(element.Id).Result;
                SettingContent(movie);
            }
            else
            {
                var tvShow = GeneralInfo.client.GetTvShowAsync(element.Id).Result;
                SettingContent(tvShow);
            }
        }


        private void SettingContent(dynamic elementInfo)
        {
            // Setting title on the top of window
            Title.Text = elementInfo is TvShow ? elementInfo.Name : elementInfo.Title;

            // Setting rate on the top of widnow
            Rate.Text = elementInfo.VoteAverage == 10 ? elementInfo.VoteAverage.ToString("N0") : elementInfo.VoteAverage.ToString("N1");

            // Setting poster to border
            Poster.ImageSource = CreatingImage.SetPoster(elementInfo);

            // Setting overview
            Overview.Text = elementInfo.Overview;

            //Setting average vote
            AverageVote.Text = elementInfo.VoteAverage == 10 ? elementInfo.VoteAverage.ToString("N0") : elementInfo.VoteAverage.ToString("N1");

            // Setting popularity
            Popularity.Text = elementInfo.Popularity.ToString();

            // Setting date
            ReleaseDate.Text = elementInfo is TvShow ? elementInfo.FirstAirDate.ToString() : elementInfo.ReleaseDate.ToString();

            // Setting adult
            Adult.Text = elementInfo.Adult.ToString();

            // Genres
            List<Genre> genres = elementInfo.Genres;
            string[] description = genres.Select(x => x.Name).ToArray();
            Genres.Text = string.Join(",", description);

        }

        private void LoadButton()
        {
            Close.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/closeIcon.png", UriKind.Relative));

            if (Library.titles.Any(x => x.Id == element.Id))
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStarFill.png", UriKind.Relative));
                IsButtonEnter = true;
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStar.png", UriKind.Relative));
                IsButtonEnter = false;
            }
        }

        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FavoritClick(object sender, RoutedEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStar.png", UriKind.Relative));
                IsButtonEnter = false;
                MainWindow.library.DeletingNewElement(element.Id);
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStarFill.png", UriKind.Relative));
                IsButtonEnter = true;
                MainWindow.library.AddingNewElement(element.Id);
            }

        }

        private void FavoritMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStar.png", UriKind.Relative));
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStarFill.png", UriKind.Relative));
            }
        }
        private void FavoritMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStarFill.png", UriKind.Relative));
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri("/PrefMovieApi;component/Images/grayStar.png", UriKind.Relative));
            }
        }

        /// <summary>
        /// Feature to allows us to move our app throw the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BorderClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    MainWindow.logger.Log(LogLevel.Info, "Window is changing place on screen");
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                MainWindow.logger.Log(LogLevel.Error, ex.ToString());
            }
        }
    }
}

