using BugBusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bughound.Controllers
{
    public class ProgramController : Controller
    {
        // GET: Program
        public static List<ProgramModel> programs = null;
        public static List<UserModel> users = null;
        public static IEnumerable<AreaModel> areas = null;

        [HttpGet]
        public ActionResult Export()
        {
            if (Session["LoggedUserName"] != null || Session["IsAdmin"] == "0")
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Bh");
        }

        public void ExportToExcel()
        {
            if (Session["LoggedUserName"] != null && Session["IsAdmin"] == "1")
            {
                BugBusinessLayer.BugBusinessLayerControl bbc = new BugBusinessLayer.BugBusinessLayerControl();
                List<BugModel> defects = bbc.Defects.ToList();
                var grid = new GridView();
                grid.DataSource = from data in defects    //take Specific content only.
                                  select new
                                  {
                                      DefectId = data.DefectId,
                                      Program = data.Program,
                                      ProblemSummary = data.ProblemSummary,
                                      Problem = data.Problem,
                                      FunctionalArea = data.FunctionalArea,
                                      Status = data.SqlStatus,
                                      Priority = data.Priority,
                                      ReportedBy = data.ReportedBy,
                                      ReportedOn = data.SqlDate,
                                      AssignedTo = data.AssignedTo
                                  };

                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=Exported_Defects.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            else
                RedirectToAction("Login", "Bh");
        }

        public ActionResult ExportToCSV()
        {
            if (Session["LoggedUserName"] != null && Session["IsAdmin"] == "1")
            {
                StringWriter sw = new StringWriter();
                sw.WriteLine("\"DefectId\",\"Program\",\"ProblemSummary\",\"Problem\",\"FunctionalArea\",\"Status\",\"Priority\",\"ReportedBy\",\"Date\",\"AssignedTo\"");
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=Exported_Defects.csv");
                Response.ContentType = "text/csv";
                BugBusinessLayer.BugBusinessLayerControl bbc = new BugBusinessLayer.BugBusinessLayerControl();
                List<BugModel> defects = bbc.Defects.ToList();
                foreach (var data in defects)
                {
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\"",
                                      data.DefectId,
                                      data.Program,
                                      data.ProblemSummary,
                                      data.Problem,
                                      data.FunctionalArea,
                                      data.SqlStatus,
                                      data.Priority,
                                      data.ReportedBy,
                                      data.SqlDate,
                                      data.AssignedTo));
                }
                Response.Write(sw.ToString());
                Response.End();
            }
            return RedirectToAction("Login", "Bh");
        }

        public ActionResult Index()
        {
            if (Session["LoggedUserName"] != null && Session["IsAdmin"] == "1")  
            {
                programs = null;
                BugBusinessLayer.OperationsBusinessLayerControl obc = new BugBusinessLayer.OperationsBusinessLayerControl();
                programs = obc.Programs.ToList();
                return View(programs);
            }
            return RedirectToAction("Login","Bh");
        }


        public ActionResult IndexUsers()
        {
            if (Session["LoggedUserName"] != null && Session["IsAdmin"] == "1")   // TODO - Uncomment.
            {
                users = null;
                BugBusinessLayer.BugBusinessLayerControl obc = new BugBusinessLayer.BugBusinessLayerControl();
                users = obc.Users.ToList();
                return View(users);
            }
            return RedirectToAction("Login", "Bh");
        }

        [HttpGet]
        public ActionResult Create()   //Create Program
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login","Bh");
            return View();
        }


        [HttpPost]
        public ActionResult Create(ProgramModel pm)  //Create Program
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login","Bh");
            if (ModelState.IsValid && programFlag == 1)
            {
                BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
                obc.AddProgram(pm);
                return RedirectToAction("Index");
            }
            return View(pm);
        }

        [HttpPost]
        public ActionResult CancelNewProgram()  //Create Program
        {
            return RedirectToAction("Index");          
        }

        [AllowAnonymous]
        public string DoesProgramExist(string input)
        {
            if (OperationsBusinessLayerControl.DoesProgramNameExists(input))
            {
                programFlag = 0;
                return "Not Available";
            }
            else
            {
                programFlag = 1;
                return "Available";
            }
        }

        public static int programFlag = -1;

        [HttpGet]
        public ActionResult CreateUser()
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");
            return View();
        }


        [AllowAnonymous]
        public string DoesUserNameExist(string input)
        {
            if (String.IsNullOrEmpty(input))
            {                
                usernameFlag = 0;
                return "";
            }
            var regexItem = new Regex("^[a-zA-Z0-9_]*$");

            if (!regexItem.IsMatch(input))
            {
                usernameFlagSpace = 0;
                return "Not OK";
            }  
            else
                usernameFlagSpace = 1;

            if (BugBusinessLayerControl.DoesUsernameExists(input))
            {
                usernameFlag = 0;
                return "Not Available";
            }
            else
            {
                usernameFlag = 1;
                return "Available";
            }
        }

      
        public static int usernameFlag = -1;
        public static int usernameFlagSpace = -1;

        [HttpPost]
        public ActionResult CreateUser(UserModel pm)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");
            if (ModelState.IsValid && usernameFlag == 1 && usernameFlagSpace == 1)
            {
                BugBusinessLayer.BugBusinessLayerControl obc = new BugBusinessLayerControl();
                obc.AddUser(pm);
                return RedirectToAction("IndexUsers");
            }
            return View(pm);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProgramModel bm = programs.Single(x => x.Id == id);
            return View(bm);
        }

        [HttpPost]
        public ActionResult Edit(ProgramModel bm)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            if (ModelState.IsValid)
            {
                BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
                //FillEmptyFields(bm);
                obc.UpdateProgram(bm);
                Response.Write("<script>alert('Data Updated Successfully')</script>");
                return RedirectToAction("Index");
            }
            else
                return View(bm);
        }


        [HttpGet]
        public ActionResult EditUser(int id)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");
            UserModel um = users.Single(x => x.Id == id);
            return View(um);
        }

        [HttpPost]
        public ActionResult EditUser(UserModel bm)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");
            //if (ModelState.IsValid)
            {
                BugBusinessLayer.BugBusinessLayerControl obc = new BugBusinessLayerControl();
                //FillEmptyFields(bm);
                obc.UpdateUser(bm);                
                return RedirectToAction("IndexUsers");
            }
            //else
            //    return View(bm);
        }

       
        [HttpGet]
        public ActionResult Areas(int id)
        {
            if (Session["LoggedUserName"] == null)
                return RedirectToAction("Login", "Bh");

            BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();           
            areas = obc.GetAreasByProgramId(id);
            string programName = obc.GetProgramNameByPrgmId(id);
            ViewBag.ForProgram = programName;
            tempProgramId = id;
            tempProgram = programName;
            return View(areas);
        }
        public static string tempProgram = String.Empty;
        public static int tempProgramId = 0;

        [HttpGet]
        public ActionResult CreateArea()
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            ViewBag.ForProgram = tempProgram;
            return View();
        }


        [HttpPost]
        public ActionResult CreateArea(AreaModel am)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            if (ModelState.IsValid)
            {
                BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
                am.ProgramId = tempProgramId;
                am.ProgramName = tempProgram;
                obc.AddArea(am);
                return RedirectToAction("Areas", new { id=tempProgramId});
            }
            return null;
        }

        [HttpPost]
        public ActionResult CancelNewArea()
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");
            else
                return RedirectToAction("Areas");
        }


        [HttpGet]
        public ActionResult EditArea(int id)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            ViewBag.ForProgram = tempProgram;
            AreaModel am = areas.Single(x => x.Id == id);
            return View(am);
        }

        [HttpPost]
        public ActionResult EditArea(AreaModel am)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            if (ModelState.IsValid)
            {
                BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
                //FillEmptyFields(bm);
                am.ProgramId = tempProgramId;
                obc.UpdateArea(am);                
                return RedirectToAction("Areas", new { id=tempProgramId});
            }
            else
                return View(am);
        }

        [HttpPost]
        public ActionResult DeleteArea(int id)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            AreaModel am = areas.Single(x => x.Id == id);
            BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
            obc.DeleteArea(am);
            return RedirectToAction("Areas", new { id = tempProgramId });
        }

     

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"].ToString() == "0")
                return RedirectToAction("Login", "Bh");
            
            UserModel bm = users.Single(x => x.Id == id);
            if (bm.IsAdmin && users.Count(x => x.IsAdmin) == 1)
                return RedirectToAction("IndexUsers");
           
            BugBusinessLayer.BugBusinessLayerControl obc = new BugBusinessLayerControl();
            obc.DeleteUser(bm);
            return RedirectToAction("IndexUsers");
        }

        [HttpPost]
        public ActionResult Delete(int id)  //Program Delete function
        {
            if (Session["LoggedUserName"] == null || Session["IsAdmin"] == "0")
                return RedirectToAction("Login", "Bh");

            ProgramModel bm = programs.Single(x => x.Id == id);
            BugBusinessLayer.OperationsBusinessLayerControl obc = new OperationsBusinessLayerControl();
            obc.DeleteProgram(bm);
            return RedirectToAction("Index");
        }
    }
}