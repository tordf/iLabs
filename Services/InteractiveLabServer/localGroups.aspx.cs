/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: localGroups.aspx.cs,v 1.3 2007/03/16 15:44:57 pbailey Exp $
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iLabs.Ticketing;

using iLabs.UtilLib;


namespace iLabs.LabServer.LabView
{
	/// <summary>
	/// Summary description for RunLab.
	/// </summary>
	public partial class localGroups : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblCoupon;
		protected System.Web.UI.WebControls.Label lblTicket;
		protected System.Web.UI.WebControls.Label lblGroupNameTitle;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// if credentials pass / Get Cookie
			//HttpCookie cookie = (HttpCookie) Request.Cookies.Get("viData");
			// if credentials pass / Get Cookie
            String returnURL = (string)Session["returnURL"];
            if ((returnURL != null) && (returnURL.Length > 0))
                lnkBackSB.NavigateUrl = returnURL;

			
			
			

			
/*
			}

			else
			{
				
				Response.Redirect("AccessDenied.aspx", true);
			}
*/
		}
        protected void ddlGroups_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }
        protected void btnRegister_Click(object sender, System.EventArgs e)
        {
        }

        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
        }

        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
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
	}
}
