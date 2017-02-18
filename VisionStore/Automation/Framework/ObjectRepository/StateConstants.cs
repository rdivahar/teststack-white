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

namespace Jesta.VStore.Automation.Framework.ObjectRepository
{
    public class StateConstants
    {

        public static string APPSTATE_LABEL_ID = "lblState";

        //APPPSTATE - AUTOMATION IDs
        public static string STATE_130 = "[130]";
        public static string STATE_140 = "[140]";
        public static string STATE_200 = "[200]";
        public static string STATE_460 = "[460]";
        public static string STATE_461 = "[461]";
        public static string STATE_900 = "[900]";
        public static string STATE_1000 = "[1000]";
        public static string STATE_7000 = "[7000]";
        public static string STATE_7040 = "[7040]";
        public static string STATE_9900 = "[9900]";
        public static string STATE_9905 = "[9905]";
        public static string STATE_9010 = "[9010]";
        public static string STATE_1415 = "[1415]";
   
        public enum AppStatus200
        {
            STATE_900 = 900,
            STATE_905 = 905,
            STATE_1415 = 1415, 
            STATE_9010 = 9010,
            STATE_461 = 461
        }
    }
}










    