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

        //TestSuiteName
        public static string sBatSuite = ConfigurationManager.AppSettings["BATS"];
        public static string sCustomerSuite = ConfigurationManager.AppSettings["CUSTOMER"];
        public static string sDefaultSuite = "VisionStoreReport";

        //DataBase
        public static string sCustCountFieldName = "COUNT(*)";
        public static string qCustomersCount = "select count(*) from customer";

        //Employee Info
        public static string EMP_ID = "8600";
        public static string EMP_PWD = "jestais";
        public static string SALES_ADVISOR_ID = "1001";

        //Customer Info
        public static string sCutomerName = "Paul Summer";
        public static string[] PhoneOrEmail = new string[] { "4389282785", "stephinjerish@gmail.com",};
        public static object[] CustomerNames = {new object[] { "Paul", "Summer" }, /*new object[] { "Jack", "Daniels" },*/};

        //Product Details
        public static object[] PRODUCTBARCODES = new string[] {"01050896-06", "53364005-01", "05630001-01", "499999018750"};
        public static string BLUE_MINISTR_PRODCODE = "01050896-06";
        public static string VJ_PEREWINKLE_CREW = "53364005-01";
        public static string HUGO_BOSS_SOCKS = "05630001-01";
        public static string ZERIA_BLACK_9 = "499999018750";

        //TimeOut Wait Data
        public static int iMinWait = 500;
        public static int iLoadingTime = 3000;
        public static int iWinLoadingWait = 1000;

        //Payment Method
        public static string BY_CASH = "Cash";

        public struct adminCreds
        {
            public static string username = "realmverify04@mailinator.com";//"abhinav_pgtp7@mailinator.com";
            public static string password = "Test-123";
        }

    }
}










    