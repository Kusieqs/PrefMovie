using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Movies;

namespace PrefMovieApi
{
    /// <summary>
    /// Logika interakcji dla klasy GeneralInfo.xaml
    /// </summary>

    public partial class GeneralInfo : UserControl
    {
        // Client object
        public static TMDbClient client = null;

        // Delegate for loading content page
        public delegate void LoadContent(object sender, RoutedEventArgs e);
        public LoadContent loadContent;

        // Main window
        public Window mainWindow;

        public GeneralInfo(Window mainWindow)
        {
            InitializeComponent();

            // Adding methods to delegate 
            loadContent = (LoadContent)Delegate.Combine(
                new LoadContent(SetLatestMovie),
                new LoadContent(SetTheBestMovie),
                new LoadContent(SetLatestTvShow),
                new LoadContent(SetTheBestTvShow)
                );

            this.mainWindow = mainWindow;

            try
            {
                // Connect to API TMDb
                client = new TMDbClient(Config.API_KEY_TO_TMDB);
                client.DefaultLanguage = "en";

                // Checking that the API key is correct
                var testRequest = client.GetMovieAsync(550).Result;
                if (testRequest == null)
                {
                    MainWindow.logger.Log(LogLevel.Error, "Test request is null");
                    throw new FormatException();
                }
                MainWindow.logger.Log(LogLevel.Info, "API is correct");

                // Setting new style for infomration
                MainWindow.logger.Log(LogLevel.Info, "Style for infomrations about elemnts was loaded");

                // Setting movies
                loadContent(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Crticial Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.logger.Log(LogLevel.Error, ex.Message);
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

        private void RemoveFromDictionary(object sender)
        {
            MessageBox.Show($"{sender != null} {Config.buttons.Count}");

            if (sender != null && int.TryParse((sender as Button).Tag.ToString(), out int result) && Config.buttons.Count > 0)
            {
                var keys = Config.buttons.Keys.Skip(result * 8).Take(8).ToList();
                foreach (var key in keys)
                {
                    Config.buttons.Remove(key);
                }
                MessageBox.Show($"Remove {Config.buttons.Count}");
            }
        }
        #region Scroll viewer logic
        /// <summary>
        /// Methods to Scroll Viewer to scroll by button on mouse
        /// </summary>
        private bool isMouseDown = false;
        private Point mouseStartPosition;
        private double scrollViewerStartOffset;
        private ScrollViewer scrollViewer;

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