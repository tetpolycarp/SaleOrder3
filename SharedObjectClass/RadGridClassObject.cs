using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaintPolycarp.BanhChung.SharedObjectClass.RadGrid
{
    //**** Object for Grid Table *****************************/
    //this is this class object and will used in Invoice Form
    public class RadGridSaleItem
    {
        public string Index { get; set; }
        public string PickupDate { get; set; }
        public string PickedUp { get; set; }
        public string Packed { get; set; }
        public string OrderNumber { get; set; }
        public string SaleItem { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Discount { get; set; }
        public string SubTotal { get; set; }

    }


    //this is this class object and will used in Invoice Form
    public class RadGridOrder
    {
        private double total;
        private double alreadyPaid;
        private double alreadyPaidByCheck;
        private double balance;
        public RadGridOrder()
        {
            total = 0;
            alreadyPaid = 0;
            balance = 0;

        }
        public string OrderNumber { get; set; }
        public string PickupDate { get; set; }
        public string TotalString { get; set; }
        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }
        public string AlreadyPaidString { get; set; }
        public double AlreadyPaid
        {
            get
            {
                return alreadyPaid;
            }
            set
            {
                alreadyPaid = value;
            }
        }
        public string AlreadyPaidByCheckString { get; set; }
        public double AlreadyPaidByCheck
        {
            get
            {
                return alreadyPaidByCheck;
            }
            set
            {
                alreadyPaidByCheck = value;
            }
        }
        public string CheckNumber { get; set; }
        public string RemainBalanceString { get; set; }
        public double RemainBalance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }
    }
}