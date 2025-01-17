﻿using System;
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
using PrefMovieApi.Setup;
using TMDbLib.Objects.Movies;

namespace PrefMovieApi
{
    public partial class MainWindow : Window
    {
        // Library as control user
        public static Library library = new Library();

        public MainWindow()
        {
            InitializeComponent();
            LoadingStyles();

            // Logger checking
            if (Config.logger is FileLogger && !File.Exists(SetupPaths.PATH_TO_LOG))
            {
                File.WriteAllText(SetupPaths.PATH_TO_LOG, "");
            }
            else if (Config.logger is FileLogger)
            {
                File.AppendAllText(SetupPaths.PATH_TO_LOG, "\n\n");
                Config.logger.Log(LogLevel.Info, "New log");
            }
            else
            {
                Config.logger.Log(LogLevel.Info, "New log");
            }

            // Deploying content
            DeployMainContent();
        }

        /// <summary>
        /// Deploying content to control by Network connection
        /// </summary>
        public void DeployMainContent()
        {
            try
            {
                // Checking netowrk connection
                if (ConnectionInternet.NetworkCheck())
                {
                    Config.logger.Log(LogLevel.Info, "Connection to internet is correct");
                    MainContent.Content = new GeneralInfo(this);
                    Sorting.Content = new Sorting(MainContent);
                    Library.Content = library;
                }
                else
                {
                    Config.logger.Log(LogLevel.Info, "Connection to internet is not correct");
                    MainContent.Content = new NoConnection();
                }
            }
            catch (Exception ex)
            {
                // Error Log
                Config.logger.Log(LogLevel.Error, ex.Message);
                Config.logger.ShowErrors();
                Close();
            }
        }

        /// <summary>
        /// Exit from app by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitWindow(object sender, EventArgs e)
        {
            Config.logger.Log(LogLevel.Info, "Closing window");
            Config.logger.ShowErrors();
            Close();
        }

        /// <summary>
        /// Refreshing window by button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshWindow(object sender, EventArgs e)
        {
            Config.logger.Log(LogLevel.Info, "Refreshing window");
            Config.buttons.Clear();
            DeployMainContent();
        }

        /// <summary>
        /// Feature to allows us to move our app throw the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BorderClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Config.logger.Log(LogLevel.Info, "Window is changing place on screen");
                DragMove();
            }
        }

        /// <summary>
        /// Downwriting resources to Config file
        /// </summary>
        private void LoadingStyles()
        {
            Config.styleForButton = FindResource("ButtonImage") as Style;
            Config.styleThemeOfElement = FindResource("InfomrationAboutMovieOrShow") as Style;
            Config.styleForPosterButton = FindResource("PosterButton") as Style;
            Config.styleForThemeGeneral = FindResource("ThemeOfElements") as Style;
            Config.styleRefreshButton = FindResource("ButtonRefreshScroll") as Style;
        }
    }
}
