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
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
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
