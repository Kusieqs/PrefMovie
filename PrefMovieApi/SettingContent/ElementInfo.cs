﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PrefMovieApi.Setup;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace PrefMovieApi
{
    internal static class ElementInfo
    {
        // id for element
        private static int id = 0;

        /// <summary>
        /// Setting information about movie.
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where it will be</param>
        /// <param name="randomMoviesOrTvShows">IEnumerable<dynamic> with 2 type of class (SearchMovie, SearchTv) class</param>
        /// <returns>Stack panel with inputed information about movies</returns>
        public static StackPanel SetInformationToStackPanel(StackPanel mainStackPanel, IEnumerable<dynamic> randomMoviesOrTvShows)
        {
            Config.logger.Log(LogLevel.Info, "SetInformationToStackPanel activated");

            foreach (var movieOrTvShow in randomMoviesOrTvShows)
            {
                // Setting stack panel for poster and informations 
                StackPanel itemStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(20, 0, 10, 0),
                    Width = 410
                };

                // Grid for poster with average vote and button
                string idOfElement;
                Grid posterGrid = new Grid();
                posterGrid.Children.Add(PosterDiploy(out idOfElement, movieOrTvShow));
                posterGrid.Children.Add(AverageRateDiploy(movieOrTvShow));

                bool isInLibrary = Library.titles.Any(x => x.Id == movieOrTvShow.Id);
                posterGrid.Children.Add(FavortieElementDiploy(idOfElement, isInLibrary));

                // Add the bordered poster to the item stack panel
                itemStackPanel.Children.Add(posterGrid);


                // Setting stack panel for information of movie
                StackPanel informationMovie = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(20, 10, 0, 0)
                };

                // Adding infomration about element
                informationMovie = SettingInformationAboutElement(movieOrTvShow, randomMoviesOrTvShows, informationMovie);
                itemStackPanel.Children.Add(informationMovie);
                mainStackPanel.Children.Add(itemStackPanel);

                MediaType media = movieOrTvShow is SearchMovie ? MediaType.Movie : MediaType.TvShow;
                string title = movieOrTvShow is SearchMovie ? movieOrTvShow.Title : movieOrTvShow.Name;

                Config.IdForMovie.Add(new ElementParameters(media, movieOrTvShow.Id, title, movieOrTvShow.GenreIds));
            }

            return mainStackPanel;
        }

        /// <summary>
        /// Setting information about movie and tvShows.
        /// </summary>
        /// <param name="mainStackPanel">Stack panel where it will be</param>
        /// <param name="searchMovie">IEnumerable<SearchMovie> with elements</param>
        /// <param name="searchTv">IEnumerable<SearchTv> with elements</param>
        /// <returns>Stack panel with inputed information about movie and tvShow</returns>
        public static StackPanel SetInformationToStackPanel(StackPanel mainStackPanel, IEnumerable<SearchMovie> searchMovie, IEnumerable<SearchTv> searchTv)
        {
            Config.logger.Log(LogLevel.Info, "SetInformationToStackPanel (Double class) activated");

            List<object> combinedList = new List<object>();
            combinedList.Add(searchMovie);
            combinedList.Add(searchTv);

            foreach (var item in combinedList)
            {
                IEnumerable<dynamic> list = item as IEnumerable<dynamic>;

                if (item is IEnumerable<SearchMovie> || item is IEnumerable<SearchTv>)
                {
                    foreach (var movieOrTvShow in list)
                    {
                        // Setting stack panel for poster and informations 
                        StackPanel itemStackPanel = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(20, 0, 10, 0),
                            Width = 410
                        };

                        // Grid for poster with average vote and button
                        string idOfElement;
                        Grid posterGrid = new Grid();
                        posterGrid.Children.Add(PosterDiploy(out idOfElement, movieOrTvShow));
                        posterGrid.Children.Add(AverageRateDiploy(movieOrTvShow));

                        bool isInLibrary = Library.titles.Any(x => x.Id == movieOrTvShow.Id);
                        posterGrid.Children.Add(FavortieElementDiploy(idOfElement, isInLibrary));

                        // Add the bordered poster to the item stack panel
                        itemStackPanel.Children.Add(posterGrid);


                        // Setting stack panel for information of movie
                        StackPanel informationMovie = new StackPanel()
                        {
                            Orientation = Orientation.Vertical,
                            Margin = new Thickness(20, 10, 0, 0)
                        };

                        // Adding infomration about element
                        informationMovie = SettingInformationAboutElement(movieOrTvShow, list, informationMovie);
                        itemStackPanel.Children.Add(informationMovie);
                        mainStackPanel.Children.Add(itemStackPanel);

                        MediaType media = movieOrTvShow is SearchMovie ? MediaType.Movie : MediaType.TvShow;
                        string title = movieOrTvShow is SearchMovie ? movieOrTvShow.Title : movieOrTvShow.Name;

                        Config.IdForMovie.Add(new ElementParameters(media, movieOrTvShow.Id, title, movieOrTvShow.GenreIds));
                    }
                }
            }
            return mainStackPanel;
        }

        /// <summary>
        /// Creating special rectangle to radius corners in poster
        /// </summary>
        /// <param name="movieOrTvShow"></param>
        /// <returns>Poster</returns>
        public static Button PosterDiploy(out string idOfElement, dynamic movieOrTvShow)
        {
            idOfElement = $"{movieOrTvShow.Id}";

            // Creating ImageBrush
            ImageBrush posterBrush = new ImageBrush
            {
                ImageSource = CreatingImage.SetPoster(movieOrTvShow),
                Stretch = Stretch.UniformToFill
            };

            // Creating button
            Button button = new Button
            {
                Tag = idOfElement,
                Style = Config.styleForPosterButton,
                Background = posterBrush,
            };
            button.Click += ClickPosterButton;

            return button;
        }

        /// <summary>
        /// Creating special border with raiuds corners to show average rate of element
        /// </summary>
        /// <param name="movieOrTvShow"></param>
        /// <returns>Rate</returns>
        public static Border AverageRateDiploy(dynamic movieOrTvShow)
        {
            // Average rate
            TextBlock averageRate = new TextBlock()
            {
                Text = movieOrTvShow.VoteAverage == 10 ? movieOrTvShow.VoteAverage.ToString("N0") : movieOrTvShow.VoteAverage.ToString("N1"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.DemiBold,
                FontSize = 15,
                FontFamily = new FontFamily("Calibri")
            };

            // Border for textblock
            Border averageBorder = new Border()
            {
                CornerRadius = new CornerRadius(10),
                Child = averageRate,
                Width = 30,
                Height = 30,
                Background = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 7, 7)
            };

            return averageBorder;
        }

        /// <summary>
        /// Creating button which add new feature to add element to library
        /// </summary>
        /// <param name="idOfElement">name of id of element</param>
        /// <returns>Favorite button star</returns>
        public static Button FavortieElementDiploy(string idOfElement, bool isInLibrary)
        {
            // Creating button as star
            Button favoriteButton = new Button()
            {
                Content = CreatingImage.SettingImage(isInLibrary == true ? SetupPaths.STAR : SetupPaths.EMPTY_STAR),
                Tag = idOfElement.ToString(),
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = Brushes.Transparent,
                BorderBrush = null,
                Margin = new Thickness(7, 0, 0, 7),
                Style = Config.styleForButton,
            };

            Config.buttons[idOfElement] = favoriteButton;

            favoriteButton.MouseLeave += FavoriteButtonMouseLeave;
            favoriteButton.MouseEnter += FavoriteButtonMouseEnter;
            favoriteButton.Click += AddingElementToLibraryOrDeleting;

            return favoriteButton;
        }

        /// <summary>
        /// Creating infomration about gendre, date relaise and title
        /// </summary>
        /// <param name="movieOrTvShow"></param>
        /// <param name="randomMoviesOrTvShows"></param>
        /// <returns>Infomration about element</returns>
        public static StackPanel SettingInformationAboutElement(dynamic movieOrTvShow, dynamic randomMoviesOrTvShows, StackPanel information)
        {
            // Setting information of movie
            for (int i = 0; i < 3; i++)
            {
                double fontSize = i == 0 ? 22 : 17;
                TextBlock text = new TextBlock()
                {
                    Style = Config.styleThemeOfElement,
                    FontSize = fontSize,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                switch (i)
                {
                    // Title
                    case 0:
                        text.Text = movieOrTvShow is SearchMovie ? $"{movieOrTvShow.Title}" : $"{movieOrTvShow.Name}";
                        break;
                    // Release Date
                    case 1:
                        text.Text = movieOrTvShow is SearchMovie ? $"{movieOrTvShow.ReleaseDate.Year}" : $"{movieOrTvShow.FirstAirDate.Year}";
                        break;
                    // Genre
                    case 2:
                        List<int> genreId = movieOrTvShow.GenreIds;

                        if (movieOrTvShow is SearchMovie)
                        {
                            foreach (var genre in genreId)
                            {
                                var genreName = (MoviesGenre)genre;
                                string changedGenre = genreName.ToString().Replace('_', ' ') + Environment.NewLine;
                                text.Text += changedGenre;
                            }
                        }
                        else
                        {
                            foreach (var genre in genreId)
                            {
                                var genreName = (TvShowsGenre)genre;
                                string changedGenre = genreName.ToString().Replace("AND", "&").Replace('_', ' ') + Environment.NewLine;
                                text.Text += changedGenre;
                            }
                        }
                        break;
                }

                // Checking empty text
                if (string.IsNullOrEmpty(text.Text))
                {
                    string nameOfTheme = i == 0 ? "Title" : i == 1 ? "Date Relase" : "Genre";
                    Config.logger.Log(LogLevel.Warn, $"Text is empty for: {nameOfTheme} {nameof(randomMoviesOrTvShows)}");
                }

                information.Children.Add(text);

            }
            return information;
        }

        /// <summary>
        /// When mouse is enter into star then image will be change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FavoriteButtonMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            if (Library.titles.Any(x => x.Id == int.Parse(button.Tag.ToString())))
            {
                // Creating image as star
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_STAR);
            }
            else
            {
                // Creating image as star
                button.Content = CreatingImage.SettingImage(SetupPaths.STAR);
            }
        }

        /// <summary>
        /// When mouse is enter into star then image will be change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FavoriteButtonMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            if (Library.titles.Any(x => x.Id == int.Parse(button.Tag.ToString())))
            {
                // Creating image as star
                button.Content = CreatingImage.SettingImage(SetupPaths.STAR);
            }
            else
            {
                // Creating image as star
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_STAR);
            }
        }

        /// <summary>
        /// Adding or deleting elemnt from library. It depends on fill of star
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AddingElementToLibraryOrDeleting(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (Library.titles.Any(x => x.Id == int.Parse(button.Tag.ToString())))
            {
                Config.logger.Log(LogLevel.Info, "Deleting element");
                button.Content = CreatingImage.SettingImage(SetupPaths.EMPTY_STAR);
                MainWindow.library.DeletingNewElement(int.Parse(button.Tag.ToString()));
            }
            else
            {
                Config.logger.Log(LogLevel.Info, "Adding element");
                button.Content = CreatingImage.SettingImage(SetupPaths.STAR);
                MainWindow.library.AddingNewElement(int.Parse(button.Tag.ToString()));
            }
        }

        /// <summary>
        /// Clicking posters to open window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ClickPosterButton(object sender, RoutedEventArgs e)
        {
            Config.logger.Log(LogLevel.Info, "Opening new window when poster was clicked");
            try
            {
                Button button = sender as Button;
                ElementParameters element = Config.IdForMovie.Where(x => x.Id == int.Parse(button.Tag.ToString())).FirstOrDefault();

                if (!Config.existingWindows.Any(x => x == element.Id.ToString()))
                {
                    // Adding control to list
                    Config.existingWindows.Add(element.Id.ToString());
                    DetailInformation detailInformation = new DetailInformation(element, Config.buttons[button.Tag.ToString()]);

                    // Opening new window
                    Config.existingWindows.Add(element.Id.ToString());
                    detailInformation.Show();
                }
                else
                {
                    Config.logger.Log(LogLevel.Warn, "Window is existing");
                }
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Error, ex.Message);
            }
        }
    }
}
