using CreditRatingModel.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CreditRatingSystem.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
//using CreditRatingModel.Model;
using CreditRatingModel.Model;




namespace CreditRatingSystem.Controllers
{

    public class ReportController : Controller
    {
        //
        // GET: /Report/
        creaditratingEntities db = new creaditratingEntities();
        //Report R = new Report();

        ReportSS r = new ReportSS();




        public void dropList(ReportSS R)
        {
            //ViewBag.College = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName").Distinct();
            //ViewBag.AY_ID = new SelectList(db.AcademicYears.ToList(), "Id", "AyId").Distinct();
            //ViewBag.Course = new SelectList(db.CourseMasters.ToList(), "Id", "CourseName").Distinct();

            ViewBag.College = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName");
            ViewBag.AY_ID = new SelectList(db.AcademicYears.ToList(), "Id", "AyId");
            ViewBag.Course = new SelectList(db.CourseMasters.ToList(), "Id", "CourseName");
        }
        public ActionResult Student_Wise_Loan_Details_View()
        {

            return View();
        }
        public Boolean CheckNID(string NID)
        {
            Boolean result;
            var sqlstr = (from ED in db.EmployeeDetails
                          where ED.NId == NID
                          select new
                          {
                              ED.NId
                          }
                            ).FirstOrDefault();

            if (sqlstr == null)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public ActionResult Student_Wise_Loan_Details(string NID)
        {
            //var output=CheckNID(NID);
            //if (output=="N")
            //{

            //}
            //else
            //{

            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters = new SqlParameter();
                    parameters.ParameterName = "@NID";
                    if (NID != "")
                    {
                        parameters.Value = NID;
                    }



                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_StudWiseLoneDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (NID != "")
                            {
                                cmd.Parameters.Add(parameters);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/Student_Wise_Loan_Details_View';  </script>"));

                    }
                    else
                    {

                        using (ReportClass RC = new ReportClass())
                        {
                            RC.FileName = Server.MapPath("~/Report/Stud_Wise_Loan_Details.rpt");
                            RC.Load();
                            RC.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            //  RC.Database.Tables[1].SetDataSource(ds.Tables[1]);
                            RC.Refresh();
                            RC.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }

                    }


                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            //}

        }

        public ActionResult college_Wise_Course_Details_View()
        {
            ReportSS RSS = new ReportSS();

            dropList(RSS);
            // ViewBag.College = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName");
            return View();

        }

        public ActionResult college_Wise_Course_Details(string collegeID, string YearID)
        {

            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();
                    if (collegeID != "")
                    {
                        parameters1.ParameterName = "@collegeId";
                        parameters1.Value = collegeID;
                    }
                    if (YearID != "")
                    {
                        parameters2.ParameterName = "@yearid";
                        parameters2.Value = YearID;
                    }


                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_CollegeWiseCourseDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (collegeID != "")
                            {
                                cmd.Parameters.Add(parameters1);
                            }
                            if (YearID != "")
                            {
                                cmd.Parameters.Add(parameters2);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }

                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/college_Wise_Course_Details_View';  </script>"));
                    }
                    else
                    {
                        using (ReportClass RC = new ReportClass())
                        {
                            RC.FileName = Server.MapPath("~/Report/College_Wise_Course_Details.rpt");
                            RC.Load();
                            RC.Database.Tables[0].SetDataSource(ds.Tables[0]);

                            RC.Refresh();
                            RC.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

        }

        public ActionResult Course_Wise_Loan_Details()
        {
            ReportSS RSS = new ReportSS();

            dropList(RSS);
            return View();
        }

        public ActionResult Course_Wise_Loan_Details(string courseID, string YearID)
        {

            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();
                    if (courseID != "")
                    {
                        parameters1.ParameterName = "@courseId";
                        parameters1.Value = courseID;
                    }
                    if (YearID != "")
                    {
                        parameters2.ParameterName = "@yearid";
                        parameters2.Value = YearID;
                    }


                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_CourseWiseLoanDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (courseID != "")
                            {
                                cmd.Parameters.Add(parameters1);
                            }
                            if (YearID != "")
                            {
                                cmd.Parameters.Add(parameters2);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }


                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/Course_Wise_Loan_Details';  </script>"));
                    }
                    else
                    {
                        using (ReportClass RC = new ReportClass())
                        {
                            RC.FileName = Server.MapPath("~/Report/Course_Wise_Loan_Details.rpt");
                            RC.Load();
                            RC.Database.Tables[0].SetDataSource(ds.Tables[0]);

                            RC.Refresh();
                            RC.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

        }

        public ActionResult StudentInfo()
        {

            return View();
        }


        public ActionResult StudentInfoData(string user_id)
        {

            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {


                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();
                    if (user_id != "")
                    {
                        parameters1.ParameterName = "@NID";
                        parameters1.Value = user_id;

                    }


                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "spPersonalInfo";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (user_id != "")
                            {

                                cmd.Parameters.Add(parameters1);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }




                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/StudentInfo';  </script>"));
                    }
                    else
                    {
                        using (ReportClass rptH = new ReportClass())
                        {
                            //rptH.FileName = "E:/tanmay/ProjectsNew/CRS/CreditRatingSystem/CreditRatingSystem/CrystalReport1.rpt";
                            rptH.FileName = Server.MapPath("~/Report/Student_Info.rpt");


                            rptH.Load();
                            rptH.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            //rptH.Database.Tables[1].SetDataSource(ds.Tables[1]);
                            rptH.Refresh();
                            rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    //Json(new { msg = "Error : " + ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

        }





        public void RepaymentReciept(string Id)
        {

            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {


                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();
                    parameters1.ParameterName = "@TransactionId";
                    parameters1.Value = Id;




                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_RepaymentReciept";
                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.Add(parameters1);

                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }




                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        // return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/StudentInfo';  </script>"));
                    }
                    else
                    {
                        using (ReportClass rptH = new ReportClass())
                        {
                            //rptH.FileName = "E:/tanmay/ProjectsNew/CRS/CreditRatingSystem/CreditRatingSystem/CrystalReport1.rpt";
                            rptH.FileName = Server.MapPath("~/Report/RepaymentReciept.rpt");


                            rptH.Load();
                            rptH.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            rptH.Refresh();
                            rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            //    return View();
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    //Json(new { msg = "Error : " + ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

        }


        public ActionResult Transaction_Loan_Details()
        {
            return View();
        }

        public ActionResult Transaction_Loan_Details_View(string NID, string LoanAccountNo, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();
                    SqlParameter parameters3 = new SqlParameter();
                    SqlParameter parameters4 = new SqlParameter();
                    if (NID != "")
                    {
                        parameters1.ParameterName = "@NID";
                        parameters1.Value = NID;
                    }

                    if (LoanAccountNo != "")
                    {
                        parameters2.ParameterName = "@LoanAccountNo";
                        parameters2.Value = LoanAccountNo;
                    }
                    if (FromDate != null)
                    {
                        parameters3.ParameterName = "@FromDate";
                        parameters3.Value = FromDate;
                    }
                    if (ToDate != null)
                    {
                        parameters4.ParameterName = "@ToDate";
                        parameters4.Value = ToDate;
                    }
                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_TRANSACTION_LOAN_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (NID != "")
                            {
                                cmd.Parameters.Add(parameters1);
                            }
                            if (LoanAccountNo != "")
                            {
                                cmd.Parameters.Add(parameters2);
                            }
                            if (FromDate != null)
                            {
                                cmd.Parameters.Add(parameters3);
                            }
                            if (ToDate != null)
                            {
                                cmd.Parameters.Add(parameters4);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }

                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/Transaction_Loan_Details';  </script>"));
                    }
                    else
                    {
                        using (ReportClass RC = new ReportClass())
                        {
                            RC.FileName = Server.MapPath("~/Report/Transaction_Loan_Details.rpt");
                            RC.Load();
                            RC.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            RC.Refresh();
                            RC.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }

                    }

                }
                catch (Exception ex)
                {


                    throw ex;
                }

            }
        }

        public ActionResult ConsumerCreditDetails()
        {
            return View();
        }

        public ActionResult ConsumerCreditDetailsView(string NID)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds=new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameter1 = new SqlParameter();
                    if (NID != "")
                    {
                        parameter1.ParameterName = "@NID";
                        parameter1.Value = NID;
                    }


                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();
                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SP_ConsumerCreditDetails";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (NID != "")
                            {
                                cmd.Parameters.Add(parameter1);
                            }
                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }

                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Report/ConsumerCreditDetails';  </script>"));
                    }
                    else
                    {
                        using (ReportClass RC = new ReportClass())
                        {
                            RC.FileName = Server.MapPath("~/Report/Consumer_Credit_Details.rpt");
                            RC.Load();
                            RC.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            RC.Refresh();
                            RC.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

              
            }
        }
    }
}
