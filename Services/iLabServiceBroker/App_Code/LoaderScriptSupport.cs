using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for LoaderScriptProcesser
/// </summary>
public class LoaderScriptSupport
{
    public LoaderScriptSupport()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    /// <summary>
    /// Process will take all %field% in the loader script and replace them with values from
    /// the HTTPsession
    /// </summary>
    /// <param name="loaderScript"></param>
    /// <param name="session"></param>
    public static string process(ref string loaderScript, HttpSessionState session)
    {
        string newLoaderScript = loaderScript;
        int i = 0;
        while (-1 != (i = loaderScript.IndexOf('%')))
        {
            //int i = loaderScript.IndexOf('%');
            int y = loaderScript.IndexOf('%', i + 1);
            string variableName = loaderScript.Substring(i + 1, y - i - 1);
            string sessionValue = session[variableName] as string;
            newLoaderScript = newLoaderScript.Replace("%" + variableName + "%", sessionValue);
        }
        loaderScript = newLoaderScript;
        return newLoaderScript;
    }

    /// <summary>
    /// After it was discovered that the information was stored in a different way the above method was
    /// changed to support a hashtable instead.
    /// </summary>
    /// <param name="loaderScript"></param>
    /// <param name="session"></param>
    public static string process(ref string loaderScript, IDictionary<string,string> session)
    {
        string newLoaderScript = loaderScript;
        int i = 0;
        while (-1 != (i = newLoaderScript.IndexOf('%')))
        {
            //int i = loaderScript.IndexOf('%');
            int y = newLoaderScript.IndexOf('%', i + 1);
            string variableName = newLoaderScript.Substring(i + 1, y - i - 1);
            string sessionValue = session[variableName] as string;
            newLoaderScript = newLoaderScript.Replace("%" + variableName + "%", sessionValue);
        }
        loaderScript = newLoaderScript;
        return newLoaderScript;
    }
}
