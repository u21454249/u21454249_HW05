using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u21454249_HW05.Models
{
    public class Students
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Birthdate { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int Point { get; set; }
        public bool book { get; set; }
    }
}