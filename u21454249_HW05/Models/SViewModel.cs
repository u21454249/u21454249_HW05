using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class SViewModel
    {
        public List<Students> Students { get; set; }
        public Books Book { get; set; }
        public List<Grade> Class { get; set; }
    }
}