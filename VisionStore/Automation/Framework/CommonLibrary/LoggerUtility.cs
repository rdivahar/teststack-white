using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RelevantCodes.ExtentReports;
using Jesta.VStore.Automation.Framework.Configuration;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;


namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public static class LoggerUtility
    {
        public static ExtentReports extent;
        public static ExtentTest test;

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

        public static void SetupReportConfig()
        {
            string ReportsPath = CommonData.Proj_Path + "Reports\\VisionStoreReport.Html";
            extent = new ExtentReports(ReportsPath, true);
            string Environment = ConfigurationManager.AppSettings["ENVIRONMENT"];
            string Configuration = ConfigurationManager.AppSettings["CONFIGURATION"];
            string Suite = ConfigurationManager.AppSettings["SUITE"];
            extent.AddSystemInfo("Environment", Environment);
            extent.AddSystemInfo("Configuration", Configuration);
            extent.AddSystemInfo("Suite", Suite);
            extent.LoadConfig(CommonData.Proj_Path + "extent-config.xml");
        }

        public static void GenerateReport(string testName)
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "&lt;pre&gt;" + TestContext.CurrentContext.Result.StackTrace + "&lt;/pre&gt;";
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                string ScreenshotPath = ScreenShotUtility.GetScreenshot(testName);
                test.Log(LogStatus.Fail, stackTrace + errorMessage);
                test.Log(LogStatus.Fail, "Error Screenshot Below: " + test.AddScreenCapture(ScreenshotPath));
            }
            extent.EndTest(test);
            LoggerUtility.WriteLog("Closing the Vision Store Client");
        }

        public static void FlushResultsAndClose()
        {
            extent.Flush();
            extent.Close();
        }

        public static void StartTest(string TestCaseName)
        {
            test = extent.StartTest(TestCaseName);
        }

        public static void StatusInfo(string InfoMessage)
        {
            test.Log(LogStatus.Info, InfoMessage);
        }

        public static void StatusPass(string PassMessage)
        {
            test.Log(LogStatus.Pass, PassMessage);
        }

    }
}
