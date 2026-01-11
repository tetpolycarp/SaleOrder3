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
    public partial class OrderDetails : System.Web.UI.Page
    {
        private const string PHONE_FORMAT = "{0:(###) ###-####}";
        static List<SaleItem> saleItems = new List<SaleItem>();
        static List<OrderInvoice> orderInvoices = new List<OrderInvoice>();

        static List<RadGridOrderDetail> orderDetailList = new List<RadGridOrderDetail>();
        private IEnumerable<RadGridOrderDetail> distinctOrderDetailList = new List<RadGridOrderDetail>();

        public static DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isLimitView = true;
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
                    //1/16/25 - allow everyone to see this page, but limitted, only admin can see all.
                   
                    if (!string.IsNullOrEmpty(Request.QueryString["AdminView"]))
                    {
                        if (Request.QueryString["AdminView"].Equals("true") && !SharedIdentityMethod.UserBelongToRole(User.Identity.Name, "inventory admin"))
                        {
                            string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                            string OriginalUrl = HttpContext.Current.Request.RawUrl;
                            string UnthorizedPageUrl = rootURL + "GenericHandlers/UnauthorizedHandle.aspx";
                            HttpContext.Current.Response.Redirect(String.Format("{0}", UnthorizedPageUrl));
                        }
                        else 
                            isLimitView = false;
                    }

                }
                LoadOrderDetails();
                LoadSummaryOrderDetails();
                LoadChart();

                //1/16/25 - if view from regular menu or the users is not "inventory admin", then don't show it
                if (isLimitView)
                {
                    ButtonExportOrderDetails.Enabled = false;
                    ButtonExportOverall.Enabled = false;
                    ButtonPrintSelected.Enabled = false;
                    RadHtmlChartPickupStatuses.Visible = false;
                    RadGridOrderDetails.MasterTableView.GetColumn("Quantity").Visible = false;
                }
            }


        }
        private void LoadOrderDetails()
        {
            try
            {
                //RadGridOrderDetails is the table that lists all the Order detail in this page
                orderDetailList = new List<RadGridOrderDetail>();
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
                            orderDetail.PackStatus = saleItem.Packed;
                            orderDetail.OrderNumber = saleItem.OrderNumber;
                            orderDetail.OrderItem = saleItem.SaleItem;
                            orderDetail.Quantity = saleItem.Quantity;

                            orderDetailList.Add(orderDetail);
                        }



                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadData() at invoice #" + orderInvoice.InvoiceNumber + ". " + ex.Message + "');", true);

                    }
                }

                RadGridOrderDetails.DataSource = orderDetailList;
                RadGridOrderDetails.DataBind();





            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadData(): " + ex.Message + "');", true);
            }
        }
        private void LoadSummaryOrderDetails()
        {
            try
            {
                //**************Initial Sale Items

                dt = new DataTable();
                dt.Columns.Add("QuantityType");

                //start removing all the columns backward except the first one
                //so it will be added back again below
                for (int i = RadGridSummaryOrderDetails.MasterTableView.Columns.Count - 1; i > 0; i--)
                {
                    RadGridSummaryOrderDetails.MasterTableView.Columns.RemoveAt(i);
                }

                saleItems = SharedMethod.GetSaleItems();

                //start adding the column again
                foreach (SaleItem saleItem in saleItems)
                {
                    if (!string.IsNullOrEmpty(saleItem.name))
                    {

                        GridBoundColumn boundColumn = new GridBoundColumn();
                        boundColumn.DataField = saleItem.name;
                        boundColumn.HeaderText = saleItem.shortname;
                        boundColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        boundColumn.HeaderStyle.Width = Unit.Percentage(5);
                        boundColumn.HeaderStyle.Font.Bold = true;
                        boundColumn.HeaderStyle.Font.Size = FontUnit.Smaller;


                        boundColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                        RadGridSummaryOrderDetails.MasterTableView.Columns.Add(boundColumn);

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
                totalOrderQuantityRow["QuantityType"] = "Overall Quantity";

                DataRow totalPickedupRow = dt.NewRow();
                totalPickedupRow["QuantityType"] = "Picked-up";

                DataRow totalNotPickupRow = dt.NewRow();
                totalNotPickupRow["QuantityType"] = "Not Pick-up";

                DataRow totalNotPickupStillNeedRow = dt.NewRow();
                totalNotPickupStillNeedRow["QuantityType"] = "[Still Need]";

                DataRow totalNotPickupPackedRow = dt.NewRow();
                totalNotPickupPackedRow["QuantityType"] = "[Packed]";

                //calculate the overall quantity
                CalculateSummaryFromOrderDetails(RadGridOrderDetails, ref totalOrderQuantityRow, ref totalPickedupRow, ref totalNotPickupRow, ref totalNotPickupStillNeedRow, ref totalNotPickupPackedRow);

                //*************** Add the 3 total rows to the end of the table data table
                dt.Rows.Add(totalPickedupRow);
                dt.Rows.Add(totalNotPickupRow);
                dt.Rows.Add(totalNotPickupStillNeedRow);
                dt.Rows.Add(totalNotPickupPackedRow);
                dt.Rows.Add(totalOrderQuantityRow);


                //add data table into RadGrid
                RadGridSummaryOrderDetails.DataSource = dt;
                RadGridSummaryOrderDetails.DataBind();


                //after Binding and fill-up the data, now set the color of the few last rows dynamically
                //set the last row, which is "Packed"
                RadGridSummaryOrderDetails.Items[RadGridSummaryOrderDetails.Items.Count - 2].BackColor = Color.Green; //Packed row, because need to exclude the Over Quantity and the index start from 0
                RadGridSummaryOrderDetails.Items[RadGridSummaryOrderDetails.Items.Count - 3].BackColor = Color.Red; //Still Need row

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in LoadSummaryOrderDetails(): " + ex.Message + "');", true);
            }
        }
        private void LoadChart()
        {
            RadHtmlChartPickupStatuses.PlotArea.Series[0].Items.Clear();
            RadHtmlChartPickupStatuses.PlotArea.Series[1].Items.Clear();

            int chungPickup = 0;
            int chungNotPickup = 0;
            int tetPickup = 0;
            int tetNotPickup = 0;
            //this session for loading up the Graph
            //parse through the Summary table and only need to read Pickup/Not pickup row. Then read Chung/Tet column
            //therefore, will give us the value for 4 cells
            foreach (GridDataItem item in RadGridSummaryOrderDetails.MasterTableView.Items)
            {
                if (item["QuantityType"].Text.Contains("Picked-up"))
                {
                    try
                    {
                        chungPickup += Convert.ToInt16(item["Bánh Chưng"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        chungPickup += Convert.ToInt16(item["Bánh Chưng (Bán Sỉ)"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        tetPickup += Convert.ToInt16(item["Bánh Tét"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        tetPickup += Convert.ToInt16(item["Bánh Tét (Bán Sỉ)"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                }
                else if (item["QuantityType"].Text.Contains("Not Pick-up"))
                {
                    try
                    {
                        chungNotPickup += Convert.ToInt16(item["Bánh Chưng"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        chungNotPickup += Convert.ToInt16(item["Bánh Chưng (Bán Sỉ)"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        tetNotPickup += Convert.ToInt16(item["Bánh Tét"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                    try
                    {
                        tetNotPickup += Convert.ToInt16(item["Bánh Tét (Bán Sỉ)"].Text); //if the cell is null or empty, it will throw exception and should do anything
                    }
                    catch (Exception ex)
                    {
                        //if the cell is null or empty, it will throw exception and should do anything
                    }
                }
            }
            //series index #1 is Pickup
            RadHtmlChartPickupStatuses.PlotArea.Series[0].Items.Add(new SeriesItem(chungPickup));
            RadHtmlChartPickupStatuses.PlotArea.Series[0].Items.Add(new SeriesItem(tetPickup));

            //series index #2 is Not Pickup
            RadHtmlChartPickupStatuses.PlotArea.Series[1].Items.Add(new SeriesItem(chungNotPickup));
            RadHtmlChartPickupStatuses.PlotArea.Series[1].Items.Add(new SeriesItem(tetNotPickup));
        }
        //from the radGridTable, parse through each of the row to calcuate each item for overall quantity, picked-up and not pick-up
        private void CalculateSummaryFromOrderDetails(RadGrid radGridOrderDetails, ref DataRow totalOrderQuantityRow, ref DataRow totalPickedupRow, ref DataRow totalNotPickupRow, ref DataRow totalNotPickupStillNeedRow, ref DataRow totalNotPickupPackedRow)
        {
            foreach (GridDataItem item in radGridOrderDetails.MasterTableView.Items)
            {
                try
                {
                    //say the current row with OrderItem is "BanhChung", we need to confirm if it is in the sale item list
                    SaleItem saleItem = saleItems.Find(x => x.name.ToUpper().Equals(item["OrderItem"].Text.ToUpper()));

                    //if the sale item is found, then start 
                    if (saleItem != null)
                    {
                        //calculate the overall
                        int currentOverallQuantity = 0;
                        try
                        {
                            currentOverallQuantity = Convert.ToInt16((String)totalOrderQuantityRow[saleItem.name]); //if the cell is null or empty, it will throw exception and should do anything
                        }
                        catch (Exception ex)
                        {
                            //if the cell is null or empty, it will throw exception and should do anything
                        }
                        int saleItemQuantity = Convert.ToInt16(item["Quantity"].Text); //the quantity that currently have in OrderDetails table
                        currentOverallQuantity += saleItemQuantity; //doing integer math

                        //after doing the math, now write the integer back into the cell in the overall row
                        totalOrderQuantityRow[saleItem.name] = currentOverallQuantity.ToString();

                        //calculate the pickup items
                        if (item["PickupStatus"].Text.ToUpper().Equals("YES"))
                        {
                            int currentPickedup = 0;
                            try
                            {
                                currentPickedup = Convert.ToInt16((String)totalPickedupRow[saleItem.name]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }
                            currentPickedup += saleItemQuantity; //doing integer math

                            //after doing the math, now write the integer back into the cell in the overall row
                            totalPickedupRow[saleItem.name] = currentPickedup.ToString();
                        }

                        //calculate the NOT pickup items
                        else
                        {
                            int currentNotPickdup = 0;
                            try
                            {
                                currentNotPickdup = Convert.ToInt16((String)totalNotPickupRow[saleItem.name]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }
                            currentNotPickdup += saleItemQuantity; //doing integer math

                            //after doing the math, now write the integer back into the cell in the overall row
                            totalNotPickupRow[saleItem.name] = currentNotPickdup.ToString();

                            //*** now start adding new feature for "Still Need" item (as part of Packed feature)
                            int currentNotPickupStillNeed = 0; //which is the items that still need
                            try
                            {
                                currentNotPickupStillNeed = Convert.ToInt16((String)totalNotPickupStillNeedRow[saleItem.name]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }
                            if (!item["PackStatus"].Text.ToUpper().Equals("YES")) //if not packed, then still needed
                            {
                                //now check if this item is "Still Need", which is the opposite of "Packed"
                                currentNotPickupStillNeed += saleItemQuantity; //add when it's not Packed
                            }
                            //after doing the math, now write the integer back into the cell in the overall row
                            totalNotPickupStillNeedRow[saleItem.name] = currentNotPickupStillNeed.ToString();

                            //*** now start adding new feature for "Packed" item
                            int currentNotPickupPacked = 0; //which is the items that already Packed
                            try
                            {
                                currentNotPickupPacked = Convert.ToInt16((String)totalNotPickupPackedRow[saleItem.name]); //if the cell is null or empty, it will throw exception and should do anything
                            }
                            catch (Exception ex)
                            {
                                //if the cell is null or empty, it will throw exception and should do anything
                            }
                            if (item["PackStatus"].Text.ToUpper().Equals("YES"))
                            {
                                //now check if this item is "Packed", 
                                currentNotPickupPacked += saleItemQuantity; //add when it's Packed
                            }
                            //after doing the math, now write the integer back into the cell in the overall row
                            totalNotPickupPackedRow[saleItem.name] = currentNotPickupPacked.ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertBox", "alert('Exception in CalculateSummaryFromOrderDetails() at invoice: " + item["InvoiceNumber"].Text + ". " + ex.Message + "');", true);
                }
            }
        }
        protected void Page_Init(object source, System.EventArgs e)
        {
        }



        protected void RadGridOrderDetails_FilterCheckListItemsRequested(object sender, Telerik.Web.UI.GridFilterCheckListItemsRequestedEventArgs e)
        {
            distinctOrderDetailList = new List<RadGridOrderDetail>();
            string dataField = (e.Column as IGridDataColumn).GetActiveDataField();

            //set the list view filter for each item
            if (dataField.ToUpper().Contains("INVOICENUMBER"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.InvoiceNumber).Select(x => x.First());

                //         //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.InvoiceNumber)).ToList();
            }
            else if (dataField.ToUpper().Contains("CREATEDBY"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.CreatedBy).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.CreatedBy)).OrderBy(x => x.CreatedBy).ToList();
            }
            else if (dataField.ToUpper().Contains("CUSTOMERNAME"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.CustomerName).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.CustomerName)).OrderBy(x => x.CustomerName).ToList();
            }
            else if (dataField.ToUpper().Contains("CUSTOMERPHONE"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.CustomerPhone).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.CustomerPhone)).OrderBy(x => x.CustomerPhone).ToList();
            }
            else if (dataField.ToUpper().Contains("INVOICETYPE"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.InvoiceType).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.InvoiceType)).OrderByDescending(x => x.InvoiceType).ToList();
            }
            else if (dataField.ToUpper().Contains("PICKUPDATE"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.PickupDate).Select(x => x.First());
                distinctOrderDetailList = distinctOrderDetailList.OrderBy(x => x.PickupDateTimeFormat).ToList();

            }
            else if (dataField.ToUpper().Contains("PICKUPSTATUS"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.PickupStatus).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.PickupStatus)).OrderBy(x => x.PickupStatus).ToList();
            }
            else if (dataField.ToUpper().Contains("PACKSTATUS"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.PackStatus).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.PackStatus)).OrderBy(x => x.PackStatus).ToList();
            }
            else if (dataField.ToUpper().Contains("ORDERNUMBER"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.OrderNumber).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.OrderNumber)).ToList();
            }
            else if (dataField.ToUpper().Contains("ORDERITEM"))
            {
                //since orderDetailList is the static variable
                //so it should have value by this point
                distinctOrderDetailList = orderDetailList.GroupBy(x => x.OrderItem).Select(x => x.First());

                //don't display the null or empty value in the filter
                distinctOrderDetailList = distinctOrderDetailList.Where(x => !string.IsNullOrEmpty(x.OrderItem)).ToList();
            }

            e.ListBox.DataSource = distinctOrderDetailList;
            e.ListBox.DataKeyField = dataField;
            e.ListBox.DataTextField = dataField;
            e.ListBox.DataValueField = dataField;
            e.ListBox.DataBind();
        }

        protected void RadGridOrderDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridDataItem)
                {


                }

            }
            catch (Exception ex)
            {

            }

        }
        bool isFilterOrSort = false;
        protected void RadGridOrderDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.SortCommandName)
            {
                //chau note: need to set this for PreRender method when filtering or sort
                isFilterOrSort = true;

            }
            else
            {
                isFilterOrSort = false;
            }

        }

        protected void RadGridOrderDetails_PreRender(object sender, EventArgs e)
        {
            //Chau note: this filter or sort is alread set in ItemCommand method
            if (isFilterOrSort)
            {
                LoadSummaryOrderDetails();
                LoadChart();
            }
        }

        protected void RadGridOrderDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            RadGridOrderDetails.DataSource = orderDetailList;
        }

        public class RadGridOrderDetail
        {
            public string Link { get; set; }
            public string InvoiceNumber { get; set; }
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
            public string PackStatus { get; set; }
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


        protected void ButtonExportOrderDetails_Click(object sender, EventArgs e)
        {
            //RadGridOrderDetails.ExportSettings.Excel.Format = GridExcelExportFormat.Html;
            RadGridOrderDetails.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
            RadGridOrderDetails.MasterTableView.GetColumn("ResetButton").Display = false;
            RadGridOrderDetails.MasterTableView.GetColumn("Link").Display = false;
            RadGridOrderDetails.ExportSettings.FileName = "OrderDetails";
            RadGridOrderDetails.ExportSettings.ExportOnlyData = true;
            RadGridOrderDetails.ExportSettings.OpenInNewWindow = true;
            RadGridOrderDetails.MasterTableView.ExportToExcel();
        }

        protected void ButtonExportOverall_Click(object sender, EventArgs e)
        {
            LoadSummaryOrderDetails();
            //RadGridSummaryOrderDetails.ExportSettings.Excel.Format = GridExcelExportFormat.Html;
            RadGridSummaryOrderDetails.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
            RadGridSummaryOrderDetails.ExportSettings.FileName = "OrderOverall";
            RadGridSummaryOrderDetails.ExportSettings.ExportOnlyData = true;
            RadGridSummaryOrderDetails.ExportSettings.OpenInNewWindow = true;
            RadGridSummaryOrderDetails.MasterTableView.ExportToExcel();
        }
        protected void RadGrid_HTMLExporting(object sender, GridHTMLExportingEventArgs e)
        {
            e.Styles.Append("body { border:solid 0.1pt #CCCCCC; }");
        }

        protected void ButtonResetFilter_Click(object sender, EventArgs e)
        {
            //redirect to the page itseft
            string OriginalUrl = HttpContext.Current.Request.RawUrl;
            HttpContext.Current.Response.Redirect(OriginalUrl);
        }

        protected void ButtonPrintSelected_Click(object sender, EventArgs e)
        {
            //the list just contained to distinct item
            List<string> distinctListOfInvoiceNumber = new List<string>();
            //parse through each item in the table and get the invoice number
            foreach (GridDataItem item in RadGridOrderDetails.MasterTableView.Items)
            {
                string invoiceNumber = item["InvoiceNumber"].Text;

                //if the invoice is not in the list, then add it and print this invoice
                if (!distinctListOfInvoiceNumber.Exists(i => i.Equals(invoiceNumber)))
                {
                    //print invoice
                    string rootURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                    HttpContext.Current.Response.Redirect(String.Format("{0}InvoicePrint.aspx?InvoiceNumber={1}", rootURL, invoiceNumber));

                }
            }
        }
    }


}