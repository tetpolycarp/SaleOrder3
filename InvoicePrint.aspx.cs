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
    public partial class InvoicePrint : System.Web.UI.Page
    {

        private string identity = "";
        private const string CURRENCY_FORMAT = "$ {0:#,##0.00}";
        private const int NUMBER_OF_ORDER = 15;
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
                    InitialData(invoiceNumber);
                    LoadExistingInvoice(invoiceNumber);
                }

            }
       
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:printpage(); ", true);
        }

        private void LoadExistingInvoice(string invoiceNumber)
        {
            try
            {
                OrderInvoice invoice = SharedMethod.GetInvoice(invoiceNumber);


                //if the invoice number not found
                if (invoice == null)
                {
                    LabelServeError.Text = "Invoice #" + invoiceNumber + " - Not Found";

                }
                //if the invoice found, then load the data
                else
                {
                    Page.Title = "Print Inv#" + invoice.InvoiceNumber;
                    LabelInvoiceNumber.Text += invoice.InvoiceNumber;
                    LabelInvoiceDate.Text += invoice.InvoiceDate;
                    LabelStatus.Text += invoice.Status;
                    LabelCreatedBy.Text += invoice.CreatedBy;
                    if(invoice.InvoiceType.Equals("Complex"))
                    {
                        LabelPageTitle.Text = "HÓA ĐƠN (*)";
                    }
                    /*if (!string.IsNullOrEmpty(invoice.LastUpdateBy))
                    {
                        LabelLastUpdatedBy.Text += invoice.LastUpdateBy;
                       
                    }*/
                    
                    LabelGrandTotal.Text += invoice.GrandTotal;
                    LabelTotalAlreadyPaid.Text += invoice.AlreadyPaid;
                    if (string.IsNullOrEmpty(invoice.AlreadyPaidByCheck))
                    {
                        LabelTotalAlreadyPaidByCheck.Text += "$ 0.00"; ;
                    }
                    else
                    {
                        LabelTotalAlreadyPaidByCheck.Text += invoice.AlreadyPaidByCheck;
                    }

                    LabelTotalRemainBalance.Text += invoice.RemainBalance;
                    if (!string.IsNullOrEmpty(invoice.Notes))
                    {
                        LiteralNotes.Text = "Notes:" + invoice.Notes;
                    }




                    LabelCustomer.Text += invoice.CustomerName;
                    try
                    {
                        double phoneNumber = Convert.ToDouble(invoice.PhoneNumber);
                        LabelPhoneNumber.Text += String.Format("{0:(###) ###-####}", phoneNumber);
                    }
                    catch(Exception ex)
                    {

                    }
                    int saleItemIndex = 0;
                    List<RadGridSaleItem> orderSaleItems = invoice.OrderSaleItems;
                    //set the number of rows for this table
                    RadGridSaleItems.PageSize = orderSaleItems.Count;
                    RadGridSaleItems.Rebind();

                    foreach (RadGridSaleItem order in orderSaleItems)
                    {
                        GridDataItem item = null;
                        try
                        {
                            //if each of the record was found with the select sale item, then write it into the table
                            if (!string.IsNullOrEmpty(order.SaleItem))
                            {
                                
                                item = RadGridSaleItems.MasterTableView.Items[saleItemIndex++];
                                item["PickupDate"].Text = order.PickupDate;
                                item["PickedUp"].Text = order.PickedUp;
                                if(!string.IsNullOrEmpty(order.OrderNumber)) //only for complex type
                                {
                                    item["OrderNumber"].Text = order.OrderNumber;
                                }
                                item["SaleItem"].Text = order.SaleItem;
                                item["Quantity"].Text = order.Quantity;
                                item["UnitPrice"].Text = order.UnitPrice;
                                item["DiscountPerUnit"].Text = order.Discount;
                                item["SubTotal"].Text = order.SubTotal;
                            }
                        }
                        catch (Exception ex)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadSaleItem in SaleItems() at index: " + item["Index"].Text + "." + ex.Message + "');", true);
                        }
                    }

              
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadExistingInvoice()" + "." + ex.Message + "');", true);
            }
        }
        private void InitialData(string invoiceNumber)
        {
       

            //**************Initial Sale Items
            saleItems = SharedMethod.GetSaleItems();
            OrderInvoice invoice = SharedMethod.GetInvoice(invoiceNumber);
            List<RadGridSaleItem> orderSaleItems = invoice.OrderSaleItems;

            //****************** Load Initial Order Number
            //convert into DataTable. It will be use in ItemDataBound
            saleItemDataTable = NewTable("SaleItemTable", saleItems);

            for (int i = 1; i <= NUMBER_OF_ORDER; i++)
            {
                orderNumber.Add("#" + i.ToString());
            }

            //prepopulate with empty row
            listOfRadGridSaleItems = new List<RadGridSaleItem>();
            for (int i = 1; i <= orderSaleItems.Count; i++)
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




    }



}