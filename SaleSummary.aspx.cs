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
    public partial class SaleSummary : System.Web.UI.Page
    {
        private const string PHONE_FORMAT = "{0:(###) ###-####}";
        static List<SaleItem> saleItems = new List<SaleItem>();
        static List<OrderInvoice> orderInvoices = new List<OrderInvoice>();

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
                    if (!SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "inventory admin"))
                    {
                        string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                        string OriginalUrl = HttpContext.Current.Request.RawUrl;
                        string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                        HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                    }
                }
                LoadInventoryForecast();
            }
        }
        protected void Page_Init(object source, System.EventArgs e)
        {
            //LoadInventoryForecast();
        }

        private void LoadInventoryForecast()
        {
            try
            {
             
                //**************Initial Sale Items

                DataTable dt = new DataTable();
                dt.Columns.Add("InvoiceNumber"); //this first column is always InvoiceNumber, which is extra
                dt.Columns.Add("CustomerName");
                dt.Columns.Add("CustomerPhone");
                dt.Columns.Add("QuantityType");

                saleItems = SharedMethod.GetSaleItems();
                foreach (SaleItem saleItem in saleItems)
                {
                    if (!string.IsNullOrEmpty(saleItem.name))
                    {

                        GridBoundColumn boundColumn = new GridBoundColumn();
                        boundColumn.DataField = saleItem.name;
                        boundColumn.HeaderText = saleItem.shortname;
                        boundColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        boundColumn.HeaderStyle.Width = Unit.Percentage(8);
                        boundColumn.HeaderStyle.Font.Bold = true;
                        boundColumn.HeaderStyle.Font.Size = FontUnit.Large;


                        boundColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                        RadGridInventoryForecast.MasterTableView.Columns.Add(boundColumn);

                        //add the data table to have the same column as the RadGrid
                        dt.Columns.Add(saleItem.name);
                    }
                }
                //*****************************************************************

                
                //*************** Start creating Row **************************************
                //Final Total Summary rows: but these 3 row need to be created first so it can hold data.
                //However, it won't be added to the Data Table last because they should be the last 3 row in the table
                //first row, total quantity
                DataRow totalOrderQuantityRow = dt.NewRow();
                totalOrderQuantityRow["InvoiceNumber"] = "ALL INV";
                totalOrderQuantityRow["QuantityType"] = "TOTAL QTY";

                DataRow totalPickedupRow = dt.NewRow();
                totalPickedupRow["QuantityType"] = HttpUtility.HtmlDecode("&lt;img src='images/pickedup_yes.png' style='height: 16px; width: 16px' /&gt;");
                totalPickedupRow["CustomerName"] = "[Picked-up]";
                
                DataRow totalNotPickupRow = dt.NewRow();
                totalNotPickupRow["QuantityType"] = HttpUtility.HtmlDecode("&lt;img src='images/pickedup_no.png' style='height: 15px; width: 15px' /&gt;");
                totalNotPickupRow["CustomerName"] = "[Not Pick-up]";

                DataRow totalStillNeed = dt.NewRow();
                totalStillNeed["CustomerName"] = "[Not Pick-up]";
                totalStillNeed["QuantityType"] = "[Still Need]";


                DataRow totalPackedRow = dt.NewRow();
                totalPackedRow["CustomerName"] = "[Not Pick-up]";
                totalPackedRow["QuantityType"] = "[Packed]";

                
                //******************Create the data row first *********************
                orderInvoices = SharedMethod.GetInvoices();
                //parse through each of the invoice
                foreach (OrderInvoice orderInvoice in orderInvoices)
                {
                    try
                    {
                        //for each invoce, there are 3 rows: total quantity, total pickedup and total not pickup
                        //first row, total quantity
                        DataRow orderQuantityRow = dt.NewRow();
                        orderQuantityRow["InvoiceNumber"] = HttpUtility.HtmlDecode("&lt;a href='http://" + HttpContext.Current.Request.Url.Authority + "/Invoice.aspx?InvoiceNumber=" + orderInvoice.InvoiceNumber + "' target='_blank' title='Open invoice #" + orderInvoice.InvoiceNumber + "' &gt; &lt;span style='text-decoration: underline'&gt;" + orderInvoice.InvoiceNumber + "&lt;/span&gt; &lt;/ a &gt;");
                        orderQuantityRow["CustomerName"] = orderInvoice.CustomerName;
                        if (!string.IsNullOrEmpty(orderInvoice.PhoneNumber))
                        {
                            orderQuantityRow["CustomerPhone"] = string.Format(PHONE_FORMAT, double.Parse(orderInvoice.PhoneNumber));
                        }
                        orderQuantityRow["QuantityType"] = "Order Quantity";
                        //for each of the invoice, parse through each of the order/sale item to calculate the number
                        foreach (var orderSaleItem in orderInvoice.OrderSaleItems)
                        {
                            int currentQuality = 0;
                            try
                            {
                                currentQuality = Convert.ToInt16((String)orderQuantityRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }

                            int currentTotalQuantity = 0;
                            try
                            {
                                currentTotalQuantity = Convert.ToInt16((String)totalOrderQuantityRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }
                            int saleItemQuality = Convert.ToInt16(orderSaleItem.Quantity);
                            currentQuality += saleItemQuality; //add the quantity for the current invoice
                            if (currentQuality == 0)
                            {
                                orderQuantityRow[orderSaleItem.SaleItem] = ""; //don't want to display 0 on the table
                            }
                            else
                            {
                                orderQuantityRow[orderSaleItem.SaleItem] = currentQuality.ToString();
                            }

                            currentTotalQuantity += saleItemQuality; //continue to add the quantity into the overall total
                            if (currentTotalQuantity == 0)
                            {
                                totalOrderQuantityRow[orderSaleItem.SaleItem] = ""; //don't want to display 0 on the table
                            }
                            else
                            {
                                totalOrderQuantityRow[orderSaleItem.SaleItem] = currentTotalQuantity.ToString(); //this cell will hold the overall total
                            }
                        }
                        dt.Rows.Add(orderQuantityRow);


                        //second row, total pickup
                        DataRow pickupRow = dt.NewRow();
                        string createdBy = "";
                        if (!string.IsNullOrEmpty(orderInvoice.CreatedBy))
                        {
                            string[] createdBySplit = orderInvoice.CreatedBy.Split(new string[] { " on " }, StringSplitOptions.None);
                            if (createdBySplit.Count() > 0)
                            {
                                createdBy = " [" + createdBySplit[0].Trim() + "]";
                            }

                        }
                        pickupRow["CustomerName"] = "Created by: " + createdBy;
                        
                        pickupRow["QuantityType"] = HttpUtility.HtmlDecode("&lt;img src='images/pickedup_yes.png' style='height: 16px; width: 16px' /&gt;");
                        //for each of the invoice, parse through each of the order/sale item to calculate the number
                        foreach (var orderSaleItem in orderInvoice.OrderSaleItems)
                        {
                            if (orderSaleItem.PickedUp == "yes")
                            {
                                int currentPickedup = 0;
                                try
                                {
                                    currentPickedup = Convert.ToInt16((String)pickupRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and should do anything
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and should do anything
                                }

                                int currentTotalPickedup = 0;
                                try
                                {
                                    currentTotalPickedup = Convert.ToInt16((String)totalPickedupRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and should do anything
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and should do anything
                                }
                                int saleItemQuality = Convert.ToInt16(orderSaleItem.Quantity);
                                currentPickedup += saleItemQuality; //add the quantity for the current invoice
                                if (currentPickedup == 0)
                                {
                                    pickupRow[orderSaleItem.SaleItem] = "";
                                }
                                else
                                {
                                    pickupRow[orderSaleItem.SaleItem] = currentPickedup.ToString();
                                }

                                currentTotalPickedup += saleItemQuality; //continue to add the quantity into the overall total
                                if (currentTotalPickedup == 0)
                                {
                                    totalPickedupRow[orderSaleItem.SaleItem] = "";
                                }
                                else
                                {
                                    totalPickedupRow[orderSaleItem.SaleItem] = currentTotalPickedup.ToString(); //this cell will hold the overall total
                                }
                            }
                        }
                        dt.Rows.Add(pickupRow);


                        //third row - Not Pickup Quantity
                        DataRow notPickupRow = dt.NewRow();
                        if (orderInvoice.InvoiceType.ToUpper().Contains("COMPLEX"))
                        {
                            notPickupRow["CustomerName"] = "Type: Complex";
                        }
                        notPickupRow["QuantityType"] = HttpUtility.HtmlDecode("&lt;img src='images/pickedup_no.png' style='height: 15px; width: 15px' /&gt;");

                        //for each of the invoice, parse through each of the order/sale item to calculate the number
                        foreach (var orderSaleItem in orderInvoice.OrderSaleItems)
                        {
                            //*** process not pickup item
                            if (orderSaleItem.PickedUp == "no")
                            {
                                int currentNotPickup = 0;
                                try
                                {
                                    currentNotPickup = Convert.ToInt16((String)notPickupRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and should do anything
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and should do anything
                                }

                                
                                int currentTotalNotPickup = 0;
                                try
                                {
                                    currentTotalNotPickup = Convert.ToInt16((String)totalNotPickupRow[orderSaleItem.SaleItem]); //if the cell is null or empty, it will throw exception and shouldn't do anything
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and shouldn't do anything
                                }
                                int saleItemQuality = Convert.ToInt16(orderSaleItem.Quantity);
                                currentNotPickup += saleItemQuality;
                                if (currentNotPickup == 0)
                                {
                                    notPickupRow[orderSaleItem.SaleItem] = "";
                                }
                                else
                                {
                                    notPickupRow[orderSaleItem.SaleItem] = currentNotPickup.ToString();
                                }

                                currentTotalNotPickup += saleItemQuality; //continue to add the quantity into the overall total
                                if (currentTotalNotPickup == 0)
                                {
                                    totalNotPickupRow[orderSaleItem.SaleItem] = "";
                                }
                                else
                                {
                                    totalNotPickupRow[orderSaleItem.SaleItem] = currentTotalNotPickup.ToString(); //this cell will hold the overall total
                                }

                                //**** now start adding new feature for Packed item, implement the similar as above
                                //we only need to process when these items are not pickup yet
                                int currentTotalPacked = 0;
                                try
                                {
                                    //if the cell is null or empty, it will throw exception and shouldn't need to do anything
                                    currentTotalPacked = Convert.ToInt16((String)totalPackedRow[orderSaleItem.SaleItem]);
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and shouldn't do anything
                                }
                                
                                //now check if this item is Packed or not, if it is then adding it into the TotalPacked
                                if(!string.IsNullOrEmpty(orderSaleItem.Packed)) //not NULL
                                    if(orderSaleItem.Packed == "yes")
                                        currentTotalPacked += saleItemQuality; //continue to add the quantity into the overall total

                                //then set the value back to totalPackedRow for this particular cell
                                if(currentTotalPacked == 0)
                                    totalPackedRow[orderSaleItem.SaleItem] = "";
                                else
                                    totalPackedRow[orderSaleItem.SaleItem] = currentTotalPacked.ToString(); //this cell will hold the number of Packed for this particular sale item
                             
                                //**** now start adding new feature for "Still Need" item, implement the similar as above
                                //we only need to process when these items are not pickup yet
                                int currentTotalStillNeed = 0;
                                try
                                {
                                    //if the cell is null or empty, it will throw exception and shouldn't need to do anything
                                    currentTotalStillNeed = Convert.ToInt16((String)totalStillNeed[orderSaleItem.SaleItem]);
                                }
                                catch (Exception ex)
                                {
                                    //if the cell is null or empty, it will throw exception and shouldn't do anything
                                }

                                //now check if this item is "Still Need", which is the opposite of "Packed"
                                if (string.IsNullOrEmpty(orderSaleItem.Packed)) //when NULL
                                    currentTotalStillNeed += saleItemQuality; //add when it's NULL< which is not Packed
                                else if (orderSaleItem.Packed == "no")
                                    currentTotalStillNeed += saleItemQuality; //add when it's not Packed

                                //then set the value back to the totalStillNeed for this particular cell
                                if (currentTotalStillNeed == 0)
                                    totalStillNeed[orderSaleItem.SaleItem] = "";
                                else
                                    totalStillNeed[orderSaleItem.SaleItem] = currentTotalStillNeed.ToString();
                            }
                        }
                        dt.Rows.Add(notPickupRow);
                    }
                    catch(Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DefineGridStructure() at invoice #" + orderInvoice.InvoiceNumber + ". " + ex.Message + "');", true);

                    }
                }



                //*************** Add the 3 total rows to the end of the table data table
                dt.Rows.Add(totalOrderQuantityRow);
                dt.Rows.Add(totalPickedupRow);
                dt.Rows.Add(totalNotPickupRow);
                dt.Rows.Add(totalStillNeed);
                dt.Rows.Add(totalPackedRow);


                //add data table into RadGrid
                RadGridInventoryForecast.DataSource = dt;
                RadGridInventoryForecast.DataBind();

                //after Binding and fill-up the data, now set the color of the few last rows dynamically
                //set the last row, which is "Packed"
                RadGridInventoryForecast.Items[RadGridInventoryForecast.Items.Count - 1].BackColor = Color.Green; //Packed
                RadGridInventoryForecast.Items[RadGridInventoryForecast.Items.Count - 1].ForeColor = Color.White; //Packed
                RadGridInventoryForecast.Items[RadGridInventoryForecast.Items.Count - 2].BackColor = Color.Red; //Still Need
                RadGridInventoryForecast.Items[RadGridInventoryForecast.Items.Count - 2].ForeColor = Color.White; //Still Need
              
                if (string.IsNullOrEmpty(Request.QueryString["Mobile"]))
                {
                    RadGridInventoryForecast.ClientSettings.Scrolling.AllowScroll = true;
                    RadGridInventoryForecast.ClientSettings.Scrolling.UseStaticHeaders = true;
                    RadGridInventoryForecast.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(700);
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in DefineGridStructure(): " + ex.Message + "');", true);
            }
        }

      
        protected void RadGridInventoryForecast_FilterCheckListItemsRequested(object sender, Telerik.Web.UI.GridFilterCheckListItemsRequestedEventArgs e)
        {

        }

        protected void RadGridInventoryForecast_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    // access/modify the edit item template settings here
                    if(item["InvoiceNumber"].Text.Contains("href")) //this meant there is an Invoice Number in this row, therefore, make background color
                    {
                        item.BackColor = Color.LightGray;
                        item.Font.Bold = true;
                        item.Font.Size = FontUnit.Medium;
                    }
                    else if(item["InvoiceNumber"].Text.Contains("ALL INV")) //this is the last final summary
                    {
                        item.BackColor = Color.Yellow;
                        item.Font.Bold = true;
                        item.Font.Size = FontUnit.Large;
                    }

                }

            }
            catch (Exception ex)
            {
           
            }

        }

        protected void RadGridInventoryForecast_PreRender(object sender, EventArgs e)
        {

        }

        protected void RadGridInventoryForecast_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            LoadInventoryForecast();
            RadGridInventoryForecast.ExportSettings.Excel.Format = GridExcelExportFormat.Html;
            //RadGridInventoryForecast.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
            RadGridInventoryForecast.ExportSettings.FileName = "SaleSummary";
            RadGridInventoryForecast.ExportSettings.ExportOnlyData = true;
            RadGridInventoryForecast.ExportSettings.OpenInNewWindow = true;
            RadGridInventoryForecast.ExportSettings.HideStructureColumns = true;
            RadGridInventoryForecast.MasterTableView.ExportToExcel();
        }
        protected void RadGrid_HTMLExporting(object sender, GridHTMLExportingEventArgs e)
        {
            e.Styles.Append("body { border:solid 0.1pt #CCCCCC; }");
        }
    }
   

}