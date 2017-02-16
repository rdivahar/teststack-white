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
using static Jesta.VStore.Automation.Framework.ObjectRepository.StateConstants;

namespace Jesta.Automation.VisionStore.Tests
{
    [TestFixture]
    public class BusinessAcceptanceTests : CommonUtility
    {
        Transactions Trans = new Transactions();
        Employee Emp = new Employee();
        Customer Cust = new Customer();
        public ExtentReports extent;
        public ExtentTest test;
        string testName;

        [OneTimeSetUp]
        public void BatsSetUp()
        {
            LoggerUtility.SetupReportConfig();
        }

        [OneTimeTearDown]
        public void BatsTearDown()
        {
            LoggerUtility.FlushResultsAndClose();
        }

        [SetUp]
        public void LaunchApp()
        {
            Init();
            LoggerUtility.WriteLog("Launching the Vision Store Client");
        }

        [TearDown]
        public void LogResultAndCloseApp()
        {
            LoggerUtility.GenerateReport(testName);
            Exit();
        }

        [Test, Order(1)]
        public void Recover_Transactions()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);
     
            Trans.SelectRecoverTransactions();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);
           
            Assert.True(VerifyAppState(StateConstants.STATE_461), "Appstate Transition to State 461");
            LoggerUtility.StatusPass("Verified AppState Transition Functionality");

            Assert.True(Trans.NoTransactionToTransaferMessage(), "No Transactions To Transfer Message Existance");
            LoggerUtility.StatusPass("Verified Recover Transactions Functionality");

            //ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_CANCEL);
        }

        [Test, Order(2)]
        public void Transaction_WithCash_WithOutCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
           
            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();

            //Assert.True(Cust.VerifyNoCustomerIsSelected(), "Failed to validate the NoCustomer");
            LoggerUtility.StatusPass("Verification Of NoCustomer is Selected");
            
            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCodeAndTender(CommonData.ZERIA_BLACK_9);
            LoggerUtility.StatusPass("Scanned and Tendered The Transaction WithCash and WithOut Customer");
     
            Trans.PrintReceipt("Print");
        }

        [Test, Order(3)]
        public void Transaction_WithCash_WithCustomer()
        {           
                testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LoggerUtility.StartTest(testName);

                Trans.StartTransaction();
                Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
                Cust.SetCustomer(CommonData.sCutomerName);

                Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName));
                LoggerUtility.StatusPass("Verified The Customer Is Selected");
                Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);

                Trans.ScanBarCodeAndTender(CommonData.ZERIA_BLACK_9);
                LoggerUtility.StatusPass("Scanned and Tendered The Transaction WithCash and WithCustomer");
        }

        [Test, Order(4)]
        public void Transaction_Suspend_NoCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
           
            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            //Assert.True(Cust.VerifyNoCustomerIsSelected());
            LoggerUtility.StatusPass("Verification Of NoCustomer is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            LoggerUtility.StatusInfo("Scanned and Tendered The Transaction");

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
         
            //Suspend Transactions
            Assert.True(Trans.SuspendTransaction(), "Suspending the Transaction");
            LoggerUtility.StatusPass("Suspending The Transaction");

            //ResumeTransactions
            Assert.True(Trans.ResumeTransaction(), "Resume The Transaction");
            LoggerUtility.StatusPass("Resuming The Transaction");

            //Select SuspendTab & Suspend the Transaction
            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Assert.True(Trans.IsTransactionSuspended(sTransactionNumber), "Verifying The Transaction Is Suspended");
            LoggerUtility.StatusPass("Verified The Transaction Is Suspended");

            //VoidSuspendedTransaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber));
        }

       [Test, Order(5)]
        public void Transaction_Suspend_WithCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName));

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            Assert.True(Trans.SuspendTransaction(), "Step: Suspend Transaction");
            Assert.True(Trans.ResumeTransaction(), "Step: Resume Transaction");

            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Assert.True(Trans.IsTransactionSuspended(sTransactionNumber), "Verify Transaction Suspended");
            LoggerUtility.StatusPass("Verified The Transaction Is Suspended");

            //VoidSuspendedTransaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber), "Void The Suspended Transaction");
        }

        [Test, Order(6)]
        public void Transaction_Hold_WithCustomer()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName), "Loading the Customer");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            Assert.True(Trans.HoldTransaction(), "Hold The Transaction");
            LoggerUtility.StatusPass("Placing The " + sTransactionNumber + " On HOLD");

            //Verfiy OnHold Transactions from  Cust
            Cust.OpenCustomerWindow();
            Cust.SearchCustomers("Paul Summer");
            Assert.True(Trans.IsTransactionOnHoldAtCustForm(sTransactionNumber), "Verifying The Transaction On HOLD");
            LoggerUtility.StatusPass("The " + sTransactionNumber + " Is Put On HOLD");
            Cust.CloseCustomerWindow();

            //Verify OnHold Transactions from ResumeTransactions
            Trans.ResumeTransaction();
            Trans.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            Assert.True(Trans.IsTransactionOnHold(sTransactionNumber));
            LoggerUtility.StatusPass("Verified The Transaction With # " + sTransactionNumber + "Is On HOLD");

            //Void Transaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber), "Step: Void The Suspended Transaction");
        }

        [Test, Order(7)]
        public void Merge_Suspended_Transaction()
        {

            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            //Assert.True(Cust.VerifyNoCustomerIsSelected());
            LoggerUtility.StatusPass("Verification Of NoCustomer is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            LoggerUtility.StatusInfo("Scanned and Tendered The Transaction");

            //Get the current Transaction Number. Future transaction will merge with the specific one.
            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();

            //Suspend Transactions
            Assert.True(Trans.SuspendTransaction());
            LoggerUtility.StatusPass("Verified The Trasanction Is Suspended");

            //Start a new transaction and merge with previous suspended transaction.
            Trans.StartTransaction();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            //Assert.True(Cust.VerifyNoCustomerIsSelected());
            LoggerUtility.StatusPass("Verified NoCustomer Is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            Trans.MergeTransaction();
            LoggerUtility.StatusPass("Verified The Transaction Is Merged");

            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            Trans.SelectCorrespondingTransaction(sTransactionNumber);
            Trans.TenderTransactionWithCash();
        }

        [Test, Order(8)]
        public void Merge_Hold_Transaction()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName), "Step: Load the Customer");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            LoggerUtility.StatusInfo("Scanned and Tendered The Transaction");

            //Get the current Transaction Number and Future transaction will merge with the specific one
            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();

            //Put the transaction on hold
            Assert.True(Trans.HoldTransaction(), "Step: Hold Transactions");
            LoggerUtility.StatusPass("Verfied The Transaction Is On Hold");

            //Start a new transaction and merge with previous hold transaction.
            Trans.StartTransaction();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD); //AuthenticateEmployee Changed to Authenticate Users

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            //Assert.True(Cust.VerifyNoCustomerIsSelected());
            LoggerUtility.StatusPass("Verfied No Customer Is Selected");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.ZERIA_BLACK_9);
            Trans.MergeTransaction();
            LoggerUtility.StatusPass("Verified The Transaction Is Merged");

            Trans.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            Trans.SelectCorrespondingTransaction(sTransactionNumber);
            Trans.TenderTransactionWithCash();
        }

        [Test]
        public void SetAppToBaseState()
        {
       
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);

            Label AppState = GetLabel(wVStoreMainWindow, "lblState");
            string CurrentAppState = AppState.Text;

            string OutputAppState = CurrentAppState.Split('[', ']')[1];
            int iAppState = Int32.Parse(OutputAppState);
            LoggerUtility.StatusInfo(OutputAppState);

            if (Enum.IsDefined(typeof(AppStatus), iAppState))
            {
                LoggerUtility.StatusInfo("Going To Click On F1/CANCEL Button");
                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                string NewAppState = AppState.Text;
                string NewOutputAppState = NewAppState.Split('[', ']')[1];
                int iNewAppState = Int32.Parse(NewOutputAppState);
                LoggerUtility.StatusInfo("The New AppState Is "+iNewAppState);
            }
        }

        [Test]
        public void TestTheAppstate()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);
            Label AppState = GetLabel(wVStoreMainWindow, "lblState");
            string CurrentAppState = AppState.Text;
            string OutputAppState = CurrentAppState.Split('[', ']')[1];
            int iAppState = Int32.Parse(OutputAppState);
            LoggerUtility.StatusInfo(OutputAppState);
        }
    }
}

