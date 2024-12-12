using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PrefMovieApi.Setup;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Movies;

namespace PrefMovieApi
{
    public partial class GeneralInfo : UserControl
    {
        // Delegate for loading content page
        public delegate void LoadContent(object sender, RoutedEventArgs e);
        public LoadContent loadContent;

        // MainWindow object
        private Window mainWindow;

        // Control to checking mouse down
        private bool isMouseDown = false;

        // Setting point of start position
        private Point mouseStartPosition;

        // Setting offset of scroll viewer
        private double scrollViewerStartOffset;

        // Scrollviewer object
        private ScrollViewer scrollViewer;

        public GeneralInfo(Window mainWindow)
        {
            InitializeComponent();

            // Adding methods to delegate 
            loadContent = (LoadContent)Delegate.Combine(
                new LoadContent(SetLatestMovie),
                new LoadContent(SetTheBestMovie),
                new LoadContent(SetLatestTvShow),
                new LoadContent(SetTheBestTvShow),
                new LoadContent(SetSeasonPrefering),
                new LoadContent(SetPrefering)
            );

            this.mainWindow = mainWindow;

            try
            {
                // Connect to API TMDb
                Config.client = new TMDbClient(Config.API_KEY_TO_TMDB);
                Config.client.DefaultLanguage = "en";

                // Checking that the API key is correct
                var testRequest = Config.client.GetMovieAsync(550).Result;
                if (testRequest == null)
                {
                    Config.logger.Log(LogLevel.Error, "Test request is null");
                    throw new FormatException();
                }
                Config.logger.Log(LogLevel.Info, "API is correct");

                // Setting new style for infomration
                Config.logger.Log(LogLevel.Info, "Style for infomrations about elemnts was loaded");

                // Setting movies
                loadContent(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Crticial Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Config.logger.Log(LogLevel.Error, ex.Message);
                Config.logger.ShowErrors();
                mainWindow.Close();
            }

        }

        /// <summary>
        /// Setting latest movies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLatestMovie(object sender, RoutedEventArgs e)
        {
            RemoveFromDictionary(sender);
            TheNewOnceMovies.Children.Clear();
            TheNewOnceMovies = SettingElements.TheLatestMovies(TheNewOnceMovies);
        }

        /// <summary>
        /// Setting the best movies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTheBestMovie(object sender, RoutedEventArgs e)
        {
            RemoveFromDictionary(sender);
            TheBestMovies.Children.Clear();
            TheBestMovies = SettingElements.TheBestMovies(TheBestMovies);
        }

        /// <summary>
        /// Setting latest tv shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLatestTvShow(object sender, RoutedEventArgs e)
        {
            RemoveFromDictionary(sender);
            TheNewOnceSeries.Children.Clear();
            TheNewOnceSeries = SettingElements.TheLatestSeries(TheNewOnceSeries);
        }

        /// <summary>
        /// Setting the best tv shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTheBestTvShow(object sender, RoutedEventArgs e)
        {
            RemoveFromDictionary(sender);
            TheBestSeries.Children.Clear();
            TheBestSeries = SettingElements.TheBestSeries(TheBestSeries);
        }

        /// <summary>
        /// Setting season prefering elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSeasonPrefering(object sender, RoutedEventArgs e)
        {
            RemoveFromDictionary(sender);
            SeasonPrefering.Children.Clear();
            SeasonPrefering = SettingElements.SeasonPrefering(SeasonPrefering);
        }

        /// <summary>
        /// Setting prefering elements by library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPrefering(object sender, RoutedEventArgs e)
        {
            if(Library.titles.Count > 0)
            {
                RemoveFromDictionary(sender);
                if(PreferingElements.isElementsExist)
                {
                    ContentStackPanel.Children.RemoveRange(0, 2);
                }
                else
                {
                    Config.logger.Log(LogLevel.Warn, "Lack of elements to delete");
                }

                var borders = PreferingElements.CreatePreferingElements();

                ContentStackPanel.Children.Insert(0, borders.Item1);
                ContentStackPanel.Children.Insert(1, borders.Item2);
            }
        }

        /// <summary>
        /// Reseting dicitonary 
        /// </summary>
        /// <param name="sender">Object as button</param>
        private void RemoveFromDictionary(object sender)
        {
            try
            {
                if (sender != null && int.TryParse((sender as Button).Tag.ToString(), out int result) && Config.buttons.Count > 0)
                {
                    var keys = Config.buttons.Keys.Skip(result * 8).Take(8).ToList();
                    foreach (var key in keys)
                    {
                        Config.buttons.Remove(key);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Crticial Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Config.logger.Log(LogLevel.Error, $"Removing from dictionary are not correct: {ex.Message}");
            }
        }

        #region Scroll viewer logic

        /// <summary>
        ///  Blocking mouse with override position of mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewerMouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            scrollViewer = sender as ScrollViewer;

            // Override Position of mouse
            mouseStartPosition = e.GetPosition(null);

            // Downwrite start position
            scrollViewerStartOffset = scrollViewer.HorizontalOffset;

            // Block mouse
            scrollViewer.CaptureMouse();
        }

        /// <summary>
        /// Setting new position for mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewerMouseMove(object sender, MouseEventArgs e)
        {
            scrollViewer = sender as ScrollViewer;

            if (isMouseDown)
            {
                Point currentMousePosition = e.GetPosition(null);

                // Position of mouse
                double delta = currentMousePosition.X - mouseStartPosition.X;

                // Scroll to new position
                scrollViewer.ScrollToHorizontalOffset(scrollViewerStartOffset - delta);
            }
        }

        /// <summary>
        /// Unblocking mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewerMouseUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer = sender as ScrollViewer;
            isMouseDown = false;

            //unblock mouse
            scrollViewer.ReleaseMouseCapture();
        }

        /// <summary>
        /// Method to miss the child scroll viewer to focus on parent scroll viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentScrollViewer(object sender, MouseWheelEventArgs e)
        {
            // Przekazujemy zdarzenie do głównego ScrollViewer
            MainContentMoviesPref.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent
            });
            e.Handled = true; // Powstrzymujemy wewnętrzny ScrollViewer od obsługi zdarzenia
        }
        #endregion
    }
}