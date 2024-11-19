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
        public ElementParameters(MediaType mediaType, int id)
        {
            this.MediaType = mediaType;
            this.Id = id;
        }
        public ElementParameters(ElementParameters e1)
        {
            this.MediaType = e1.MediaType;
            this.Id = e1.Id;
        }
        public ElementParameters() { }
    }
}
