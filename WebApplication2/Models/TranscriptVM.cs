using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class TranscriptVM
    {
        public int CourseID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public int Semester { get; set; }
        public bool IsActive { get; set; }
        public double Midterm { get; set; }
        public double Final { get; set; }
        public double Average { get; set; }

      
    }

}

