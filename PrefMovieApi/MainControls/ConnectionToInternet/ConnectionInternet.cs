using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PrefMovieApi
{
    internal static class ConnectionInternet
    {
        /// <summary>
        /// Checking connection with internet.
        /// </summary>
        /// <returns>Return boolean value, if coonection with internet is correct than method will return true, otherwise flase</returns>
        public static bool NetworkCheck()
        {
            try
            {
                // Creating request
                WebRequest request = WebRequest.Create("http://www.google.com");
                request.Timeout = 5000;

                // Checking resposne from site
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Config.logger.Log(LogLevel.Warn, ex.Message);
                return false;
            }
        }
    }
}
