using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    public class AdminController : Controller
    {
        Database1EF db = new Database1EF();

        public ActionResult Login()
        {
            return View();
        }




        public ActionResult EditStudent(int id)
        {
            var ogr = db.Students.Find(id);
            if (ogr == null)
            {
                return HttpNotFound();
            }

            UserLoginVM stdnt = new UserLoginVM
            {

                StudentId = Convert.ToInt32(ogr.StudentId),
                StudentNo = Convert.ToInt32(ogr.StudentNo),
                FirstName = ogr.FirstName,
                LastName = ogr.LastName,
                Email = ogr.Email,
            };
            return View(stdnt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(GradesStudent_VM model)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Find(model.StudentId);

                student.StudentNo = model.StudentNo;
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.Email = model.Email;
                db.SaveChanges();
                return RedirectToAction("HomeAdmin", "Home");
            }
            db.SaveChanges();
            return View(model);
        }

        [HttpGet]

        public ActionResult DeleteStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            student.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("HomeAdmin", "Home");

        }


        public ActionResult EditCourse(int id)
        {
            var ders= db.Courses.Find(id);
            if (ders == null)
            {
                return HttpNotFound();
            }

            CourseEnrollment_VM newDers = new CourseEnrollment_VM
            {
                CourseID = ders.CourseID,
                Code = ders.Code,
                Name= ders.Name,
                Credit = ders.Credit,
                Semester = ders.Semester,

            };
            return View(newDers);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(CourseEnrollment_VM model)
        {
            if (ModelState.IsValid)
            {
                var dersYeni = db.Courses.Find(model.CourseID);

                dersYeni.Code= model.Code;
                dersYeni.Name= model.Name;
                dersYeni.Credit= model.Credit;
                dersYeni.Semester= model.Semester;
                db.SaveChanges();
                return RedirectToAction("Courses", "Home");
                

            }
            
            return View(model);
        }


        [HttpGet]

        public ActionResult DeleteCourse(int id)
        {
            var dltCourse = db.Courses.Find(id);
            if (dltCourse == null)
            {
                return HttpNotFound();
            }
            dltCourse.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("Courses", "Home");

        }

        public ActionResult ManageGrades(int id)
        {
            var ogrenci = db.Students.Find(id);
            if (ogrenci == null)
            {
                return HttpNotFound();
            }
            List<GradesStudent_VM> modelList = new List<GradesStudent_VM>();

            foreach (var e in ogrenci.Enrollments)
            {
                
                // Eğer not tablosu boşsa null hatası almamak için FirstOrDefault kullanıyoruz
                var not = db.Grades.FirstOrDefault(x => x.EnrollmentId == e.EnrollmentId);

                modelList.Add(new GradesStudent_VM
                {
                    StudentId = ogrenci.StudentId,
                    FirstName = ogrenci.FirstName,
                    LastName = ogrenci.LastName,
                    EnrollmentId = e.EnrollmentId,
                    Name = e.Courses.Name, 
                    Midterm = not != null ? (float)not.Midterm : 0,
                    Final = not != null ? (float)not.Final : 0,
                    Average = not != null ? (float)not.Average : 0
                });
            }

            
            return View(modelList);
        }
        [HttpPost]
        public ActionResult ManageGrades(List<GradesStudent_VM> modelList)
        {
           
            foreach (var mdl in modelList) {

            var mevcutNot = db.Grades.FirstOrDefault(x => x.EnrollmentId == mdl.EnrollmentId);

            if (mevcutNot != null)
            {
                mevcutNot.Midterm = mdl.Midterm;
                mevcutNot.Final = mdl.Final;
                mevcutNot.Average= (mdl.Midterm * 0.4f) + (mdl.Final * 0.6f);
            }
            else
            {
                var yeniNot = new Grades
                {
                    EnrollmentId = mdl.EnrollmentId,
                    Midterm = mdl.Midterm,
                    Final = mdl.Final,
                    Average = (mdl.Midterm * 0.4f) + (mdl.Final * 0.6f),
                };
                db.Grades.Add(yeniNot);
            }
            }
            db.SaveChanges();

            // İlk elemandan StudentId alarak geri dönüyoruz
            int studentId = modelList.First().StudentId;
            TempData["Success"] = "Notlar Kaydedildi";
            return RedirectToAction("HomeAdmin", "Home", new { id = studentId });
        }















    }

}