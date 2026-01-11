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
using Newtonsoft.Json;
using SaintPolycarp.BanhChung.SharedObjectClass;
using SaintPolycarp.BanhChung.SharedObjectClass.RadGrid;
using SaintPolycarp.BanhChung.SharedMethods;
using Excel = Microsoft.Office.Interop.Excel;

namespace SaintPolycarp.BanhChung
{
    public partial class TrackingSheet : System.Web.UI.Page
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
        static double grandTotal = 0;

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
                LoadSaveData();
                UpdateDisplaySettings();
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

                     
                        if (!string.IsNullOrEmpty(orderInvoice.CreatedBy))
                        {
                            string[] createdBy = orderInvoice.CreatedBy.Split(new string[] { "on" }, StringSplitOptions.None);
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
        private void LoadSaveData()
        {
            try
            {
                Finance finance = SharedMethod.GetTrackingSheet();
                if (finance == null)
                    return;

                TextBoxNote.Text = finance.MainNotes;
                //traverse to get footer for the current column
                //since we only has 1 footer, therefore, it's safe to be at index 0
                GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                //start filling up the Footer first

                if (!string.IsNullOrEmpty(finance.LastUpdateBy))
                {
                    LabelLastUpdate.Text = "Last updated by " + finance.LastUpdateBy;
                    if (!string.IsNullOrEmpty(finance.LastUpdateOn))
                    {
                        LabelLastUpdate.Text += " on " + finance.LastUpdateOn;
                    }
                }

     

                //get the row of all the header
                GridHeaderItem headerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                double grandTotalCalculateFromHorizontalFooter = 0;
                //still load data to the footer, load the total amount for each Giao
                foreach (TotalAmountInEachGiao giao in finance.TotalAmountFromGiaoList)
                {

                    if (!string.IsNullOrEmpty(giao.Amount))
                    {
                        footerItem["Giao" + giao.GiaoNumber].Text = giao.Amount;
                        //get each giao vertically 
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        double giaoAmount = 0;
                        if (giao.Amount.StartsWith("$ "))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            giaoAmount = Convert.ToDouble(giao.Amount.Substring(2));
                        }
                        else if (giao.Amount.StartsWith("$"))
                        {
                            //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                            giaoAmount = Convert.ToDouble(giao.Amount.Substring(1));
                        }
                        else
                        {
                            giaoAmount = Convert.ToDouble(giao.Amount);
                        }
                        grandTotalCalculateFromHorizontalFooter += giaoAmount;
                    }
                    //load the checkbox in the header
                    CheckBox giaoCheckBox = headerItem.FindControl("Giao" + giao.GiaoNumber) as CheckBox;
                    giaoCheckBox.Checked = Convert.ToBoolean(giao.GiaoSelected);

                    //load giao date time
                    if (giao.GiaoDate != null)
                    {
                        RadDatePicker giaoDate = headerItem.FindControl("RadDatePickerInvoicePickupDate" + giao.GiaoNumber) as RadDatePicker;
                        giaoDate.SelectedDate = Convert.ToDateTime(giao.GiaoDate);
                    }

                    //disable the column if found the checkbox is check, it's include the header, the column and the footer
                    if (giaoCheckBox.Checked)
                    {
                        RadGridMoneyTransfers.MasterTableView.GetColumn("Giao" + giao.GiaoNumber).ItemStyle.BackColor = Color.FromArgb(Convert.ToInt32(giao.GiaoSelectedColor));

                        //parse through each row and look for the column of the same checkbox.ID=Giao1
                        //in this case, the ID of the checkbox is "TextBoxGiao1" to yellow
                        foreach (GridDataItem item in RadGridMoneyTransfers.MasterTableView.Items)
                        {
                            //the way our naming convention is that the checkboxID is the same as the text box
                            TextBox textbox = (TextBox)item.FindControl("Giao" + giao.GiaoNumber);
                            //textbox.BackColor = Color.FromArgb(Convert.ToInt32(giao.GiaoSelectedColor));
                            textbox.Enabled = false;
                        }
                        footerItem["Giao" + giao.GiaoNumber].BackColor = Color.FromArgb(Convert.ToInt32(giao.GiaoSelectedColor));
                    }
                }

                double grandTotalCalculateFromVerticalTotalColumn = 0;
                //now parse throu each invoice and fill in each of the row
                foreach (GridDataItem row in RadGridMoneyTransfers.MasterTableView.Items)
                {
                    string invoiceNumber = row["InvoiceNumber"].Text;

                    GiaoInvoice invoice = finance.Invoices.Find(x => x.Number.Equals(invoiceNumber));
                    if (invoice != null)
                    {
                        //traverse through each column in the row and fill in the amount for each giao
                        foreach (GiaoAmount giao in invoice.GiaoAmount)
                        {
                            TextBox giaoTextBox = row.FindControl("Giao" + giao.GiaoNumber) as TextBox;
                            if (!string.IsNullOrEmpty(giao.Amount))
                                giaoTextBox.Text = giao.Amount;
                        }

                        //fill in the total for giao for each row
                        if (!string.IsNullOrEmpty(invoice.GiaoTotalAmount))
                        {
                            TextBox giaoTotalTextBox = row.FindControl("TextBoxTotal") as TextBox;
                            giaoTotalTextBox.Text = invoice.GiaoTotalAmount;
                            double giaoAmount = Convert.ToDouble(invoice.GiaoTotalAmount.Substring(1)); //exclude the first 1 character to take out "$"

                            grandTotalCalculateFromVerticalTotalColumn += giaoAmount;
                            //update the Giao Status
                            UpdateValidation(row, giaoTotalTextBox, giaoAmount);


                        }

                        //fill in the notes
                        if (!string.IsNullOrEmpty(invoice.Notes))
                        {
                            TextBox noteTextBox = row.FindControl("TextBoxNotes") as TextBox;
                            noteTextBox.Text = invoice.Notes;
                        }
                    }

                }

                //set the GrandTotal Footer
                footerItem["Total"].Text = "Grand Total: " + string.Format(CURRENCY_FORMAT, grandTotalCalculateFromHorizontalFooter);
                footerItem["Validation"].Text = string.Format("Horizontal <br />{0}<br />Vertical<br />{1}", string.Format(CURRENCY_FORMAT, grandTotalCalculateFromHorizontalFooter), string.Format(CURRENCY_FORMAT, grandTotalCalculateFromVerticalTotalColumn));
                if (grandTotalCalculateFromVerticalTotalColumn != grandTotalCalculateFromHorizontalFooter)
                {
                    footerItem["Validation"].BackColor = Color.Red;
                    footerItem["Validation"].Text = "Unmatch <br />" + footerItem["Validation"].Text;

                }

            }
            catch (Exception ex)
            {
                LiteralLogs.Text = "Exception in LoadSaveData(). " + ex.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadSaveData()" + "." + ex.Message + "');", true);
            }

        }
        //from the radGridTable, parse through each of the row to calcuate each item for overall quantity, picked-up and not pick-up
        protected void Page_Init(object source, System.EventArgs e)
        {
        }

  
      
        protected void RadGridMoneyTransfers_FilterCheckListItemsRequested(object sender, Telerik.Web.UI.GridFilterCheckListItemsRequestedEventArgs e)
        {
            IEnumerable<RadGridMoneyTransfer> distinctList = new List<RadGridMoneyTransfer>();
            string dataField = (e.Column as IGridDataColumn).GetActiveDataField();

            //set the list view filter for each item
            if(dataField.ToUpper().Contains("INVOICENUMBER"))
            {
                //since listOfMoneyTransfers is the static variable
                //so it should have value by this point
                distinctList = listOfMoneyTransfers.GroupBy(x => x.InvoiceNumber).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctList = distinctList.Where(x => !string.IsNullOrEmpty(x.InvoiceNumber)).ToList();
            }
            else if (dataField.ToUpper().Contains("CUSTOMERNAME"))
            {
                //since listOfMoneyTransfers is the static variable
                //so it should have value by this point
                distinctList = listOfMoneyTransfers.GroupBy(x => x.CustomerName).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctList = distinctList.Where(x => !string.IsNullOrEmpty(x.CustomerName)).OrderBy(x => x.CustomerName).ToList();
            }
            else if (dataField.ToUpper().Contains("PICKUPSTATUS"))
            {
                //since listOfMoneyTransfers is the static variable
                //so it should have value by this point
                distinctList = listOfMoneyTransfers.GroupBy(x => x.PickupStatus).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctList = distinctList.Where(x => !string.IsNullOrEmpty(x.PickupStatus)).OrderBy(x => x.PickupStatus).ToList();
            }
            else if (dataField.ToUpper().Contains("PAIDSTATUS"))
            {
                //since listOfMoneyTransfers is the static variable
                //so it should have value by this point
                distinctList = listOfMoneyTransfers.GroupBy(x => x.PaidStatus).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctList = distinctList.Where(x => !string.IsNullOrEmpty(x.PaidStatus)).OrderBy(x => x.PaidStatus).ToList();
            }

            //set the RadGrid again
            e.ListBox.DataSource = distinctList;
            e.ListBox.DataKeyField = dataField;
            e.ListBox.DataTextField = dataField;
            e.ListBox.DataValueField = dataField;
            e.ListBox.DataBind();
        }

        protected void RadGridMoneyTransfers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    //turn on the checkbox if it is the first row
                    
           
                }
                else if (e.Item is GridFooterItem)
                {
                    GridFooterItem footerItem = e.Item as GridFooterItem;
                 
       
                }

            }
            catch (Exception ex)
            {
           
            }

        }
        bool isFilterOrSort = false;
        protected void RadGridMoneyTransfers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if(e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.SortCommandName)
            {
                //chau note: need to set this for PreRender method when filtering or sort
                isFilterOrSort = true;
                
            }
            else
            {
                isFilterOrSort = false;
            }
        
        }

        protected void RadGridMoneyTransfers_PreRender(object sender, EventArgs e)
        {
            //Chau note: this filter or sort is already set in ItemCommand method
            if(isFilterOrSort)
            {
                ReCalculateTotalWhenFilterOrSort();
            }

            GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            //set the footer for Invoice amount and Paid amount
            footerItem["InvoiceAmount"].Text = string.Format(CURRENCY_FORMAT, totalInvoiceAmount);
            footerItem["PaidByCash"].Text = string.Format(CURRENCY_FORMAT, totalPaidByCashAmount);
            footerItem["PaidByCheck"].Text = string.Format(CURRENCY_FORMAT, totalPaidByCheckAmount);
            footerItem["TotalPaid"].Text = string.Format(CURRENCY_FORMAT, totalPaid);
            footerItem["RemainBalance"].Text = string.Format(CURRENCY_FORMAT, totalRemainBalance);

        }
   
        private void ReCalculateTotalWhenFilterOrSort()
        {
            try
            {
                totalInvoiceAmount = 0;
                totalPaidByCashAmount = 0;
                totalPaidByCheckAmount = 0;
                foreach (GridDataItem item in RadGridMoneyTransfers.MasterTableView.Items)
                {
                    try
                    {
                        double invoiceAmount = 0;
                        if (!string.IsNullOrEmpty(item["InvoiceAmount"].Text))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            if (item["InvoiceAmount"].Text.StartsWith("$ "))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                invoiceAmount = Convert.ToDouble(item["InvoiceAmount"].Text.Substring(2));
                            }
                            else if (item["InvoiceAmount"].Text.StartsWith("$"))
                            {
                                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                invoiceAmount = Convert.ToDouble(item["InvoiceAmount"].Text.Substring(1));
                            }
                            else
                            {
                                invoiceAmount = Convert.ToDouble(item["InvoiceAmount"].Text);
                            }
                            totalInvoiceAmount += invoiceAmount;

                        }
                    }
                    catch (Exception e)
                    {
                        //set the value to error
                     //   invoiceTransfer.InvoiceAmount = "<error>";
                    }


                    try
                    {
                        double paidByCashAmount = 0;
                        if (!string.IsNullOrEmpty(item["PaidByCash"].Text))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            if (item["PaidByCash"].Text.StartsWith("$ "))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                paidByCashAmount = Convert.ToDouble(item["PaidByCash"].Text.Substring(2));
                            }
                            else if (item["PaidByCash"].Text.StartsWith("$"))
                            {
                                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                paidByCashAmount = Convert.ToDouble(item["PaidByCash"].Text.Substring(1));
                            }
                            else
                            {
                                paidByCashAmount = Convert.ToDouble(item["PaidByCash"].Text);
                            }
                            totalPaidByCashAmount += paidByCashAmount;

                        }
                    }
                    catch (Exception e)
                    {
                        //set the value to error
                       // invoiceTransfer.PaidByCash = "<error>";
                    }

                    try
                    {
                        double paidByCheckAmount = 0;
                        if (!string.IsNullOrEmpty(item["PaidByCheck"].Text))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            if (item["PaidByCheck"].Text.StartsWith("$ "))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                paidByCheckAmount = Convert.ToDouble(item["PaidByCheck"].Text.Substring(2));
                            }
                            else if (item["PaidByCheck"].Text.StartsWith("$"))
                            {
                                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                paidByCheckAmount = Convert.ToDouble(item["PaidByCheck"].Text.Substring(1));
                            }
                            else
                            {
                                paidByCheckAmount = Convert.ToDouble(item["PaidByCheck"].Text);
                            }
                            totalPaidByCheckAmount += paidByCheckAmount;

                        }
                    }
                    catch (Exception e)
                    {
                        //set the value to error
                        // invoiceTransfer.PaidByCheck = "<error>";
                    }

                }


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in CalculateTotal(). " + ex.Message + "');", true);

            }
        }
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
        }

        protected void TextBoxGiao_TextChanged(object sender, EventArgs e)
        {
            
           //Calculate total
            TextBox changedTextBox = (TextBox)sender;
            GridDataItem currentRow = (GridDataItem)changedTextBox.NamingContainer;

            //now parse through each of the textbox control which ID name is "Giao" and sum them up
            double totalGiaoForThisInvoice = 0;
            TextBox totalGiaoForThisInvoiceText = currentRow.FindControl("TextBoxTotal") as TextBox;

            //Every time when the text box is change, it need to update 3 numbers: the total giao at Vertical, the total at Horizontal and the GrandTotal

            //Update the first number, the total at Horizontal
            //**************** Now calculate Horizonatally for the Total in the same row ***************************
            for (int giaoIndex = 1; giaoIndex <= 10; giaoIndex ++)
            {
                double giao = 0;
                //because horizontally, for each row, get each of the text box in the same row
                TextBox giaoTextBox = currentRow.FindControl("Giao" + giaoIndex.ToString()) as TextBox;
                try
                {
                    if(!string.IsNullOrEmpty(giaoTextBox.Text))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        if (giaoTextBox.Text.StartsWith("$ "))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            giao = Convert.ToDouble(giaoTextBox.Text.Substring(2));
                        }
                        else if (giaoTextBox.Text.StartsWith("$"))
                        {
                            //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                            giao = Convert.ToDouble(giaoTextBox.Text.Substring(1));
                        }
                        else
                        {
                            giao = Convert.ToDouble(giaoTextBox.Text);
                        }
                        totalGiaoForThisInvoice += giao;
                        giaoTextBox.Text = string.Format(CURRENCY_FORMAT, giao);
                        giaoTextBox.BackColor = default(Color);
                    }
                }
                catch(Exception ex)
                {
                    //don't do anything, just use to catch the conversion to double
                    giaoTextBox.Text = "try again";
                    giaoTextBox.BackColor = Color.Red;
                }
            }
            //set the total giao for this invoice
            totalGiaoForThisInvoiceText.Text = string.Format(CURRENCY_FORMAT, totalGiaoForThisInvoice);
            //update the status of this invoice after getting the total giao in this invoice
            UpdateValidation(currentRow, totalGiaoForThisInvoiceText, totalGiaoForThisInvoice);

            //parse through each row and look for the column of the same checkbox.ID=Giao1
            //in this case, the ID of the checkbox is "TextBoxGiao1" to yellow
            double totalForEachGiao = 0;
            double grandTotal = 0;

            foreach (GridDataItem row in RadGridMoneyTransfers.MasterTableView.Items)
            {
            
                //because vertically, then for each row, only look at the column that has the text box
                TextBox giaoInvoiceForChagedColumn = row.FindControl(changedTextBox.ID) as TextBox;

                try
                {
                    //Update the 2nd number, the total at vertical
                    //**************** Now calculate vertically for the Total Giao in the Footer ***************************

                    double giao = 0;
                    if (!string.IsNullOrEmpty(giaoInvoiceForChagedColumn.Text))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        if (giaoInvoiceForChagedColumn.Text.StartsWith("$ "))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            giao = Convert.ToDouble(giaoInvoiceForChagedColumn.Text.Substring(2));
                        }
                        else if (giaoInvoiceForChagedColumn.Text.StartsWith("$"))
                        {
                            //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                            giao = Convert.ToDouble(giaoInvoiceForChagedColumn.Text.Substring(1));
                        }
                        else
                        {
                            giao = Convert.ToDouble(giaoInvoiceForChagedColumn.Text);
                        }
                        totalForEachGiao += giao;
                    
                    }

                }
                catch (Exception ex)
                {
                  
                }

                //Update the 3rd number, the Grand Total
                //**************** Now calculate vertically for the GRAND Total Giao in the Footer ***************************

                //if the current row is the same row that being changed, then user the Total above
                if (row.DataSetIndex == currentRow.DataSetIndex)
                {
                    grandTotal += totalGiaoForThisInvoice;
                }
                else
                {
                    //get the Total in each row
                    TextBox totalGiaoForEachInvoiceText = row.FindControl("TextBoxTotal") as TextBox;
                    try
                    {
                        double totalGiaoForEachInvoice = 0;
                        if (!string.IsNullOrEmpty(totalGiaoForEachInvoiceText.Text))
                        {
                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            if (totalGiaoForEachInvoiceText.Text.StartsWith("$ "))
                            {
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                totalGiaoForEachInvoice = Convert.ToDouble(totalGiaoForEachInvoiceText.Text.Substring(2));
                            }
                            else if (totalGiaoForEachInvoiceText.Text.StartsWith("$"))
                            {
                                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                                totalGiaoForEachInvoice = Convert.ToDouble(totalGiaoForEachInvoiceText.Text.Substring(1));
                            }
                            else
                            {
                                totalGiaoForEachInvoice = Convert.ToDouble(totalGiaoForEachInvoiceText.Text);
                            }
                            grandTotal += totalGiaoForEachInvoice;

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            //traverse to get footer for the current column
            //since we only has 1 footer, therefore, it's safe to be at index 0
            GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

            //the way we name our naming convention for controls is that, the column name is the same name as the text box or checkbox "Giao1"
            string columnName = changedTextBox.ID;
            //set the TotalGiao, which is the 2nd number that need to change every time
            footerItem[columnName].Text = string.Format(CURRENCY_FORMAT, totalForEachGiao);


            //set the GrandTotal, which is the 3rd number that need to change every time
            footerItem["Total"].Text = "Grand Total: " + string.Format(CURRENCY_FORMAT, grandTotal);
            footerItem["Validation"].Text = "";
            footerItem["Validation"].BackColor = default(Color);

        }

        private void UpdateValidation(GridDataItem row, TextBox giaoTotalTextBox, double giaoAmount )
        {
            //get paid by cash amount
            double paidByCashAmount = 0;
            try
            {
                if (!string.IsNullOrEmpty(row["PaidByCash"].Text))
                {
                    paidByCashAmount = Convert.ToDouble(row["PaidByCash"].Text.Substring(2)); //exclude the first 2 characters to take out "$ "
                }
            }
            catch (Exception ex)
            {
                //doing nothing since the amount=0 anyway
            }

            //get paid by check amount
            double paidByCheckAmount = 0;
            try
            {
                if (!string.IsNullOrEmpty(row["PaidByCheck"].Text))
                {
                    paidByCheckAmount = Convert.ToDouble(row["PaidByCheck"].Text.Substring(2)); //exclude the first 2 characters to take out "$ "
                }
            }
            catch (Exception ex)
            {
                //doing nothing since the amount=0 anyway
            }

            //get paid by check amount
            double invoiceAmount = 0;
            try
            {
                if (!string.IsNullOrEmpty(row["InvoiceAmount"].Text))
                {
                    invoiceAmount = Convert.ToDouble(row["InvoiceAmount"].Text.Substring(2)); //exclude the first 2 characters to take out "$ "
                }
            }
            catch (Exception ex)
            {
                //doing nothing since the amount=0 anyway
            }

            double paidAmount = paidByCashAmount + paidByCheckAmount;
            
            //compare the paid amount with the total at horizonal, if it's equal, then mark the background to green
            if (paidAmount == giaoAmount && paidAmount == invoiceAmount)
            {
                row["Validation"].Text = "Completed";
                giaoTotalTextBox.BackColor = Color.Green;
                giaoTotalTextBox.ForeColor = Color.White;
            }
            else if (paidAmount > 0)
            {
                if (paidAmount == giaoAmount && paidAmount != invoiceAmount)
                {
                    row["Validation"].Text = "Paid == Giao<br />Paid <> Invoice";
                    giaoTotalTextBox.BackColor = Color.Orange;
                    giaoTotalTextBox.ForeColor = Color.White;
                }
                else if (paidAmount != giaoAmount && paidAmount == invoiceAmount)
                {
                    row["Validation"].Text = "Paid <> Giao<br />Paid == Invoice";
                    giaoTotalTextBox.BackColor = Color.Blue;
                    giaoTotalTextBox.ForeColor = Color.White;
                }
            }
        }

        private void UpdateDisplaySettings()
        {
            if (CheckBox1.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao1").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao1").Display = false;
            }
            if (CheckBox2.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao2").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao2").Display = false;
            }
            if (CheckBox3.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao3").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao3").Display = false;
            }
            if (CheckBox4.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao4").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao4").Display = false;
            }
            if (CheckBox5.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao5").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao5").Display = false;
            }
            if (CheckBox6.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao6").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao6").Display = false;
            }
            if (CheckBox7.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao7").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao7").Display = false;
            }
            if (CheckBox8.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao8").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao8").Display = false;
            }
            if (CheckBox9.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao9").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao9").Display = false;
            }
            if (CheckBox10.Checked)
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao10").Display = true;
            }
            else
            {
                RadGridMoneyTransfers.MasterTableView.GetColumn("Giao10").Display = false;
            }
            if (CheckBoxExpandWidth.Checked)
            {
                RadGridMoneyTransfers.Width = Unit.Percentage(120);
            }
            else
            {
                RadGridMoneyTransfers.Width = Unit.Percentage(100);
            }
            

        }
        protected void CheckBoxGiao_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;

            //the column is check, which mean it's disabled from editing and yello
            if (checkbox.Checked)
            {
                //For example, checkbox.ID=Giao1, therefore make the column name Giao1 to be yellow
                //the way we name our naming convention for controls is that, the column name is the same name as the text box or checkbox "Giao1"
                string columnName = checkbox.ID;

                Random r = new Random();
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                RadGridMoneyTransfers.MasterTableView.GetColumn(columnName).ItemStyle.BackColor = randomColor;

                //parse through each row and look for the column of the same checkbox.ID=Giao1
                //in this case, the ID of the checkbox is "TextBoxGiao1" to yellow
                foreach (GridDataItem item in RadGridMoneyTransfers.MasterTableView.Items)
                {
                    //the way our naming convention is that the checkboxID is the same as the text box
                    TextBox textbox = (TextBox)item.FindControl(checkbox.ID);
                    //textbox.BackColor = randomColor;
                    textbox.Enabled = false;
                }

                //traverse to get footer for the current column
                //since we only has 1 footer, therefore, it's safe to be at index 0
                GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                footerItem[checkbox.ID].BackColor = randomColor;
               
            }
            else
            {
                //For example, checkbox.ID=Giao1, therefore make the column name Giao1 back to default background
                RadGridMoneyTransfers.MasterTableView.GetColumn(checkbox.ID).ItemStyle.BackColor = default(Color);

                //parse through each row and look for the column of the same checkbox.ID=Giao1
                //in this case, the ID of the checkbox is "TextBoxGiao1" to default color
                foreach (GridDataItem item in RadGridMoneyTransfers.MasterTableView.Items)
                {
                    //the way our naming convention is that the checkboxID is the same as the text box
                    TextBox textbox = (TextBox)item.FindControl(checkbox.ID);
                    textbox.BackColor = default(Color);
                    textbox.Enabled = true;
                }

                //traverse to get footer for the current column
                //since we only has 1 footer, therefore, it's safe to be at index 0
                GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                footerItem[checkbox.ID].BackColor = default(Color);

            }



        }

        private void SaveFinance()
        {
            try
            {
            
                //traverse to get footer for the current column
                //since we only has 1 footer, therefore, it's safe to be at index 0
                GridFooterItem footerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

                Finance finance = new Finance();

                finance.MainNotes = TextBoxNote.Text;
                finance.LastUpdateBy = User.Identity.Name;
                finance.LastUpdateOn = DateTime.Now.ToString("MM/dd HH:mm");

                
                //get the row of all the header
                GridHeaderItem headerItem = RadGridMoneyTransfers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                //get each item from Giao in the footer
                List<TotalAmountInEachGiao> totalAmountList = new List<TotalAmountInEachGiao>();
                for (int i=1; i <= 10; i++)
                {

                    CheckBox giaoCheckBox = headerItem.FindControl("Giao" + i.ToString()) as CheckBox;
                    RadDatePicker giaoDate = headerItem.FindControl("RadDatePickerInvoicePickupDate" + i.ToString()) as RadDatePicker;

                    string amount = footerItem["Giao" + i.ToString()].Text;
                    if (!(string.IsNullOrEmpty(amount) || amount.Equals("&nbsp;")))
                    {
                        TotalAmountInEachGiao tAmount = new TotalAmountInEachGiao();
                        tAmount.GiaoNumber = i.ToString();
                        tAmount.Amount = amount;
                        tAmount.GiaoSelected = giaoCheckBox.Checked.ToString();
                        tAmount.GiaoSelectedColor = RadGridMoneyTransfers.MasterTableView.GetColumn("Giao" + i.ToString()).ItemStyle.BackColor.ToArgb().ToString();
                        if (giaoDate.SelectedDate != null)
                        {
                            tAmount.GiaoDate = giaoDate.SelectedDate.ToString();
                        }
                        totalAmountList.Add(tAmount);
                    }

                    
                }
                finance.TotalAmountFromGiaoList = totalAmountList;


                //parse through each row and look for the column with the index of 1 to 10, for example Giao1
                //in this case, the ID of the checkbox is "TextBoxGiao1" to yellow
                List<GiaoInvoice> invoices = new List<GiaoInvoice>();
                foreach (GridDataItem row in RadGridMoneyTransfers.MasterTableView.Items)
                {
                    GiaoInvoice inv = new GiaoInvoice();
                    inv.Number = row["InvoiceNumber"].Text;

                    TextBox giaoTotalTextBox = row.FindControl("TextBoxTotal") as TextBox;
                    inv.GiaoTotalAmount = giaoTotalTextBox.Text;

                    TextBox notesTextBox = row.FindControl("TextBoxNotes") as TextBox;
                    inv.Notes = notesTextBox.Text;

                    List<GiaoAmount> giaoAmounts = new List<GiaoAmount>();

                    for (int i = 1; i <= 10; i++)
                    {
                        TextBox giaoTextBox = row.FindControl("Giao" + i.ToString()) as TextBox;
                        if(!(string.IsNullOrEmpty(giaoTextBox.Text) || giaoTextBox.Text.Equals("&nbsp;")))
                        {
                            GiaoAmount gAmount = new GiaoAmount();
                            gAmount.GiaoNumber = i.ToString();
                            gAmount.Amount = giaoTextBox.Text;
                            giaoAmounts.Add(gAmount);
                        }
                    }
                    inv.GiaoAmount = giaoAmounts;
                   
                    invoices.Add(inv);
                }
                finance.Invoices = invoices;

                //save finance
                SharedMethod.SaveTrackingSheet(finance);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('Data saved.');", true);


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SaveFinance()" + "." + ex.Message + "');", true);
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFinance();
            LoadSaveData();
        }

     
        protected void RadDatePickerInvoicePickupDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {

        }

        protected void ButtonExportXls_Click(object sender, EventArgs e)
        {
            //RadGridMoneyTransfers.ExportSettings.Excel.Format = GridExcelExportFormat.Html;
            RadGridMoneyTransfers.ExportSettings.FileName = "TrackingSheet";
            RadGridMoneyTransfers.ExportSettings.ExportOnlyData = true;
            RadGridMoneyTransfers.ExportSettings.OpenInNewWindow = true;
            RadGridMoneyTransfers.MasterTableView.ExportToExcel();
        }
        protected void ButtonExportXlsx_Click(object sender, EventArgs e)
        {
            RadGridMoneyTransfers.MasterTableView.GetColumn("InvoiceNumber").Display = true;
            RadGridMoneyTransfers.MasterTableView.GetColumn("InvoiceNumberWithLink").Display = false;
            RadGridMoneyTransfers.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
            RadGridMoneyTransfers.ExportSettings.FileName = "TrackingSheet";
            RadGridMoneyTransfers.ExportSettings.ExportOnlyData = true;
            RadGridMoneyTransfers.ExportSettings.OpenInNewWindow = true;
            RadGridMoneyTransfers.MasterTableView.ExportToExcel();
        }
        protected void RadGrid_HTMLExporting(object sender, GridHTMLExportingEventArgs e)
        {
            e.Styles.Append("body { border:solid 0.1pt #CCCCCC; }");
        }

        protected void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDisplaySettings();
        }
    }
   

}