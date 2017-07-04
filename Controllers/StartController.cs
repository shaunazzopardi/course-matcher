using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCFM.Controllers
{
    public class StartController : Controller
    {
        public static List<string> subjects = new List<string>() { "Maltese", "Arabic", "English", "French", "German", "Greek", "Italian", "Latin", "Russian", "Spanish", "Accounting", "Classical Studies", "Economics", "Geography", "History", "Marketing", "Philosophy", "Psychology", "Religious Knowledge", "Sociology", "Applied Mathematics/Mechanics", "Biology", "Chemistry", "Environmental Science", "Physics", "Pure Mathematics", "Art", "Computing", "Engineering Drawing", "Graphical Communication", "Home Economics and Human Ecology/Home Economics", "Information Technology/IT", "Music", "Physical Education", "Theatre and Performance/Theatre" };
        public static List<string> marks = new List<string>() { "Any Mark", "A", "B", "C", "D", "E", "F" };

        // GET: Start
        public ActionResult Index()
        {
            //List<string> subjectsChosen = new List<string>();
            //subjectsChosen.Add("English");
            //subjectsChosen.Add("Maltese");

            //List<char> marksChosen = new List<char>();
            //marksChosen.Add("A");
            //marksChosen.Add("B");
            //marksChosen.Add("C");
            //marksChosen.Add("D");
            //marksChosen.Add("E");
            //marksChosen.Add("F");

            if (TempData["error"] == null)
            {
                ViewBag.ALevels1 = new SelectList(subjects);
                ViewBag.ALevels2 = new SelectList(subjects);
                ViewBag.Intermediates1 = new SelectList(subjects);
                ViewBag.Intermediates2 = new SelectList(subjects);
                ViewBag.Intermediates3 = new SelectList(subjects);
                ViewBag.Marks1 = new SelectList(marks);
                ViewBag.Marks2 = new SelectList(marks);
                ViewBag.Marks3 = new SelectList(marks);
                ViewBag.Marks4 = new SelectList(marks);
                ViewBag.Marks5 = new SelectList(marks);
                ViewBag.Marks6 = new SelectList(marks);
            }
            else
            {
                ViewBag.error = true;
                ViewBag.errorMessage = TempData["errorMessage"];
                List<string> subjectsChosen = TempData["subjects"] as List<string>;
                List<string> marksChosen = TempData["marks"] as List<string>;

                ViewBag.ALevels1 = new SelectList(subjects, subjectsChosen[0]);
                ViewBag.ALevels2 = new SelectList(subjects, subjectsChosen[1]);
                ViewBag.Intermediates1 = new SelectList(subjects, subjectsChosen[3]);
                ViewBag.Intermediates2 = new SelectList(subjects, subjectsChosen[4]);
                ViewBag.Intermediates3 = new SelectList(subjects, subjectsChosen[5]);

                ViewBag.Marks1 = new SelectList(marks, marksChosen[0]);
                ViewBag.Marks2 = new SelectList(marks, marksChosen[1]);
                ViewBag.Marks3 = new SelectList(marks, marksChosen[2]);
                ViewBag.Marks4 = new SelectList(marks, marksChosen[3]);
                ViewBag.Marks5 = new SelectList(marks, marksChosen[4]);
                ViewBag.Marks6 = new SelectList(marks, marksChosen[5]);
            }

            return View();
        }

        //[HttpPost]
        //public ActionResult Courses()
        //{
        //    CourseHelper.CourseHelper courseReader = new CourseHelper.CourseHelper();

        //    var data = courseReader.ReturnListOfCourses();

        //    ViewBag.Courses = data.ToList();
        //    //Show all courses
        //    return View(data.ToList()); 
        //}
    }
}