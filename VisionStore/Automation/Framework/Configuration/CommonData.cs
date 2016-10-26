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

namespace Jesta.VStore.Automation.Framework.Configuration
{
    public static class CommonData
    {
       
        //Application Data
        public static string PROG_PATH = @"C:\VisionStore\VSClient\VisionStore.exe";
        public static string PROG_NAME = "VisionStore";

        //Employee Info 
        public static string EMP_ID = "8600";
        public static string EMP_PWD = "jestais";
        public static string SALES_ADVISOR_ID = "5656";

        //Customer Info
        public static string sCutomerName = "Paul Summer"; 

        //Product Details
        public static string BLUE_MINISTR_PRODCODE = "01050896-06";

        //TimeOut Wait Data
        public static int iMinWait = 1000;
        public static int iLoadingTime = 2500;
    }
}










    