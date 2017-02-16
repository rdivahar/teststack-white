using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using System.Diagnostics;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.WindowsAPI;
using System.Threading;
using System.Configuration;

namespace Jesta.VStore.Automation.Framework.Configuration
{
    public static class CommonData
    {
       
        //Application Data
        public static string PROG_PATH = @"C:\VisionStore\VSClient\VisionStore.exe";
        public static string PROG_NAME = "VisionStore";
        public static string Proj_Path = ConfigurationManager.AppSettings["AUTOMATIONDIR"];
        public static string screenshotDir = ConfigurationManager.AppSettings["SCREENSHOTDIR"];
        
        //Employee Info 
        public static string EMP_ID = "8600";
        public static string EMP_PWD = "jestais";
        public static string SALES_ADVISOR_ID = "1001";

        //Customer Info
        public static string sCutomerName = "Paul Summer"; 

        //Product Details
        public static string BLUE_MINISTR_PRODCODE = "01050896-06";
        public static string VJ_PEREWINKLE_CREW = "53364005-01";
        public static string HUGO_BOSS_SOCKS = "05630001-01";
        public static string ZERIA_BLACK_9 = "499999018750";

        //TimeOut Wait Data
        public static int iMinWait = 500;
        public static int iLoadingTime = 1000;
        public static int iWinLoadingWait = 1000;

        //Payment Method
        public static string BY_CASH = "Cash";
    }
}










    