using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.IO;
using System.Diagnostics;

//****************************************
// Copyright (c) SystemsAdminPro 2011
//****************************************

namespace ActiveDirectoryBuilder
{
    class ADBMain
    {
        static void LogToEventLog(string strAppName, string strEventMsg, int intEventType)
        {
            string sLog;

            sLog = "Application";

            if (!EventLog.SourceExists(strAppName))
                EventLog.CreateEventSource(strAppName, sLog);

            //EventLog.WriteEntry(strAppName, strEventMsg);
            EventLog.WriteEntry(strAppName, strEventMsg, EventLogEntryType.Information, intEventType);
        }

        static void Main(string[] args)
        {
            LogToEventLog("ActiveDirectoryBuilder", "ActiveDirectoryBuilder started successfully.", 8201);

            // [Comment] Get local domain context
            string rootDSE;

            System.DirectoryServices.DirectorySearcher objrootDSESearcher = new System.DirectoryServices.DirectorySearcher();
            rootDSE = objrootDSESearcher.SearchRoot.Path;
            // [DebugLine]Console.WriteLine(rootDSE);

            // [Comment] Construct DirectorySearcher object using rootDSE string
            System.DirectoryServices.DirectoryEntry objrootDSEentry = new System.DirectoryServices.DirectoryEntry(rootDSE);

            //TextReader tr = new StreamReader("C:\\ProgramConfig\\ADEProjectProto4Config.txt");
            TextReader tr = new StreamReader("ADBConfig.txt");
            string strnewOUList = tr.ReadToEnd();
            tr.Close();
            string[] strOUListElements = strnewOUList.Split(',');
            int numListSeparator = 0; // hold position of the colon in the string
            int numListSeparator2 = 0; // hold position of the pipe character in the string
            string strnewOULevel1 = ""; // first level OU below the root when creating a sub OU
            string strnewOULevel2 = "";
            foreach (string strOUListElement in strOUListElements)
            {
                numListSeparator = strOUListElement.IndexOf(':');
                if (strOUListElement.Substring(0, numListSeparator) == "Root")
                {
                    //DirectoryEntry newou = objrootDSEentry.Children.Add("OU="+strOUListElement.Substring(numListSeparator,((int)strOUListElement.Length)-numListSeparator) , "organizationalUnit");
                    DirectoryEntry newou = objrootDSEentry.Children.Add("OU=" + strOUListElement.Substring(numListSeparator + 1), "organizationalUnit");
                    newou.CommitChanges();
                }
                //if (strOUListElement.Substring(0, numListSeparator) == "Sub")
                // {
                //      DirectoryEntry newou = objrootDSEentry.Children.Add("OU=" + strOUListElement.Substring(numListSeparator, ((int)strOUListElement.Length) - numListSeparator), "organizationalUnit");
                //}
            }



            //DirectoryEntry newou = objrootDSEentry.Children.Add("OU=NewTestOrg", "organizationalUnit");
            //newou.CommitChanges();

            LogToEventLog("ActiveDirectoryBuilder", "ActiveDirectoryBuilder stopped.", 8202);
        }
    }
}
