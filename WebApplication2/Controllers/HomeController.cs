using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{ 
    public class HomeController : Controller
    { 
        Database1EF db = new Database1EF();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon(); 

            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult LoginUser()
        {
            return View(new Students());
        }

        [HttpPost]
        public ActionResult LoginUser(Students model)
        {
            var student = db.Students.FirstOrDefault(x => x.FirstName == model.FirstName
            && x.Password == model.Password && x.IsActive==true);

            if (student != null)
            {
                if(student.StudentNo == 1)
                {
                    ViewBag.ErrorMessage = "Admin yetkisiyle öğrenci panelinden giriş yapamazsınız.";
                    return View(model);
                }


                Session["FirstName"] = student.FirstName;
                Session["LastName"] = student.LastName;
                Session["StudentId"] = student.StudentId;

                return RedirectToAction("HomeUser", "Home");
            }
            else {
                ViewBag.Error = "İsim veya Şifre hatalı !";
            }
            

            return View(model);
        }


        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View(new Students());
        }

        [HttpPost]
        public ActionResult LoginAdmin(Students model)
        {
            var student = db.Students.FirstOrDefault(x => x.FirstName == model.FirstName && x.Password == model.Password);
            if (student != null)
            {
                if (student.FirstName == "Admin")
                {
                    Session["FirstName"] = student.FirstName;
                    return RedirectToAction("HomeAdmin", "Home");
                }
                else
                {
                    ViewBag.Error = "Bilgiler doğru fakat Admin yetkiniz yok!";
                }
            }
            else
            {
                ViewBag.Mesaj = "İsim veya şifre hatalı!";
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
           
            return View(new Students());
        }

        [HttpPost]
        public ActionResult Register(Students model)
        {


            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName) || model.StudentNo == 0)
            {
                ViewBag.Error = "Lütfen tüm alanları eksiksiz doldurunuz!";
                return View(model);
            }
            var studentControl = db.Students.FirstOrDefault(x => x.StudentNo == model.StudentNo && x.Password==model.Password);

            if (studentControl != null)
            {
                ViewBag.Error = "Bu kişi zaten kayıtlı !";
                return View(model);
            }
            else {Students newUser = new Students();
            newUser.FirstName = model.FirstName;
            newUser.LastName = model.LastName;
            newUser.StudentNo = Convert.ToInt32(model.StudentNo);
            newUser.Email = model.Email;
            newUser.Password = model.Password;
            newUser.StdSemester=model.StdSemester;

            newUser.IsActive = true;

            db.Students.Add(newUser);
            db.SaveChanges();
            TempData["Success"] = "Kayıt başarılı. Giriş yapabilirsiniz."; }

            
            return RedirectToAction("Login","Home");
        }

        public ActionResult HomeAdmin(Students model)
        {
            //if (Session["StudentId"] == null)
            //{
            //    return RedirectToAction("Login", "Home");   Burayı aktif edince hata oluşuyor
            //}
            var studentlist = db.Students.Where(x=> x.IsActive==true).Select(x => new UserLoginVM
            {
                StudentId = x.StudentId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                StudentNo =(int)x.StudentNo,
                
            }).ToList();


            return View(studentlist);
        }

        public ActionResult Courses(int? semester)
        {
            var query = db.Courses.AsQueryable();

            if (semester.HasValue)
            {
                query = query.Where(x=> x.Semester == semester.Value);
            }
            
            query = query.Where(x => x.IsActive == true);

            var courselist = query.Select(x => new CourseEnrollment_VM
            {
                Code = x.Code,
                Name = x.Name,
                Credit = x.Credit,
                Semester = x.Semester,
                CourseID = x.CourseID,

            }).OrderBy(x=>x.Semester).ToList();

            return View(courselist);   
        }
        [HttpGet]
        public ActionResult CourseAdd()
        {
            if (Session["FirstName"]?.ToString() != "Admin") return RedirectToAction("LoginAdmin");

            return View(new Courses());
        }

        [HttpPost]
        public ActionResult CourseAdd(Courses model)
        {
            if (Session["FirstName"]?.ToString() != "Admin") return RedirectToAction("LoginAdmin");


            if (string.IsNullOrWhiteSpace(model.Code) || string.IsNullOrWhiteSpace(model.Name) || model.Credit == 0)
            {
               ViewBag.Error = "Lütfen tüm alanları eksiksiz doldurunuz!";
               return View(model);
            }
            var courseControl = db.Courses.FirstOrDefault(x=> x.Code == model.Code);
            if (courseControl != null)
            {
                ViewBag.Error = "Bu ders zaten kayıtlı !";
                return View(model);

            }
            else
            {
                
                Courses newCourse = new Courses();
                newCourse.Code = model.Code;
                newCourse.Name = model.Name;
                newCourse.Credit = model.Credit;
                newCourse.Semester = model.Semester;
                newCourse.IsActive = model.IsActive;

                newCourse.IsActive = true;
                db.Courses.Add(newCourse);
                db.SaveChanges();
                TempData["Success"] = "Ders kaydedildi. ";
            }


            return RedirectToAction("Courses", "Home");


        }
        public ActionResult HomeUser()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int studentId = Convert.ToInt32(Session["StudentId"]) ;


           var dersler = db.Enrollments.Where(x=> x.Students.StudentId == studentId && x.IsActive==true)
                .Select(x=> new GradesStudent_VM
                {
                    Code = x.Courses.Code,
                    Name = x.Courses.Name,
                    Semester = x.Courses.Semester,
                 
                    // Notlar henüz girilmemiş olabilir, bu yüzden DefaultIfEmpty veya null kontrolü gerekebilir
                    Midterm = (float?)(x.Grades.FirstOrDefault().Midterm) ?? 0,
                    Final = (float?)(x.Grades.FirstOrDefault().Final) ?? 0,
                    Average = (float?)(x.Grades.FirstOrDefault().Average) ?? 0
                }).OrderBy(x=> x.Semester).ToList();


            return View(dersler);
        }

        [HttpGet]
        public ActionResult DersKayıt(int? semester)
        {

            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Login");
            }


            int stId = Convert.ToInt32(Session["StudentId"]);

            var student = db.Students.Find(stId);
            var currentSemester =student.StdSemester; 

            var kayıtlıDersler = db.Enrollments.Where(x=> x.StudentId == stId).Select(x=> x.CourseID).ToList();

            var sorgu = db.Courses.Where(x => !kayıtlıDersler.Contains(x.CourseID) && x.IsActive == true && x.Semester<=currentSemester);

            // Eğer semester parametresi dolu gelmişse filtreyi uygula 
            if (semester.HasValue)
            {
                sorgu = sorgu.Where(x => x.Semester == semester.Value);
            }

            
            var alınabilirDersler = sorgu.Select(x => new CourseEnrollment_VM
            {
                CourseID = x.CourseID,
                Code = x.Code,
                Name = x.Name, 
                Credit = x.Credit,
                Semester = x.Semester
            })
            .OrderBy(x => x.Semester) 
            .ToList();

            var currentCredit = db.Enrollments
                .Where(x=> x.StudentId==stId).
                Sum(x => (int?)x.Courses.Credit) ?? 0;

            ViewBag.CurrentCredits = currentCredit;
            ViewBag.MaxCredit = 36;
            ViewBag.StudentSemester = currentSemester;

            return View(alınabilirDersler);
        }
        
        public ActionResult DersiAl(int id)
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Login");
            }
            int stId = Convert.ToInt32(Session["StudentId"]);

            var currentCredit = db.Enrollments.Where(x=> x.StudentId==stId).Sum(x => (int?)x.Courses.Credit) ?? 0;

            //  Eklemek istediğin dersin kredisini bul
            var eklenecekDers = db.Courses.Find(id);
            int courseCredit = eklenecekDers?.Credit ?? 0;
            if (currentCredit + courseCredit > 36)
            {
                TempData["Error"] = "Toplam kredi sınırını (36) aşamazsınız!";
                return RedirectToAction("DersKayıt");
            }

            var yeniKayıt = new Enrollments
             {
             StudentId = stId,
             CourseID = id,
             IsActive = false,
             };
             db.Enrollments.Add(yeniKayıt);
             db.SaveChanges();
            
           
            return RedirectToAction("DersKayıt");
        }

        public ActionResult DersOnay(int? enrollmentId, int? secilenDonem)
        {
            if (enrollmentId.HasValue)
            {
                var kayit = db.Enrollments.Find(enrollmentId.Value);
                if (kayit != null)
                {
                    kayit.IsActive = true;
                    db.SaveChanges();
                    TempData["Mesaj"] = "Ders başarıyla onaylandı.";
                }

                return RedirectToAction("DersOnay", new { secilenDonem = secilenDonem });
            }

            var sorgu = db.Enrollments.Where(x => x.IsActive == false);

            if (secilenDonem.HasValue)
            {
                sorgu = sorgu.Where(x => x.Courses.Semester == secilenDonem.Value);
            }

            var bekleyenler = sorgu.OrderBy(x => x.StudentId).ToList();

            ViewBag.SelectedSemester = secilenDonem;
            return View(bekleyenler);
        }

        public ActionResult Transcript()
        {
            if (Session["StudentId"] == null)
            {
                return RedirectToAction("Login");
            }


            int stId = Convert.ToInt32(Session["StudentId"]);

            var transcriptionData = db.Enrollments.Where(x=> x.IsActive==true && x.StudentId==stId)
                .Select(x=> new TranscriptVM
                {
                    CourseID= x.CourseID,
                    Code = x.Courses.Code,
                    Name = x.Courses.Name,
                    Credit = x.Courses.Credit,
                    Semester = x.Courses.Semester,
                    IsActive= x.Courses.IsActive,

                    Midterm = x.Grades.Select(g => (double?)g.Midterm).FirstOrDefault() ?? 0.0,
                    Final = x.Grades.Select(g => (double?)g.Final).FirstOrDefault() ?? 0.0,
                    Average = x.Grades.Select(g => (double?)g.Average).FirstOrDefault() ?? 0.0

                }).OrderBy(x => x.Semester)
        .ToList();

            return View(transcriptionData);
        }


        public ActionResult DersRed(int? enrollmentId, int? secilenDonem)
        {
            if (enrollmentId.HasValue)
            {
                var kayit = db.Enrollments.Find(enrollmentId.Value);
                if (kayit != null)
                {
                    kayit.IsActive = null;
                    db.SaveChanges();
                    TempData["Mesaj"] = "Ders isteği reddedildi.";
                }

                return RedirectToAction("DersRed", new { secilenDonem = secilenDonem });
            }
            return RedirectToAction("DersOnay", new { secilenDonem = secilenDonem });

        }








































        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1. Gitmek istenen Action ismini alalım
            string actionName = filterContext.ActionDescriptor.ActionName;

            // 2. İstisnalar: Login ve Register sayfalarındaysak kontrolü geç (Yoksa sonsuz döngü olur)
            if (actionName == "Login" || actionName == "LoginUser" || actionName == "LoginAdmin" || actionName == "Register")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // 3. Session kontrolü (Hem Admin hem Student için)
            bool isAdminLoggedIn = Session["FirstName"]?.ToString() == "Admin";
            bool isUserLoggedIn = Session["StudentId"] != null;

            // 4. Eğer giriş yapılmamışsa doğrudan Login sayfasına fırlat
            if (!isAdminLoggedIn && !isUserLoggedIn)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                { "controller", "Home" },
                { "action", "Login" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }

    }
}