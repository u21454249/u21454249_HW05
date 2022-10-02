using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class Borrows
    {
        public int BorrowID { get; set; }
        public string Studentname { get; set; }
        public int BookID { get; set; }
        public string Taken { get; set; }
        public string Brought { get; set; }
    }
}