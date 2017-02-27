using NUnit.Framework;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.AppLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using Jesta.VStore.Automation.Framework.Framework.CommonLibrary;

namespace Jesta.Automation.VisionStore.Tests
{
    [TestFixture, Category("CustomerTests"), Category("RegressionTests")]
    public class CustomerTests : CommonUtility
    {
        Customer Cust = new Customer();
        DatabaseUtility dbUtil = new DatabaseUtility();
        string testName;

        [SetUp]
        public void LaunchApp()
        {
            InitializeAndSetToBaseState();
        }

        [TearDown]
        public void LogResultAndCloseApp()
        {
            LoggerUtility.GenerateReport(testName);
            CloseVStoreAndChildWindows();
        }

        //[Test]
        public void TestDBConnections()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            DatabaseUtility dbUtil = new DatabaseUtility();
            string sCustomersCount = dbUtil.RetrieveFieldValueFromDB(CommonData.qCustomersCount, CommonData.sCustCountFieldName);
            LoggerUtility.StatusInfo("[" + sCustomersCount + "]");
        }

        [Test]
        public void CustomerWin_UserInterface()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Cust.OpenCustomerWindow();
            string sValue = GetLabel(Cust.wCustomerWin, AppConstants.LBL_RESULTSCOUNT).Name;
            LoggerUtility.WriteLog("<Info: The Number Of Customers Are ["+sValue+"]>");

            Assert.True(Cust.IsTableHeaderContains(AppConstants.TBL_CUST_DETAILS, AppConstants.LBL_TITLE_FNAME));
            LoggerUtility.StatusPass("Verified The TableHeader For The Title - "+ AppConstants.LBL_TITLE_FNAME);

            Assert.True(Cust.IsTableHeaderContains(AppConstants.TBL_CUST_DETAILS, AppConstants.LBL_TITLE_LNAME));
            LoggerUtility.StatusPass("Verified The TableHeader For The Title - " + AppConstants.LBL_TITLE_LNAME);

            Assert.True(Cust.IsTableHeaderContains(AppConstants.TBL_CUST_DETAILS, AppConstants.LBL_TITLE_PHONE));
            LoggerUtility.StatusPass("Verified The TableHeader For The Title - " + AppConstants.LBL_TITLE_PHONE);

            Assert.True(Cust.IsTableHeaderContains(AppConstants.TBL_CUST_DETAILS, AppConstants.LBL_TITLE_EMAIL));
            LoggerUtility.StatusPass("Verified The TableHeader For The Title - " + AppConstants.LBL_TITLE_EMAIL);

            Assert.True(Cust.btnAddCustomer.Enabled);
            LoggerUtility.StatusPass("Verified The Button [" + Cust.btnAddCustomer.Name + "] Is Enabled");

            Assert.True(Cust.btnSearchCustomer.Enabled);
            LoggerUtility.StatusPass("Verified The Button [" + Cust.btnSearchCustomer.Name + "] Is Enabled");

            Cust.CloseCustomerWindow();
        }

        [Test]
        public void NumberOfCustomers()
        {
            string sCountInApp;
            string sCountInDB;

            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Cust.OpenCustomerWindow();
            sCountInApp = Cust.GetNumberOfCustomersInApp();
            LoggerUtility.StatusInfo("The Number Of Customers In the App = "+sCountInApp);

            sCountInDB = dbUtil.RetrieveFieldValueFromDB(CommonData.qCustomersCount, CommonData.sCustCountFieldName);
            LoggerUtility.StatusInfo("The Number Of Customers In the DataBase = "+sCountInDB);

            Assert.That(sCountInApp == sCountInDB);
            LoggerUtility.StatusPass("Verified The Number Of Customers In App [" + sCountInApp + "], Equals the Count In the DB ["+ sCountInDB+"]");
            Cust.CloseCustomerWindow();
        }

        [Test] [TestCaseSource(typeof(CommonData), "CustomerNames")]
        public void SearchCustomersByName(string sFirstName, string sLastName)
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Cust.OpenCustomerWindow();
            Cust.SearchCustomerAndSelect(sFirstName, sLastName);
            Cust.CloseCustomerWindow();
        }

        [Test][TestCaseSource(typeof(CommonData), "PhoneOrEmail")]
        public void SearchCustomersByPhoneOrEmail(string sPhoneOrEmail)
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Cust.OpenCustomerWindow();
            Assert.True(Cust.SearchCustomerAndSelect(sPhoneOrEmail));
            LoggerUtility.StatusPass("Verified The Customer Search And Set");

            Cust.CloseCustomerWindow();
        }

        //[Test]
        public void InvalidSearchTest()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);
            LoggerUtility.WriteLog("GETHU MAMAEE");
        //Need to Add Tests For Invalid Searches
        }

        //[Test]
        public  void Test()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Cust.OpenCustomerWindow();
            LoggerUtility.WriteLog(Cust.tblCustomerList.ToString());
            Cust.CloseCustomerWindow();
        }
        

    }
}