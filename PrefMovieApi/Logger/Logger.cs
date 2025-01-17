﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrefMovieApi.Setup;

namespace PrefMovieApi
{
    internal class DebugLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Debug.WriteLine($"[{DateTime.Now}] {level}: {message}");
            Config.loggerMessages.Add((level, $"[{DateTime.Now}] {level}: {message}"));
        }

        public void ShowErrors()
        {

            if (Config.loggerMessages.Where(x => x.Item1 == LogLevel.Error).Any())
            {
                Debug.WriteLine("Errors:");
                foreach (var item in Config.loggerMessages)
                {
                    if (item.Item1 == LogLevel.Error)
                    {
                        Debug.WriteLine(item.Item2);
                    }
                }
            }
            else
            {
                Debug.WriteLine("Lack of errors");
            }
        }
    }

    internal class FileLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            string textToFile = $"[{DateTime.Now}] {level}: {message}\n";
            File.AppendAllText("log.txt", textToFile);
            Config.loggerMessages.Add((level, $"[{DateTime.Now}] {level}: {message}"));
        }

        public void ShowErrors()
        {
            if (Config.loggerMessages.Where(x => x.Item1 == LogLevel.Error).Any())
            {
                string listOfErrors = "\n\nERRORS:";
                foreach (var item in Config.loggerMessages)
                {
                    if (item.Item1 == LogLevel.Error)
                    {
                        listOfErrors += item.Item2 + "\n";
                    }
                }
                File.AppendAllText("log.txt", listOfErrors);
            }
            else
            {
                File.AppendAllText("log.txt", "Lack of errors");
            }
        }
    }

}
