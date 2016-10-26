using System;
using TestStack.White;
using System.Diagnostics;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using TestStack.White.Factory;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WPFUIItems;
using NUnit.Framework;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using TestStack.White.UIItems.TabItems;

namespace Jesta.VStore.Automation.Framework.AppLibrary
{
    
    public class Transactions : CommonUtility
    {
        public Window wResumeTransactionWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_RESUME_TRANSACTIONS);
            }
        }

        private Window wHoldExpiryDateWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_HOLD_TRANSEXPIRY);
            }
        }

        public void SellOrReturn()
        {
            wVStoreMainWindow.WaitWhileBusy();
            Label appState = GetLabel(StateConstants.APPSTATE_LABEL_ID);

            if (appState.NameMatches(StateConstants.STATE_200))
            {
                try
                {
                    //Click On Sell/Return Button
                    ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_SELL_RETURN);
                    Label appState_900 = GetAppState(StateConstants.STATE_900);
                    Assert.True(appState_900.NameMatches(StateConstants.STATE_900), "App is in different State");
                    Console.WriteLine("Note: CustomerScreen Doesnt Appear");
                }
                catch (AssertionException ex)
                {
                    Console.WriteLine("Error: The App is not in state 900. StackTrace: " + ex.StackTrace);
                    throw;
                }
            }
        }

        public void ScanBarCodeAndTender(string sBarCode)
        {
            //Check the Appstate is [1000]
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            Thread.Sleep(2000);
            Label appState_1000 = GetAppState(StateConstants.STATE_1000);
            Assert.True(appState_1000.Enabled, "Appstate is Enabled");

            if (appState_1000.NameMatches(StateConstants.STATE_1000) && majorPromptLabel.NameMatches(AppConstants.SCAN_BARCODE_LABEL))
            {
                try
                {
                    this.EnterCredential(sBarCode);
                    Assert.True(appState_1000.Enabled);
                    this.TenderTransactionWithCash();
                }
                catch (AssertionException ex)
                {
                    LoggerUtility.WriteLog("Failure Message: " + ex.StackTrace);
                }
            }
        }

        public void ScanBarCode(string sBarCode)
        {
            //Check the Appstate is [1000]
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            Thread.Sleep(2000);
            Label appState_1000 = GetAppState(StateConstants.STATE_1000);
            Assert.True(appState_1000.Enabled, "Appstate is Enabled");

            if (appState_1000.NameMatches(StateConstants.STATE_1000) && majorPromptLabel.NameMatches(AppConstants.SCAN_BARCODE_LABEL))
            {
                try
                {
                    this.EnterCredential(sBarCode);
                    Assert.True(appState_1000.Enabled);
                }
                catch (AssertionException ex)
                {
                    LoggerUtility.WriteLog("Failure Message: " + ex.StackTrace);
                }
            }
        }

        public void SelectRecoverTransactions()
        {
            ///<summary>
            ///Description: This Method is to select the Recover Transactions Functionality
            ///Parameters: N/A
            ///Verification: On Selection, the state should change from [200] to [900]
            ///</summary>

            WaitForWinToLoad(wVStoreMainWindow);
            Label appState_200 = GetAppState(StateConstants.STATE_200);

            if (appState_200.NameMatches(StateConstants.STATE_200))
            {
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_RECOVER_TRANSACTIONS);
                wVStoreMainWindow.WaitWhileBusy();
                Assert.True(GetAppState(StateConstants.STATE_9010).Enabled);
            }
            else
            {
                Console.WriteLine("Error: The Application is Not in State 200");
            }
        }

        public bool NoTransactionAppState()
        {
            Label appState_461 = GetAppState(StateConstants.STATE_461);
            return (appState_461.NameMatches(StateConstants.STATE_461));
        }

        public bool NoTransactionToTransaferMessage()
        {
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            bool bNoTransMessage = (majorPromptLabel.NameMatches("There are no transactions to transfer."));
            wVStoreMainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            wVStoreMainWindow.WaitWhileBusy();
            return bNoTransMessage;
        }

        public void TenderTransactionWithCash()
        {

            ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_TENDER);
            wVStoreMainWindow.WaitWhileBusy();
            Thread.Sleep(2000);

            if (VerifyAppStateAndLabel(StateConstants.STATE_7000, AppConstants.PAYMENT_MODE_PROMPT))
            {
                PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                //PressEnter(wVStoreMainWindow);

                Thread.Sleep(CommonData.iMinWait);
                VerifyAppStateAndLabel(StateConstants.STATE_7040, "Please enter the tender amount.");
                PressEnter(wVStoreMainWindow);

                Thread.Sleep(CommonData.iMinWait);
                VerifyAppStateAndLabel(StateConstants.STATE_9900, "Change Due ...");
                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                //PressEnter(wVStoreMainWindow);

                Thread.Sleep(CommonData.iMinWait);
                VerifyAppStateAndLabel(StateConstants.STATE_9905, "Email / Print Receipt");
                PressEnter(wVStoreMainWindow);
            }
            else
            {
                LoggerUtility.WriteLog("Fail: Failed to tender the transaction with Cash");
            }
        }

        public bool SuspendTransaction()
        {
            ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
            PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            Thread.Sleep(2500);
            return VerifyAppState(StateConstants.STATE_200);
        }

        public bool HoldTransaction()
        {
            VerifyAppState(StateConstants.STATE_1000);
            PressSpecialKey(KeyboardInput.SpecialKeys.F11);
            Thread.Sleep(CommonData.iMinWait);
            ClickOnButton(wHoldExpiryDateWin, "btnSelectPromiseDate");
            Thread.Sleep(CommonData.iMinWait);
            wVStoreMainWindow.WaitWhileBusy();
            return VerifyAppState(StateConstants.STATE_200);
        }

        public bool VoidTransaction()
        {
            ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
            PressSpecialKey(KeyboardInput.SpecialKeys.F3);
            Thread.Sleep(2500);
            return VerifyAppState(StateConstants.STATE_200);
        }

        public bool ResumeTransaction()
        {
            ClickOnButton(ButtonConstants.BTN_RESUME);

            Employee Emp = new Employee();
            Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
            Thread.Sleep(2500);
            return wResumeTransactionWin.Enabled;
        }

        public void SelectResumeTransactionTab(int iHoldOrSuspend)
        { 
            Tab tabCtrlResumeTransactions = GetTab(wResumeTransactionWin, AppConstants.TAB_CTRL_RESUMETRANSACTIONS);
            tabCtrlResumeTransactions.SelectTabPage(iHoldOrSuspend);
            Thread.Sleep(CommonData.iLoadingTime);
        }

        public string GetCurrentTransactionNmbr()
        {
            Label lblTransactionNumber = GetLabel(AppConstants.LBL_TRANS_NUM);
            return lblTransactionNumber.Name.ToString();
        }

        public bool IsTransactionSuspended(string sTransactionNumber)
        {
            this.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            ListView tableResumeTransaction = GetListView(wResumeTransactionWin, AppConstants.TBL_RESUME_TRANS);
            return tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        public bool IsTransactionOnHold(string sTransactionNumber)
        {
            this.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            ListView tableResumeTransaction = GetListView(wResumeTransactionWin, AppConstants.TBL_RESUME_TRANS);
            return tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        public bool IsTransactionOnHoldAtCustInfo(string sTransactionNumber)
        {
            Customer Cust = new Customer();
            Cust.SelectFromCustomerInfoTab(AppConstants.TAB_CUST_ONHOLD);
            ListView TableOnHoldInfo = GetListView(Cust.wCustomerWin, AppConstants.TBL_CUST_ONHOLD_TRANS);
            return TableOnHoldInfo.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        public bool SelectFrmResumeTransactionGrid(string sTranNo)
        {
            Boolean bResults = false;
            try
            {
                ListView tableResumeTransaction = GetListView(wResumeTransactionWin, AppConstants.TBL_RESUME_TRANS);
                tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTranNo).Select();
                return tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTranNo).IsSelected;
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Failed to Select From Resume Transactions", Ex.StackTrace);
            }  
        }

        public bool VoidSuspendedTransaction(string sTransactionNumber)
        {
            String sTranNo = sTransactionNumber.Substring(14);

            SelectFrmResumeTransactionGrid(sTranNo);
            ClickOnButton(wResumeTransactionWin, ButtonConstants.BTN_APPLY);
            Thread.Sleep(CommonData.iLoadingTime);
            wVStoreMainWindow.WaitWhileBusy();
            VerifyAppState(StateConstants.STATE_1000);
            return VoidTransaction();
        }

        public bool VoidOnHoldTransaction(string sTransactionNumber)
        {
            String sTranNo = sTransactionNumber.Substring(14);

            SelectFrmResumeTransactionGrid(sTranNo);
            ClickOnButton(wResumeTransactionWin, ButtonConstants.BTN_APPLY);
            Thread.Sleep(CommonData.iLoadingTime);
            wVStoreMainWindow.WaitWhileBusy();
            VerifyAppState(StateConstants.STATE_1000);
            return VoidTransaction();
        }
    }
  }










