using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SaintPolycarp.BanhChung.SharedObjectClass;
using SaintPolycarp.BanhChung.SharedObjectClass.RadGrid;
using SaintPolycarp.BanhChung.SharedMethods;

namespace SaintPolycarp.BanhChung
{
    public partial class SendSmsTextMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LabelTitle.Text = (string)Request.QueryString["title"];
                TextBoxPhoneNumber.Text = (string)Request.QueryString["phoneNumber"];
                string invoiceNumber = (string)Request.QueryString["invoiceNumber"];
                string invoiceReceiptURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/InvoiceMobileReceipt.aspx?InvoiceNumber=" + invoiceNumber);
        
                TextBoxMessageConfirmation.Text = string.Format("Gian Hàng Tết St. Polycarp - Xuân Bính Ngọ 2026. \nXác nhận hoá đơn #{0}. \nThis is the confirmation for your order. The invoice number is #{0}. [{1}]", invoiceNumber, invoiceReceiptURL);
                TextBoxMessageToday.Text = string.Format("Gian Hàng Tết St. Polycarp - Xuân Bính Ngọ 2026. \nXin nhắc nhở quí vị ghé lấy hàng hôm nay, hoá đơn #{0}. \nFriendly reminder: Please come and pickup your order TODAY, {2}. The invoice number is #{0}. [{1}].", invoiceNumber, invoiceReceiptURL, DateTime.Now.ToString("MM/dd/yyyy"));
                TextBoxMessageTomorrow.Text = string.Format("Gian Hàng Tết St. Polycarp - Xuân Bính Ngọ 2026. \nXin nhắc nhở quí vị ghé lấy hàng ngày mai, hoá đơn #{0}. \nFriendly reminder: Please come and pickup your order TOMORROW, {2}. The invoice number is #{0}. [{1}].", invoiceNumber, invoiceReceiptURL, DateTime.Now.AddDays(1).ToString("MM/dd/yyyy"));
                

                ButtonSendConfirmation.Enabled = true;
                ButtonSendToday.Enabled = true;
                ButtonSendTomorrow.Enabled = true;
            }
        }

        protected void ButtonSendConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonSendConfirmation.Enabled = false;
                ButtonSendConfirmation.BackColor = System.Drawing.Color.Gray;
                SharedMethod.SendNewSmsTextMessage(TextBoxPhoneNumber.Text, TextBoxMessageConfirmation.Text);
                Response.Write("<script>alert('Confirmed. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('Confirm in Catch. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");

            }

        }
        protected void ButtonSendToday_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonSendToday.Enabled = false;
                ButtonSendToday.BackColor = System.Drawing.Color.Gray;
                SharedMethod.SendNewSmsTextMessage(TextBoxPhoneNumber.Text, TextBoxMessageToday.Text);
                Response.Write("<script>alert('Confirmed. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Confirm in Catch. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");

            }

        }
        protected void ButtonSendTomorrow_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonSendTomorrow.Enabled = false;
                ButtonSendTomorrow.BackColor = System.Drawing.Color.Gray;
                SharedMethod.SendNewSmsTextMessage(TextBoxPhoneNumber.Text, TextBoxMessageTomorrow.Text);
                Response.Write("<script>alert('Confirmed. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Confirm in Catch. Message has sent to " + TextBoxPhoneNumber.Text + ". ')</script>");

            }

        }
    }
}