using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class GradesStudent_VM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public string Email { get; set; }
        public int Semester { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudentNo { get; set; }
        public bool IsActive { get; set; }=true;
        public int  EnrollmentId {  get; set; }
        public float Midterm { get; set; }
        public float Final {  get; set; }
        public float Average { get; set; }
        public int GradesId { get; set; }
        public int StudentId { get; set; }
    }
}
