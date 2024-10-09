using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public static string path = "log.txt";
        public MainWindow()
        {
            InitializeComponent();

            // Logger checking
            if(logger is FileLogger && !File.Exists(path))
            {
                File.WriteAllText(path, "");
            }
            else if(logger is FileLogger)
            {
                File.AppendAllText(path, "\n\n");
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
        /// Exit from app by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitWindow(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "Closing window");
            this.Close();
        }

        /// <summary>
        /// Showing information about window by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoClick(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "Checking infomration about app");
            // TODO: Infomracja odnosnie okna
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
        /// Deploying content to control by Network connection
        /// </summary>
        public void DeployMainContent()
        {
            // Checking netowrk connection
            if (ConnectionInternet.NetworkCheck())
            {
                logger.Log(LogLevel.Info, "Opening GeneralInfo control");
                MainContent.Content = new GeneralInfo();
            }
            else
            {
                logger.Log(LogLevel.Info, "Opening NoConnection control");
                MainContent.Content = new NoConnection();
            }
        }
    }
}
