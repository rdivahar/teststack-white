using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public static class LoggerUtility
    {
        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetTempPath() + "VSAutomationLog.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "C:\\VStoreLogs\\";
            logFilePath = logFilePath + "VSAutomation_TestLog-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }
    }
}
