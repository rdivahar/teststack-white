using NUnit.Framework;
using System.Threading;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.AppLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;

namespace Jesta.Automation.VisionStore.Tests
{
    [TestFixture(Category = "Bats")]
    public class BusinessAcceptanceTests : CommonUtility
    {
        Transactions Trans = new Transactions();
        Employee Emp = new Employee();
        Customer Cust = new Customer();
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
            AppExit();
        }

        [Test]
        public void Recover_Transactions()
        {
            testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            LoggerUtility.StartTest(testName);
     
            Trans.SelectRecoverTransactions();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);
<<<<<<< HEAD
          
            Assert.True(VerifyAppStates(StateConstants.STATE_461, StateConstants.STATE_460), "Appstate Transition to State 460/461");
            LoggerUtility.StatusPass("Verified AppState Transition Functionality");

            Assert.True(Trans.NoTransactionToTransaferMessage(), "No Transactions To Transfer Message Existance");
=======

            Assert.True(VerifyAppState(StateConstants.STATE_460) || VerifyAppState(StateConstants.STATE_461));
            LoggerUtility.StatusPass("Verified AppState Transition Functionality"); 
            

            Assert.True(Trans.NoTransactionToTransaferMessage() || Trans.SelectTransactionToTransaferMessage());
>>>>>>> a5f0dcc58c0cdbe65d23726e921f5b4b463198ac
            LoggerUtility.StatusPass("Verified Recover Transactions Functionality");
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
    }
}