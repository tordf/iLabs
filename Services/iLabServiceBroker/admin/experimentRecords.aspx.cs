/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: experimentRecords.aspx.cs,v 1.12 2008/04/11 19:53:33 pbailey Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web .Security ;

using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker;
using iLabs.Ticketing;
using iLabs.UtilLib;

//using iLabs.Services;
using iLabs.DataTypes;
using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.ESS;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for experimentRecords.
	/// </summary>
	public partial class experimentRecords : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddlTimeis;
        int userTZ;
        CultureInfo culture = null;
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        int userId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["UserID"] == null)
            {
                Response.Redirect("../login.aspx");
            }
            userId = Convert.ToInt32(Session["UserID"]);

			if(ddlTimeAttribute.SelectedValue!="between")                
			{
				txtTime2.ReadOnly=true;
				txtTime2.BackColor=Color.Lavender;
			}
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            if(Session["UserTZ"] != null)
                userTZ = Convert.ToInt32(Session["UserTZ"]);
            // "Are you sure" javascript for DeleteExperiment button
            btnDeleteExperiment.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete this experiment?')== false) return false;");
           
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
		
        
        protected void clearExperimentDisplay()
        {
            txtExperimentID.Text = null;
            txtUserName1.Text = null;
            txtLabServerName.Text = null;
            txtClientName.Text = null;
            txtGroupName1.Text = null;
            txtStatus.Text = null;
            txtSubmissionTime.Text = null;
            txtCompletionTime.Text = null;
            txtRecordCount.Text = null;
            txtAnnotation.Text = null;
            txtAnnotation.ReadOnly = true;
            lblResponse.Text = null;

            trSaveAnnotation.Visible = false;
            trShowExperiment.Visible = false;
            trDeleteExperiment.Visible = false;
        }

        protected void  selectExperiments(){
			//	 get all criteria in place
			lbxSelectExperiment.Items .Clear ();
            clearExperimentDisplay();

			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);

			
				List<Criterion> cList = new List<Criterion>();
				if(txtGroupname.Text != "")
				{
					int gID = wrapper.GetGroupIDWrapper(txtGroupname.Text);
					cList.Add(new Criterion ("Group_ID", "=",gID.ToString()));
				}

				if(txtUsername.Text != "")
				{
					int uID = wrapper.GetUserIDWrapper(txtUsername.Text);
					cList.Add(new Criterion ("User_ID", "=", uID.ToString() ));
				}
			if((ddlTimeAttribute.SelectedValue.ToString() != "") && ((txtTime1.Text != null) && (txtTime1.Text != "")))
			{
				DateTime time1 = new DateTime();
				DateTime time2 = new DateTime();

				try
				{
                    time1 = DateUtil.ParseUserToUtc(txtTime1.Text,culture,Convert.ToInt32(Session["UserTZ"]));
                }
                catch
				{	
					lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid time.");
					lblResponse.Visible = true;
					return;
                }
				if( (ddlTimeAttribute.SelectedValue.ToString().CompareTo("between") ==0)
                    ||(ddlTimeAttribute.SelectedValue.ToString().CompareTo("on date") ==0))
				{	
                        try{
						    time2 = DateUtil.ParseUserToUtc(txtTime2.Text,culture,Convert.ToInt32(Session["UserTZ"]));
					    }
                        catch{	
					        lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid time in the second time field.");
					        lblResponse.Visible = true;
					        return;
                        }
                }
				if(ddlTimeAttribute.SelectedValue.ToString().CompareTo("before")== 0)
				{
					cList.Add(new Criterion ("CreationTime", "<", time1.ToString()));
				}
				else if(ddlTimeAttribute.SelectedValue.ToString().CompareTo("after") == 0)
				{
					cList.Add(new Criterion ("CreationTime", ">=", time1.ToString()));
				}
				else if(ddlTimeAttribute.SelectedValue.ToString().CompareTo("between") == 0)
				{
					cList.Add(new Criterion ("CreationTime", ">=",time1.ToString()));
					cList.Add(new Criterion ("CreationTime", "<", time2.ToString()));
				}
                else if (ddlTimeAttribute.SelectedValue.ToString().CompareTo("on date") == 0)
                {
                    cList.Add(new Criterion("CreationTime", ">=",  time1.ToString()));
                    cList.Add(new Criterion("CreationTime", "<", time1.AddDays(1).ToString()));
                }              
             }
        
			
		 long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userId,sessionGroupID, cList.ToArray());
            LongTag[] expTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture,true,true,true,false,false,false,true,false);

            for (int i = 0; i < expTags.Length; i++)
            {
              
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(expTags[i].tag, expTags[i].id.ToString());
                lbxSelectExperiment.Items.Add(item);
            }

            if (eIDs.Length == 0)
            {
                string msg = "No experiments were found for the selection criteria!";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
            }
		
		}

        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            selectExperiments();
        }

		protected void lbxSelectExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
		{

            clearExperimentDisplay();
			try
			{
				ExperimentSummary[] expInfo = wrapper.GetExperimentSummaryWrapper (new long[] {Int64.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value)});
			
				if(expInfo[0] != null)
				{
                    if( expInfo[0].essGuid != null){
                        int expStatus = expInfo[0].status;
                        if ((expStatus == StorageStatus.UNKNOWN || expStatus == StorageStatus.INITIALIZED
                        || expStatus == StorageStatus.OPEN || expStatus == StorageStatus.REOPENED
                        || expStatus == StorageStatus.RUNNING
                        || expStatus == StorageStatus.BATCH_QUEUED || expStatus == StorageStatus.BATCH_RUNNING
                        || expStatus == StorageStatus.BATCH_TERMINATED || expStatus == StorageStatus.BATCH_TERMINATED_ERROR))
                        {

                            // This operation should happen within the Wrapper
                            BrokerDB ticketIssuer = new BrokerDB();
                            ProcessAgentInfo ess = ticketIssuer.GetProcessAgentInfo(expInfo[0].essGuid);
                            if (ess.retired)
                            {
                                throw new Exception("The experiments ESS has been retired");
                            }
                            Coupon opCoupon = ticketIssuer.GetEssOpCoupon(expInfo[0].experimentId, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                            if (opCoupon != null)
                            {

                                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                                OperationAuthHeader header = new OperationAuthHeader();
                                header.coupon = opCoupon;
                                essProxy.Url = ess.webServiceUrl;
                                essProxy.OperationAuthHeaderValue = header;

                                StorageStatus curStatus = essProxy.GetExperimentStatus(expInfo[0].experimentId);
                                if (expInfo[0].status != curStatus.status || expInfo[0].recordCount != curStatus.recordCount
                                    || expInfo[0].closeTime != curStatus.closeTime)
                                {
                                    DataStorageAPI.UpdateExperimentStatus(curStatus);
                                    expInfo[0].status = curStatus.status;
                                    expInfo[0].recordCount = curStatus.recordCount;
                                    expInfo[0].closeTime = curStatus.closeTime;

                                }
                            }
                        }

                    }
					txtExperimentID.Text = expInfo[0].experimentId.ToString () ;
					txtUserName1.Text = expInfo[0].userName ;
					txtLabServerName.Text =expInfo[0].labServerName;
                    txtClientName.Text = expInfo[0].clientName;
					txtGroupName1.Text = expInfo[0].groupName;

                    txtStatus.Text = DataStorageAPI.getStatusString(expInfo[0].status);
					txtSubmissionTime.Text = DateUtil.ToUserTime(expInfo[0].creationTime,culture,userTZ);
                    if ((expInfo[0].closeTime != null) && (expInfo[0].closeTime != DateTime.MinValue))
                        txtCompletionTime.Text = DateUtil.ToUserTime(expInfo[0].closeTime, culture, userTZ);
                    else
                        txtCompletionTime.Text = "Not Closed!";
                    txtRecordCount.Text = expInfo[0].recordCount.ToString();
			
					txtAnnotation.Text = expInfo[0].annotation;
                    txtAnnotation.ReadOnly = false;

                    trShowExperiment.Visible = (expInfo[0].recordCount >0);
                    trSaveAnnotation.Visible = true;
                    trDeleteExperiment.Visible = true;

				}
			
				//txtExperimentSpecification.Text = wrapper.RetrieveExperimentSpecificationWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
				//txtExperimentResult.Text = wrapper.RetrieveExperimentResultWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
				//txtLabconfig.Text = wrapper.RetrieveLabConfigurationWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
			
			}
			catch(Exception ex)
			{
				lblResponse.Text ="<div class=errormessage><p>Exception: Error retrieving experiment information. "+ex.Message+"</p></div>";
				lblResponse.Visible=true;
			}
		}

		protected void ddlTimeAttribute_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtTime1.Text=null;
			txtTime2.Text=null;
			if (ddlTimeAttribute.SelectedValue.ToString().CompareTo("between")==0)
			{
				txtTime2.ReadOnly=false;
				txtTime2.BackColor=Color.White;
			}
		}

        protected void btnSaveAnnotation_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;

            try
            {
                wrapper.SaveExperimentAnnotationWrapper(Int32.Parse(txtExperimentID.Text), txtAnnotation.Text);

                lblResponse.Text = Utilities.FormatConfirmationMessage("Annotation saved for experiment ID " + txtExperimentID.Text);
                lblResponse.Visible = true;
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error saving experiment annotation. " + ex.Message);
                lblResponse.Visible = true;
            }
        }

        protected void btnShowExperiment_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("showAdminExperiment.aspx?expid=" + txtExperimentID.Text, true);
        }

        protected void btnDeleteExperiment_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            try
            {
                wrapper.RemoveExperimentsWrapper(new long[] { Convert.ToInt32(txtExperimentID.Text) });
                selectExperiments();
                lblResponse.Text = Utilities.FormatConfirmationMessage("Deleted experiment ID " + txtExperimentID.Text);
                lblResponse.Visible = true;
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error deleting experiment. " + ex.Message);
                lblResponse.Visible = true;
            }
        }
	}
}

