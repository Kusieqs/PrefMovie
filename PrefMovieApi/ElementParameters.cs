using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefMovieApi
{
    public class ElementParameters
    {
        public readonly string Title;
        public readonly MediaType MediaType;
        public readonly DateTime Date;
        public readonly string Id;
        public ElementParameters(string title, MediaType mediaType, DateTime date, string id)
        {
            this.Title = title;
            this.MediaType = mediaType;
            this.Date = date;
            this.Id = id;
        }
        public ElementParameters(ElementParameters e1)
        {
            this.Title = e1.Title;
            this.MediaType = e1.MediaType;
            this.Date = e1.Date;
            this.Id = e1.Id;
        }
        public ElementParameters() { }
    }
}
