using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMDbLib.Objects.Movies;

namespace PrefMovieApi
{
    public partial class MainWindow : Window
    {
        public static List<(LogLevel, string)> loggerMessages = new List<(LogLevel, string)>();
        public static ILogger logger = new FileLogger();
        public static Library library = new Library();
        public MainWindow()
        {
            InitializeComponent();

            Config.styleForButton = FindResource("ButtonImage") as Style;
            Config.styleThemeOfElement = FindResource("InfomrationAboutMovieOrShow") as Style;
            Config.styleForPosterButton = FindResource("PosterButton") as Style;

            // Logger checking
            if (logger is FileLogger && !File.Exists(Config.PATH_TO_LOG))
            {
                File.WriteAllText(Config.PATH_TO_LOG, "");
            }
            else if(logger is FileLogger)
            {
                File.AppendAllText(Config.PATH_TO_LOG, "\n\n");
                logger.Log(LogLevel.Info, "New log");
            }
            else
            {
                logger.Log(LogLevel.Info, "New log");
            }

            // Deploying content
            DeployMainContent();
        }

        /// <summary>
        /// Deploying content to control by Network connection
        /// </summary>
        public void DeployMainContent()
        {
            Sorting.Content = new Sorting(MainContent);
            Library.Content = library;

            // Checking netowrk connection
            if (ConnectionInternet.NetworkCheck())
            {
                logger.Log(LogLevel.Info, "Opening GeneralInfo control");
                Sorting.IsEnabled = true;

                logger.Log(LogLevel.Warn, "General info is creating");
                MainContent.Content = new GeneralInfo(this);
            }
            else
            {
                logger.Log(LogLevel.Info, "Opening NoConnection control");
                MainContent.Content = new NoConnection();
                Sorting.IsEnabled = false;
            }
        }


        /// <summary>
        /// Exit from app by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitWindow(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "Closing window");
            Close();
        }

        /// <summary>
        /// Refreshing window by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshWindow(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "Refreshing window");
            DeployMainContent();
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
                    logger.Log(LogLevel.Info, "Window is changing place on screen");
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
            }
        }
    }
}
