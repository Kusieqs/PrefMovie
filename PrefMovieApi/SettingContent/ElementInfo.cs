using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

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
        /// <param name="randomMoviesOrTvShows">IEnumerable<dynamic> with diffrent class</param>
        /// <returns>Stack panel with inputed information about movies</returns>
        public static StackPanel SetInformationToStackPanel(StackPanel mainStackPanel, IEnumerable<dynamic> randomMoviesOrTvShows)
        {
            MainWindow.logger.Log(LogLevel.Info, "SetInformationToStackPanel activated");

            foreach (var movieOrTvShow in randomMoviesOrTvShows)
            {
                // Setting stack panel for poster and informations 
                StackPanel itemStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(20, 0, 10, 0),
                    Width = 440
                };

                // Grid for poster with average vote and button
                Grid posterGrid = new Grid();
                posterGrid.Children.Add(PosterDiploy(movieOrTvShow));
                posterGrid.Children.Add(AverageRateDiploy(movieOrTvShow));

                string idOfElement;
                posterGrid.Children.Add(FavortieElementDiploy(out idOfElement));

                // Add the bordered poster to the item stack panel
                itemStackPanel.Children.Add(posterGrid);


                // Setting stack panel for information of movie
                StackPanel informationMovie = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(20, 10, 0, 0)
                };

                // Adding infomration about element
                informationMovie = SettingInformationAboutElement(movieOrTvShow,randomMoviesOrTvShows,informationMovie);
                itemStackPanel.Children.Add(informationMovie);
                mainStackPanel.Children.Add(itemStackPanel);

                Config.IdForMovie.Add(idOfElement, movieOrTvShow is SearchMovie ? movieOrTvShow.Title : movieOrTvShow.Name);
            }

            return mainStackPanel;
        }

        /// <summary>
        /// Creating special rectangle to radius corners in poster
        /// </summary>
        /// <param name="movieOrTvShow"></param>
        /// <returns>Poster</returns>
        private static Rectangle PosterDiploy(dynamic movieOrTvShow)
        {
            // Setting poster to posterBrush
            ImageBrush posterBrush = new ImageBrush
            {
                ImageSource = SetPoster(movieOrTvShow),
                Stretch = Stretch.UniformToFill
            };

            // Create a Rectangle to display the image with rounded corners
            Rectangle posterRectangle = new Rectangle
            {
                RadiusX = 10,
                RadiusY = 10,
                Width = 200,
                Fill = posterBrush
            };

            return posterRectangle;
        }

        /// <summary>
        /// Creating special border with raiuds corners to show average rate of element
        /// </summary>
        /// <param name="movieOrTvShow"></param>
        /// <returns>Rate</returns>
        private static Border AverageRateDiploy(dynamic movieOrTvShow)
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
        private static Button FavortieElementDiploy(out string idOfElement)
        {
            // Creating image as star
            Image star = new Image()
            {
                Source = new BitmapImage(new Uri("Images/emptyStar.png", UriKind.Relative)),
                Stretch = Stretch.UniformToFill
            };

            idOfElement = $"Id{++id}";

            // Creating button as star
            Button favoriteButton = new Button()
            {
                Content = star,
                Name = idOfElement,
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = Brushes.Transparent,
                BorderBrush = null,
                Margin = new Thickness(7, 0, 0, 7),
                Style = Config.styleForButton,
            };


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
        private static StackPanel SettingInformationAboutElement(dynamic movieOrTvShow, dynamic randomMoviesOrTvShows, StackPanel information)
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

                if (string.IsNullOrEmpty(text.Text))
                {
                    string nameOfTheme = i == 0 ? "Title" : i == 1 ? "Date Relase" : "Genre";
                    MainWindow.logger.Log(LogLevel.Warn, $"Text is empty for: {nameOfTheme} {nameof(randomMoviesOrTvShows)}");
                }

                information.Children.Add(text);

            }
            return information;
        }

        /// <summary>
        /// Setting poster as image to application
        /// </summary>
        /// <param name="randomMoviesOrTvShows">Object of SearchMovie or SearchTv instance</param>
        /// <returns>BitmapImage object</returns>
        private static BitmapImage SetPoster(dynamic randomMoviesOrTvShows)
        {
            string posterUrl = Config.BASE_URL + randomMoviesOrTvShows.PosterPath;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(posterUrl, UriKind.Absolute);
            image.DecodePixelWidth = 200;
            image.EndInit();

            return image;
        }

        private static void FavoriteButtonMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            string title = Config.IdForMovie[button.Name];

            if (Library.titles.Any(x => x == title))
            {
                // Creating image as star
                button.Content = CreatingImage("Images/emptyStar.png");
            }
            else
            {
                // Creating image as star
                button.Content = CreatingImage("Images/star.png");
            }
        }

        private static void FavoriteButtonMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = sender as Button;
            string title = Config.IdForMovie[button.Name];

            if(Library.titles.Any(x => x == title))
            {
                // Creating image as star
                button.Content = CreatingImage("Images/star.png");
            }
            else
            {
                // Creating image as star
                button.Content = CreatingImage("Images/emptyStar.png");
            }
        }


        private static void AddingElementToLibraryOrDeleting(object sender, RoutedEventArgs e) 
        {
            Button button = sender as Button;
            string title = Config.IdForMovie[button.Name];

            if(Library.titles.Any(x => x == title))
            {
                button.Content = CreatingImage("Images/emptyStar.png");
                MainWindow.library.DeletingNewElement(button.Name);
            }
            else
            {
                button.Content = CreatingImage("Images/star.png");
                MainWindow.library.AddingNewElement(button.Name);
            }

        }

        private static Image CreatingImage(string path)
        {
            Image star = new Image()
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative)),
                Stretch = Stretch.UniformToFill
            };

            return star;
        }
    }
}
