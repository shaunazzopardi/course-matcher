using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseHelper;
using CourseHelper.External.Models;

namespace TCFM.Controllers
{
    public class CoursesController : Controller
    {
        public static List<string> Group1 = new List<string>() { "Maltese", "Arabic", "English", "French", "German", "Greek", "Italian", "Latin", "Russian", "Spanish" };
        public static List<string> Group2 = new List<string>() { "Accounting", "Classical Studies", "Economics", "Geography", "History", "Marketing", "Philosophy", "Psychology", "Religious Knowledge", "Sociology" };
        public static List<string> Group3 = new List<string>() { "Applied Mathematics/Mechanics", "Biology", "Chemistry", "Environmental Science", "Physics", "Pure Mathematics" };
        public static List<string> Group4 = new List<string>() { "Art", "Computing", "Engineering Drawing", "Graphical Communication", "Home Economics and Human Ecology/Home Economics", "Information Technology/IT", "Music", "Physical Education", "Theatre and Performance/Theatre" };
        // GET: Courses
        public ActionResult Index()
        {
            ViewBag.ALevels = new SelectList(Group1);
            ViewBag.ALevels = new SelectList(Group2);
            ViewBag.ALevels = new SelectList(Group3);
            ViewBag.ALevels = new SelectList(Group4);

            CourseHelper.CourseHelper courseReader = new CourseHelper.CourseHelper();

            var data = courseReader.ReturnListOfCourses();

            ViewBag.Courses = data.ToList();
            //Show all courses
            return View(data.ToList());
        }

        public ActionResult FindCourses(string selectedALevel1, string selectedALevelMark1, string selectedALevel2, string selectedALevelMark2, string IntermediatesSOK, string IntermediatesMarkSOK, string selectedIntermediate1, string selectedIntermediateMark1, string selectedIntermediate2, string selectedIntermediateMark2, string selectedIntermediate3, string selectedIntermediateMark3)
        {
            List<string> subjectsChosen = new List<string>();

            subjectsChosen.Add(selectedALevel1);
            subjectsChosen.Add(selectedALevel2);
            subjectsChosen.Add(IntermediatesSOK);
            subjectsChosen.Add(selectedIntermediate1);
            subjectsChosen.Add(selectedIntermediate2);
            subjectsChosen.Add(selectedIntermediate3);

            List<string> marksChosen = new List<string>();

            marksChosen.Add(selectedALevelMark1);
            marksChosen.Add(selectedALevelMark2);
            marksChosen.Add(IntermediatesMarkSOK);
            marksChosen.Add(selectedIntermediateMark1);
            marksChosen.Add(selectedIntermediateMark2);
            marksChosen.Add(selectedIntermediateMark3);

            if (this.ValidateResults(subjectsChosen, marksChosen))
            {
                CourseHelper.CourseHelper courseReader = new CourseHelper.CourseHelper();

                var data = courseReader.getApplicableAndNoReqCourses(subjectsChosen, marksChosen);

                List<Course> withReqs = courseReader.bachelorMastersOthers(data[0])[0];
                List<Course> withoutReqs = courseReader.bachelorMastersOthers(data[1])[0];
                //ViewBag.Courses = data.ToList();
                ////Show all courses
                //return View(data.ToList());

                return View(new List<List<Course>>() { withReqs, withoutReqs });
            }
            else
            {
                TempData["error"] = true;
                TempData["subjects"] = subjectsChosen;
                TempData["marks"] = marksChosen;
                TempData["errorMessage"] = TempData["errorMessage"];
                return RedirectToAction("", "Start");
            }
        }

        public bool ValidateResults(List<string> subjects, List<string> marks)
        {
            TempData["errorMessage"] = "";
            TempData["aPrioriCriteriaSat"] = true;

            if(subjects.Contains(""))
            {
                TempData["errorMessage"] += "You still need to choose " + (subjects.Count(a => a == "")) + " more subject/s." + "\n";

                TempData["aPrioriCriteriaSat"] = false;

            }
            
            List<string> distinctSubjects = subjects.Distinct().ToList();
            distinctSubjects.Remove("");

            foreach (string s in distinctSubjects)
            {
                if (subjects.Count(a => a == s) > 1)
                {
                    TempData["errorMessage"] += "You have chosen " + s + " more than once." + "\n";
                    TempData["aPrioriCriteriaSat"] = false;
                }
            }

           
            
            List<string> group1 = subjects.Intersect(Group1).Distinct().ToList();

            bool noGroup1 = group1.Count() == 0;

            if (noGroup1)
            {
                TempData["errorMessage"] += "No subject from Group 1 chosen." + "\n";

                TempData["aPrioriCriteriaSat"] = false;
            }
            else
            {
                bool failed = true;

                foreach(string sub in group1){
                    if(marks[subjects.IndexOf(sub)] != "F")
                    {
                        failed = false;
                    }
                }

                if (failed) TempData["1stCriteria"] = false;
                else TempData["1stCriteria"] = true;

            }

            List<string> group2 = subjects.Intersect(Group2).Distinct().ToList();

            bool noGroup2 = group2.Count() == 0;

            if (noGroup2)
            {
                TempData["errorMessage"] += "No subject from Group 2 chosen." + "\n";

                TempData["aPrioriCriteriaSat"] = false;
            }
            else
            {
                bool failed = true;

                foreach(string sub in group2){
                    if(marks[subjects.IndexOf(sub)] != "F")
                    {
                        failed = false;
                    }
                }

                if (failed) TempData["2ndCriteria"] = false;
                else TempData["2ndCriteria"] = true;

            }

            List<string> group3 = subjects.Intersect(Group3).Distinct().ToList();

            bool noGroup3 = group3.Count() == 0;

            if (noGroup3)
            {
                TempData["errorMessage"] += "No subject from Group 3 chosen." + "\n";

                TempData["aPrioriCriteriaSat"] = false;
            }
            else
            {
                bool failed = true;

                foreach(string sub in group3){
                    if(marks[subjects.IndexOf(sub)] != "F")
                    {
                        failed = false;
                    }
                }

                if (failed) TempData["3rdCriteria"] = false;
                else TempData["3rdCriteria"] = true;

            }

            if (marks[2] == "F") TempData["4thCriteria"] = false;
            else TempData["4thCriteria"] = true;


            if((bool)TempData["aPrioriCriteriaSat"] != false){
            //check if grades are enough

                int matsecPoints = 0;

                matsecPoints += points(marks[0], "A");
                matsecPoints += points(marks[1], "A");
                matsecPoints += points(marks[2], "I");
                matsecPoints += points(marks[3], "I");
                matsecPoints += points(marks[4], "I");
                matsecPoints += points(marks[5], "I");

                if (matsecPoints < 44) TempData["5thCriteria"] = false;
                else TempData["5thCriteria"] = true;

                Dictionary<int, bool> criteriaSatisfactionArray = new Dictionary<int, bool>();

                criteriaSatisfactionArray.Add(1, (bool)TempData["1stCriteria"]);
                criteriaSatisfactionArray.Add(2, (bool)TempData["2ndCriteria"]);
                criteriaSatisfactionArray.Add(3, (bool)TempData["3rdCriteria"]);
                criteriaSatisfactionArray.Add(4, (bool)TempData["4thCriteria"]);
                criteriaSatisfactionArray.Add(5, (bool)TempData["5thCriteria"]);
                criteriaSatisfactionArray.Add(6, (bool)TempData["1stCriteria"]
                                                &&(bool)TempData["2ndCriteria"]
                                                &&(bool)TempData["3rdCriteria"]
                                                &&(bool)TempData["4thCriteria"]
                                                &&(bool)TempData["5thCriteria"]);

                ViewBag.criteriaSatisfactionArray = criteriaSatisfactionArray;
            }

            ViewBag.aPrioriCriteriaSat = TempData["aPrioriCriteriaSat"];

            if ((string)TempData["errorMessage"] == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public int points(string point, string AorI){
                if(AorI == "A"){
                    switch(point)
                    {
                        case "Any": return 30;
                        case "A" : return 30;
                        case "B" : return 24;
                        case "C" : return 18;
                        case "D" : return 12;
                        case "E" : return 6;
                    }
                }
                else if(AorI == "I")
                {
                    switch(point)
                    {
                        case "Any": return 10;
                        case "A" : return 10;
                        case "B" : return 8;
                        case "C" : return 6;
                        case "D" : return 4;
                        case "E" : return 2;
                    }
                }

                return 0;
            }
    }
}