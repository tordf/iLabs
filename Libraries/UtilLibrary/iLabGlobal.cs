using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.UtilLib
{
    /// <summary>
    /// static class to provide release tags.
    /// </summary>
    public class iLabGlobal
    {
        static private string date = "$Date: 2010-02-16 11:22:13 -0500 (Tue, 16 Feb 2010) $";
        static private string revision = "$Revision: 168 $";
        //static private string release = "$ilab:Release$";
	static private string release = "Release 3.0.1";
        static private string buildDate = "$ilab:BuildDate$";
        /// <summary>
        /// Returns the date and svn revision last set, still not auto setting..... 
        /// </summary>
        public static string Revision
        {
            get
            {
                return revision + " " + date;
            }
        }
        /// <summary>
        /// returns a release string specified in iLabGlobal
        /// </summary>
        public static string Release
        {
            get
            {
                return release + " ( " + revision.Replace("$", "") + " ) " + date.Replace("$", "");
            }
        }

        /// <summary>
        /// returns the build date of the release, this currently is not set automaticly.
        /// </summary>
        public static string BuildDate
        {
            get
            {
                return buildDate;
            }
        }
    }
}
