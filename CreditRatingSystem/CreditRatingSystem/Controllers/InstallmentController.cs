using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;


namespace CreditRating.Controllers
{
    public class InstallmentController : Controller
    {
        creaditratingEntities cre = new creaditratingEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Installment()
        {

            ViewData["a"] = null;
            return View();
        }

        [HttpPost]

        public ActionResult Installment(HttpPostedFileBase file)
        {
           
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
               
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/files/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
               
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }




                //if (hfsesstime.Value.ToString().Trim() == "")
                //{
                  string sessid = Session.SessionID + DateTime.UtcNow.ToFileTimeUtc();
                 //}


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TempInstallmentTransaction TIT = new TempInstallmentTransaction();
                    TIT.PaymentAmount = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                    TIT.ReceivedAmount = Convert.ToInt32(ds.Tables[0].Rows[i][1]);
                    TIT.InstallmentDate = Convert.ToDateTime(ds.Tables[0].Rows[i][2]);
                    TIT.LoanDisburseId = Convert.ToInt32(ds.Tables[0].Rows[i][3]);
                    TIT.TempSessionId = sessid;
                    cre.AddToTempInstallmentTransactions(TIT);
                    cre.SaveChanges();

                   installment.PaymentAmount = Convert.ToInt32(TIT.PaymentAmount);
                   installment.ReceivedAmount = Convert.ToInt32(TIT.ReceivedAmount);
                   installment.InstallmentDate = Convert.ToDateTime(TIT.InstallmentDate);
                   installment.tempid = TIT.TempSessionId;
                   installment.LoanDisburseId = Convert.ToInt32(TIT.LoanDisburseId);
                }
              
   
                //installmentTrans install = new installmentTrans();
                //install.file(ds);
               

            }

            var g = (from ins in cre.TempInstallmentTransactions



                     select new
                     {
                         ins.PaymentAmount,
                         ins.ReceivedAmount,
                         ins.InstallmentDate,
                         ins.LoanDisburseId
                     }).ToList();
            ViewData["a"] = g;
           return View();
         
        }
  
        public ActionResult Installment1()
        {
          
            return View();
        }



        public ActionResult upload()
        {
            installment install = new installment();
            install.upload();

            return View("Installment");

        }
        //--------------Darshana-------------------
        public ActionResult ViewEMIInstallment()
        {
            var strSql = (from ET in cre.EMITransactions                  
                          join LAT in cre.LoanApplicationTransactions on ET.LoanApplicationId equals LAT.Id
                          join IT in cre.InstallmentTransactions on ET.InstallmentId equals IT.Id
                          select new
                          {
                              ET.Id,
                              LAT.LoanApplicationNo,
                              IT.ReceivedAmount,
                              IT.PaidAmount,
                              IT.InstallmentDate,
                              ET.NoOfEMI,
                              ET.Pan,
                              ET.EMIAmount
                            
                          }
                           ).ToList();

            return View(strSql);
        }
    }
}
