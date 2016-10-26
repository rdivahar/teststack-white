using NUnit.Framework;
using System;
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

namespace Jesta.Automation.VisionStore.Tests
{
    [TestFixture]
    public class BusinessAcceptanceTests : CommonUtility
    {
        Transactions Trans = new Transactions();
        Employee Emp = new Employee();
        Customer Cust = new Customer();
      
        [OneTimeSetUp]
        public void LaunchApp()
        {
            LoggerUtility.WriteLog("Launching the Vision Store Client");
            Init();
        }

        [OneTimeTearDown]
        public void CloseApp()
        {
            LoggerUtility.WriteLog("Closing the Vision Store Client");
            Exit();   
        }

        [SetUp]
        public void SetUp()
        {
        // if (GetAppState("[200]").Visible == false)
        //    {
        //        PressSpecialKey(KeyboardInput.SpecialKeys.F1);
        //    }
        }

        [TearDown]
        public void TearDown()
        {
            //Do Nothing
        }
        
        //[Test, Order(1)]
        public void Test_RecoverTransactions()
        {
            Trans.SelectRecoverTransactions();
            Emp.AuthenticateEmployee(CommonData.EMP_ID, CommonData.EMP_PWD);

            Assert.True(Trans.NoTransactionAppState(), "Load the App to State 461");
            Assert.True(Trans.NoTransactionToTransaferMessage(), "No Transactions To Transfer Message Existance");
        }

        [Test, Order(2)]
        public void Test_Transaction_WithCash_NoCustomer()
        {
            Trans.SellOrReturn();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();

            Assert.True(Cust.VerifyNoCustomerIsSelected());
            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCodeAndTender(CommonData.BLUE_MINISTR_PRODCODE);
        }

        //[Test, Order(3)]
        public void Test_Transaction_WithCash_WithCustomer()
        {
            Trans.SellOrReturn();

            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
            Cust.SetCustomer(CommonData.sCutomerName);

            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName));

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCodeAndTender(CommonData.BLUE_MINISTR_PRODCODE);
        }

        //[Test, Order(4)]
        public void Test_Transaction_Suspend_NoCustomer()
        {
            Trans.SellOrReturn();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            WaitForWinToLoad(Cust.wCustomerWin);
            Cust.CloseCustomerWindow();
            Assert.True(Cust.VerifyNoCustomerIsSelected());

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.BLUE_MINISTR_PRODCODE);

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();

            //Suspend Transactions
            Assert.True(Trans.SuspendTransaction());

            //ResumeTransactions
            Assert.True(Trans.ResumeTransaction(), "Step: Resume Transaction");

            //Select SuspendTab
            Trans.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);

            //Verify the Transaction is Suspeded 
            Assert.True(Trans.IsTransactionSuspended(sTransactionNumber), "Step: Verify Transaction Suspended");

            //VoidSuspendedTransaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber));
        }  

        //[Test, Order(5)]
        public void Test_Transaction_Suspend_WithCustomer()
        {
            Trans.SellOrReturn();
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
            Trans.SellOrReturn();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);

            Cust.SetCustomer(CommonData.sCutomerName);
            Assert.True(Cust.VerifyCustomerIsSet(CommonData.sCutomerName),"Step: Load the Customer");

            Emp.EnterSalesAdvisor(CommonData.SALES_ADVISOR_ID);
            Trans.ScanBarCode(CommonData.BLUE_MINISTR_PRODCODE);

            string sTransactionNumber = Trans.GetCurrentTransactionNmbr();
            Assert.True(Trans.HoldTransaction(), "Step: Hold Transactions");

            //Verfiy OnHold Transactions from  Cust
            Cust.OpenCustomerWindow();
            Cust.SearchCustomers("Paul Summer");
            Trans.IsTransactionOnHoldAtCustInfo(sTransactionNumber);
            Cust.CloseCustomerWindow();

            //Verify OnHold Transactions from ResumeTransactions
            Assert.True(Trans.ResumeTransaction(), "Step: Resume Transaction");
            Trans.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            Trans.IsTransactionOnHold(sTransactionNumber);

            //Void Transaction
            Assert.True(Trans.VoidSuspendedTransaction(sTransactionNumber), "Step: Void The Suspended Transaction");
        }
    }
}

