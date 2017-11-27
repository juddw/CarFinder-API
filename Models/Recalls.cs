using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder_API.Models
{
    public class Recalls
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public List<Results> Results { get; set; }
    }
}