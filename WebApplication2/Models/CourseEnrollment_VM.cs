using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CourseEnrollment_VM
    {
        public int CourseID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credit {  get; set; }
        public int Semester {  get; set; }
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
    }
}
