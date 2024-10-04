using System;
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
        // Api Key
        const string API_KEY_TO_TMDB = "";

        // Client object
        public static TMDbClient client = null;

        // Special control
        public static bool isReload = true;

        // Delegate for loading content page
        public delegate void LoadContent(object sender, RoutedEventArgs e);
        public LoadContent loadContent;

        // Style for infomration about element
        public static Style styleThemeOfElement;

        public GeneralInfo()
        {
            InitializeComponent();

            // Adding methods to delegate 
            loadContent = (LoadContent)Delegate.Combine(
                new LoadContent(SetLatestMovie),
                new LoadContent(SetTheBestMovie),
                new LoadContent(SetLatestTvShow),
                new LoadContent(SetTheBestTvShow)
                );

            try
            {
                // Connect to API TMDb
                client = new TMDbClient(API_KEY_TO_TMDB);
                client.DefaultLanguage = "en";

                // Checking that the API key is correct
                var testRequest = client.GetMovieAsync(550).Result;
                if(testRequest == null)
                {
                    MainWindow.logger.Log(LogLevel.Error, "Test request is null");
                    throw new FormatException();
                }
                MainWindow.logger.Log(LogLevel.Info, "Api is correct");

                // Setting new style for infomration
                styleThemeOfElement = FindResource("InfomrationAboutMovieOrShow") as Style;
                MainWindow.logger.Log(LogLevel.Info, "Style for infomrations about elemnts was loaded");

                // Setting movies
                if (isReload)
                {
                    MainWindow.logger.Log(LogLevel.Info, $"{nameof(isReload)} = {isReload}");
                    isReload = false;

                    // Setting list of prefering movies in main screen 
                    loadContent(null, null);
                }
                else
                {
                    MainWindow.logger.Log(LogLevel.Warn, $"{nameof(isReload)} = {isReload}");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Crticial Error!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                MainWindow.logger.Log(LogLevel.Error, ex.Message);
                Window window = Window.GetWindow(this);
                // TODO: Zamkniecie okna glownego
            }
        }

        /// <summary>
        /// Setting latest movies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLatestMovie(object sender, RoutedEventArgs e)
        {
            TheNewOnceMovies = SettingMovies.TheLatestMovies(TheNewOnceMovies);
        }

        /// <summary>
        /// Setting the best movies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTheBestMovie(object sender, RoutedEventArgs e)
        {
            TheBestMovies = SettingMovies.TheBestMovies(TheBestMovies);
        }

        /// <summary>
        /// Setting latest tv shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLatestTvShow(object sender, RoutedEventArgs e)
        {
            TheNewOnceSeries = SettingMovies.TheLatestSeries(TheNewOnceSeries);
        }

        /// <summary>
        /// Setting the best tv shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTheBestTvShow(object sender, RoutedEventArgs e)
        {
            TheBestSeries = SettingMovies.TheBestSeries(TheBestSeries);
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
