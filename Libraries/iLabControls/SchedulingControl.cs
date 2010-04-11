/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: SchedulingControl.cs,v 1.5 2007/02/16 22:50:36 pbailey Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iLabs.DataTypes.SchedulingTypes;
using iLabs.UtilLib;

namespace iLabs.Controls.Scheduling
{
   
    public class SchedulingControl : DataBoundControl, IPostBackEventHandler
    {
        DateTime startTime;
        DateTime endTime;
        TimeSpan maxTOD;
        TimeSpan minTOD;
        TimeSpan minDuration;
        TimeSpan tzSpan;
    
        int numCols;
        CultureInfo culture;
        List<TimePeriod> periods = new List<TimePeriod>();
        List<Reservation> reservations = null;

        string scheduleTableClass = "scheduling";
        string hourTableClass = "hours";
        string dayTableClass = "day";
        Unit one = new Unit(1);
        Unit zero = new Unit(0);
       
        #region Properties

        bool bindReservations = false;
        bool hours24 = false;

        int columnWidth = 120;
        int headerHeight = 25;
        int hourHeight = 40;
        int hourWidth = 40; 
        int defaultCellDuration = 30;
        int userTZ;
        Unit scheduleWidth = new Unit("100%");
        
        Color availableColor = ColorTranslator.FromHtml("#55ff55");
        Color scheduledColor = ColorTranslator.FromHtml("#aaaaaa");
        Color timeBorderColor = ColorTranslator.FromHtml("#000000");
        Color voidColor = ColorTranslator.FromHtml("#ff0000");

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
               
            }
        }
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                
            }
        }
        public DateTime StartDate
        {
            get
            {
                return startTime.AddMinutes(userTZ).Date;
            }
           
        }
        public DateTime EndDate
        {
            get
            {
                return endTime.AddMinutes(userTZ).Date;
            }
          
        }
        public int UserTZ
        {
            get
            {
                return userTZ;
            }
            set
            {
                userTZ = value;
                tzSpan = TimeSpan.FromMinutes(userTZ);
            }
        }
        public CultureInfo Culture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value;
            }
        }

        public TimeSpan MaxTOD
        {
            get
            {
                return maxTOD;
            }
     
        }
        public TimeSpan MinTOD
        {
            get
            {
                return minTOD;
            }
  
        }
        public TimeSpan MinDuration
        {
            get
            {
                return minDuration;
            }
            set
            {
                minDuration = value;
            }

        }
        public int NumDays
        {
            get
            {
                int tmp = Convert.ToInt32((EndDate - StartDate).TotalDays);
                if (tmp < 1){
                    tmp = 1;
                }
                return tmp;
                
            }
           
        }

         public bool BindResevations
        {
            get
            {
                return bindReservations;
            }
            set
            {
                bindReservations = value;
            }
        }

        public bool Hours24
        {
            get
            {
                return hours24;
            }
            set
            {
                hours24 = value;
            }
        }

        public int ColumnWidth
        {
            get
            {
                return columnWidth;
            }
            set
            {
                columnWidth = value;
            }
        }
        public int HeaderHeight
        {
            get
            {
                return headerHeight;
            }
            set
            {
                headerHeight = value;
            }
        }
         public int HourHeight
        {
            get
            {
                return hourHeight;
            }
            set
            {
               hourHeight = value;
            }
        }
        public int HourWidth
        {
            get
            {
                return hourWidth;
            }
            set
            {
                hourWidth = value;
            }
        }
        
        public Unit ScheduleWidth
        {
            get
            {
                return scheduleWidth;
            }
            set
            {
                scheduleWidth = value;
            }
        }
        public Color  AvailableColor {
            get {
                return availableColor;
            }
            set{
                availableColor = value;
            }
        }
        public Color ScheduledColor{
            get {
                return scheduledColor;
            }
            set{
                scheduledColor = value;
            }
        }
        public Color TimeBorderColor{
            get {
                return timeBorderColor;
            }
            set{
                timeBorderColor = value;
            }
        }
        public Color VoidColor{
            get {
                return voidColor;
            }
            set{
                voidColor = value;
            }
        }

        #endregion

        /// <summary>
        /// Event called when the user clicks a reservation in the calendar. It's only called when DoPostBackForEvent is true.
        /// </summary>
        public event ScheduledClickDelegate ScheduledClick;

        /// <summary>
        /// Event called when the user clicks an availible time block in the calendar. It's only called when DoPostBackForFreeTime is true.
        /// </summary>
        public event AvailableClickDelegate AvailableClick;
   
        #region Rendering

        /// <summary>
        /// Renders the component HTML code.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
           
            Type bType = Page.Request.Browser.TagWriter;
            output.WriteLine();
            output.WriteLine("<!- Scheduling Table -->");
            // <table>
            output.AddAttribute("id", ID);
            output.AddAttribute("class", scheduleTableClass);
            output.AddAttribute("cols",(NumDays + 1).ToString());
            //output.AddAttribute("width", scheduleWidth);
            output.AddAttribute("cellpadding", zero.ToString());
            output.AddAttribute("cellspacing", zero.ToString());
            //output.AddStyleAttribute("position", "relative");
            output.RenderBeginTag("table");
            //output.WriteLine();
            
            // <tr> Table contents
            output.AddAttribute("id", "trTableContents");
            output.RenderBeginTag("tr");
            output.WriteLine();

            int offset = 1;
            // hours column
            renderHoursTable(output,offset);
            offset += hourWidth;

            output.Write("<!- Day Tables -->");
            for (DateTime day = StartDate; day < EndDate; day = day.AddDays(1))
            {
                renderDay(output,day.AddMinutes(-userTZ),offset);
                offset += columnWidth;
            }
           
            output.RenderEndTag(); // </tr> Table Contents
            //output.WriteLine();
            output.RenderEndTag();   // </table>
            //output.WriteLine("<!- End Scheduling  Table -->");
        }


        private void renderHoursTable(HtmlTextWriter output, int offset)
        {
            output.AddAttribute("id", "tdHourTable");
            output.AddAttribute("width", hourWidth + "px");
            //output.AddStyleAttribute("position", "absolute");
            //output.AddStyleAttribute("top", zero);
            //output.AddStyleAttribute("left", offset.ToString() + "px");
            output.RenderBeginTag("td");
            output.WriteLine();
            output.WriteLine("<!- Hour Table -->");
            output.AddAttribute("class", hourTableClass);         
            output.AddAttribute("width", hourWidth + "px");
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "0");
            output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(timeBorderColor));
             //output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(timeBorderColor));
             output.AddStyleAttribute("text-align", "right");
            output.RenderBeginTag("table");
            //output.WriteLine();
            // <tr> first emtpy
            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", "1px");
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(timeBorderColor));
            output.RenderBeginTag("td");
            output.RenderEndTag();
            output.RenderEndTag();
            //output.WriteLine();

            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", HeaderHeight + "px");
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            output.RenderBeginTag("th");
            output.Write("&nbsp;");
            output.RenderEndTag();
            output.RenderEndTag();
            //output.WriteLine();
            
            for (int i = minTOD.Hours; i < (int) maxTOD.TotalHours; i++)
            {
                renderHourCell(output, i);
            }

            output.RenderEndTag();
            //output.WriteLine();
            output.RenderEndTag(); // </td> End of hours column
            //output.WriteLine();

        }

        private int renderHourCell(HtmlTextWriter output, int hour)
        {
            output.RenderBeginTag("tr");

            //output.AddStyleAttribute("top", top + "px");
            output.AddStyleAttribute("height", HourHeight + "px");
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            output.RenderBeginTag("th");
            
            
            //output.RenderBeginTag("div");
            
            bool am = (hour / 12) == 0;
            if (!hours24)
            {
                hour = hour % 12;
                if (hour == 0)
                    hour = 12;
            }

            output.Write(hour);
            output.Write("<span style='font-size:10px; '>&nbsp;");
            if (hours24)
            {
                output.Write("00");
            }
            else
            {
                if (am)
                    output.Write("AM");
                else
                    output.Write("PM");
            }
            output.Write("</span>");
            //output.RenderEndTag();

            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
            output.WriteLine();
            return HourHeight;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="day">UTC corrected TZ start of day</param>
        /// <param name="offset"></param>
        private void renderDay(HtmlTextWriter output, DateTime day, int offset)
        {
            output.WriteLine();
            //output.AddStyleAttribute("position", "absolute");
            //output.AddStyleAttribute("top", "0px");
            //output.AddStyleAttribute("left", offset.ToString() + "px");
            output.RenderBeginTag("td");
            // <table>
            output.AddAttribute("class", dayTableClass);
            output.AddAttribute("width", columnWidth +"px");
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            //output.AddAttribute("border", "0");
            output.RenderBeginTag("table");

            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", "1px");
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(timeBorderColor));
            output.RenderBeginTag("td");
            output.RenderEndTag();
            output.RenderEndTag();
            output.WriteLine();

            //Day Header 
            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", headerHeight + "px");
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            //output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(timeBorderColor));
            output.RenderBeginTag("th");
            output.Write(DateUtil.ToUserDate(day,culture,userTZ));
            output.RenderEndTag();
            output.RenderEndTag();
            output.WriteLine();
            if (bindReservations)
            {
                renderReservations(output, day, reservations);
            }
            else
            {
                renderTimePeriods(output, day, periods);
            }
            output.RenderBeginTag("tr");
            output.AddStyleAttribute("height", one.ToString());
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(BorderColor));
            output.RenderBeginTag("td");
            output.RenderEndTag();
            output.RenderEndTag();
            output.WriteLine();
            output.RenderEndTag();
            output.RenderEndTag();
        }

        private void renderTimePeriods(HtmlTextWriter output, DateTime date, IEnumerable periods)
        {
          
            TimeBlock validTime = new TimeBlock(date.AddHours((int)minTOD.Hours), date.AddHours((int)maxTOD.TotalHours));
            if (periods != null)
            {
                IEnumerator enumTP = null;
                try
                {
                    enumTP = periods.GetEnumerator();
                    enumTP.MoveNext();
                    TimePeriod first = (TimePeriod)enumTP.Current;
                    if (first.Start > validTime.Start)
                    {
                        renderVoidTime(output, Convert.ToInt32((first.Start - validTime.Start).TotalSeconds));
                    }
                }
                catch (Exception ex) { }
                finally
                {
                    enumTP.Reset();
                }

                foreach (TimePeriod tp in periods)
                {
                    if (validTime.Intersects(tp))
                    {
                        TimeBlock valid = validTime.Intersection(tp);
                        if (tp.quantum == 0)
                        {
                            renderScheduledTime(output, valid);
                        }
                        else
                        {
                            int cellDur = 0;
                            int tDur = valid.Duration;
                            DateTime cur = valid.Start;
                            DateTime end = valid.End;

                            while (cur < end)
                            {
                                cellDur = defaultCellDuration -(cur.TimeOfDay.Minutes % defaultCellDuration);
                                if(cellDur < defaultCellDuration) 
                                    cellDur += defaultCellDuration;
                                if ((end - cur.AddMinutes(cellDur)).TotalMinutes < (defaultCellDuration/2))
                                {
                                    cellDur += Convert.ToInt32((end - cur.AddMinutes(cellDur)).TotalMinutes );
                                }
                                cellDur = (cur.AddMinutes(cellDur) <= end) ? cellDur : (int)(end - cur).TotalMinutes;
                                renderAvailableTime(output, cur, tDur, tp.quantum, cellDur * 60, false);
                                cur = cur.AddMinutes(cellDur);
                                tDur = tDur - (cellDur * 60);

                                //cellDur = (cur.TimeOfDay.Minutes % defaultCellDuration) + defaultCellDuration;
                                //cellDur = (cur.AddMinutes(cellDur) <= end) ? cellDur : (int)(end - cur).TotalMinutes;
                                //renderAvailableTime(output, cur, tDur, tp.quantum, cellDur * 60, false);
                                //cur = cur.AddMinutes(cellDur);
                                //tDur = tDur - (cellDur * 60);
                            }
                        }
                    }
                }
            }
        }
       
        private int renderVoidTime(HtmlTextWriter output, int duration)
        {
            output.WriteLine();
            output.RenderBeginTag("tr");
           int height = Convert.ToInt32((hourHeight* duration)/3600.0);
                //output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, startTime.ToString("s")));
          
            //output.AddStyleAttribute("background-color",scheduledColor);
           output.AddAttribute("title", (duration / 60.0).ToString());
           output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(voidColor));
            output.AddStyleAttribute("height", height + "px");
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            output.RenderBeginTag("td");
            output.RenderEndTag();
            output.RenderEndTag();
            return height;
        }

        private int renderScheduledTime(HtmlTextWriter output,  ITimeBlock tb)
        {
            output.WriteLine();
            output.RenderBeginTag("tr");
           int height = Convert.ToInt32((((hourHeight * tb.Duration)/3600.0)));
           //output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, startTime.ToString("s")));

           output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(scheduledColor));
            output.AddStyleAttribute("cursor", "hand");
            output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            output.AddStyleAttribute("height", height + "px");
            output.AddAttribute("title", tb.Start.AddMinutes(userTZ).TimeOfDay.ToString() + " - " + tb.End.AddMinutes(userTZ).TimeOfDay.ToString());
            output.RenderBeginTag("td");
            //if(height > 24)
            //    output.Write(tb.Start.AddMinutes(userTZ).TimeOfDay );
            //if(height > 48)
            //    output.Write(" - " + tb.End.AddMinutes(userTZ).TimeOfDay);
            output.RenderEndTag();
            output.RenderEndTag();
            return height;
             
        }

        private int renderAvailableTime(HtmlTextWriter output, DateTime startTime, int duration,int quantum, int cellDuration,bool lastCell)
        {
            string str = startTime.ToString("o") + ", " + duration + ", " + quantum;
            output.WriteLine();
            output.RenderBeginTag("tr");
            int height = Convert.ToInt32((((hourHeight* cellDuration)/3600.0)));

            output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, str));
            output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(availableColor));
            output.AddStyleAttribute("cursor", "hand");
            output.AddStyleAttribute("height", height + "px");
          
            if(lastCell)
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            else
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(timeBorderColor));
            
            output.AddAttribute("title", startTime.AddMinutes(userTZ).TimeOfDay.ToString());
            output.RenderBeginTag("td");
            //DateTime end = startTime.AddSeconds(duration);
            //output.Write(startTime.AddMinutes(userTZ).TimeOfDay + " - " + end.AddMinutes(userTZ).TimeOfDay);
           
            output.RenderEndTag();
            output.RenderEndTag();
            return height;
        }

        private void renderReservations(HtmlTextWriter output, DateTime day, IEnumerable reservations)
        { }
 
        #endregion

        #region Viewstate

        /// <summary>
        /// Loads ViewState.
        /// </summary>
        /// <param name="savedState"></param>
        //protected override void LoadViewState(object savedState)
        //{
        //    if (savedState == null)
        //        return;

        //    object[] vs = (object[])savedState;

        //    if (vs.Length != 2)
        //        throw new ArgumentException("Wrong savedState object.");

        //    if (vs[0] != null)
        //        base.LoadViewState(vs[0]);

        //    if (vs[1] != null)
        //        items = (ArrayList)vs[1];

        //}

        /// <summary>
        /// Saves ViewState.
        /// </summary>
        /// <returns></returns>
        //protected override object SaveViewState()
        //{
        //    object[] vs = new object[2];
        //    vs[0] = base.SaveViewState();
        //    vs[1] = items;

        //    return vs;
        //}

        #endregion

        #region PostBack


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            AvailableClick(this, new AvailableClickEventArgs  (eventArgument));   
        }

        #endregion

        #region Data binding


        protected override void PerformSelect()
        {
            // Call OnDataBinding here if bound to a data source using the
            // DataSource property (instead of a DataSourceID), because the
            // databinding statement is evaluated before the call to GetData.       
            if (!IsBoundUsingDataSourceID)
            {
                this.OnDataBinding(EventArgs.Empty);
            }

            // The GetData method retrieves the DataSourceView object from  
            // the IDataSource associated with the data-bound control.            
            GetData().Select(CreateDataSourceSelectArguments(),
                this.OnDataSourceViewSelectCallback);

            // The PerformDataBinding method has completed.
            RequiresDataBinding = false;
            MarkAsDataBound();

            // Raise the DataBound event.
            OnDataBound(EventArgs.Empty);
        }

        private void OnDataSourceViewSelectCallback(IEnumerable retrievedData)
        {
            // Call OnDataBinding only if it has not already been 
            // called in the PerformSelect method.
            if (IsBoundUsingDataSourceID)
            {
                OnDataBinding(EventArgs.Empty);
            }
            // The PerformDataBinding method binds the data in the  
            // retrievedData collection to elements of the data-bound control.
            PerformDataBinding(retrievedData);
        }

        protected override void PerformDataBinding(IEnumerable schedulingData)
        {
            // don't bind in design mode
            if (DesignMode)
            {
                return;
            }

            base.PerformDataBinding(schedulingData);

            // Verify data exists.
            if (schedulingData != null)
            {

                TimeSpan tmpTS;
                TimeSpan oneDay = TimeSpan.FromDays(1);
                maxTOD = TimeSpan.Zero.Subtract(TimeSpan.FromMinutes(userTZ));
                minTOD = oneDay;

                foreach (ITimeBlock tb in schedulingData)
                {
                    tmpTS = tb.Start.AddMinutes(userTZ).TimeOfDay;
                    if (minTOD > tmpTS)
                        minTOD = tmpTS;
                    tmpTS = tmpTS.Add(TimeSpan.FromSeconds(tb.Duration));
                    if (tmpTS.TotalHours < 24)
                    {
                        if (maxTOD < tmpTS)
                        {
                            maxTOD = tmpTS;
                        }
                    }
                    else if (tmpTS.TotalHours == 24)
                    {
                        maxTOD = tmpTS;
                    }
                    else
                    {
                        minTOD = TimeSpan.Zero;
                        maxTOD = TimeSpan.FromHours(24);
                        break;
                    }
                }
                if (bindReservations)
                { // should be Reservations
                    reservations = new List<Reservation>();
                    foreach (Reservation r in schedulingData)
                    {
                        reservations.Add(r);
                    }
                }
                else
                {  // should be TimePeriods
                    TimeBlock range = new TimeBlock(StartTime, EndTime);
                    periods = new List<TimePeriod>();
                    foreach (TimePeriod tp in schedulingData)
                    {
                        if(range.Contains(tp)){
                            periods.Add(tp);
                        }
                        else if (range.Intersects(tp))
                        {
                            TimeBlock tmpTB = range.Intersection(tp);
                            periods.Add(new TimePeriod(tmpTB.Start, tmpTB.Duration, tp.quantum));
                        }
                    }
                    TimeBlock[] scheduledTBs = TimeBlock.Remaining(new TimeBlock[] { range }, periods.ToArray());
                    if (scheduledTBs != null && scheduledTBs.Length > 0)
                    {
                        periods.AddRange(TimePeriod.MakeArray(scheduledTBs, 0));
                    }
                    periods.Sort();
                }
            }
            else
            {
                minTOD = TimeSpan.Zero;
                maxTOD = TimeSpan.FromHours(24);
                periods = new List<TimePeriod>();
                periods.Add(new TimePeriod(StartTime, EndTime, 0));
            }
        }


        #endregion


    }

    /// <summary>
	/// Delegate for passing an event primary key.
	/// </summary>
	public delegate void ScheduledClickDelegate(object sender, ScheduledClickEventArgs e);

	/// <summary>
	/// Delegate for passing a starting time.
	/// </summary>
	public delegate void AvailableClickDelegate(object sender, AvailableClickEventArgs e);


    public class ScheduledClickEventArgs : EventArgs
    {
        private string value;

        public string Value
        {
            get { return value; }
        }

        public ScheduledClickEventArgs(string value)
        {
            this.value = value;
        }
    }

    public class AvailableClickEventArgs : EventArgs
    {
        private DateTime start;
        private int duration;
        private int quantum;


        public DateTime Start
        {
            get { return start; }
        }

        public int Duration
        {
            get
            {
                return duration;
            }
        }
        public int Quantum
        {
            get
            {
                return quantum;
            }
        }

        public AvailableClickEventArgs(string values)
        {
            string[] data = values.Split(new char[] { ',' });
            start = DateUtil.ParseUtc(data[0]);
           // start.Kind = DateTimeKind.Utc;
            duration = Int32.Parse(data[1]);
            quantum = Int32.Parse(data[2]);
        }
    }
}
