using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class DetailsVM
    {
        public Books Book { get; set; }
        public List<Borrows> Borrowed { get; set; }
    }
}