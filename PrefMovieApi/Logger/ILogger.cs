﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PrefMovieApi
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);

        void ShowErrors();
    }
}
