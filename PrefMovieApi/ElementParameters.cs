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
        public MediaType MediaType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public ElementParameters(MediaType mediaType, int id, string title)
        {
            this.MediaType = mediaType;
            this.Id = id;
            this.Title = title;
        }
        public ElementParameters(ElementParameters e1)
        {
            this.MediaType = e1.MediaType;
            this.Id = e1.Id;
            this.Title = e1.Title;
        }
        public ElementParameters() { }
    }
}
