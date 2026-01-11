using System;
using System.Collections.Generic;
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
    public partial class PickupForecast : System.Web.UI.Page
    {
        private const string PHONE_FORMAT = "{0:(###) ###-####}";
        static List<SaleItem> saleItems = new List<SaleItem>();
        static List<OrderInvoice> orderInvoices = new List<OrderInvoice>();

        static List<RadGridOrderDetail> listOfBanhChung = new List<RadGridOrderDetail>();
        static List<RadGridOrderDetail> listOfBanhTet = new List<RadGridOrderDetail>();
        public static DataTable dt;

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
                else
                {
                    //check again, if user login but not inventory admin, then reject as well.
                    //if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "inventory admin"))
                    //1/16/25 - Allow for all the users to see this page, except when they try to look for all items already picked-up or not pickup. It's privacy.
                    //therefore, when the queryString doesn't have "NumberOfDays" and "Date", which means it has queryString for "Pickup" or not, then the caller want to get the total of already pickup or not pickup yet, we only allow "inventory admin"
           
                    if (string.IsNullOrEmpty(Request.QueryString["NumberOfDays"]) && string.IsNullOrEmpty(Request.QueryString["Date"]) && !SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "inventory admin"))
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                    }
                }
                LoadPickupForecast();

                LabelBanhChung.Text = "- Bánh Chưng: " + CalculateAmount(RadGridForecastBanhChung);

                LabelBanhTet.Text = "- Bánh Tét: " + CalculateAmount(RadGridForecastBanhTet);

                if (!string.IsNullOrEmpty(Request.QueryString["Date"]))
                {
                    LabelForecastMessage.Text = "Pickup " + Request.QueryString["Date"] + ":";
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["NumberOfDays"]))
                {
                    LabelForecastMessage.Text = "Pickup within next " + Request.QueryString["NumberOfDays"] + " day(s):";
                }
            }


        }
        private void LoadPickupForecast()
        {
            try
            {
                //RadGridForecastBanhChung is the table that lists all the Order detail in this page
                listOfBanhChung = new List<RadGridOrderDetail>();
                listOfBanhTet = new List<RadGridOrderDetail>();
                List<OrderInvoice> orderInvoices = SharedMethod.GetInvoices();
                //parse through each of the invoice
                foreach (OrderInvoice orderInvoice in orderInvoices)
                {
                    try
                    {
                        //traverse each of the sale item in the invoice
                        //RadGridSaleItem is the Table in Invoice page
                        List<RadGridSaleItem> saleItems = orderInvoice.OrderSaleItems;
                        foreach (RadGridSaleItem saleItem in saleItems)
                        {
                            //only care about Banh Chung and Banh Tet
                            if (saleItem.SaleItem.Contains("Chưng") || saleItem.SaleItem.Contains("Tét"))
                            {
                                //want to display all the pickup items
                                if (!string.IsNullOrEmpty(Request.QueryString["Pickup"]))
                                {
                                    if (Request.QueryString["Pickup"].ToUpper().Equals("ALL"))
                                    {
                                        LabelForecastMessage.Text = "Already picked-up amount:";
                                        //now only add up the pickup items
                                        if (saleItem.PickedUp.ToUpper().Equals("YES"))
                                        {
                                            //******************************** END *******************************************

                                            RadGridOrderDetail orderDetail = new RadGridOrderDetail();
                                            //set the data from Invoice object
                                            orderDetail.InvoiceNumber = orderInvoice.InvoiceNumber;


                                            if (string.IsNullOrEmpty(orderInvoice.CreatedBy))
                                            {
                                                orderDetail.CreatedBy = "";
                                            }
                                            else
                                            {
                                                orderDetail.CreatedBy = orderInvoice.CreatedBy;
                                            }

                                            orderDetail.Link = HttpUtility.HtmlDecode("&lt;a href='http://" + HttpContext.Current.Request.Url.Authority + "/Invoice.aspx?InvoiceNumber=" + orderInvoice.InvoiceNumber + "' target='_blank' title='Open invoice #" + orderInvoice.InvoiceNumber + "' &gt; &lt;span style='text-decoration: underline'&gt; link &lt;/span&gt; &lt;/ a &gt;");
                                            orderDetail.CustomerName = orderInvoice.CustomerName;
                                            if (!string.IsNullOrEmpty(orderInvoice.PhoneNumber))
                                            {
                                                orderDetail.CustomerPhone = string.Format(PHONE_FORMAT, double.Parse(orderInvoice.PhoneNumber));
                                            }
                                            orderDetail.InvoiceType = orderInvoice.InvoiceType;

                                            //set the data from SaleItem table object
                                            orderDetail.PickupDate = saleItem.PickupDate;
                                            orderDetail.PickupStatus = saleItem.PickedUp;
                                            orderDetail.OrderNumber = saleItem.OrderNumber;
                                            orderDetail.OrderItem = saleItem.SaleItem;
                                            orderDetail.Quantity = saleItem.Quantity;

                                            if (saleItem.SaleItem.Contains("Chưng"))
                                            {
                                                listOfBanhChung.Add(orderDetail);
                                            }
                                            else if (saleItem.SaleItem.Contains("Tét"))
                                            {
                                                listOfBanhTet.Add(orderDetail);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //only care about those NOT PICKUP
                                    if (saleItem.PickedUp.ToUpper().Equals("NO"))
                                    {
                                        //******************************** Evaluate Date *******************************************
                                        //compare the date, if there is Date=today, tomorrow passed-in, then compare with the current day
                                        if (!string.IsNullOrEmpty(Request.QueryString["Date"])) //say from now until the next 3 days, display any other there
                                        {
                                            DateTime currentDate = DateTime.Now;
                                            DateTime currentDateToNumberOfDays = DateTime.Now.AddDays(Convert.ToInt16(Request.QueryString["NumberOfDays"]));

                                            //if the date is not selected or not there, then continue
                                            try
                                            {
                                                DateTime pickupDate = Convert.ToDateTime(saleItem.PickupDate);
                                                if (Request.QueryString["Date"].ToUpper().Equals("TODAY ONLY"))
                                                {
                                                    if (currentDate.Date == pickupDate)
                                                    {
                                                        //it's OK, doing nothing, which mean this item will be added  
                                                    }
                                                    else
                                                    {
                                                        //if it's not the same date, then skip the current loop
                                                        continue;
                                                    }
                                                }
                                                else if (Request.QueryString["Date"].ToUpper().Equals("TOMORROW ONLY"))
                                                {
                                                    if (currentDate.Date.AddDays(1) == pickupDate)
                                                    {
                                                        //it's OK, doing nothing, which mean this item will be added  
                                                    }
                                                    else
                                                    {
                                                        //if it's not the same date, then skip the current loop
                                                        continue;
                                                    }
                                                }
                                                else if (Request.QueryString["Date"].ToUpper().Equals("TODAY AND TOMORROW"))
                                                {
                                                    if (currentDate.Date <= pickupDate && currentDate.Date.AddDays(1) >= pickupDate)
                                                    {
                                                        //it's OK, doing nothing, which mean this item will be added  
                                                    }
                                                    else
                                                    {
                                                        //if it's not the same date, then skip the current loop
                                                        continue;
                                                    }
                                                }

                                            }
                                            catch (Exception e)
                                            {
                                                //if the date is not selected or throw exception, then continue
                                                continue;
                                            }

                                        }
                                        //compare the date, if there is NumberOfDays passed-in, then compare with the current day
                                        else if (!string.IsNullOrEmpty(Request.QueryString["NumberOfDays"])) //say from now until the next 3 days, display any other there
                                        {
                                            DateTime currentDate = DateTime.Now;
                                            DateTime currentDateToNumberOfDays = DateTime.Now.AddDays(Convert.ToInt16(Request.QueryString["NumberOfDays"]));

                                            //if the date is not selected or not there, then continue
                                            try
                                            {
                                                DateTime pickupDate = Convert.ToDateTime(saleItem.PickupDate);
                                                if (currentDate.Date <= pickupDate.Date && pickupDate.Date <= currentDateToNumberOfDays.Date)
                                                {
                                                    //it's OK, doing nothing, which mean this item will be added  
                                                }
                                                else
                                                {
                                                    // if it's out side number of date, then skip the current loop
                                                    continue;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //if the date is not selected or throw exception, then continue
                                                continue;
                                            }

                                        }


                                        //******************************** END *******************************************

                                        RadGridOrderDetail orderDetail = new RadGridOrderDetail();
                                        //set the data from Invoice object
                                        orderDetail.InvoiceNumber = orderInvoice.InvoiceNumber;

                                        //if this sale item already packed, then set the attribute for OrderDetail, which is list of object, that will fill into the table
                                        if (saleItem.Packed != null)
                                            if (saleItem.Packed == "yes")
                                                orderDetail.PackedForPickup = "yes";

                                        if (string.IsNullOrEmpty(orderInvoice.CreatedBy))
                                        {
                                            orderDetail.CreatedBy = "";
                                        }
                                        else
                                        {
                                            orderDetail.CreatedBy = orderInvoice.CreatedBy;
                                        }

                                        orderDetail.Link = HttpUtility.HtmlDecode("&lt;a href='http://" + HttpContext.Current.Request.Url.Authority + "/Invoice.aspx?InvoiceNumber=" + orderInvoice.InvoiceNumber + "' target='_blank' title='Open invoice #" + orderInvoice.InvoiceNumber + "' &gt; &lt;span style='text-decoration: underline'&gt; link &lt;/span&gt; &lt;/ a &gt;");
                                        orderDetail.CustomerName = orderInvoice.CustomerName;
                                        if (!string.IsNullOrEmpty(orderInvoice.PhoneNumber))
                                        {
                                            orderDetail.CustomerPhone = string.Format(PHONE_FORMAT, double.Parse(orderInvoice.PhoneNumber));
                                        }
                                        orderDetail.InvoiceType = orderInvoice.InvoiceType;

                                        //set the data from SaleItem table object
                                        orderDetail.PickupDate = saleItem.PickupDate;
                                        orderDetail.PickupStatus = saleItem.PickedUp;
                                        orderDetail.OrderNumber = saleItem.OrderNumber;
                                        orderDetail.OrderItem = saleItem.SaleItem;
                                        orderDetail.Quantity = saleItem.Quantity;

                                        if (saleItem.SaleItem.Contains("Chưng"))
                                        {
                                            listOfBanhChung.Add(orderDetail);
                                        }
                                        else if (saleItem.SaleItem.Contains("Tét"))
                                        {
                                            listOfBanhTet.Add(orderDetail);
                                        }
                                    }
                                }
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadPickupForecast() at invoice #" + orderInvoice.InvoiceNumber + ". " + ex.Message + "');", true);

                    }
                }

                RadGridForecastBanhChung.DataSource = listOfBanhChung;
                RadGridForecastBanhChung.DataBind();

                RadGridForecastBanhTet.DataSource = listOfBanhTet;
                RadGridForecastBanhTet.DataBind();


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadData(): " + ex.Message + "');", true);
            }
        }
        //from the radGridTable, parse through each of the row to calcuate each item for overall quantity, picked-up and not pick-up
        private string CalculateAmount(RadGrid RadGridForecast)
        {

            //**************Initial Sale Items
            saleItems = SharedMethod.GetSaleItems();

            int currentOverallQuantity = 0;
            int totalPackedButNotPickup = 0;
            foreach (GridDataItem item in RadGridForecast.MasterTableView.Items)
            {
                try
                {
                    //say the current row with OrderItem is "BanhChung", we need to confirm if it is in the sale item list
                    SaleItem saleItem = saleItems.Find(x => x.name.ToUpper().Equals(item["OrderItem"].Text.ToUpper()));

                    //if the sale item is found, then start 
                    if (saleItem != null)
                    {
                        //calculate the overall
                        int saleItemQuantity = Convert.ToInt16(item["Quantity"].Text); //the quantity that currently have in OrderDetails table
                        currentOverallQuantity += saleItemQuantity; //doing integer math

                        //get the value of "PackedForPickup" in the current row
                        string packedForPickup = item["PackedForPickup"].Text;
                        //if found this is the packaged only item, then add it into the total
                        if (!string.IsNullOrEmpty(packedForPickup))
                        {
                            if (packedForPickup == "yes")
                                totalPackedButNotPickup += saleItemQuantity;
                        }



                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in CalculateSummaryFromOrderDetails(): " + ex.Message + "');", true);
                }
            }

            //now only return with append text if totalPacked is greater than 0
            if (totalPackedButNotPickup > 0)
            {
                //Actually, when we get here, it's only for displaying the none pick-up items with there are Packed items
                int totalNeeded = currentOverallQuantity - totalPackedButNotPickup; //calculate the items that really need to be around the inventory in St Lawrence
                return string.Format("{0} ({1} neded, {2} already packed)", currentOverallQuantity.ToString(), totalNeeded.ToString(), totalPackedButNotPickup.ToString());
            }
            //otherwise, just return regular
            return currentOverallQuantity.ToString();
        }


        protected void RadGridForecastBanhChung_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            RadGridForecastBanhChung.DataSource = listOfBanhChung;
        }

        protected void RadGridForecastBanhTet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            RadGridForecastBanhTet.DataSource = listOfBanhTet;
        }




        public class RadGridOrderDetail
        {
            public string Link { get; set; }
            public string InvoiceNumber { get; set; }
            public string PackedForPickup { get; set; }
            public string CreatedBy { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string InvoiceType { get; set; }
            public string PickupDate { get; set; }
            public DateTime PickupDateTimeFormat
            {
                get
                {
                    try
                    {
                        return Convert.ToDateTime(PickupDate);
                    }
                    catch (Exception ex)
                    {
                        return new DateTime(2000, 01, 01);
                    }
                }
            }
            public string PickupStatus { get; set; }
            public string OrderNumber { get; set; }
            public string OrderItem { get; set; }
            public string Quantity { get; set; }
        }


        protected void RadGridSummaryOrderDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    // access/modify the edit item template settings here
                    if (item["QuantityType"].Text.Contains("Overall")) //this meant there is an Invoice Number in this row, therefore, make background color
                    {
                        item.BackColor = Color.LightGray;
                        item.Font.Bold = true;
                        item.Font.Size = FontUnit.Medium;
                    }


                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void RadGridSummaryOrderDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string s = "";
        }



    }


}