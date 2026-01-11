using System;
using System.Collections.Generic;
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
using SaintPolycarp.BanhChung.SharedConstansts;

namespace SaintPolycarp.BanhChung
{
    public partial class InvoiceAdmin : System.Web.UI.Page
    {

        private string identity = "";
        private const string CURRENCY_FORMAT = "$ {0:#,##0.00}";
        private const int NUMBER_OF_ORDER = 10;
        //this need to defined as static because we don't need to get the value of this variable all the time.
        //it only set while the page is not in Post Back 
        static List<RadGridSaleItem> listOfRadGridSaleItems = new List<RadGridSaleItem>();
        static List<RadGridOrder> listOfRadGridOrders = new List<RadGridOrder>();

        static List<SaleItem> saleItems = new List<SaleItem>();
        static DataTable saleItemDataTable = new DataTable();
        static List<string> orderNumber = new List<string>();

        private enum InvoicePickupStatusEnum { Not_Pickup, Partial_Pickup, Full_Pickup, Cancelled };
        private enum InvoicePaidStatusEnum { Unpaid, Partial_Paid, Paid, Cancelled };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    string OriginalUrl = HttpContext.Current.Request.RawUrl;
                    string LoginPageUrl = rootURL + "Account/Login.aspx";
                    HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                }


                InitialData();
                TextBoxInvoiceStatus.Text = InvoicePickupStatusEnum.Not_Pickup.ToString() + ", " + InvoicePaidStatusEnum.Unpaid.ToString(); //need to use this instead of using SetInvoiceStatus()
                TextBoxInvoiceDate.Text = DateTime.Now.Date.ToShortDateString();

                string invoiceNumber = Request.QueryString["invoicenumber"];
                string isJustCreated = Request.QueryString["IsJustCreated"];

                //if this is loading an existing Invoice, then load this information.
                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    if (!string.IsNullOrEmpty(isJustCreated))
                    {
                        if (isJustCreated.ToUpper().Equals("TRUE"))
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('New invoice " + invoiceNumber + " has created." + "');", true);
                        }
                    }
                    LoadExistingInvoice(invoiceNumber);
                   
                }
       
            }
            if (string.IsNullOrEmpty(RadMaskedTextBoxPhoneNumber.Text))
            {
                LabelSendTextReminder.Visible = false;
            }
            else
            {
                LabelSendTextReminder.Visible = true;
            }

            //handling popup for reminder
            string title = "Send the following SMS text message to " + RadTextBoxCustomerName.Text;
            string invoiceNum = TextBoxInvoiceNumber.Text;
            LabelSendTextReminder.Text = HttpUtility.HtmlDecode("&lt;a href='#' title='Send SMS Text Reminder'  onclick=\"openRadWin_PopupSendTextReminder('" + title + "','" + RadMaskedTextBoxPhoneNumber.Text + "','" + invoiceNum + "')\" &gt; Text Reminder &lt;/a&gt;");

        }

        private void InitialData()
        {
            TextBoxCreatedBy.Text = Context.User.Identity.Name + " on desktop";



            //**************Initial Sale Items
            saleItems = SharedMethod.GetSaleItems();


            //****************** Load Initial Order Number
            //convert into DataTable. It will be use in ItemDataBound
            saleItemDataTable = NewTable("SaleItemTable", saleItems);

            for (int i = 1; i <= NUMBER_OF_ORDER; i++)
            {
                orderNumber.Add("#" + i.ToString());
            }

            //prepopulate with empty row
            listOfRadGridSaleItems = new List<RadGridSaleItem>();
            for (int i = 1; i <= NUMBER_OF_ORDER * 2; i++)
            {
                RadGridSaleItem saleItem = new RadGridSaleItem();
                saleItem.Index = i.ToString();
                listOfRadGridSaleItems.Add(saleItem);
            }
            RadGridSaleItems.Rebind();

            listOfRadGridOrders = new List<RadGridOrder>();
            for (int i = 1; i <= NUMBER_OF_ORDER; i++)
            {
                RadGridOrder order = new RadGridOrder();
                order.OrderNumber = "#" + i.ToString();
                listOfRadGridOrders.Add(order);
            }
            RadGridOrders.Rebind();


        }
        //convert that list of json to RadGrid list to display on the table


        //********************** RAD GRID ****************************************************************

        protected void RadGridSaleItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridSaleItems.DataSource = listOfRadGridSaleItems;
        }
        protected void RadGridSaleItems_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    // access/modify the edit item template settings here
                    DropDownList saleItemDropDown = item.FindControl("DropDownListSaleItem") as DropDownList;

                    //saleItemDataTable already loaded in the InitialzedData
                    saleItemDropDown.DataSource = saleItemDataTable;
                    saleItemDropDown.DataTextField = "name";
                    saleItemDropDown.DataValueField = "name";
                    saleItemDropDown.DataBind();

                    DropDownList orderNumberDropDown = item.FindControl("DropDownListOrderNumber") as DropDownList;
                    orderNumberDropDown.DataSource = orderNumber;
                    orderNumberDropDown.DataBind();

                }
            }
            catch (Exception ex)
            {
                string error = String.Format("Serve Error when calling RadGridSaleItems ItemDataBound().");
                error += String.Format("<br />Further info: exception: {0}", ex.Message);
                if (ex.InnerException != null)
                    error += String.Format("<br />Further info: inner exception: {0}", ex.InnerException.Message);
                LabelServeError.Text = error;
            }




        }


        protected void RadGridOrders_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridOrders.DataSource = listOfRadGridOrders;
        }
        protected void RadGridOrders_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    // access/modify the edit item template settings here
                    //item["OrderNumber"]
                    //when initial, only set the date time for Order #1 which is the current date time
                    if (item["OrderNumber"].Text.Equals("#1"))
                    {
                        RadDatePicker saleItemDropDown = item.FindControl("RadDatePickerPickupDate") as RadDatePicker;
                        saleItemDropDown.SelectedDate = DateTime.Now.Date;

                    }

                }
            }
            catch (Exception ex)
            {
                string error = String.Format("Serve Error when calling RadGridOrders ItemDataBound().");
                error += String.Format("<br />Further info: exception: {0}", ex.Message);
                if (ex.InnerException != null)
                    error += String.Format("<br />Further info: inner exception: {0}", ex.InnerException.Message);
                LabelServeError.Text = error;
            }
        }

        //********************** END RAD GRID ****************************************************************


        public DataTable NewTable<T>(string name, IEnumerable<T> list)
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            DataTable table = Table<T>(name, list, propInfo);
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
            return table;
        }
        public DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
        {
            foreach (PropertyInfo p in pi)
                row[p.Name.ToString()] = p.GetValue(listItem, null);
            return row;
        }
        private static DataTable Table<T>(string name, IEnumerable<T> list, PropertyInfo[] pi)
        {
            DataTable table = new DataTable(name);
            foreach (PropertyInfo p in pi)
                table.Columns.Add(p.Name, p.PropertyType);
            return table;
        }




        protected void RadDatePickerPickupDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            try
            {
                RadDatePicker pickupDateInOrdersTable = (RadDatePicker)sender;
                GridDataItem item = (GridDataItem)pickupDateInOrdersTable.NamingContainer;

                //Parse through all the item in SaleItems, if the Order Number is the same as the Order Number the current row in Orders, then update the PickupDate
                foreach (GridDataItem saleItem in RadGridSaleItems.MasterTableView.Items)
                {
                    DropDownList dropdownListSaleItem = (DropDownList)saleItem.FindControl("DropDownListSaleItem");
                    DropDownList dropdownListOrderNumber = (DropDownList)saleItem.FindControl("DropDownListOrderNumber");

                    //if there is a sale item selected
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        //if the order number is the same as the current Order Number
                        if (dropdownListOrderNumber.SelectedValue.Equals(item["OrderNumber"].Text))
                        {
                            //set the pickup date time
                            saleItem["PickupDate"].Text = Convert.ToDateTime(pickupDateInOrdersTable.SelectedDate).ToShortDateString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in RadDatePickerPickupDate_SelectedDateChanged(): " + ex.Message + "');", true);
            }

        }
        protected void RadDatePickerInvoicePickupDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            try
            {
                foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
                {
                    DropDownList dropdownSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");

                    //if select item is not null or empty, then there must be an item selected. Then enable other field in the same row
                    if (!string.IsNullOrEmpty(dropdownSaleItem.SelectedValue))
                    {
                        SaleItem selectedSaleItem = saleItems.First(x => x.name.Equals(dropdownSaleItem.SelectedValue));
                        if (selectedSaleItem != null)
                        {
                            if (RadDatePickerInvoicePickupDate.SelectedDate != null)
                            {
                                //set the  pickup date
                                item["PickupDate"].Text = Convert.ToDateTime(RadDatePickerInvoicePickupDate.SelectedDate).ToShortDateString();
                            }
                            else
                            {
                                //not selected
                                item["PickupDate"].Text = "Not Selected";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in RadDatePickerInvoicePickupDate_SelectedDateChanged(): " + ex.Message + "');", true);
            }

        }





        protected void DropDownListTakeOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void DropDownListSaleItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList dropdownSaleItem = (DropDownList)sender;
                GridDataItem item = (GridDataItem)dropdownSaleItem.NamingContainer;
                TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");

                ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
                ImageButton imageButtonNotPickedup = (ImageButton)item.FindControl("ImageButtonNotPickedUp");

                //if select item is not null or empty, then there must be an item selected. Then enable other field in the same row
                if (!string.IsNullOrEmpty(dropdownSaleItem.SelectedValue))
                {
                    item.Selected = true;
                    SaleItem selectedSaleItem = saleItems.First(x => x.name.Equals(dropdownSaleItem.SelectedValue));
                    if (selectedSaleItem != null)
                    {
                        textboxDiscount.Enabled = true;
                        textboxQuantity.Enabled = true;
                        dropdownListOrderNumber.Enabled = true;
                        imageButtonNotPickedup.Visible = true;
                        imageButtonNotPickedup.Enabled = true;
                        imageButtonPickedup.Visible = false;
                        imageButtonPickedup.Enabled = false;

                        dropdownSaleItem.BackColor = Color.Orange;
                        dropdownListOrderNumber.BackColor = Color.Orange;
                        textboxQuantity.BackColor = Color.Orange;
                        textboxDiscount.BackColor = Color.Orange;

                        //if the Quantity is empty, then default it to be 1
                        if (string.IsNullOrEmpty(textboxQuantity.Text))
                        {
                            textboxQuantity.Text = "1";
                        }
                        //if the Discount is empty, then default it to be 0
                        if (string.IsNullOrEmpty(textboxDiscount.Text))
                        {
                            textboxDiscount.Text = "$ 0.00";
                        }
                        textboxUnitPrice.Text = string.Format(CURRENCY_FORMAT, double.Parse(selectedSaleItem.saleprice));


                        //therefore, set SubTotal to be the same as Unit Price
                        textboxSubTotal.Text = string.Format(CURRENCY_FORMAT, double.Parse(selectedSaleItem.saleprice));

                        //if Invoice Simple Type
                        if (RadioButtonListInvoiceType.SelectedIndex == 0)
                        {
                            UpdateBalancesWhenChangesInSaleItemsTableForSimpleType(item["Index"].Text, textboxSubTotal.Text);
                            TextBoxTotalAlreadyPaid.Enabled = true;
                            TextBoxTotalAlreadyPaid.BackColor = Color.Orange;

                            TextBoxTotalAlreadyPaidByCheck.Enabled = true;
                            TextBoxTotalAlreadyPaidByCheck.BackColor = Color.Orange;


                        }
                        //if Invoice Complex Type
                        else
                        {
                            //refresh the Order Table with the udpated values and other Balances
                            UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTableForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, textboxSubTotal.Text);
                            TextBoxTotalAlreadyPaid.Enabled = false;
                            TextBoxTotalAlreadyPaid.BackColor = default(Color);

                            TextBoxTotalAlreadyPaidByCheck.Enabled = false;
                            TextBoxTotalAlreadyPaidByCheck.BackColor = default(Color);
                        }

                        //update  Pickup Date
                        UpdatePickupDateInSaleItems(dropdownListOrderNumber.SelectedValue, item);
                        SyncPickupStatus(item["Index"].Text, false);
                        //display the summary panel
                        PanelSummary.Visible = true;
                        PanelSummaryBalance.Visible = true;
                    }

                }
                //if clear and don't pick any Sale item, then clear everything
                else
                {
                    textboxQuantity.Enabled = false;
                    textboxDiscount.Enabled = false;
                    dropdownListOrderNumber.Enabled = false;
                    imageButtonNotPickedup.Visible = false;
                    imageButtonNotPickedup.Enabled = false;
                    imageButtonPickedup.Visible = false;
                    imageButtonPickedup.Enabled = false;

                    textboxQuantity.Text = string.Empty;
                    textboxUnitPrice.Text = string.Empty;
                    textboxDiscount.Text = string.Empty;
                    textboxSubTotal.Text = string.Empty;

                    dropdownSaleItem.BackColor = default(Color);
                    dropdownListOrderNumber.BackColor = default(Color);
                    textboxQuantity.BackColor = default(Color);
                    textboxDiscount.BackColor = default(Color);
                    item.BackColor = default(Color);

                    dropdownListOrderNumber.SelectedIndex = 0;
                    textboxUnitPrice.Text = string.Empty;
                    textboxQuantity.Text = string.Empty;
                    textboxDiscount.Text = string.Empty;
                    textboxSubTotal.Text = string.Empty;
                    item["PickupDate"].Text = "";

                    //if invoice type is Simple
                    if (RadioButtonListInvoiceType.SelectedIndex == 0)
                    {
                        UpdateBalancesWhenChangesInSaleItemsTableForSimpleType(item["Index"].Text, "0");
                    }
                    else
                    {
                        //refresh the Order Table with the udpated values and other Balances
                        //there is Subtotal = 0 to refresh the balance
                        UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTableForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, "0");
                    }


                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DropDownListSaleItem SelectedIndexChanged(): " + ex.Message + "');", true);
            }

        }

        private void UpdatePickupDateInSaleItems(string orderNumber, GridDataItem saleItem)
        {
            //if this is the complex invoice (index #1), then base on the selected date in Order Table
            if (RadioButtonListInvoiceType.SelectedIndex == 1)
            {
                //get the index of Order number. For example, #1 => 1
                int orderNumberIndex = Convert.ToInt16(orderNumber.Substring(1));

                //from Order table, get the row that has the same order number as the current update sale item
                //how, need to subtract 1 to get the index
                GridDataItem orderItem = RadGridOrders.MasterTableView.Items[orderNumberIndex - 1];
                RadDatePicker radDatePickerPickupDate = (RadDatePicker)orderItem.FindControl("RadDatePickerPickupDate");

                if (radDatePickerPickupDate.SelectedDate != null)
                {
                    //set the  pickup date
                    saleItem["PickupDate"].Text = Convert.ToDateTime(radDatePickerPickupDate.SelectedDate).ToShortDateString();
                }
                else
                {
                    //not selected
                    saleItem["PickupDate"].Text = "Not Selected";
                }
            }
            //if this is not the complect invoice, then read the pickup date base on the Invoice Pickup date
            else
            {
                if (RadDatePickerInvoicePickupDate.SelectedDate != null)
                {
                    //set the  pickup date
                    saleItem["PickupDate"].Text = Convert.ToDateTime(RadDatePickerInvoicePickupDate.SelectedDate).ToShortDateString();
                }
                else
                {
                    //not selected
                    saleItem["PickupDate"].Text = "Not Selected";
                }
            }
        }
        protected void DropDownListOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList dropdownListOrderNumber = (DropDownList)sender;
                GridDataItem item = (GridDataItem)dropdownListOrderNumber.NamingContainer;
                item.Selected = true;
                TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");

                //if invoice type is Simple
                if (RadioButtonListInvoiceType.SelectedIndex == 0)
                {
                    UpdateBalancesWhenChangesInSaleItemsTableForSimpleType(item["Index"].Text, textboxSubTotal.Text);
                }
                //if invoice type is complex\
                else
                {
                    //refresh the Order Table with the udpated values and other Balances
                    UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTableForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, textboxSubTotal.Text);
                }

                //update  Pickup Date
                UpdatePickupDateInSaleItems(dropdownListOrderNumber.SelectedValue, item);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DropDownListOrderNumber SelectedIndexChanged(): " + ex.Message + "');", true);
            }

        }
        protected void TextBoxAlreadyPaid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textboxAlreadyPaid = (TextBox)sender;
                GridDataItem item = (GridDataItem)textboxAlreadyPaid.NamingContainer;
                item.Selected = true;

                //also need to get the text box for Already Paid by Check to perform the calucation
                TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");

                double alreadyPaid = 0;
                if (!string.IsNullOrEmpty(textboxAlreadyPaid.Text))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    if (textboxAlreadyPaid.Text.StartsWith("$ "))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(2));
                    }
                    else if (textboxAlreadyPaid.Text.StartsWith("$"))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 1 character then convert it into double
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(1));
                    }
                    else
                    {
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaid.Text);
                    }
                    textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaid);
                }
                else
                {
                    textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                }

                UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTableForComplexType(item["OrderNumber"].Text, textboxTotal.Text, textboxAlreadyPaid.Text, textboxAlreadyPaidByCheck.Text);


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxAlreadyPaid_TextChanged(): " + ex.Message + "');", true);
            }
        }
        protected void TextBoxAlreadyPaidByCheck_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textboxAlreadyPaidByCheck = (TextBox)sender;
                GridDataItem item = (GridDataItem)textboxAlreadyPaidByCheck.NamingContainer;
                item.Selected = true;

                //also need to get the text box for Already Paid by Cash to perform the calucation
                TextBox textboxAlreadyPaidByCash = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");

                double alreadyPaid = 0;
                if (!string.IsNullOrEmpty(textboxAlreadyPaidByCheck.Text))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    if (textboxAlreadyPaidByCheck.Text.StartsWith("$ "))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(2));
                    }
                    else if (textboxAlreadyPaidByCheck.Text.StartsWith("$"))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 1 character then convert it into double
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(1));
                    }
                    else
                    {
                        alreadyPaid = Convert.ToDouble(textboxAlreadyPaidByCheck.Text);
                    }
                    textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, alreadyPaid);
                }
                else
                {
                    textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                }

                UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTableForComplexType(item["OrderNumber"].Text, textboxTotal.Text, textboxAlreadyPaidByCash.Text, textboxAlreadyPaidByCheck.Text);


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxAlreadyPaidByCheck_TextChanged(): " + ex.Message + "');", true);
            }
        }
        protected void TextBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }
        protected void TextBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateSubTotal(sender);
        }
        protected void TextBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateSubTotal(sender);
        }
        protected void TextBoxTotal_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateBalancesWhenChangesInSaleItemsTableForSimpleType(string indexInSelectedRow, string subTotalInSelectedRow)
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;
            double alreadyPaidByCheckGrandTotal = 0;

            //parse through all the rows in the first table, SaleItems, to calculate the Total for each Order
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                    TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                    TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                    TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                    TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                    DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                    //if found the current row that has a control being update, then can't read the value in this row because it's not refreshed and have the latest value
                    //therefore, we'll get the value from input parameter
                    if (item["Index"].Text.Equals(indexInSelectedRow))
                    {
                        double subTotal = Convert.ToDouble(subTotalInSelectedRow.Substring(2)); //when getting the subtable, take out the first 2 char, which is "$ ". ie: "$ 17.00"
                        grandTotal += subTotal;
                    }
                    //for the remaining row, if there is an sale item selected
                    else if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        //if there is an order number selected
                        if (!string.IsNullOrEmpty(dropdownListOrderNumber.SelectedValue))
                        {
                            //get the index of the selected order

                            //if there is a value in subtotal
                            if (!string.IsNullOrEmpty(textboxSubTotal.Text))
                            {
                                double subTotal = Convert.ToDouble(textboxSubTotal.Text.Substring(2)); //when getting the subtable, take out the first 2 char, which is "$ ". ie: "$ 17.00"

                                //add the subTotal into the total with index minus 1
                                grandTotal += subTotal;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateBalancesWhenChangesInSaleItemsTableForSimpleType() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }

            alreadyPaidGrandTotal = 0;
            if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaid.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxTotalAlreadyPaid.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    alreadyPaidGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxTotalAlreadyPaid.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    alreadyPaidGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(1));
                }
                else
                {
                    alreadyPaidGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text);
                }
                TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaidGrandTotal);
            }
            else
            {
                TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                alreadyPaidGrandTotal = 0;
            }

            alreadyPaidByCheckGrandTotal = 0;
            if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaidByCheck.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(1));
                }
                else
                {
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text);
                }
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, alreadyPaidByCheckGrandTotal);
            }
            else
            {
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                alreadyPaidByCheckGrandTotal = 0;
            }

            SyncPaidStatusAndGrandTotal(grandTotal, alreadyPaidGrandTotal, alreadyPaidByCheckGrandTotal);

        }
        private void UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTableForComplexType(string indexInSelectedRow, string orderNumberInSelectedRow, string subTotalInSelectedRow)
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;
            double alreadyPaidByCheckGrandTotal = 0;
            List<RadGridOrder> orders = new List<RadGridOrder>();
            for (int i = 0; i < NUMBER_OF_ORDER; i++)
            {
                RadGridOrder order = new RadGridOrder();
                orders.Add(order);
            }

            //parse through all the rows in the first table, SaleItems, to calculate the Total for each Order
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                    TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                    TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                    TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                    TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                    DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                    int orderIndexInSaleItemsGrid;
                    //if found the current row that has a control being update, then can't read the value in this row because it's not refreshed and have the latest value
                    //therefore, we'll get the value from input parameter
                    if (item["Index"].Text.Equals(indexInSelectedRow))
                    {
                        orderIndexInSaleItemsGrid = Convert.ToInt16(orderNumberInSelectedRow.Substring(1)); //when getting the Order Number, take out the first char, which is "#"
                        double subTotal = Convert.ToDouble(subTotalInSelectedRow.Substring(2)); //when getting the subtable, take out the first 2 char, which is "$ ". ie: "$ 17.00"

                        orders[orderIndexInSaleItemsGrid - 1].Total += subTotal;
                    }
                    //for the remaining row, if there is an sale item selected
                    else if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        //if there is an order number selected
                        if (!string.IsNullOrEmpty(dropdownListOrderNumber.SelectedValue))
                        {
                            //get the index of the selected order
                            orderIndexInSaleItemsGrid = Convert.ToInt16(dropdownListOrderNumber.SelectedValue.Substring(1)); //when getting the Order Number, take out the first char, which is "#"

                            //if there is a value in subtotal
                            if (!string.IsNullOrEmpty(textboxSubTotal.Text))
                            {
                                double subTotal = Convert.ToDouble(textboxSubTotal.Text.Substring(2)); //when getting the subtable, take out the first 2 char, which is "$ ". ie: "$ 17.00"

                                //add the subTotal into the total with index minus 1
                                orders[orderIndexInSaleItemsGrid - 1].Total += subTotal;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTable() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }

            //parse through all the rows in the second table, Orders, to update the Total and Remain Balance
            int orderIndexInOrdersGrid = 0;
            foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                    TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                    TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                    TextBox textboxCheckNumber = (TextBox)item.FindControl("TextBoxCheckNumber");
                    TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                    RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                    //because in the contructor of the object, we initial it as 0. Therefore, to make sure only set when it's not 0
                    if (orders[orderIndexInOrdersGrid].Total != 0)
                    {
                        double paidByCash = 0;
                        double paidByCheck = 0;

                        textboxAlreadyPaid.Enabled = true;
                        textboxAlreadyPaidByCheck.Enabled = true;
                        textboxCheckNumber.Enabled = true;
                        radDatePickerPickupDate.Enabled = true;

                        textboxAlreadyPaid.BackColor = Color.Orange;
                        textboxAlreadyPaidByCheck.BackColor = Color.Orange;
                        textboxCheckNumber.BackColor = Color.Orange;
                        radDatePickerPickupDate.BackColor = Color.Orange;

                        textboxTotal.Text = string.Format(CURRENCY_FORMAT, orders[orderIndexInOrdersGrid].Total);

                        //if there is an amount in paid by cash, then get it
                        if (!string.IsNullOrEmpty(textboxAlreadyPaid.Text))
                        {
                            paidByCash = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                        }
                        else
                        {
                            //already paid is 0
                            textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                        }

                        //if there is an amount in paid by check, then get it
                        if (!string.IsNullOrEmpty(textboxAlreadyPaidByCheck.Text))
                        {
                            paidByCheck = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                        }
                        else
                        {
                            //already paid is 0
                            textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                        }


                        alreadyPaidGrandTotal += paidByCash;
                        alreadyPaidByCheckGrandTotal += paidByCheck;
                        textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, orders[orderIndexInOrdersGrid].Total - paidByCash - paidByCheck);

                        //update the grandTotal
                        grandTotal += Convert.ToDouble(textboxTotal.Text.Substring(2));

                    }
                    else
                    {
                        textboxAlreadyPaid.Enabled = false;
                        textboxAlreadyPaidByCheck.Enabled = false;
                        textboxCheckNumber.Enabled = false;
                        radDatePickerPickupDate.Enabled = false;

                        textboxAlreadyPaid.BackColor = default(Color);
                        textboxAlreadyPaidByCheck.BackColor = default(Color);
                        textboxCheckNumber.BackColor = default(Color);
                        radDatePickerPickupDate.BackColor = default(Color);

                        textboxTotal.Text = "";
                        textboxAlreadyPaid.Text = "";
                        textboxRemainBalance.Text = "";
                        //    radDatePickerPickupDate.SelectedDate = null;
                    }


                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTable: " + item["Index"].Text + "." + ex.Message + "');", true);

                }
                orderIndexInOrdersGrid++;
            }

            SyncPaidStatusAndGrandTotal(grandTotal, alreadyPaidGrandTotal, alreadyPaidByCheckGrandTotal);


        }

        private void UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTableForComplexType(string orderNumberInSelectedRow, string totalInSelectedRow, string alreadyPaidInSelectedRow, string alreadyPaidByCheckInSelectedRow)
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;
            double alreadyPaidByCheckGrandTotal = 0;
            double totalBalance = 0;
            //parse through all the rows in the second table, Orders, to update the Total and Remain Balance
            foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                    TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                    TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                    TextBox textboxCheckNumber = (TextBox)item.FindControl("TextBoxCheckNumber");
                    TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                    RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                    double paidByCash = 0;
                    double paidByCheck = 0;
                    //if the select row, then get all the value from parameter
                    //if not, then read the value from the table
                    if (item["OrderNumber"].Text.Equals(orderNumberInSelectedRow))
                    {
                        textboxAlreadyPaid.Enabled = true;
                        textboxAlreadyPaidByCheck.Enabled = true;
                        textboxCheckNumber.Enabled = true;
                        radDatePickerPickupDate.Enabled = true;

                        textboxAlreadyPaid.BackColor = Color.Orange;
                        textboxAlreadyPaidByCheck.BackColor = Color.Orange;
                        textboxCheckNumber.BackColor = Color.Orange;
                        radDatePickerPickupDate.BackColor = Color.Orange;

                        double total = Convert.ToDouble(totalInSelectedRow.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00

                        //if get some value paid by cash, then get the value
                        if (!string.IsNullOrEmpty(alreadyPaidInSelectedRow))
                        {
                            paidByCash = Convert.ToDouble(alreadyPaidInSelectedRow.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00

                        }
                        else
                        {
                            //already paid is 0
                            textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                        }

                        //if get some value paid by check, then get the value
                        if (!string.IsNullOrEmpty(alreadyPaidInSelectedRow))
                        {
                            paidByCheck = Convert.ToDouble(alreadyPaidByCheckInSelectedRow.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00

                        }
                        else
                        {
                            //already paid by check is 0
                            textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                        }

                        alreadyPaidGrandTotal += paidByCash;
                        alreadyPaidByCheckGrandTotal += paidByCheck;
                        textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total - paidByCash - paidByCheck);

                        //update the grandTotal
                        grandTotal += total;

                    }
                    //if this is the the seleted row that being update
                    else
                    {
                        if (!string.IsNullOrEmpty(textboxTotal.Text))
                        {
                            double total = Convert.ToDouble(textboxTotal.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                                                                                             //if there is no paid, then the remaind balance is the same amount as the Total

                            //if get some value paid by cash, then get the value
                            if (!string.IsNullOrEmpty(textboxAlreadyPaid.Text))
                            {
                                paidByCash = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                            }
                            else
                            {
                                //already paid is 0
                                textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                            }


                            //if get some value paid by check, then get the value
                            if (!string.IsNullOrEmpty(textboxAlreadyPaidByCheck.Text))
                            {
                                paidByCheck = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                            }
                            else
                            {
                                //already paid is 0
                                textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                            }

                            alreadyPaidGrandTotal += paidByCash;
                            alreadyPaidByCheckGrandTotal += paidByCheck;
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total - paidByCash - paidByCheck);

                            //update the grandTotal
                            grandTotal += total;

                        }

                    }




                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTable() at index: " + item["Index"].Text + "." + ex.Message + "');", true);

                }

            }
            //set the Grand Total
            TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, grandTotal);
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaidGrandTotal);
            TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, alreadyPaidByCheckGrandTotal);

            totalBalance = grandTotal - alreadyPaidGrandTotal - alreadyPaidByCheckGrandTotal;
            //set the Remain Balance
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, totalBalance);


            if (totalBalance == 0)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Paid.ToString());
            }
            else if (totalBalance == grandTotal)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Unpaid.ToString());
            }
            else
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Partial_Paid.ToString());
            }

        }
        private void CalculateSubTotal(object sender)
        {
            TextBox changedTextBox = (TextBox)sender;
            GridDataItem item = (GridDataItem)changedTextBox.NamingContainer;
            TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
            TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
            TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
            TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
            DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
            DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");

            //the the sale item is not select, then wipe out the value of the text field and don't bother to calculate the subTotal
            if (string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
            {
                changedTextBox.Text = "";
                return;
            }
            try
            {
                int quantity = 0;
                if (!string.IsNullOrEmpty(textboxQuantity.Text))
                {

                    quantity = Convert.ToInt16(textboxQuantity.Text);
                }
                else
                {
                    textboxQuantity.Text = "0"; //if it's null or empty out, then default to 0
                }

                double unitPrice = 0;
                if (!string.IsNullOrEmpty(textboxUnitPrice.Text))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    if (textboxUnitPrice.Text.StartsWith("$ "))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        unitPrice = Convert.ToDouble(textboxUnitPrice.Text.Substring(2));
                    }
                    else if (textboxUnitPrice.Text.StartsWith("$"))
                    {
                        //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                        unitPrice = Convert.ToDouble(textboxUnitPrice.Text.Substring(1));
                    }
                    else
                    {
                        unitPrice = Convert.ToDouble(textboxUnitPrice.Text);
                    }
                    textboxUnitPrice.Text = string.Format(CURRENCY_FORMAT, unitPrice);
                }
                else
                {
                    textboxUnitPrice.Text = string.Format(CURRENCY_FORMAT, 0);
                }

                double discount = 0;
                if (!string.IsNullOrEmpty(textboxDiscount.Text))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    if (textboxDiscount.Text.StartsWith("$ "))
                    {
                        //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                        discount = Convert.ToDouble(textboxDiscount.Text.Substring(2));
                    }
                    else if (textboxDiscount.Text.StartsWith("$"))
                    {
                        //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                        discount = Convert.ToDouble(textboxDiscount.Text.Substring(1));
                    }
                    else
                    {
                        discount = Convert.ToDouble(textboxDiscount.Text);
                    }
                    textboxDiscount.Text = string.Format(CURRENCY_FORMAT, discount);
                }
                else
                {
                    textboxDiscount.Text = string.Format(CURRENCY_FORMAT, 0);
                }

                double subTotal = quantity * (unitPrice - discount);
                textboxSubTotal.Text = string.Format(CURRENCY_FORMAT, subTotal);

                //if invoice type is Simple
                if (RadioButtonListInvoiceType.SelectedIndex == 0)
                {
                    UpdateBalancesWhenChangesInSaleItemsTableForSimpleType(item["Index"].Text, textboxSubTotal.Text);
                }
                else
                {
                    UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTableForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, textboxSubTotal.Text);
                }
            }
            catch (Exception ex)
            {
                textboxSubTotal.Text = string.Format(CURRENCY_FORMAT, 0);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in Calculate SubTotal(): " + ex.Message + "');", true);
            }
        }

        protected void ImageButtonNotPickedUp_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imageButton = (ImageButton)sender;
            GridDataItem item = (GridDataItem)imageButton.NamingContainer;

            ImageButton imageButtonNotPickedup = (ImageButton)item.FindControl("ImageButtonNotPickedUp");
            imageButtonNotPickedup.Visible = false;
            imageButtonNotPickedup.Enabled = false;

            ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
            imageButtonPickedup.Visible = true;
            imageButtonPickedup.Enabled = true;

            //when Not Picked Up click, which mean it change into Picked up, then hightlight the row
            item.BackColor = Color.Yellow;

            //when not pickup clicks, which mean, it is now flipped to be pickedup. Therefore, it's TRUE
            SyncPickupStatus(item["Index"].Text, true);
        }
        protected void ImageButtonPickedUp_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imageButton = (ImageButton)sender;
            GridDataItem item = (GridDataItem)imageButton.NamingContainer;

            ImageButton imageButtonNotPickedup = (ImageButton)item.FindControl("ImageButtonNotPickedUp");
            imageButtonNotPickedup.Visible = true;
            imageButtonNotPickedup.Enabled = true;

            ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
            imageButtonPickedup.Visible = false;
            imageButtonPickedup.Enabled = false;

            //when Picked Up click, which mean it change into Not Picked up, then un-hightlight the row
            item.BackColor = default(Color);

            //when Picked Up clicks, which mean it change into NOT Picked up, Therefore, it's FALSE
            SyncPickupStatus(item["Index"].Text, false);
        }

        private void SyncPickupStatus(string indexInSelectedRow, bool isPickup)
        {
            int numberOfPickups = 0;
            int numberOfSaleItems = 0;
            //parse through all the rows in sale items table to count numberofPickup and numberOfSaleItems
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");

                    //if found the current row that has a control being update, then can't read the value in this row because it's not refreshed and have the latest value
                    //therefore, we'll get the value from input parameter
                    if (item["Index"].Text.Equals(indexInSelectedRow))
                    {
                        if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                        {
                            numberOfSaleItems++;
                            if (isPickup)
                            {
                                numberOfPickups++;
                            }
                        }
                    }
                    //for the remaining row, if there is an sale item selected
                    else if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        //if there is an order number selected
                        if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                        {
                            numberOfSaleItems++;
                            //if the picked image flip on, then this item is pickedup
                            if (imageButtonPickedup.Enabled)
                            {
                                numberOfPickups++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetPickupStatus() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }

            //now after we perform the count, we need to set the pickup status base on the count above

            if (numberOfPickups == 0)
            {
                SetInvoiceStatus(InvoicePickupStatusEnum.Not_Pickup.ToString(), "");
            }
            else if (numberOfPickups == numberOfSaleItems)
            {
                SetInvoiceStatus(InvoicePickupStatusEnum.Full_Pickup.ToString(), "");
            }
            else if (numberOfPickups > 0)
            {
                SetInvoiceStatus(InvoicePickupStatusEnum.Partial_Pickup.ToString(), "");
            }

        }
        private void SyncPaidStatusAndGrandTotal(double grandTotal, double alreadyPaidGrandTotal, double alreadyPaidByCheckGrandTotal)
        {
            //set the Grand Total
            TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, grandTotal);
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaidGrandTotal);
            TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, alreadyPaidByCheckGrandTotal);

            double totalBalance = grandTotal - alreadyPaidGrandTotal - alreadyPaidByCheckGrandTotal;

            //set the Remain Balance
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, totalBalance);

            if (totalBalance == 0)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Paid.ToString());
            }
            else if (totalBalance == grandTotal)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Unpaid.ToString());
            }
            else
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Partial_Paid.ToString());
            }
        }

        protected void RadioButtonListInvoiceType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (RadioButtonListInvoiceType.SelectedValue.Contains("Complex"))
            {
                RadGridOrders.Visible = true;
                LabelInvoicePickupDate.Visible = false;
                RadDatePickerInvoicePickupDate.Visible = false;
                TextBoxTotalAlreadyPaid.Enabled = false;
                TextBoxTotalAlreadyPaid.BackColor = default(Color);
                TextBoxTotalAlreadyPaidByCheck.Enabled = false;
                TextBoxTotalAlreadyPaidByCheck.BackColor = default(Color);
                RadGridSaleItems.Columns[3].Visible = true; //display the Order Number column

                //display paging size to 20
                RadGridSaleItems.PageSize = 20;

            }
            else
            {
                RadGridOrders.Visible = false;
                LabelInvoicePickupDate.Visible = true;
                RadDatePickerInvoicePickupDate.Visible = true;
                TextBoxTotalAlreadyPaid.Enabled = true;
                TextBoxTotalAlreadyPaid.BackColor = Color.Orange;
                TextBoxTotalAlreadyPaidByCheck.Enabled = true;
                TextBoxTotalAlreadyPaidByCheck.BackColor = Color.Orange;
                RadGridSaleItems.Columns[3].Visible = false; //hide the Order Number column

                //change the paging size to 10
                RadGridSaleItems.PageSize = 10;
            }
            RadGridSaleItems.Rebind();
            RadGridOrders.Rebind();
            RadDatePickerInvoicePickupDate.SelectedDate = null;
            TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, 0);
            SetInvoiceStatus(InvoicePickupStatusEnum.Not_Pickup.ToString(), InvoicePaidStatusEnum.Unpaid.ToString());
        }

        protected void TextBoxTotalAlreadyPaid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateGrandTotal();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxTotalAlreadyPaid_TextChanged(): " + ex.Message + "');", true);
            }
        }

        protected void TextBoxTotalAlreadyPaidByCheck_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateGrandTotal();

                //only display the check number if this is Simple type
                if (RadioButtonListInvoiceType.SelectedIndex == 0)
                {
                    LabelGrandCheckNumber.Visible = true;
                    TextBoxGrandCheckNumber.Visible = true;
                }
                else
                {
                    LabelGrandCheckNumber.Visible = false;
                    TextBoxGrandCheckNumber.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxTotalAlreadyPaidByCheck_TextChanged(): " + ex.Message + "');", true);
            }
        }

        private void UpdateGrandTotal()
        {
            double paidByCash = 0;
            double paidByCheck = 0;
            double paidAmount = 0;
            if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaid.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxTotalAlreadyPaid.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    paidByCash = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxTotalAlreadyPaid.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    paidByCash = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(1));
                }
                else
                {
                    paidByCash = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text);
                }
                TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, paidByCash);

            }
            else
            {
                TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                paidByCash = 0;
            }

            if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaidByCheck.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    paidByCheck = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    paidByCheck = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(1));
                }
                else
                {
                    paidByCheck = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text);
                }
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, paidByCheck);

            }
            else
            {
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                paidByCheck = 0;
            }

            double grandTotal = Convert.ToDouble(TextBoxGrandTotal.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
            double totalBalance = grandTotal - paidByCash - paidByCheck;
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, totalBalance);

            if (totalBalance == 0)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Paid.ToString());
            }
            else if (totalBalance == grandTotal)
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Unpaid.ToString());
            }
            else
            {
                SetInvoiceStatus("", InvoicePaidStatusEnum.Partial_Paid.ToString());
            }
        }
        private void SetInvoiceStatus(string pickupStatus = "", string payStatus = "")
        {
            try
            {


                //get the current status into array
                string[] invoiceStatus = TextBoxInvoiceStatus.Text.Split(',');
                if (invoiceStatus.Length == 2)
                {
                    string currentPickupStatus = invoiceStatus[0].Trim();
                    string currentPayStatus = invoiceStatus[1].Trim();

                    //set the status to the new pass in parameter
                    if (!string.IsNullOrEmpty(pickupStatus))
                    {
                        currentPickupStatus = pickupStatus;
                    }
                    if (!string.IsNullOrEmpty(payStatus))
                    {
                        currentPayStatus = payStatus;
                    }

                    //now set the invoice status back to textbox
                    TextBoxInvoiceStatus.Text = currentPickupStatus + ", " + currentPayStatus;


                    //show the image icon from the master page
                    if (currentPickupStatus.Equals(InvoicePickupStatusEnum.Full_Pickup.ToString()))
                    {
                        System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)this.Page.Master.FindControl("ImageOrderPickup");
                        if (image != null)
                        {
                            image.Visible = true;
                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)this.Page.Master.FindControl("ImageOrderPickup");
                        if (image != null)
                        {
                            image.Visible = false;
                        }
                    }
                    //show the image icon from the master page
                    if (currentPayStatus.Equals(InvoicePaidStatusEnum.Paid.ToString()))
                    {
                        System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)this.Page.Master.FindControl("ImagePaidInFull");
                        if (image != null)
                        {
                            image.Visible = true;
                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)this.Page.Master.FindControl("ImagePaidInFull");
                        if (image != null)
                        {
                            image.Visible = false;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetInvoiceStatus(): " + ex.Message + "');", true);
            }

        }

        protected void ButtonSaveInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                SaveInvoice();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in ButtonSaveInvoice_Click()" + "." + ex.Message + "');", true);
            }
        }

        protected void ButtonPickupAll_Click(object sender, EventArgs e)
        {

            //parse through all the rows in SaleItems table, then mark all of them to be picked-up
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                    ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
                    ImageButton imageButtonNotPickedup = (ImageButton)item.FindControl("ImageButtonNotPickedUp");
                    //for the remaining row, if there is an sale item selected
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        imageButtonNotPickedup.Visible = false;
                        imageButtonNotPickedup.Enabled = false;
                        imageButtonPickedup.Visible = true;
                        imageButtonPickedup.Enabled = true;
                        item.BackColor = Color.Yellow;
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in ButtonPickupAll_Click() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }

            //set voice status to be fully pickup
            SetInvoiceStatus(InvoicePickupStatusEnum.Full_Pickup.ToString(), "");
            SaveInvoice();
        }

        protected void ButtonPaidAll_Click(object sender, EventArgs e)
        {
            //if this is the Complex type, then fill out all the paid for Order table
            if (RadioButtonListInvoiceType.SelectedIndex == 1)
            {
                foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
                {
                    TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                    TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                    TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                    TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");

                    //because in the contructor of the object, we initial it as 0. Therefore, to make sure only set when it's not 0
                    if (!string.IsNullOrEmpty(textboxTotal.Text))
                    {
                        //if there is no check paid amount, the assume all the paid amount is cash
                        if (string.IsNullOrEmpty(textboxAlreadyPaidByCheck.Text))
                        {
                            //set the amount already Paid to the total
                            textboxAlreadyPaid.Text = textboxTotal.Text;
                            textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                        }
                        //if there is some check paid amount, then leave the check paid amount alone. Just subtract and get the total of paid cash amount
                        else
                        {
                            try
                            {
                                double paidByCheck = 0;
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (textboxAlreadyPaidByCheck.Text.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    paidByCheck = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(2));
                                }
                                else if (textboxAlreadyPaidByCheck.Text.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 1 character then convert it into double
                                    paidByCheck = Convert.ToDouble(textboxAlreadyPaidByCheck.Text.Substring(1));
                                }
                                else
                                {
                                    paidByCheck = Convert.ToDouble(textboxAlreadyPaidByCheck.Text);
                                }

                                double total = 0;
                                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                if (textboxTotal.Text.StartsWith("$ "))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                                    total = Convert.ToDouble(textboxTotal.Text.Substring(2));
                                }
                                else if (textboxTotal.Text.StartsWith("$"))
                                {
                                    //since the value of the field is Currency format, so have to exclude the first 1 character then convert it into double
                                    total = Convert.ToDouble(textboxTotal.Text.Substring(1));
                                }
                                else
                                {
                                    total = Convert.ToDouble(textboxTotal.Text);
                                }
                                textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, total - paidByCheck);
                            }
                            //if for whatever reason, it throws exception when select Check Paid amount, then assume it is zero
                            catch (Exception ex)
                            {
                                //set the amount already Paid to the total
                                textboxAlreadyPaid.Text = textboxTotal.Text;
                                textboxAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                            }
                            //set the remain Balance to be 0
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, 0);
                        }
                    }
                }
            }

      
            double alreadyPaidByCheckGrandTotal = 0;
            if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaidByCheck.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxTotalAlreadyPaidByCheck.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text.Substring(1));
                }
                else
                {
                    alreadyPaidByCheckGrandTotal = Convert.ToDouble(TextBoxTotalAlreadyPaidByCheck.Text);
                }
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, alreadyPaidByCheckGrandTotal);
            }
            else
            {
                TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                alreadyPaidByCheckGrandTotal = 0;
            }

            double grandTotal = 0;
            if (!string.IsNullOrEmpty(TextBoxGrandTotal.Text))
            {
                //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                if (TextBoxGrandTotal.Text.StartsWith("$ "))
                {
                    //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                    grandTotal = Convert.ToDouble(TextBoxGrandTotal.Text.Substring(2));
                }
                //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                else if (TextBoxGrandTotal.Text.StartsWith("$"))
                {
                    //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                    grandTotal = Convert.ToDouble(TextBoxGrandTotal.Text.Substring(1));
                }
                else
                {
                    grandTotal = Convert.ToDouble(TextBoxGrandTotal.Text);
                }
                TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, grandTotal);
            }
            else
            {
                TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, 0);
                grandTotal = 0;
            }

            //now don't touch the PaidByCheck and calculate to get the AlreadyPaidByCash
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, grandTotal - alreadyPaidByCheckGrandTotal);
            //set the remain Balance to be 0
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, 0);

            //update invoice status
            SetInvoiceStatus("", InvoicePaidStatusEnum.Paid.ToString());
            SaveInvoice();
        }

        protected void RadMaskedTextBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ButtonCancelInvoice_Click(object sender, EventArgs e)
        {
            //go to each sale items and set to the quantity to 0 then udpate the calculation
            //parse through all the rows in the first table, SaleItems, to calculate the Total for each Order
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        //set the quantity to 0
                        textboxQuantity.Text = "0";

                        CalculateSubTotal(textboxQuantity);

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateOrderTableAndOtherBalancesWhenChangesInSaleItemsTable() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }
            //Set all the total to zero
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxGrandCheckNumber.Text = "";
            TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, 0);
            TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, 0);

            SetInvoiceStatus(InvoicePickupStatusEnum.Cancelled.ToString(), InvoicePaidStatusEnum.Cancelled.ToString());

            DisableControls();
            SaveInvoice();

        }
        private void SaveInvoice()
        {
            try
            {
                List<OrderInvoice> orderInvoices = SharedMethod.GetInvoices();

                //if invoice number is empty, which meant this is creating new invoice
                if (string.IsNullOrEmpty(TextBoxInvoiceNumber.Text))
                {
                    //query the database to get the next index
                    //if the collection is empty, then the index start from 101
                    if (orderInvoices.Count == 0)
                    {
                        TextBoxInvoiceNumber.Text = "101";
                    }
                    else
                    {
                        int latestInvoiceNumber = orderInvoices.Select(i => Convert.ToInt16(i.InvoiceNumber)).Max();

                        //increment the max number which is the current new invoice
                        latestInvoiceNumber += 1;

                        //set the Invoice Number
                        TextBoxInvoiceNumber.Text = latestInvoiceNumber.ToString();
                    }

                    OrderInvoice invoice = new OrderInvoice();
                    SetThenSaveInvoiceObject(invoice, true, TextBoxInvoiceNumber.Text);

                }
                //is the invoice is NOT empty, then this is editing the invoice
                else
                {
                    OrderInvoice invoice = orderInvoices.Find(x => x.InvoiceNumber.Equals(TextBoxInvoiceNumber.Text));
                    if (invoice != null)
                    {
                        SetThenSaveInvoiceObject(invoice, false, TextBoxInvoiceNumber.Text);


                    }
                    else
                    {
                        throw new Exception("Can't find the existing Invoice Number" + TextBoxInvoiceNumber.Text + " in the dabase");
                    }
                }


                //query the invoice object and will get it overwritten with the new object

   
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('Exception in SaveInvoice()" + "." + ex.Message + "');", true);
            }
        }
        private void SetThenSaveInvoiceObject(OrderInvoice invoice, bool newInvoice, string invoiceNumber)
        {
            invoice.InvoiceNumber = TextBoxInvoiceNumber.Text;
            invoice.InvoiceDate = TextBoxInvoiceDate.Text;
            invoice.Status = TextBoxInvoiceStatus.Text;
            invoice.PickupDate = RadDatePickerInvoicePickupDate.SelectedDate.ToString();
            invoice.InvoiceType = RadioButtonListInvoiceType.SelectedValue;

            invoice.LastUpdateBy = User.Identity.Name;
            invoice.LastUpdateOn = DateTime.Now.ToString("MM/dd HH:mm");

            invoice.GrandTotal = TextBoxGrandTotal.Text;
            invoice.AlreadyPaid = TextBoxTotalAlreadyPaid.Text;
            invoice.AlreadyPaidByCheck = TextBoxTotalAlreadyPaidByCheck.Text;
            invoice.CheckNumber = TextBoxGrandCheckNumber.Text;

            invoice.RemainBalance = TextBoxTotalRemainBalance.Text;
            invoice.Notes = TextBoxNote.Text;

            invoice.CreatedBy = TextBoxCreatedBy.Text;

            if (string.IsNullOrEmpty(RadTextBoxCustomerName.Text))
            {
                invoice.CustomerName = "";
            }
            else
            {
                invoice.CustomerName = RadTextBoxCustomerName.Text;
            }

            if (string.IsNullOrEmpty(RadMaskedTextBoxPhoneNumber.Text))
            {
                invoice.PhoneNumber = "";
            }
            else
            {
                invoice.PhoneNumber = RadMaskedTextBoxPhoneNumber.Text;
            }

            List<RadGridSaleItem> orderSaleItems = new List<RadGridSaleItem>();
            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                    TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                    TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                    TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                    TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                    DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                    DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                    ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");

                    //for the remaining row, if there is an sale item selected
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedValue))
                    {
                        RadGridSaleItem orderSaleItem = new RadGridSaleItem();
                        orderSaleItem.Index = item["Index"].Text;
                        orderSaleItem.PickupDate = item["PickupDate"].Text;
                        //if the the image Pickup visible, which mean this order sale item is picked up
                        if (imageButtonPickedup.Visible)
                        {
                            orderSaleItem.PickedUp = "yes";
                        }
                        else
                        {
                            orderSaleItem.PickedUp = "no";
                        }
                        orderSaleItem.OrderNumber = dropdownListOrderNumber.SelectedValue;
                        orderSaleItem.SaleItem = dropdownListSaleItem.SelectedValue;
                        orderSaleItem.Quantity = textboxQuantity.Text;
                        orderSaleItem.UnitPrice = textboxUnitPrice.Text;
                        orderSaleItem.Discount = textboxDiscount.Text;
                        orderSaleItem.SubTotal = textboxSubTotal.Text;

                        orderSaleItems.Add(orderSaleItem);

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetThenSaveInvoice in SaleItems() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }
            //set list of order sale items to invoice object
            invoice.OrderSaleItems = orderSaleItems;

            //if this is the complext invoice, then need to store the information of Order Table
            if (RadioButtonListInvoiceType.SelectedIndex == 1)
            {
                List<RadGridOrder> orders = new List<RadGridOrder>();
                foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
                {
                    try
                    {
                        TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                        TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                        TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                        TextBox textboxCheckNumber = (TextBox)item.FindControl("TextBoxCheckNumber");
                        TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                        RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                        if (!string.IsNullOrEmpty(textboxTotal.Text))
                        {
                            RadGridOrder order = new RadGridOrder();
                            order.OrderNumber = item["OrderNumber"].Text;
                            order.PickupDate = radDatePickerPickupDate.SelectedDate.ToString();

                            //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                            order.TotalString = textboxTotal.Text;
                            order.AlreadyPaidString = textboxAlreadyPaid.Text;
                            order.AlreadyPaidByCheckString = textboxAlreadyPaidByCheck.Text;
                            order.CheckNumber = textboxCheckNumber.Text;
                            order.RemainBalanceString = textboxRemainBalance.Text;
                            orders.Add(order);
                        }



                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetThenSaveInvoice in Orders: " + item["Index"].Text + "." + ex.Message + "');", true);

                    }
                }
                //set list of order to invoice object
                invoice.InvoiceOrders = orders;
            }


            //**************************************************
            //if this is the new invoice
            if (newInvoice)
            {
                SharedMethod.SaveInvoice(invoice);
                try
                {
                    string mobileReceiptURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/InvoiceMobileReceipt.aspx");
                    string message = string.Format("Gian Hàng Tết St. Polycarp - Xuân Bính Ngọ 2026. \nHello {2}. \nĐây là xác nhận hóa đơn mua hàng #{0}. \nThis is your receipt, invoice #{0} [{1}?InvoiceNumber={0}].", invoice.InvoiceNumber, mobileReceiptURL, invoice.CustomerName);

                    SharedMethod.SendNewSmsTextMessage(invoice.PhoneNumber, message);
                }
                catch(Exception ex)
                {

                }
                //instead of display a pop-up here, we redirect to the new invoice with the flag to say this is the newly create invoice.
                //So the new invoice will be added in the database
                //redirect the page to the Invoice with the existing one with 2 parameters after saving the invoice
                string redirectURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Invoice?IsJustCreated=true&InvoiceNumber=" + invoiceNumber);
                HttpContext.Current.Response.Redirect(redirectURL);

                //now update the inventory sheet in google with the latest pickup amount
                SharedMethod.UpdateTotalPickupToInventoryInGoogleSheet();

            }
            //this is the an existing invoice, then replace with the existing on
            else
            {
                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    SharedMethod.SaveInvoice(invoice);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('Existing invoice " + invoiceNumber + " has updated.');", true);

                    //now update the inventory sheet in google with the latest pickup amount
                    SharedMethod.UpdateTotalPickupToInventoryInGoogleSheet();


                }
            }

           
        }
        private void LoadExistingInvoice(string invoiceNumber)
        {
            try
            {
                OrderInvoice invoice = SharedMethod.GetInvoice(invoiceNumber);

                
                //if the invoice number not found
                if (invoice == null)
                {
                    LabelPageTitle.Text = "Invoice #" + invoiceNumber + " - Not Found";

                }
                //if the invoice found, then load the data
                else
                {
                    LiteralChangeHistory.Text = SharedMethod.GetChangeHistory(invoiceNumber);
                    TextBoxInvoiceNumber.Text = invoice.InvoiceNumber;
                    TextBoxInvoiceDate.Text = invoice.InvoiceDate;
                    TextBoxInvoiceStatus.Text = invoice.Status;

                    string[] invoicePickupPaidStatus = invoice.Status.Split(new char[] { ',' });
                    //evaluate if this is the cancel invoice or not, if it is then disable all the controls
                    if (TextBoxInvoiceStatus.Text.Contains(InvoicePickupStatusEnum.Cancelled.ToString()) && TextBoxInvoiceStatus.Text.Contains(InvoicePaidStatusEnum.Cancelled.ToString()))
                    {
                        DisableControls();
                    }
                    //evaluate whether to display the Pickup/Paid icon
                    else if (invoicePickupPaidStatus.Length > 1)
                    {
                        SetInvoiceStatus(invoicePickupPaidStatus[0].Trim(), invoicePickupPaidStatus[1].Trim());
                    }

                    if (!string.IsNullOrEmpty(invoice.PickupDate))
                    {
                        RadDatePickerInvoicePickupDate.SelectedDate = Convert.ToDateTime(invoice.PickupDate);
                    }
                    RadioButtonListInvoiceType.SelectedValue = invoice.InvoiceType;

                    TextBoxGrandTotal.Text = invoice.GrandTotal;
                    TextBoxTotalAlreadyPaid.Text = invoice.AlreadyPaid;
                    if (string.IsNullOrEmpty(invoice.AlreadyPaidByCheck))
                    {
                        TextBoxTotalAlreadyPaidByCheck.Text = string.Format(CURRENCY_FORMAT, 0);
                    }
                    else
                    {
                        TextBoxTotalAlreadyPaidByCheck.Text = invoice.AlreadyPaidByCheck;
                    }
                   
                    if(!string.IsNullOrEmpty(invoice.CheckNumber))
                    {
                        TextBoxGrandCheckNumber.Text = invoice.CheckNumber;
                        TextBoxGrandCheckNumber.Visible = true;
                        LabelGrandCheckNumber.Visible = true;
                    }
                    TextBoxTotalRemainBalance.Text = invoice.RemainBalance;
                    TextBoxNote.Text = invoice.Notes;

                    if (!string.IsNullOrEmpty(invoice.CreatedBy))
                    {
                        TextBoxCreatedBy.Text = invoice.CreatedBy;
                    }

                    if(!string.IsNullOrEmpty(invoice.LastUpdateBy))
                    {
                        TextBoxLastUpdatedBy.Text = invoice.LastUpdateBy;
                        if(!string.IsNullOrEmpty(invoice.LastUpdateOn))
                        {
                            TextBoxLastUpdatedBy.Text += " (" + invoice.LastUpdateOn + ")";
                        }
                    }


                    RadTextBoxCustomerName.Text = invoice.CustomerName;
                    RadMaskedTextBoxPhoneNumber.Text = invoice.PhoneNumber;

                    int saleItemindex = 0;
                    List<RadGridSaleItem> orderSaleItems = invoice.OrderSaleItems;
                    //set the number of rows for this table
                   
                    foreach (RadGridSaleItem order in orderSaleItems)
                    {
                        GridDataItem item = null;
                        try
                        {
                            //if each of the record was found with the select sale item, then write it into the table
                            if (!string.IsNullOrEmpty(order.SaleItem))
                            {
                                item = RadGridSaleItems.MasterTableView.Items[saleItemindex++];

                                TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                                TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                                TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                                TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                                TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                                DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                                if (RadioButtonListInvoiceType.SelectedValue.ToUpper().Contains("COMPLEX"))
                                {
                                    dropdownListOrderNumber.Enabled = true;
                                }

                                DropDownList dropdownListSaleItem = (DropDownList)item.FindControl("DropDownListSaleItem");
                                ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
                                ImageButton imageButtonNotPickedup = (ImageButton)item.FindControl("ImageButtonNotPickedUp");

                                item["PickupDate"].Text = order.PickupDate;

                                //if the the image Pickup visible, which mean this order sale item is picked up
                                if (order.PickedUp == "yes")
                                {
                                    imageButtonPickedup.Visible = true;
                                    imageButtonNotPickedup.Visible = false;
                                    imageButtonPickedup.Enabled = true;
                                    imageButtonNotPickedup.Enabled = false;

                                    //when Not Picked Up click, which mean it change into Picked up, then hightlight the row
                                    item.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    imageButtonPickedup.Visible = false;
                                    imageButtonNotPickedup.Visible = true;
                                    imageButtonPickedup.Enabled = false;
                                    imageButtonNotPickedup.Enabled = true;

                                    //when Picked Up click, which mean it change into Not Picked up, then un-hightlight the row
                                    item.BackColor = default(Color);
                                }
                                dropdownListOrderNumber.SelectedValue = order.OrderNumber;
                                dropdownListOrderNumber.BackColor = Color.Orange;
                                dropdownListSaleItem.SelectedValue = order.SaleItem;
                                dropdownListSaleItem.BackColor = Color.Orange;
                                textboxQuantity.Text = order.Quantity;
                                textboxQuantity.Enabled = true;
                                textboxQuantity.BackColor = Color.Orange;

                                textboxUnitPrice.Text = order.UnitPrice;
                                textboxDiscount.Text = order.Discount;
                                textboxDiscount.Enabled = true;
                                textboxDiscount.BackColor = Color.Orange;
                                textboxSubTotal.Text = order.SubTotal;

                                //if we found at least a sale item, then load up the Grand Total and other balances
                                TextBoxTotalAlreadyPaid.BackColor = Color.Orange;
                                TextBoxTotalAlreadyPaidByCheck.BackColor = Color.Orange;
                                LabelInvoicePickupDate.Visible = true;
                                RadDatePickerInvoicePickupDate.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadSaleItem in SaleItems() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                        }
                    }

                    //if this is the complex invoice, then need to store the information of Order Table
                    if (RadioButtonListInvoiceType.SelectedValue.ToUpper().Contains("COMPLEX"))
                    {
                        //display the Order table
                        RadGridOrders.Visible = true;
                        RadGridSaleItems.Columns[3].Visible = true; //display the Order Number column
                        LabelInvoicePickupDate.Visible = false;
                        RadDatePickerInvoicePickupDate.Visible = false;
                        TextBoxTotalAlreadyPaid.Enabled = false;
                        TextBoxTotalAlreadyPaid.BackColor = default(Color);
                        TextBoxTotalAlreadyPaidByCheck.Enabled = false;
                        TextBoxTotalAlreadyPaidByCheck.BackColor = default(Color);

                        int orderIndex = 0;
                        List<RadGridOrder> invoiceOrders = invoice.InvoiceOrders;
                        foreach (RadGridOrder invoiceOrder in invoiceOrders)
                        {
                            GridDataItem item = null;
                            try
                            {
                                //now get the row at this index (ordernumber) but minus 1
                                item = RadGridOrders.MasterTableView.Items[orderIndex++];

                                TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                                TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                                TextBox textboxAlreadyPaidByCheck = (TextBox)item.FindControl("TextBoxAlreadyPaidByCheck");
                                TextBox textboxCheckNumber = (TextBox)item.FindControl("TextBoxCheckNumber");
                                TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                                RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                                if (!string.IsNullOrEmpty(invoiceOrder.PickupDate))
                                {
                                    radDatePickerPickupDate.SelectedDate = Convert.ToDateTime(invoiceOrder.PickupDate);
                                }
                                radDatePickerPickupDate.BackColor = Color.Orange;
                                radDatePickerPickupDate.Enabled = true;
                                textboxTotal.Text = invoiceOrder.TotalString;
                                textboxAlreadyPaid.Text = invoiceOrder.AlreadyPaidString;
                                textboxAlreadyPaid.BackColor = Color.Orange;
                                textboxAlreadyPaid.Enabled = true;
                                textboxAlreadyPaidByCheck.Text = invoiceOrder.AlreadyPaidByCheckString;
                                textboxAlreadyPaidByCheck.BackColor = Color.Orange;
                                textboxAlreadyPaidByCheck.Enabled = true;
                                textboxCheckNumber.Text = invoiceOrder.CheckNumber;
                                textboxCheckNumber.BackColor = Color.Orange;
                                textboxCheckNumber.Enabled = true;
                                textboxRemainBalance.Text = invoiceOrder.RemainBalanceString;

                            }
                            catch (Exception ex)
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetThenSaveInvoice in Orders: " + item["Index"].Text + "." + ex.Message + "');", true);

                            }
                        }
                    }




                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadExistingInvoice()" + "." + ex.Message + "');", true);
            }
        }
        protected void ButtonTextReminder_Click(object sender, EventArgs e)
        {

        }

        protected void ButtonRefreshPage_Click(object sender, EventArgs e)
        {

        }

        private void DisableControls()
        {
            PanelSummary.Visible = false;
            RadGridOrders.Enabled = false;
            RadGridSaleItems.Enabled = false;
            RadTextBoxCustomerName.Enabled = false;
            RadMaskedTextBoxPhoneNumber.Enabled = false;
            RadioButtonListInvoiceType.Enabled = false;
            RadDatePickerInvoicePickupDate.Enabled = false;
            ButtonRefreshPage.Visible = false;
            ButtonPickupAll.Visible = false;
            ButtonPaidAll.Visible = false;
            ButtonSaveInvoice.Visible = false;
            ButtonPrintInvoice.Visible = false;
            ButtonCancelInvoice.Visible = false;



            TextBoxNote.Enabled = false;
            TextBoxTotalAlreadyPaid.Enabled = false;
            TextBoxGrandTotal.Enabled = false;
            TextBoxTotalRemainBalance.Enabled = false;
            TextBoxGrandCheckNumber.Visible = false;
            LabelGrandCheckNumber.Visible = false;

        }

        protected void ButtonPrintInvoice_Click(object sender, EventArgs e)
        {
            string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
            HttpContext.Current.Response.Redirect(String.Format("{0}InvoicePrint.aspx?InvoiceNumber={1}", rootURL, TextBoxInvoiceNumber.Text));
            
        }
    }



}