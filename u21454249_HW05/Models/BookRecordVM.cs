using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class BookRecordVM
    {
        public List<Types> Genre { get; set; }
        public List<Authors> Authors { get; set; }
        public List<Books> Book { get; set; }
    }
}