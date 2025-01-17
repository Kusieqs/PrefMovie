﻿using System;
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
using PrefMovieApi.Setup;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace PrefMovieApi
{
    public partial class DetailInformation : Window
    {
        // Parameters of element
        private readonly ElementParameters element;

        // Special control for button enter
        private bool IsButtonEnter;

        // Button object two modify some parameters
        private Button mainWindowButton;

        public DetailInformation(ElementParameters element, Button button)
        {
            this.mainWindowButton = button;
            this.element = element;
            Constructor();
        }
        public DetailInformation(ElementParameters element)
        {
            this.element = element;
            Constructor();
        }

        /// <summary>
        /// Constructor which join two constructors into one
        /// </summary>
        private void Constructor()
        {
            Config.logger.Log(LogLevel.Info, $"Open new window to search {element.Id}");
            InitializeComponent();
            LoadButton();

            if (element.MediaType == MediaType.Movie)
            {
                var movie = Config.client.GetMovieAsync(element.Id).Result;
                SettingContent(movie);
            }
            else
            {
                var tvShow = Config.client.GetTvShowAsync(element.Id).Result;
                SettingContent(tvShow);
            }
        }

        /// <summary>
        /// Setting content into window
        /// </summary>
        /// <param name="elementInfo">element with informations</param>
        private void SettingContent(dynamic elementInfo)
        {
            Config.logger.Log(LogLevel.Info, "Setting content into new page");

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

        /// <summary>
        /// Loading buttons 
        /// </summary>
        private void LoadButton()
        {
            Config.logger.Log(LogLevel.Info, "Loading buttons");
            Close.Source = new BitmapImage(new Uri(SetupPaths.CLOSE, UriKind.Relative));

            if (Library.titles.Any(x => x.Id == element.Id))
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.STAR, UriKind.Relative));
                IsButtonEnter = true;
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.EMPTY_STAR, UriKind.Relative));
                IsButtonEnter = false;
            }
        }

        /// <summary>
        /// Closing window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            Config.existingWindows.Remove(element.Id.ToString());
            Close();
        }

        /// <summary>
        /// Adding or deleting element into library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritClick(object sender, RoutedEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.EMPTY_STAR, UriKind.Relative));
                IsButtonEnter = false;

                if (Config.buttons.Any(x => x.Key == element.Id.ToString()))
                {
                    mainWindowButton.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_STAR);
                }
                MainWindow.library.DeletingNewElement(element.Id);
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.STAR, UriKind.Relative));
                IsButtonEnter = true;

                if (Config.buttons.Any(x => x.Key == element.Id.ToString()))
                {
                    mainWindowButton.Content = CreatingImage.SettingImage(SetupPaths.STAR);
                }
                MainWindow.library.AddingNewElement(element.Id);
            }

        }

        /// <summary>
        /// Logic to enter mouse into button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.EMPTY_STAR, UriKind.Relative));
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.STAR, UriKind.Relative));
            }
        }

        /// <summary>
        /// Logic to leave mouse into button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsButtonEnter)
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.STAR, UriKind.Relative));
            }
            else
            {
                StarPicture.Source = new BitmapImage(new Uri(SetupPaths.EMPTY_STAR, UriKind.Relative));
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
                    Config.logger.Log(LogLevel.Info, "Window is changing place on screen");
                    DragMove();
                }
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.ToString());
            }
        }
    }
}


