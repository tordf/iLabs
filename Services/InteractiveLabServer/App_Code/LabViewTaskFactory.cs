/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

#define LabVIEW_86

using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.ESS;
using iLabs.LabServer.Interactive;
using iLabs.UtilLib;

using iLabs.LabView;
using iLabs.LabView.LV82;
using iLabs.LabView.LV86;

namespace iLabs.LabServer.LabView
{

    /// <summary>
    /// Summary description for LabTaskFactory
    /// </summary>
    public class LabViewTaskFactory : LabTaskFactory
    {
        public LabViewTaskFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override LabTask CreateLabTask(LabAppInfo appInfo, Coupon expCoupon, Ticket expTicket)
        {

            long experimentID = -1;
            LabTask task = null;

            //Parse experiment payload 	
            string payload = expTicket.payload;
            XmlQueryDoc expDoc = new XmlQueryDoc(payload);

            string experimentStr = expDoc.Query("ExecuteExperimentPayload/experimentID");
            if ((experimentStr != null) && (experimentStr.Length > 0))
            {
                experimentID = Convert.ToInt64(experimentStr);
            }
            string sbStr = expDoc.Query("ExecuteExperimentPayload/sbGuid");



            // Check to see if an experiment with this ID is already running
            LabDB dbManager = new LabDB();
            LabTask.eStatus status = dbManager.ExperimentStatus(experimentID, sbStr);
            if (status == LabTask.eStatus.NotFound)
            {
                // Check for an existing experiment using the same resources, if found Close it

                //Create the new Task
                if(appInfo.rev.Contains("8.2")){
                   task = iLabs.LabView.LV82.LabViewTask.CreateLabTask(appInfo,expCoupon,expTicket);
                }
                else{
                     task = iLabs.LabView.LV86.LabViewTask.CreateLabTask(appInfo,expCoupon,expTicket);
                }
            }

            else
            {
                task =  TaskProcessor.Instance.GetTask(experimentID);

            }
            return task;
        }


    }

}


