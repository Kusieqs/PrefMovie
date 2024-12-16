using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PrefMovieApi.Setup;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Movies;
using System.Drawing;
using Point = System.Windows.Point;
using Image = System.Windows.Controls.Image;

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

        // Control to element whose exist
        public static bool isElementsExist = false;

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
        public void SetPrefering(object sender, RoutedEventArgs e)
        {
            if(Library.titles.Count > 0)
            {
                RemoveFromDictionary(sender);

                if(PreferingElements.isElementExist)
                {
                    ContentStackPanel.Children.RemoveRange(0, 2);
                }
                else
                {
                    Config.logger.Log(LogLevel.Warn, "Lack of elements to delete");
                }

                var borders = CreatePreferingElements();
                isElementsExist = true;
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


        public (Border, Border) CreatePreferingElements()
        {
            Border themeBorder = new Border()
            {
                CornerRadius = new CornerRadius(30, 30, 0, 0),
                Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF484848"),
                Margin = new Thickness(0, 15, 0, 0),
            };

            themeBorder.Child = SettingGridForTheme();

            Border contentElements = new Border()
            {
                CornerRadius = new CornerRadius(0, 0, 20, 20),
                Background = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(0,0,0,10)
            };

            contentElements.Child = SettingScrollViewerForContent();
            return (themeBorder, contentElements);
        }


        private Grid SettingGridForTheme()
        {
            Grid grid = new Grid()
            {
                Height = 60,
            };

            TextBlock theme = new TextBlock()
            {
                Style = Config.styleForThemeGeneral,
                Text = "---"
            };

            grid.Children.Add(theme);


            Button refreshButton = new Button
            {
                Height = 40,
                Width = 40,
                Style = Config.styleRefreshButton,
                Tag = 0
            };

            Image refreshIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/PrefMovieApi;component/Images/refreshIcon.png")),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            RenderOptions.SetBitmapScalingMode(refreshIcon, BitmapScalingMode.HighQuality);
            refreshButton.Content = refreshIcon;
            refreshButton.Click += SetPrefering;

            grid.Children.Add(refreshButton);

            return grid;
        }

        private ScrollViewer SettingScrollViewerForContent()
        {
            ScrollViewer scrollViewer = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Margin = new Thickness(10)
            };

            scrollViewer.MouseDown += ScrollViewerMouseDown;
            scrollViewer.MouseMove += ScrollViewerMouseMove;
            scrollViewer.MouseUp += ScrollViewerMouseUp;
            scrollViewer.PreviewMouseWheel += ParentScrollViewer;

            StackPanel stackPanel = PreferingElements.StackPanelWithContent();
            scrollViewer.Content = stackPanel;

            return scrollViewer;
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