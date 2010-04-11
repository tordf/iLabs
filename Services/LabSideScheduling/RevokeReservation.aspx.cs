
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;

using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Proxies.USS;
using iLabs.UtilLib;
using iLabs.Ticketing;



namespace iLabs.Scheduling.LabSide
{
	/// <summary>
	/// Summary description for RevokeReservation.
	/// </summary>
    public partial class RevokeReservation : System.Web.UI.Page
	{
		string labServerGuid = null;
        string labServerName = null;
        ProcessAgentDB dbTicketing = new ProcessAgentDB();

        
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        CultureInfo culture;
        int userTZ = 0;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
			// Put user code to initialize the page here
            txtStartDate.Attributes.Add("OnKeyPress", "return false;");
            txtEndDate.Attributes.Add("OnKeyPress", "return false;");

            if (!IsPostBack)
            {

                if (Session["couponID"] == null || Request.QueryString["coupon_id"] != null)
                    couponID = Request.QueryString["coupon_id"];
                else
                    couponID = Session["couponID"].ToString();

                if (Session["passkey"] == null || Request.QueryString["passkey"] != null)
                    passkey = Request.QueryString["passkey"];
                else
                    passkey = Session["passkey"].ToString();

                if (Session["issuerID"] == null || Request.QueryString["issuer_guid"] != null)
                    issuerID = Request.QueryString["issuer_guid"];
                else
                    issuerID = Session["issuerID"].ToString();

                if (Session["sbUrl"] == null || Request.QueryString["sb_url"] != null)
                    sbUrl = Request.QueryString["sb_url"];
                else
                    sbUrl = Session["sbUrl"].ToString();

                bool unauthorized = false;

                if (couponID != null && passkey != null && issuerID != null)
                {
                    try
                    {
                        Coupon coupon = new Coupon(issuerID, long.Parse(couponID), passkey);
                        

                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.MANAGE_LAB);

                        if (ticket == null || ticket.IsExpired() || ticket.isCancelled)
                        {
                            unauthorized = true;
                            Response.Redirect("Unauthorized.aspx", false);
                        }

                        Session["couponID"] = couponID;
                        Session["passkey"] = passkey;
                        Session["issuerID"] = issuerID;
                        Session["sbUrl"] = sbUrl;

                        XmlDocument payload = new XmlDocument();
                        payload.LoadXml(ticket.payload);

                        Session["lsGuid"] = payload.GetElementsByTagName("labServerGuid")[0].InnerText;
                        Session["lsName"] = payload.GetElementsByTagName("labServerName")[0].InnerText;
                        userTZ = Convert.ToInt32(payload.GetElementsByTagName("userTZ")[0].InnerText);
                        Session["userTZ"] = userTZ;
                        txtStartDate.Text = culture.DateTimeFormat.ShortDatePattern;
                        txtEndDate.Text = culture.DateTimeFormat.ShortDatePattern;

                        lblDescription.Text = "All reservations for LabServer: " + Session["labServerName"].ToString()
                            + "<br/>for the time span you select will be revoked."
                            + "<br/><br/>Times shown are GMT:&nbsp;&nbsp;&nbsp;" + userTZ/60.0;
                    }

                    catch (Exception ex)
                    {
                        unauthorized = true;
                        Response.Redirect("Unauthorized.aspx", false);
                    }
                }

                else
                {
                    unauthorized = true;
                    Response.Redirect("Unauthorized.aspx", false);
                }

                /*if (!unauthorized)
                {
                    //labServerID = Session["labServerGuid"].ToString();
                    //lblLabServerName.Text = Session["labServerName"].ToString();
                    lblLabServerName.Text = Session["labServerName"].ToString();
                }*/
            }
            labServerGuid = (string) Session["lsGuid"];
            labServerName = (string) Session["lsName"];
            if(Session["userTZ"] != null)
                userTZ  = (int) Session["userTZ"];
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnRevoke_Click(object sender, System.EventArgs e)
   		{
            int count = 0;
            DateTime startDate = DateTime.MinValue;
            int startHours = -1;
            int startMinutes = -1;
            DateTime endDate = DateTime.MinValue;
            int endHours = -1;
            int endMinutes = -1;

            
            // input error check
            try
            {
                if(txtStartMin.Text.Length > 0)
                    startMinutes = int.Parse(txtStartMin.Text);
                if (startMinutes >= 60 || startMinutes < 0)
                {
                    string msg = "Please input right form of minute in the start time ";
                    lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                    lblErrorMessage.Visible = true;
                }
                if (txtEndMin.Text.Length > 0)
                    endMinutes = int.Parse(txtEndMin.Text);
                if (endMinutes > 60 || endMinutes < 0)
                {
                    string msg = "Please input right form of minute in the end time ";
                    lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                    lblErrorMessage.Visible = true;
                }


                if (txtEndDate.Text.Length == 0 || txtEndDate.Text.CompareTo(culture.DateTimeFormat.ShortDatePattern) == 0)
                {
                    lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the end date of the time block.");
                    lblErrorMessage.Visible = true;
                    return;
                }
                endDate = DateTime.Parse(txtEndDate.Text, culture);
                if (txtStartDate.Text.Length == 0 || txtStartDate.Text.CompareTo(culture.DateTimeFormat.ShortDatePattern) == 0)
                {
                    lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the end date of the time block.");
                    lblErrorMessage.Visible = true;
                    return;
                }
                startDate = DateTime.Parse(txtStartDate.Text, culture);
                if(endDate < startDate)
                {
                    lblErrorMessage.Text = Utilities.FormatWarningMessage("The end date must be greater than or equal to the start date.");
                    lblErrorMessage.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }

            startHours = int.Parse(ddlStartHour.SelectedItem.Text);
            if (ddlStartAM.Text.CompareTo("PM") == 0)
            {
                startHours += 12;
            }

            endHours = int.Parse(ddlEndHour.SelectedItem.Text);
            if (ddlEndAM.Text.CompareTo("PM") == 0)
            {
                endHours += 12;
            }

            DateTime startTime = new DateTime(startDate.Year,startDate.Month,startDate.Day,
                startHours,startMinutes,0,DateTimeKind.Utc);
            startTime.AddMinutes(userTZ);
            DateTime endTime = new DateTime(endDate.Year, endDate.Month, endDate.Day,
                endHours, endMinutes, 0, DateTimeKind.Utc);
            endTime.AddMinutes(userTZ);

            //Ticket ticketforRevo = ticketRetrieval.RetrieveAndVerify(coupon, TicketTypes.REVOKE_RESERVATION);

            // the removed reservations on LSS
			ArrayList removedRes = new ArrayList();
			ArrayList ussGuids = new ArrayList();
            
            if (startTime > endTime)
            {
                string msg = "the start time should be earlier than the end time";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }
            try
			{
				//the reservations going to be removed
                int[] resIDs = DBManager.ListReservationInfoIDsByLabServer(Session["lsGuid"].ToString(), startTime, endTime);
                if (resIDs != null && resIDs.Length > 0)
                {
                    count = LSSSchedulingAPI.RevokeReservations(resIDs);
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("For the time period " 
                        + DateUtil.ToUserTime(startTime,culture,userTZ) + " to "
                    + DateUtil.ToUserTime(endTime,culture,userTZ) + ", " + count + " out of " + resIDs.Length + " reservations have been revoked successfully.");
                    lblErrorMessage.Visible = true;
                }
				
			}
			catch (Exception ex)
			{
				lblErrorMessage.Text = Utilities.FormatErrorMessage("The related reservations have not been revoked successfully." + ex.Message);
                lblErrorMessage.Visible = true;
				// rollback
				foreach (ReservationInfo resInfo in removedRes)
				{
					LSSSchedulingAPI.AddReservationInfo(resInfo.startTime, resInfo.endTime, 
                        resInfo.credentialSetId, resInfo.experimentInfoId, resInfo.resourceId,resInfo.statusCode);

				}
                return;
			}
            
            try
            {
                
                foreach (string uGuid in ussGuids)
                {
                    int uInfoID = LSSSchedulingAPI.ListUSSInfoID(uGuid);
                    USSInfo[] ussArray = LSSSchedulingAPI.GetUSSInfos(new int[] { uInfoID });
                    if (ussArray.Length > 0)
                    {
                        Coupon revokeCoupon = dbTicketing.GetCoupon(ussArray[0].couponId, ussArray[0].domainGuid);

                        UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                        ussProxy.Url = ussArray[0].ussUrl;

                        //assign the coupon from ticket to the soap header;
                        OperationAuthHeader opHeader = new OperationAuthHeader();
                        opHeader.coupon = revokeCoupon;
                        ussProxy.OperationAuthHeaderValue = opHeader;

                        //if (ussProxy.RevokeReservation(Session["lsGuid"].ToString(), startTime, endTime))
                        //{
                        //    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(" The related reservations have been revoked successfully !");
                        //    lblErrorMessage.Visible = true;
                        //}
                    }
                }
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = Utilities.FormatErrorMessage("The related reservation have not been revoked successfully." + ex.Message);
				lblErrorMessage.Visible = true;
				// rollback
				foreach (ReservationInfo resInfo in removedRes)
				{
					LSSSchedulingAPI.AddReservationInfo(resInfo.startTime, resInfo.endTime, resInfo.credentialSetId, resInfo.experimentInfoId, resInfo.resourceId, resInfo.statusCode);

				}
			}
		}
      
	}
}
