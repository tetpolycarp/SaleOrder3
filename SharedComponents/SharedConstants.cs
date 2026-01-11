using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;


namespace SaintPolycarp.BanhChung.SharedConstansts
{
    public static class SharedConstants
    {
        public static string ROOT_DATA_DIRECTORY
        {
            get
            {
                string rootDataDir = ConfigurationManager.AppSettings["RootDataDirectory"];
                if (!Directory.Exists(rootDataDir))
                    Directory.CreateDirectory(rootDataDir);

                return rootDataDir;
            }
        }
        public static string INVOICE_DIRECTORY
        {
            get
            {
                string invoiceDir = Path.Combine(ROOT_DATA_DIRECTORY, "Invoices");
                if (!Directory.Exists(invoiceDir))
                    Directory.CreateDirectory(invoiceDir);

                return invoiceDir;
            }
        }
        public static string TRACKING_SHEET_DIRECTORY
        {
            get
            {
                string trackingSheetDir = Path.Combine(ROOT_DATA_DIRECTORY, "TrackingSheets");
                if (!Directory.Exists(trackingSheetDir))
                    Directory.CreateDirectory(trackingSheetDir);

                return trackingSheetDir;
            }
        }
        public static string SALE_ITEMS_FILE
        {
            get
            {
                string saleItemsFile = Path.Combine(ROOT_DATA_DIRECTORY, "SaleItems.json");
                if (!File.Exists(saleItemsFile))
                    File.Copy(HttpContext.Current.Server.MapPath("~/SaleItems/SaleItems.json"), saleItemsFile);

                return saleItemsFile;
            }
        }
        public static string ACTIVITY_LOGS_FILE
        {
            get
            {
                string activityLogsFile = Path.Combine(ROOT_DATA_DIRECTORY, "ActivityLogs.txt");
                if (!File.Exists(activityLogsFile))
                    File.Create(activityLogsFile);

                return activityLogsFile;
            }
        }


    }
}