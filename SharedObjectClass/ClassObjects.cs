using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SaintPolycarp.BanhChung.SharedObjectClass.RadGrid;

namespace SaintPolycarp.BanhChung.SharedObjectClass
{
    public class SaleItem
    {
        public string name { get; set; }
        public string shortname { get; set; }

        public string saleprice { get; set; }

        public string estimatecost { get; set; }

        public string note { get; set; }
    }
 
    public class OrderInvoice
    {
        public string CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PickupDate { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Status { get; set; }
        public List<RadGridSaleItem> OrderSaleItems { get; set; }
        public List<RadGridOrder> InvoiceOrders { get; set; }
        public string Notes { get; set; }
        public string GrandTotal { get; set; }
        public string AlreadyPaid { get; set; }
        public string AlreadyPaidByCheck { get; set; }
        public string CheckNumber { get; set; }
        public string RemainBalance { get; set; }
        public string LastUpdateBy { get; set; }
        public string LastUpdateOn { get; set; }
   

    }
    public class Finance
    {

        public string MainNotes { get; set; }
        public string TotalInvoiceAmount { get; set; }
        public string TotalPaidByCashAmount { get; set; }
        public string TotalPaidByCheckAmount { get; set; }
        public string GrandTotalAmount { get; set; }
        public List<TotalAmountInEachGiao> TotalAmountFromGiaoList { get; set; }
        public List<GiaoInvoice> Invoices { get; set; }

        public string LastUpdateBy { get; set; }
        public string LastUpdateOn { get; set; }

    }
    public class TotalAmountInEachGiao
    {
        public string GiaoNumber { get; set; }
        public string GiaoSelected { get; set; }
        public string GiaoSelectedColor { get; set; }
        public string GiaoDate { get; set; }
        public string Amount { get; set; }
    }

    public class GiaoAmount
    {
        public string GiaoNumber { get; set; }
        public string Amount { get; set; }
    }

    public class GiaoInvoice
    {
        public string Number { get; set; }
        public string GiaoTotalAmount { get; set; }
        public string Notes { get; set; }
        public List<GiaoAmount> GiaoAmount { get; set; }
    }



}