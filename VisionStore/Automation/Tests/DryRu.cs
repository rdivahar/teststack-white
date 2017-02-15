using NUnit.Framework;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.PropertyGridItems;
using Jesta.VStore.Automation.Framework;
using System.Diagnostics;
using TestStack.White.UIItems.WindowItems;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.AppLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using TestStack.White.WindowsAPI;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;
using RelevantCodes.ExtentReports;
using NUnit.Framework.Interfaces;

namespace Jesta.Automation.VisionStore.Tests
{
    [TestFixture]
    public class DryRun : CommonUtility
    {
        Transactions Trans = new Transactions();
        Employee Emp = new Employee();
        Customer Cust = new Customer();
        public ExtentReports extent;
        public ExtentTest test;
        string testName;

        [OneTimeSetUp]
        public void SetUp()
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
            // LoggerUtility.SetupReportConfig();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            extent.Flush();
            extent.Close();
        }

        [SetUp]
        public void LaunchApp()
        {
            LoggerUtility.WriteLog("Launching the Vision Store Client");
        }

        [TearDown]
        public void LogResultAndCloseApp()
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
            Exit();
        }

       // [Test, Order(1)]
        public void RecoverTransactions()
        {
            Init();
            test = extent.StartTest("RecoverTransactions");

            Trans.SelectRecoverTransactions();
            test.Log(LogStatus.Info, "Selected RecoverTransactions");

            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);
            test.Log(LogStatus.Info, "Authenticated Employee Details");

            Assert.True(VerifyAppState(StateConstants.STATE_461), "Appstate Transition to State 461");
            test.Log(LogStatus.Pass, "AppState Transition Functionality");

            Assert.True(Trans.NoTransactionToTransaferMessage(), "No Transactions To Transfer Message Existance");
            test.Log(LogStatus.Pass, "Recover Transactions Functionality");
        }

       // [Test, Order(2)]
        public void Transaction_WithCash_WithCustomer()
        {
            Init();
            test = extent.StartTest("Transaction_WithCash_WithOutCustomer");

            Trans.StartTransaction();
            test.Log(LogStatus.Info, "Started the Transaction");

            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
            test.Log(LogStatus.Info, "Authenticated The Employee Details");

            Cust.SetCustomer(CommonData.sCutomerName);
            test.Log(LogStatus.Info, "Selected The Customer");

            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName));
            test.Log(LogStatus.Pass, "Verification Of Customer Selection");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            test.Log(LogStatus.Info, "Entered The Sales Advisor Details");

            Trans.ScanBarCodeAndTender(CommonData.BLUE_MINISTR_PRODCODE);
            test.Log(LogStatus.Pass, "Scanned and Tendered The Transaction WithCash and WithCustomer");
        }

       // [Test, Order(3)]
        public void Transaction_WithCash_WithOutCustomer()
        {
            Init();
            test = extent.StartTest("Transaction_WithCash_WithOutCustomer");

            Trans.StartTransaction();
            test.Log(LogStatus.Info, "Started the Transaction");

            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
            test.Log(LogStatus.Info, "Authenticated The Employee Details");

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            test.Log(LogStatus.Info, "Closed the Customer Window");

            Assert.True(Cust.VerifyNoCustomerIsSelected(), "Failed to validate the NoCustomer");
            test.Log(LogStatus.Pass, "Verification Of NoCustomer is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            test.Log(LogStatus.Info, "Entered The Sales Advisor Details");

            Trans.ScanBarCodeAndTender(CommonData.BLUE_MINISTR_PRODCODE);
            test.Log(LogStatus.Pass, "Scanned and Tendered The Transaction WithCash and WithOut Customer");

            Trans.PrintReceipt("Print");
            test.Log(LogStatus.Pass, "Printing the Receipt");
        }

        //[Test, Order(3)]
        public void TestThree()
        {
            Init();
            test = extent.StartTest("TestThree");
            Assert.True(true);
            test.Log(LogStatus.Pass, "Transaction With Cash and With Customer");
        }

        //[Test, Order(4)]
        public void TestFour()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Init();
            test = extent.StartTest(testName);
            Assert.True(true);
            test.Log(LogStatus.Pass, "Transaction With Cash and With Customer");
        }

        //[Test, Order(5)]
        public void TestFive()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Init();
            test = extent.StartTest(testName);
            Assert.True(true);
            test.Log(LogStatus.Pass, "Transaction With Cash and With Customer");
        }

        //[Test, Order(4)]
        public void Transaction_Suspend_NoCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            Init();
            test = extent.StartTest("Transaction_Suspend_NoCustomer");

            Trans.StartTransaction();
            test.Log(LogStatus.Info, "Started the Transaction");

            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
            test.Log(LogStatus.Info, "Authenticated The Employee Details");

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            Assert.True(Cust.VerifyNoCustomerIsSelected());
            test.Log(LogStatus.Info, "Verified No Customer Is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.BLUE_MINISTR_PRODCODE);
            test.Log(LogStatus.Info, "Authenticated The Employee Details and Scanned the Product Barcode");

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            test.Log(LogStatus.Info, "The Current Transaction Number is " + Trans.GetCurrentTransactionNmbr());

            //Suspend Transactions
            Assert.True(Trans.SuspendTransaction(), "Suspending the Transaction");
            test.Log(LogStatus.Info, "Suspending The Transaction");

            //ResumeTransactions
            Assert.True(Trans.ResumeTransaction(), "Resume The Transaction");
            test.Log(LogStatus.Pass, "Verified The Transaction Is Resumed");

            //Select SuspendTab & Suspend the Transaction
            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Assert.True(Trans.IsTransactionSuspended(sTransactionNumber), "Verifying The Transaction Is Suspended");
            test.Log(LogStatus.Pass, "Verified The Transaction Is Suspended");

            //VoidSuspendedTransaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber));
            test.Log(LogStatus.Info, "Voiding the Suspended Transaction");
        }

        //[Test, Order(5)]
        public void Transaction_Suspend_WithCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName));

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.BLUE_MINISTR_PRODCODE);

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            Assert.True(Trans.SuspendTransaction(), "Step: Suspend Transaction");
            Assert.True(Trans.ResumeTransaction(), "Step: Resume Transaction");

            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Assert.True(Trans.IsTransactionSuspended(sTransactionNumber), "Step: Verify Transaction Suspended");

            //VoidSuspendedTransaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber), "Step: Void The Suspended Transaction");
        }

        //[Test, Order(6)]
        public void Test_Transaction_Hold_WithCustomer()
        {
            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName), "Step: Load the Customer");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.BLUE_MINISTR_PRODCODE);

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            Console.WriteLine("The Transaction number is " + sTransactionNumber);
            Assert.True(Trans.HoldTransaction(), "Step: Hold Transactions");

            //Verfiy OnHold Transactions from  Cust
            Cust.OpenCustomerWindow();
            Cust.SearchCustomers("Paul Summer");
            Trans.IsTransactionOnHoldAtCustForm(sTransactionNumber);
            Cust.CloseCustomerWindow();

            //Verify OnHold Transactions from ResumeTransactions
            Assert.True(Trans.ResumeTransaction(), "Step: Resume Transaction");
            Trans.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            Trans.IsTransactionOnHold(sTransactionNumber);

            //Void Transaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber), "Step: Void The Suspended Transaction");
        }

        //[Test, Order(7)]
        public void Test_Merge_Suspended_Transaction()
        {
            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            Assert.True(Cust.VerifyNoCustomerIsSelected());

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.VJ_PEREWINKLE_CREW);

            //Get the current Transaction Number. Future transaction will merge with the specific one.
            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();

            //Suspend Transactions
            Assert.True(Trans.SuspendTransaction());

            //Start a new transaction and merge with previous suspended transaction.
            Trans.StartTransaction();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            Assert.True(Cust.VerifyNoCustomerIsSelected());

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.VJ_PEREWINKLE_CREW);

            Trans.MergeTransaction();
            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Trans.SelectCorrespondingTransaction(sTransactionNumber);

            Trans.TenderTransactionWithCash();
        }

        //[Test, Order(8)]
        public void Test_Merge_Hold_Transaction()
        {
            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName), "Step: Load the Customer");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.VJ_PEREWINKLE_CREW);

            //Get the current Transaction Number and Future transaction will merge with the specific one
            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();

            //Put the transaction on hold
            Assert.True(Trans.HoldTransaction(), "Step: Hold Transactions");

            //Start a new transaction and merge with previous hold transaction.
            Trans.StartTransaction();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            Assert.True(Cust.VerifyNoCustomerIsSelected());

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.VJ_PEREWINKLE_CREW);

            Trans.MergeTransaction();
            Trans.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            Trans.SelectCorrespondingTransaction(sTransactionNumber);

            Trans.TenderTransactionWithCash();
        }
    }
}

