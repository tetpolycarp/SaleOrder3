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

namespace SaintPolycarp.BanhChung
{
    public partial class InvoiceMobile : System.Web.UI.Page
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
            string currentPage = GetCurrentPageName();
            if (!IsPostBack)
            {
                //only need to require and login when it's InvoiceMobile.
                //however, if InvoiceMobileReceipt, then it should skip authentication
                if (!User.Identity.IsAuthenticated && currentPage.Equals("InvoiceMobile"))
                {
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    string OriginalUrl = HttpContext.Current.Request.RawUrl;
                    string LoginPageUrl = rootURL + "Account/Login.aspx";
                    HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
                }


                InitialData();
                LabelInvoiceStatus.Text = InvoicePickupStatusEnum.Not_Pickup.ToString() + ", " + InvoicePaidStatusEnum.Unpaid.ToString(); //need to use this instead of using SetInvoiceStatus()
                LabelInvoiceDate.Text = DateTime.Now.Date.ToShortDateString();


                string invoiceNumber = Request.QueryString["invoicenumber"];
                string IsJustCreated = Request.QueryString["IsJustCreated"];

                //if this is loading an existing Invoice, then load this information.
                if (!string.IsNullOrEmpty(invoiceNumber))
                {

                    if (!string.IsNullOrEmpty(IsJustCreated))
                    {
                        if (IsJustCreated.ToUpper().Equals("TRUE"))
                        {
                            //if we it's is edit mode, then we want to display the button so we can save
                            ButtonSave.Visible = true;
                            LoadExistingInvoice(invoiceNumber, true);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('New invoice " + invoiceNumber + " has created." + "');", true);
                        }
                    }
                    else
                    {
                        LoadExistingInvoice(invoiceNumber, false);
                        ImageViewOnly.Visible = true;
                    }
                    
                    PanelInvoiceInfo.Visible = true;

                }
                //if we're about to create the new invoice, then display the button so we can create it
                else
                {
                    ButtonSave.Visible = true;
                }

                LabelInvoiceType.Text = "Invoice Type: " + RadioButtonListInvoiceType.SelectedValue;
            }


        }

        private void InitialData()
        {
            LabelInvoiceCreatedBy.Text = Context.User.Identity.Name + " on mobile";




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

        public string GetCurrentPageName()
        {
            string sPath = Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }
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


                    RadDropDownList saleItemRadDropDown = item.FindControl("RadDropDownListSaleItem") as RadDropDownList;
                    //saleItemDataTable already loaded in the InitialzedData
                    saleItemRadDropDown.DataSource = saleItemDataTable;
                    saleItemRadDropDown.DataTextField = "name";
                    saleItemRadDropDown.DataValueField = "name";
                    saleItemRadDropDown.DataBind();

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
            /*    try
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
                            if(dropdownListOrderNumber.SelectedValue.Equals(item["OrderNumber"].Text))
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
                */
        }
        protected void RadDatePickerInvoicePickupDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            /*    try
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
                */
        }





        protected void DropDownListTakeOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void DropDownListSaleItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*    try
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

                            textboxUnitPrice.Text = string.Format(CURRENCY_FORMAT, double.Parse(selectedSaleItem.saleprice));

                            //copy the behavior from Inflow, whenever changing the Sale Item, reset the Discount = 0 and Quantity = 1
                            textboxDiscount.Text = string.Format(CURRENCY_FORMAT, 0);
                            textboxQuantity.Text = "1";

                            //calculate SubTotal, because every when changing the sale item, copy the the behavior from Inflow, assuming the Quantity is always start with 1 and the Discount is always 0
                            //therefore, set SubTotal to be the same as Unit Price
                            textboxSubTotal.Text = string.Format(CURRENCY_FORMAT, double.Parse(selectedSaleItem.saleprice));

                            //if Invoice Simple Type
                            if (RadioButtonListInvoiceType.SelectedIndex == 0)
                            {
                                UpdateBalancesWhenSaveInvoiceForSimpleType(item["Index"].Text, textboxSubTotal.Text);
                                TextBoxTotalAlreadyPaid.Enabled = true;
                                TextBoxTotalAlreadyPaid.BackColor = Color.Orange;


                            }
                            //if Invoice Complex Type
                            else
                            {
                               //refresh the Order Table with the udpated values and other Balances
                                UpdateOrderTableAndOtherBalancesWhenSaveInvoiceForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, textboxSubTotal.Text);
                                TextBoxTotalAlreadyPaid.Enabled = false;
                                TextBoxTotalAlreadyPaid.BackColor = default(Color);
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
                            UpdateBalancesWhenSaveInvoiceForSimpleType(item["Index"].Text, "0");
                        }
                        else
                        {
                            //refresh the Order Table with the udpated values and other Balances
                            //there is Subtotal = 0 to refresh the balance
                            UpdateOrderTableAndOtherBalancesWhenSaveInvoiceForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, "0");
                        }


                    }

                }
                catch(Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DropDownListSaleItem SelectedIndexChanged(): " + ex.Message + "');", true);
                }
                */
        }

        private void UpdatePickupDateInSaleItems(string orderNumber, GridDataItem saleItem)
        {
            //if this is the complex invoice (index #1), then base on the selected date in Order Table
            /*     if (RadioButtonListInvoiceType.SelectedIndex == 1)
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
                 }*/
        }
        protected void DropDownListOrderNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*   try
               {
                   DropDownList dropdownListOrderNumber = (DropDownList)sender;
                   GridDataItem item = (GridDataItem)dropdownListOrderNumber.NamingContainer;
                   item.Selected = true;
                   TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");

                   //if invoice type is Simple
                   if (RadioButtonListInvoiceType.SelectedIndex == 0)
                   {
                       UpdateBalancesWhenSaveInvoiceForSimpleType(item["Index"].Text, textboxSubTotal.Text);
                   }
                   //if invoice type is complex\
                   else
                   {
                     //refresh the Order Table with the udpated values and other Balances
                       UpdateOrderTableAndOtherBalancesWhenSaveInvoiceForComplexType(item["Index"].Text, dropdownListOrderNumber.SelectedValue, textboxSubTotal.Text);
                   }

                   //update  Pickup Date
                   UpdatePickupDateInSaleItems(dropdownListOrderNumber.SelectedValue, item);
               }
               catch (Exception ex)
               {
                   Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DropDownListOrderNumber SelectedIndexChanged(): " + ex.Message + "');", true);
               }
   */
        }
        protected void TextBoxAlreadyPaid_TextChanged(object sender, EventArgs e)
        {
            /*    try
                {
                    TextBox textboxAlreadyPaid = (TextBox)sender;
                    GridDataItem item = (GridDataItem)textboxAlreadyPaid.NamingContainer;
                    item.Selected = true;

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

                    UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTableForComplexType(item["OrderNumber"].Text, textboxTotal.Text, textboxAlreadyPaid.Text);


                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxAlreadyPaid_TextChanged(): " + ex.Message + "');", true);
                }*/
        }
        protected void TextBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }
        protected void TextBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            //    CalculateSubTotal(sender);
        }
        protected void TextBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            //     CalculateSubTotal(sender);
        }
        protected void TextBoxTotal_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateBalancesWhenSaveInvoiceForSimpleType()
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;

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
                    RadDropDownList dropdownListSaleItem = (RadDropDownList)item.FindControl("RadDropDownListSaleItem");
                    //for the remaining row, if there is an sale item selected
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedText))
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
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in UpdateBalancesWhenSaveInvoiceForSimpleType() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
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

            SyncPaidStatusAndGrandTotal(grandTotal, alreadyPaidGrandTotal);

        }
        private void UpdateOrderTableAndOtherBalancesWhenSaveInvoiceForComplexType()
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;
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
                    RadDropDownList dropdownListSaleItem = (RadDropDownList)item.FindControl("RadDropDownListSaleItem");
                    int orderIndexInSaleItemsGrid;

                    //for the remaining row, if there is an sale item selected
                    if (!string.IsNullOrEmpty(dropdownListSaleItem.SelectedText))
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
                    TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                    RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                    //because in the contructor of the object, we initial it as 0. Therefore, to make sure only set when it's not 0
                    if (orders[orderIndexInOrdersGrid].Total != 0)
                    {
                        textboxAlreadyPaid.Enabled = true;
                        radDatePickerPickupDate.Enabled = true;

                        textboxAlreadyPaid.BackColor = Color.Orange;
                        radDatePickerPickupDate.BackColor = Color.Orange;

                        textboxTotal.Text = string.Format(CURRENCY_FORMAT, orders[orderIndexInOrdersGrid].Total);

                        //if there is no paid, then the remaind balance is the same amount as the Total
                        if (string.IsNullOrEmpty(textboxAlreadyPaid.Text))
                        {
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, orders[orderIndexInOrdersGrid].Total);
                            //already paid is 0
                            textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                        }
                        //if there is some amount that already paid, then subtract it from the Total to get the Remain Balance
                        else
                        {
                            double paidAmount = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                            alreadyPaidGrandTotal += paidAmount;
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, orders[orderIndexInOrdersGrid].Total - paidAmount);
                        }
                        grandTotal += Convert.ToDouble(textboxTotal.Text.Substring(2));

                    }
                    else
                    {
                        textboxAlreadyPaid.Enabled = false;
                        radDatePickerPickupDate.Enabled = false;

                        textboxAlreadyPaid.BackColor = default(Color);
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

            SyncPaidStatusAndGrandTotal(grandTotal, alreadyPaidGrandTotal);


        }

        private void UpdateOrderTableAndOtherBalancesWhenChangesInOrdersTableForComplexType(string orderNumberInSelectedRow, string totalInSelectedRow, string alreadyPaidInSelectedRow)
        {
            double grandTotal = 0;
            double alreadyPaidGrandTotal = 0;
            double totalBalance = 0;
            //parse through all the rows in the second table, Orders, to update the Total and Remain Balance
            foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                    TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                    TextBox textboxRemainBalance = (TextBox)item.FindControl("TextBoxRemainBalance");
                    RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");

                    //if the select row, then get all the value from parameter
                    //if not, then read the value from the table
                    if (item["OrderNumber"].Text.Equals(orderNumberInSelectedRow))
                    {
                        textboxAlreadyPaid.Enabled = true;
                        radDatePickerPickupDate.Enabled = true;

                        textboxAlreadyPaid.BackColor = Color.Orange;
                        radDatePickerPickupDate.BackColor = Color.Orange;

                        double total = Convert.ToDouble(totalInSelectedRow.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                        //if there is no paid, then the remaind balance is the same amount as the Total
                        if (string.IsNullOrEmpty(alreadyPaidInSelectedRow))
                        {
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total);
                            //already paid is 0
                            textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                        }
                        //if there is some amount that already paid, then subtract it from the Total to get the Remain Balance
                        else
                        {
                            double paidAmount = Convert.ToDouble(alreadyPaidInSelectedRow.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                            alreadyPaidGrandTotal += paidAmount;
                            textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total - paidAmount);
                        }
                        grandTotal += Convert.ToDouble(textboxTotal.Text.Substring(2));

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(textboxTotal.Text))
                        {
                            double total = Convert.ToDouble(textboxTotal.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                                                                                             //if there is no paid, then the remaind balance is the same amount as the Total
                            if (string.IsNullOrEmpty(alreadyPaidInSelectedRow))
                            {
                                textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total);
                                //already paid is 0
                                textboxAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                            }
                            //if there is some amount that already paid, then subtract it from the Total to get the Remain Balance
                            else
                            {
                                double paidAmount = Convert.ToDouble(textboxAlreadyPaid.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                                alreadyPaidGrandTotal += paidAmount;
                                textboxRemainBalance.Text = string.Format(CURRENCY_FORMAT, total - paidAmount);
                            }
                            grandTotal += Convert.ToDouble(textboxTotal.Text.Substring(2));
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

            totalBalance = grandTotal - alreadyPaidGrandTotal;
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
        private void CalculateSubTotal(int rowIndex, DropDownList dropdownListOrderNumber, TextBox textboxUnitPrice, TextBox textboxQuantity, TextBox textboxDiscount, TextBox textboxSubTotal)
        {

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


            }
            catch (Exception ex)
            {
                textboxSubTotal.Text = string.Format(CURRENCY_FORMAT, 0);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in Calculate SubTotal(): " + ex.Message + "');", true);
            }
        }

        protected void ImageButtonNotPickedUp_Click(object sender, ImageClickEventArgs e)
        {
            /*     ImageButton imageButton = (ImageButton)sender;
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
                 SyncPickupStatus(item["Index"].Text, true);*/
        }
        protected void ImageButtonPickedUp_Click(object sender, ImageClickEventArgs e)
        {
            /*     ImageButton imageButton = (ImageButton)sender;
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
                 SyncPickupStatus(item["Index"].Text, false);*/
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
        private void SyncPaidStatusAndGrandTotal(double grandTotal, double alreadyPaidGrandTotal)
        {
            //set the Grand Total
            TextBoxGrandTotal.Text = string.Format(CURRENCY_FORMAT, grandTotal);
            TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaidGrandTotal);

            double totalBalance = grandTotal - alreadyPaidGrandTotal;
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
            /*     if(RadioButtonListInvoiceType.SelectedValue.Contains("Complex"))
                 {
                     RadGridOrders.Visible = true;
                     LabelInvoicePickupDate.Visible = false;
                     RadDatePickerInvoicePickupDate.Visible = false;
                     TextBoxTotalAlreadyPaid.Enabled = false;
                     TextBoxTotalAlreadyPaid.BackColor = default(Color);
                     RadGridSaleItems.Columns[3].Visible = true; //display the Order Number column

                 }
                 else
                 {
                     RadGridOrders.Visible = false;
                     LabelInvoicePickupDate.Visible = true;
                     RadDatePickerInvoicePickupDate.Visible = true;
                     TextBoxTotalAlreadyPaid.Enabled = true;
                     TextBoxTotalAlreadyPaid.BackColor = Color.Orange;
                     RadGridSaleItems.Columns[3].Visible = false; //hide the Order Number column
                 }
                 RadGridSaleItems.Rebind();
                 RadGridOrders.Rebind();
                 RadDatePickerInvoicePickupDate.SelectedDate = null;
                 LabelGrandTotal.Text = string.Format(CURRENCY_FORMAT, 0);
                 TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                 TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, 0);
                 SetInvoiceStatus(InvoicePickupStatusEnum.Not_Pickup.ToString(), InvoicePaidStatusEnum.Unpaid.ToString());*/
        }

        protected void TextBoxTotalAlreadyPaid_TextChanged(object sender, EventArgs e)
        {
            /*     try
                 {
                     double alreadyPaid = 0;
                     if (!string.IsNullOrEmpty(TextBoxTotalAlreadyPaid.Text))
                     {
                         //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                         if (TextBoxTotalAlreadyPaid.Text.StartsWith("$ "))
                         {
                             //since the value of the field is Currency format, so have to exclude the first 2 character then convert it into double
                             alreadyPaid = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(2));
                         }
                         //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                         else if (TextBoxTotalAlreadyPaid.Text.StartsWith("$"))
                         {
                             //since the value of the field is Currency format, so have to exclude the first character then convert it into double
                             alreadyPaid = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text.Substring(1));
                         }
                         else
                         {
                             alreadyPaid = Convert.ToDouble(TextBoxTotalAlreadyPaid.Text);
                         }
                         TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, alreadyPaid);

                     }
                     else
                     {
                         TextBoxTotalAlreadyPaid.Text = string.Format(CURRENCY_FORMAT, 0);
                         alreadyPaid = 0;
                     }

                     double grandTotal = Convert.ToDouble(LabelGrandTotal.Text.Substring(2)); //take out the first 2 char which is "$ ". ie: $ 17.00
                     double totalBalance = grandTotal - alreadyPaid;
                     TextBoxTotalRemainBalance.Text = string.Format(CURRENCY_FORMAT, totalBalance);

                     if(totalBalance == 0)
                     {
                         SetInvoiceStatus("", InvoicePaidStatusEnum.Paid.ToString());
                     }
                     else if(totalBalance == grandTotal)
                     {
                         SetInvoiceStatus("", InvoicePaidStatusEnum.Unpaid.ToString());
                     }
                     else
                     {
                         SetInvoiceStatus("", InvoicePaidStatusEnum.Partial_Paid.ToString());
                     }
                 }
                 catch (Exception ex)
                 {
                     Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in TextBoxTotalAlreadyPaid_TextChanged(): " + ex.Message + "');", true);
                 }*/
        }
        private void SetInvoiceStatus(string pickupStatus = "", string payStatus = "")
        {
            try
            {


                //get the current status into array
                string[] invoiceStatus = LabelInvoiceStatus.Text.Split(',');
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
                    LabelInvoiceStatus.Text = currentPickupStatus + ", " + currentPayStatus;

                    //there is situtation like InvoiceMobileReceipt that doesn't have master page, so need to check and make sure
                    if (this.Page.Master != null)
                    {
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
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetInvoiceStatus(): " + ex.Message + "');", true);
            }

        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                CreateInvoice();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in ButtonSave_Click()" + "." + ex.Message + "');", true);
            }
        }

        private void CreateInvoice()
        {
            try
            {
                List<OrderInvoice> orderInvoices = SharedMethod.GetInvoices();

                //if invoice number is empty, which meant this is creating new invoice
                if (string.IsNullOrEmpty(LabelInvoiceNumber.Text))
                {
                    //query the database to get the next index
                    //if the collection is empty, then the index start from 101
                    if (orderInvoices.Count == 0)
                    {
                        LabelInvoiceNumber.Text = "101";
                    }
                    else
                    {
                        int latestInvoiceNumber = orderInvoices.Select(i => Convert.ToInt16(i.InvoiceNumber)).Max();

                        //increment the max number which is the current new invoice
                        latestInvoiceNumber += 1;

                        //set the Invoice Number
                        LabelInvoiceNumber.Text = latestInvoiceNumber.ToString();
                    }

                    OrderInvoice invoice = new OrderInvoice();
                    SetThenSaveInvoiceObject(invoice, true, LabelInvoiceNumber.Text);

                }
                //is the invoice is NOT empty, then this is editing the invoice
                else
                {
                    OrderInvoice invoice = orderInvoices.Find(x => x.InvoiceNumber.Equals(LabelInvoiceNumber.Text));
                    if (invoice != null)
                    {
                        SetThenSaveInvoiceObject(invoice, false, LabelInvoiceNumber.Text);


                    }
                    else
                    {
                        throw new Exception("Can't find the existing Invoice Number" + LabelInvoiceNumber.Text + " in the dabase");
                    }
                }


                //query the invoice object and will get it overwritten with the new object


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SaveInvoice()" + "." + ex.Message + "');", true);
            }

        }
        private void SetThenSaveInvoiceObject(OrderInvoice invoice, bool newInvoice, string invoiceNumber)
        {
            invoice.InvoiceNumber = LabelInvoiceNumber.Text;
            invoice.InvoiceDate = LabelInvoiceDate.Text;
            invoice.Status = LabelInvoiceStatus.Text;
            invoice.PickupDate = RadDatePickerInvoicePickupDate.SelectedDate.ToString();
            invoice.InvoiceType = RadioButtonListInvoiceType.SelectedValue;

            invoice.LastUpdateBy = User.Identity.Name;
            invoice.LastUpdateOn = DateTime.Now.ToString("MM/dd HH:mm");

            invoice.CreatedBy = Context.User.Identity.Name + " on mobile";
            


            if (string.IsNullOrEmpty(RadTextBoxCustomerName.Text))
            {
                invoice.CustomerName = "";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('No Customer Name. Try again!');", true);
                return;
            }
            else
            {
                invoice.CustomerName = RadTextBoxCustomerName.Text;
            }

            if (string.IsNullOrEmpty(RadMaskedTextBoxPhoneNumber.Text))
            {
                invoice.PhoneNumber = "";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('No Phone Number. Try again!');", true);
                return;
            }
            else
            {
                invoice.PhoneNumber = RadMaskedTextBoxPhoneNumber.Text;
            }

            int rowIndex = 1;
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
                    ImageButton imageButtonPickedup = (ImageButton)item.FindControl("ImageButtonPickedUp");
                    RadDropDownList radDropdownListSaleItem = (RadDropDownList)item.FindControl("RadDropDownListSaleItem");
                    //if the first item is not selected, then don't borther to save
                    if (rowIndex == 1 && string.IsNullOrEmpty(radDropdownListSaleItem.SelectedText))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('First sale item is not selected. Try again!');", true);
                        return;
                    }
                    //if the item is selected but there is no quantity, then stop
                    if (!string.IsNullOrEmpty(radDropdownListSaleItem.SelectedText) && string.IsNullOrEmpty(textboxQuantity.Text))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('Selected item #" + rowIndex.ToString() + ", " + radDropdownListSaleItem.SelectedText + ", but no quantity.  Enter a number then save again!');", true);
                        return;
                    }
                    //for the remaining row, if there is an sale item selected
                    if (!string.IsNullOrEmpty(radDropdownListSaleItem.SelectedText))
                    {
                        //**************** Different in MOBILE, we have to set all of these fields when selecting SAVE
                        //get the unit price for select item
                        SaleItem selectedSaleItem = saleItems.First(x => x.name.Equals(radDropdownListSaleItem.SelectedText));
                        textboxUnitPrice.Text = string.Format(CURRENCY_FORMAT, double.Parse(selectedSaleItem.saleprice));
                        //copy the behavior from Inflow, whenever changing the Sale Item, reset the Discount = 0 and Quantity = 1
                        textboxDiscount.Text = string.Format(CURRENCY_FORMAT, 0);

                        CalculateSubTotal(rowIndex, dropdownListOrderNumber, textboxUnitPrice, textboxQuantity, textboxDiscount, textboxSubTotal);

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
                        //*************************************


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
                        orderSaleItem.SaleItem = radDropdownListSaleItem.SelectedText;
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
                rowIndex++;
            }

            //if invoice type is Simple
            if (RadioButtonListInvoiceType.SelectedIndex == 0)
            {
                UpdateBalancesWhenSaveInvoiceForSimpleType();
            }
            else
            {
                UpdateOrderTableAndOtherBalancesWhenSaveInvoiceForComplexType();
            }

            //after calculate the balance, then store it in invoice object
            invoice.GrandTotal = TextBoxGrandTotal.Text;
            invoice.AlreadyPaid = TextBoxTotalAlreadyPaid.Text;
            invoice.RemainBalance = TextBoxTotalRemainBalance.Text;
            invoice.Notes = TextBoxNote.Text;

            //set list of order sale items to invoice object
            invoice.OrderSaleItems = orderSaleItems;

            //if this is the complex invoice, then need to store the information of Order Table
            if (RadioButtonListInvoiceType.SelectedIndex == 1)
            {
                List<RadGridOrder> orders = new List<RadGridOrder>();
                foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
                {
                    try
                    {
                        TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                        TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
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
                catch (Exception ex)
                {

                }
                //redirect the page to the Invoice with the existing one with 2 parameters after saving the invoice
                string redirectURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/InvoiceMobile?IsJustCreated=true&InvoiceNumber=" + invoiceNumber);
                HttpContext.Current.Response.Redirect(redirectURL);
            }
            //this is the an existing invoice, then replace with the existing on
            else
            {
                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    SharedMethod.SaveInvoice(invoice);

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "MessageBox", "alert('Existing invoice " + invoiceNumber + " has updated');", true);

                }
            }

            PanelInvoiceInfo.Visible = true;


        }
        private void LoadExistingInvoice(string invoiceNumber, bool IsJustCreated)
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

                    LabelInvoiceNumber.Text = invoice.InvoiceNumber;
                    LabelInvoiceDate.Text = invoice.InvoiceDate;
                    LabelInvoiceStatus.Text = invoice.Status;

                    string[] invoicePickupPaidStatus = invoice.Status.Split(new char[] { ',' });

                    //evaluate whether to display the Pickup/Paid icon
                    if (invoicePickupPaidStatus.Length > 1)
                    {
                        SetInvoiceStatus(invoicePickupPaidStatus[0].Trim(), invoicePickupPaidStatus[1].Trim());
                    }

                    if (!string.IsNullOrEmpty(invoice.PickupDate))
                    {
                        RadDatePickerInvoicePickupDate.SelectedDate = Convert.ToDateTime(invoice.PickupDate);
                        RadDatePickerInvoicePickupDate.ForeColor = Color.Red;
                    }
                    RadioButtonListInvoiceType.SelectedValue = invoice.InvoiceType;

                    TextBoxGrandTotal.Text = invoice.GrandTotal;
                    TextBoxTotalAlreadyPaid.Text = invoice.AlreadyPaid;
                    TextBoxTotalRemainBalance.Text = invoice.RemainBalance;
                    TextBoxNote.Text = invoice.Notes;

                    if (!string.IsNullOrEmpty(invoice.CreatedBy))
                    {
                        LabelInvoiceCreatedBy.Text = invoice.CreatedBy;
                    }


                    RadTextBoxCustomerName.Text = invoice.CustomerName;
                    RadTextBoxCustomerName.ForeColor = Color.Red;

                    RadMaskedTextBoxPhoneNumber.Text = invoice.PhoneNumber;
                    RadMaskedTextBoxPhoneNumber.ForeColor = Color.Red;

                    int saleItemIndex = 0;
                    List<RadGridSaleItem> orderSaleItems = invoice.OrderSaleItems;
                    foreach (RadGridSaleItem order in orderSaleItems)
                    {
                        GridDataItem item = null;
                        try
                        {
                            //if each of the record was found with the select sale item, then write it into the table
                            if (!string.IsNullOrEmpty(order.SaleItem))
                            {
                                //get the index of the table
                                int index = Convert.ToInt16(order.Index);
                                //now get the row at this index minus 1
                                item = RadGridSaleItems.MasterTableView.Items[saleItemIndex++];

                                TextBox textboxUnitPrice = (TextBox)item.FindControl("TextBoxUnitPrice");
                                TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                                textboxQuantity.Enabled = false;
                                TextBox textboxDiscount = (TextBox)item.FindControl("TextBoxDiscount");
                                TextBox textboxSubTotal = (TextBox)item.FindControl("TextBoxSubTotal");
                                TextBox textboxPickupDate = (TextBox)item.FindControl("TextBoxPickupDate");
                                DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                                if (RadioButtonListInvoiceType.SelectedValue.ToUpper().Contains("COMPLEX"))
                                {
                                    dropdownListOrderNumber.Enabled = true;
                                }

                                RadDropDownList dropdownListSaleItem = (RadDropDownList)item.FindControl("RadDropDownListSaleItem");
                                dropdownListSaleItem.Enabled = false;
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
                                textboxQuantity.BackColor = Color.Orange;

                                textboxUnitPrice.Text = order.UnitPrice;
                                textboxDiscount.Text = order.Discount;
                                textboxDiscount.Enabled = true;
                                textboxDiscount.BackColor = Color.Orange;
                                textboxSubTotal.Text = order.SubTotal;

                                //if we found at least a sale item, then load up the Grand Total and other balances
                                PanelSummary.Visible = true;
                                PanelSummaryBalance.Visible = true;
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

                        int orderIndex = 0;
                        List<RadGridOrder> invoiceOrders = invoice.InvoiceOrders;
                        foreach (RadGridOrder invoiceOrder in invoiceOrders)
                        {
                            GridDataItem item = null;
                            try
                            {
                                item = RadGridOrders.MasterTableView.Items[orderIndex++];

                                TextBox textboxTotal = (TextBox)item.FindControl("TextBoxTotal");
                                TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
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
                                textboxRemainBalance.Text = invoiceOrder.RemainBalanceString;

                            }
                            catch (Exception ex)
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetThenSaveInvoice in Orders: " + item["Index"].Text + "." + ex.Message + "');", true);

                            }
                        }
                    }



                    //set controls whether to make it enable or disable depend on this is the Edit Mode or not
                    SetControls(IsJustCreated, orderSaleItems);
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

        //when IsJustCreated=true, then set all these controls to be true and vice versa
        private void SetControls(bool IsJustCreated, List<RadGridSaleItem> orderSaleItems)
        {
            RadTextBoxCustomerName.Enabled = IsJustCreated;
            RadMaskedTextBoxPhoneNumber.Enabled = IsJustCreated;
            RadDatePickerInvoicePickupDate.Enabled = IsJustCreated;
            TextBoxNote.Enabled = IsJustCreated;

            foreach (GridDataItem item in RadGridSaleItems.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxQuantity = (TextBox)item.FindControl("TextBoxQuantity");
                    textboxQuantity.Enabled = IsJustCreated;

                    RadDropDownList dropdownListSaleItem = (RadDropDownList)item.FindControl("RadDropDownListSaleItem");
                    dropdownListSaleItem.Enabled = IsJustCreated;

                    DropDownList dropdownListOrderNumber = (DropDownList)item.FindControl("DropDownListOrderNumber");
                    dropdownListOrderNumber.Enabled = IsJustCreated;

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetControls() processing RadGridSaleItems at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }

            foreach (GridDataItem item in RadGridOrders.MasterTableView.Items)
            {
                try
                {
                    TextBox textboxAlreadyPaid = (TextBox)item.FindControl("TextBoxAlreadyPaid");
                    textboxAlreadyPaid.Enabled = IsJustCreated;

                    RadDatePicker radDatePickerPickupDate = (RadDatePicker)item.FindControl("RadDatePickerPickupDate");
                    radDatePickerPickupDate.Enabled = IsJustCreated;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in SetControls() processing RadGridOrders at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                }
            }
        }


    } 
}