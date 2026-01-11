using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Telerik.Web.UI;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SaintPolycarp.BanhChung.SharedObjectClass;
using SaintPolycarp.BanhChung.SharedObjectClass.RadGrid;
using SaintPolycarp.BanhChung.SharedMethods;
using Excel = Microsoft.Office.Interop.Excel;

namespace SaintPolycarp.BanhChung
{
    public partial class TrackingChecks : System.Web.UI.Page
    {
        private const string PHONE_FORMAT = "{0:(###) ###-####}";
        private const string CURRENCY_FORMAT = "$ {0:#,##0.00}";
        static List<SaleItem> saleItems = new List<SaleItem>();
        static List<OrderInvoice> orderInvoices = new List<OrderInvoice>();

        static List<RadGridMoneyTransfer> listOfMoneyTransfers = new List<RadGridMoneyTransfer>();
        public static DataTable dt;
        static double totalInvoiceAmount = 0;
        static double totalPaidByCashAmount = 0;
        static double totalPaidByCheckAmount = 0;
        static double totalPaid = 0;
        static double totalRemainBalance = 0;
  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if user hasn't login, then need to ask user to do so
                if (!User.Identity.IsAuthenticated)
                {
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    string OriginalUrl = HttpContext.Current.Request.RawUrl;
                    string LoginPageUrl = rootURL + "Account/Login.aspx";
                    HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                }
                else //check again, if user login but not financial admin, then reject as well.
                {
                    //if the current login user is not admin
                    if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "financial admin"))
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                    }
                }

                //only when first time load the form
                LoadInvoicesData();
            }
           

        }
        private void LoadInvoicesData()
        {
            try
            {
                //RadGridMoneyTransfers is the table that lists all the Order detail in this page
                listOfMoneyTransfers = new List<RadGridMoneyTransfer>();
                totalInvoiceAmount = 0;
                totalPaidByCashAmount = 0;
                totalPaidByCheckAmount = 0;
                totalPaid = 0;
                totalRemainBalance = 0;

                List<OrderInvoice> orderInvoices = SharedMethod.GetInvoices();
                //parse through each of the invoice
                foreach (OrderInvoice orderInvoice in orderInvoices)
                {
                    try
                    {
                        RadGridMoneyTransfer invoiceTransfer = new RadGridMoneyTransfer();

                        //************************************************************
                        //make sure to display the invoice that pay by check only
                        double checkAmount = 0;
                        if (!string.IsNullOrEmpty(orderInvoice.AlreadyPaidByCheck))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            if (orderInvoice.AlreadyPaidByCheck.StartsWith("$ "))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                checkAmount = Convert.ToDouble(orderInvoice.AlreadyPaidByCheck.Substring(2));
                            }
                            else if (orderInvoice.AlreadyPaidByCheck.StartsWith("$"))
                            {
                                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                checkAmount = Convert.ToDouble(orderInvoice.AlreadyPaidByCheck.Substring(1));
                            }
                            else
                            {
                                checkAmount = Convert.ToDouble(orderInvoice.AlreadyPaidByCheck);
                            }
                 
                            //if paid by check amount is zero, then continue on
                            if(checkAmount == 0)
                            {
                                continue;
                            }

                            //now if the process get here, which means we have some check, then display the check for simple type first, if there are anyway
                            if(orderInvoice.InvoiceType.ToUpper().Equals("SIMPLE"))
                            {
                                invoiceTransfer.CheckInfo = orderInvoice.CheckNumber;
                            }
                            //Complex type, read the check number from Order Table
                            else
                            {
                                //parse through the Order Table and get the check info
                                List<RadGridOrder> orders = orderInvoice.InvoiceOrders;
                                foreach(RadGridOrder order in orders)
                                {
                                    if(!string.IsNullOrEmpty(order.CheckNumber))
                                    {
                                        invoiceTransfer.CheckInfo += order.CheckNumber + ", ";
                                    }
                                }
                            }
                            //set invoice Type
                            invoiceTransfer.InvoiceType = orderInvoice.InvoiceType;
                            //set the notes
                            invoiceTransfer.Notes = orderInvoice.Notes;
                        }
                        //if no paid amount by check, then continue the loop and don't add the invoice into the table
                        else
                        {
                            continue;
                        }


                        //**********************************************************


                        invoiceTransfer.InvoiceNumber = orderInvoice.InvoiceNumber;
                        invoiceTransfer.InvoiceNumberWithLink = HttpUtility.HtmlDecode("&lt;a href='http://" + HttpContext.Current.Request.Url.Authority + "/Invoice.aspx?InvoiceNumber=" + orderInvoice.InvoiceNumber + "' target='_blank' title='Open invoice #" + orderInvoice.InvoiceNumber + "'  &gt; &lt;span style='color: white text-decoration: underline'&gt;" + orderInvoice.InvoiceNumber + "&lt;/span&gt; &lt;/ a &gt;");

                        if (orderInvoice.InvoiceType.ToUpper().Equals("SIMPLE"))
                        {
                            invoiceTransfer.CustomerName = orderInvoice.CustomerName;
                        }
                        //add a little "*" for complex invoice
                        else
                        {
                            invoiceTransfer.CustomerName = orderInvoice.CustomerName + " **";
                        }

                        if (!string.IsNullOrEmpty(invoiceTransfer.CreatedBy))
                        {
                            string[] createdBy = invoiceTransfer.CreatedBy.Split(new string[] { "on" }, StringSplitOptions.None);
                            if (createdBy.Count() > 0)
                            {
                                invoiceTransfer.CreatedBy = createdBy[0].Trim();
                            }

                        }

                        //the status has both pickup and paid status, "Full_Pickup, Partial_Paid". So have to parse and break to get 2 status separately
                        string[] status = orderInvoice.Status.Split(new char[] { ',' });
                        invoiceTransfer.PickupStatus = status[0].Trim(); //assuming the first index is Pickup Status
                        invoiceTransfer.PaidStatus = status[1].Trim(); //assuming the second index is the Paid Status

                        //set Invoice Amount. Then continue to add the amount into total amount
                        invoiceTransfer.InvoiceAmount = orderInvoice.GrandTotal;
                        try
                        {
                            double invoiceAmount = 0;
                            if (!string.IsNullOrEmpty(invoiceTransfer.InvoiceAmount))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (invoiceTransfer.InvoiceAmount.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    invoiceAmount = Convert.ToDouble(invoiceTransfer.InvoiceAmount.Substring(2));
                                }
                                else if (invoiceTransfer.InvoiceAmount.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                    invoiceAmount = Convert.ToDouble(invoiceTransfer.InvoiceAmount.Substring(1));
                                }
                                else
                                {
                                    invoiceAmount = Convert.ToDouble(invoiceTransfer.InvoiceAmount);
                                }
                                totalInvoiceAmount += invoiceAmount;

                            }
                        }
                        catch (Exception e)
                        {
                            //set the value to error
                            invoiceTransfer.InvoiceAmount = "<error>";
                        }


                        //set Paid By Cash. Then continue to add the amount into total Paid By Cash amount
                        invoiceTransfer.PaidByCash = orderInvoice.AlreadyPaid;
                        double paidByCashAmount = 0;
                        try
                        {
                           
                            if (!string.IsNullOrEmpty(invoiceTransfer.PaidByCash))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (invoiceTransfer.PaidByCash.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    paidByCashAmount = Convert.ToDouble(invoiceTransfer.PaidByCash.Substring(2));
                                }
                                else if (invoiceTransfer.PaidByCash.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                    paidByCashAmount = Convert.ToDouble(invoiceTransfer.PaidByCash.Substring(1));
                                }
                                else
                                {
                                    paidByCashAmount = Convert.ToDouble(invoiceTransfer.PaidByCash);
                                }
                                totalPaidByCashAmount += paidByCashAmount;

                            }
                        }
                        catch (Exception e)
                        {
                            //set the value to error
                            invoiceTransfer.PaidByCash = "<error>";
                        }

                        //set Paid By Check. Then continue to add the amount into total Paid By Cash amount
                        invoiceTransfer.PaidByCheck = orderInvoice.AlreadyPaidByCheck;
                        double paidByCheckAmount = 0;
                        try
                        {
                            
                            if (!string.IsNullOrEmpty(invoiceTransfer.PaidByCheck))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (invoiceTransfer.PaidByCheck.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    paidByCheckAmount = Convert.ToDouble(invoiceTransfer.PaidByCheck.Substring(2));
                                }
                                else if (invoiceTransfer.PaidByCheck.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                    paidByCheckAmount = Convert.ToDouble(invoiceTransfer.PaidByCheck.Substring(1));
                                }
                                else
                                {
                                    paidByCheckAmount = Convert.ToDouble(invoiceTransfer.PaidByCheck);
                                }
                                totalPaidByCheckAmount += paidByCheckAmount;

                            }
                        }
                        catch (Exception e)
                        {
                            //set the value to error
                            invoiceTransfer.PaidByCheck = "<error>";
                        }

                        //set Total Paid. Then continue to add the amount into total Paid
                        double paid = (paidByCashAmount + paidByCheckAmount);
                        invoiceTransfer.TotalPaid = string.Format(CURRENCY_FORMAT, paid);

                        totalPaid += paid;


                        //set the remain balance. 
                        invoiceTransfer.RemainBalance = orderInvoice.RemainBalance;
                        try
                        {
                            double remainBalance = 0;
                            if (!string.IsNullOrEmpty(invoiceTransfer.RemainBalance))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (invoiceTransfer.RemainBalance.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    remainBalance = Convert.ToDouble(invoiceTransfer.RemainBalance.Substring(2));
                                }
                                else if (invoiceTransfer.RemainBalance.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                    remainBalance = Convert.ToDouble(invoiceTransfer.RemainBalance.Substring(1));
                                }
                                else
                                {
                                    remainBalance = Convert.ToDouble(invoiceTransfer.RemainBalance);
                                }
                                totalRemainBalance += remainBalance;

                            }
                        }
                        catch (Exception e)
                        {
                            //set the value to error
                            invoiceTransfer.RemainBalance = "<error>";
                        }



                        listOfMoneyTransfers.Add(invoiceTransfer);


            
                    }
                    catch (Exception ex)
                    {
                        LiteralLogs.Text = "Exception in LoadData(). " + ex.Message;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadData() at invoice #" + orderInvoice.InvoiceNumber + ". " + ex.Message + "');", true);

                    }
                }

                //Flag if the total paid is NOT the same as the total paid by Cash and Check
                if(totalPaid != totalPaidByCashAmount + totalPaidByCheckAmount)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Calculation warning in LoadInvoicesData(). The Total paid amount is not the same as total paid by Cash and Check');", true);

                }

                //Flag if the remain balance is NOT the same as Invoice amount - total paid
                if (totalRemainBalance != totalInvoiceAmount - totalPaid)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Calculation warning in LoadInvoicesData(). The Total remain balance is not the same as the different of Total Invoice Amount and Total Paid');", true);

                }

                //set the radgrid data source
                RadGridMoneyTransfers.DataSource = listOfMoneyTransfers;
                RadGridMoneyTransfers.DataBind();

            }
            catch (Exception ex)
            {
                LiteralLogs.Text = "Exception in LoadDataException(). " + ex.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadInvoicesData(): " + ex.Message + "');", true);
            }
        }
        //from the radGridTable, parse through each of the row to calcuate each item for overall quantity, picked-up and not pick-up
   
        protected void RadGridMoneyTransfers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            RadGridMoneyTransfers.DataSource = listOfMoneyTransfers;

        }

        public class RadGridMoneyTransfer
        {
            public string Code { get; set; }
            public string InvoiceNumber { get; set; }
            public string InvoiceNumberWithLink { get; set; }
            public string CustomerName { get; set; }
            public string CreatedBy { get; set; }
            public string PickupStatus { get; set; }
            public string PaidStatus { get; set; }
            public string InvoiceAmount { get; set; }

            public string PaidByCash { get; set; }
            public string PaidByCheck { get; set; }
            public string TotalPaid { get; set; }
            public string RemainBalance { get; set; }
            public string InvoiceType {get; set;}
            public string CheckInfo { get; set; }
            public string Notes { get; set; }
        }

      }
   

}