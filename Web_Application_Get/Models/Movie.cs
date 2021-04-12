using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Application_Get.Models
{
    public class Movie
    {
        public string ID { get; set; }
        public string Title  { get; set; }
        public string Category { get; set; }
        public string ReleaseDate { get; set; }

        public Movie(string id, string title, string category, string releaseDate)
        {
            ID = id;
            Title = title;
            Category = category;
            ReleaseDate = releaseDate;
        }


    }
}