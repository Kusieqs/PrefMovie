using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using PrefMovieApi.Setup;

namespace PrefMovieApi
{
    public static class CreatingImage
    {
        /// <summary>
        /// Creating image as a star
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image SettingImage(string path, int size = 0)
        {
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative)),
                Stretch = Stretch.UniformToFill
            };

            // Checking custom size for image
            if(size != 0)
            {
                image.Width = size;
                image.Height = size;
            }    

            return image;
        }

        /// <summary>
        /// Setting poster as image to application
        /// </summary>
        /// <param name="randomMoviesOrTvShows">Object of SearchMovie or SearchTv instance</param>
        /// <returns>BitmapImage object</returns>
        public static BitmapImage SetPoster(dynamic randomMoviesOrTvShows)
        {
            string posterUrl = Config.BASE_URL + randomMoviesOrTvShows.PosterPath;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(posterUrl, UriKind.Absolute);
            image.DecodePixelWidth = 200;
            image.EndInit();

            return image;
        }
    }
}
