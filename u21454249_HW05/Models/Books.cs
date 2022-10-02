using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class Books
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public int Pagecount { get; set; }
        public int Point { get; set; }
        public string Authors { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}