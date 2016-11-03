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

        public bool StartTransaction()
        {
            try
            {
                VerifyAppState(StateConstants.STATE_200);
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_SELL_RETURN);
                Console.WriteLine("Info: Starting the Transaction");
                return VerifyAppState(StateConstants.STATE_900);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application failed to Start a Transaction" , ex.StackTrace);
            }
        }

        public bool SuspendTransaction()
        {
            try
            {
                ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                wVStoreMainWindow.WaitWhileBusy();
                return VerifyAppState(StateConstants.STATE_200);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application failed to select Suspend Transaction", ex.StackTrace);
            }
        }

        public bool HoldTransaction()
        {
            try
            {
                VerifyAppState(StateConstants.STATE_1000);
                PressSpecialKey(KeyboardInput.SpecialKeys.F11);
                ClickOnButton(wHoldExpiryDateWin, "btnSelectPromiseDate");
                wVStoreMainWindow.WaitWhileBusy();
                return VerifyAppState(StateConstants.STATE_200);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application failed to select Hold Transaction", ex.StackTrace);
            }

        }

        public bool SelectRecoverTransactions()
        {
            try
            {
                wVStoreMainWindow.WaitWhileBusy();
                VerifyAppState(StateConstants.STATE_200);
                ClickOnButton(ButtonConstants.BTN_RECOVER_TRANSACTIONS);
                return VerifyAppState(StateConstants.STATE_9010);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application Failed to select Recover Transactions", ex.StackTrace);
            }
        }

        public bool VoidTransaction()
        {
            try
            {
                ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
                PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                wVStoreMainWindow.WaitWhileBusy();
                return VerifyAppState(StateConstants.STATE_200);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application Failed to Void Transaction", ex.StackTrace);
            }
        }

        public bool ResumeTransaction()
        {
            Employee Emp = new Employee();

            try
            {
                ClickOnButton(ButtonConstants.BTN_RESUME);
                Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
                Thread.Sleep(CommonData.iWinLoadingWait);
                return wResumeTransactionWin.Enabled;
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application Failed to Resume Transactions", ex.StackTrace);
            }
        }

        public void ScanBarCode(string sBarCode) 
        {
            try
            {
                VerifyAppStateAndLabel(StateConstants.STATE_1000, AppConstants.SCAN_BARCODE_LABEL);
                EnterCredential(sBarCode);
                Assert.True(VerifyAppState(StateConstants.STATE_1000), "The Application is not in " + StateConstants.STATE_1000);
                Console.WriteLine("Info: The Application is in State: " + StateConstants.STATE_1000);
             }
            catch (Exception ex)
            {
                throw new AutomationException("Error: The App Failed to Scan the Barcode", ex.StackTrace);
            }
        }

        public void ScanBarCodeAndTender(string sBarCode)
        {
            try
            {
                ScanBarCode(sBarCode);
                TenderTransactionWithCash();
            }
            catch (Exception ex)
            {
                throw new AutomationException("Error: The App Failed to ScanBarcode and Tender", ex.StackTrace);
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

        public void TenderTransaction(string sTenderType, string sTenderAmount = null)
        {
            try
            {
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_TENDER);
                wVStoreMainWindow.WaitWhileBusy();
                Thread.Sleep(CommonData.iMinWait);

                switch (sTenderType)
                {
                    case "Cash":
                        PayWithCash();
                        break;
                    case "Credit Card":
                        //PayWithCard()do something
                        break;
                    case "Debit Card":
                        //do something
                        break;
                    case "Gift Card":
                        //do something
                        break;
                    case "US Cash":
                        //do something
                        break;
                    case "Cheque":
                        //do something
                        break;
                    case "Gift Certificate":
                        //do something
                        break;
                    case "Mall Coupon":
                        //do something
                        break;
                    case "Payroll Deduction":
                        //do something
                        break;
                    case "Wireless Payment":
                        //do something
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new AutomationException("Error: The App Failed Tender with the Payment mode of "+ sTenderType, ex.StackTrace);
            }
        }

        public bool PayWithCash(string sPaymentAmount = null)
        {
            string sChangeDueValue;

            try
            {
                VerifyAppStateAndLabel(StateConstants.STATE_7000, AppConstants.PAYMENT_MODE_PROMPT);
                PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                VerifyAppStateAndLabel(StateConstants.STATE_7040, AppConstants.ENTER_TENDER_AMT_PROMPT);

                if (sPaymentAmount == null)
                {
                    PressEnter(wVStoreMainWindow);
                    VerifyAppStateAndLabel(StateConstants.STATE_9900, AppConstants.ENTER_CHANGE_DUE_PROMPT);
                    sChangeDueValue = GetPanel(wVStoreMainWindow, "txtSku").Text.ToString();
                    Assert.True(sChangeDueValue == "0.00", "Change Due Value is not matching with the Product Cost");
                }
                else
                {
                    EnterCredential(sPaymentAmount);
                    VerifyAppStateAndLabel(StateConstants.STATE_9900, AppConstants.ENTER_CHANGE_DUE_PROMPT);
                    sChangeDueValue = GetPanel(wVStoreMainWindow, "txtSku").Text.ToString();
                    Assert.True(sChangeDueValue != "0.00", "Change Due Value is not matching with the Entered Tender Amount");
                }

                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                return VerifyAppState(StateConstants.STATE_9900);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The App Failed to make payment with Cash", ex.StackTrace);
             }

        }

        public bool PrintReceipt(string sPrintOption, string sEmail = null)
        {
            Boolean bResults = false;
            try
            {
                VerifyAppStateAndLabel(StateConstants.STATE_9905, AppConstants.PRINT_RECEIPT_PROMPT);

                switch (sPrintOption)
                {
                    case "Print":
                        PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                        break;
                    case "Email":
                        PressSpecialKey(KeyboardInput.SpecialKeys.F2);
                        //To Be Implemented
                        break;
                    case "PrintAndEmail":
                        PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                        //To Be Implemented
                        break;
                }
                return (!bResults);
            }
            catch (Exception ex)
            {
                return bResults;
                throw new AutomationException("Error: The App Failed to Print the Receipt", ex.StackTrace);
            }
        }

        public void TenderTransactionWithCash()
        {
            ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_TENDER);
            wVStoreMainWindow.WaitWhileBusy();
            Thread.Sleep(2000);

            if (VerifyAppStateAndLabel(StateConstants.STATE_7000, AppConstants.PAYMENT_MODE_PROMPT))
            {
                PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                VerifyAppStateAndLabel(StateConstants.STATE_7040, AppConstants.ENTER_TENDER_AMT_PROMPT);

                PressEnter(wVStoreMainWindow);
                VerifyAppStateAndLabel(StateConstants.STATE_9900, AppConstants.ENTER_CHANGE_DUE_PROMPT);

                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                VerifyAppStateAndLabel(StateConstants.STATE_9905, AppConstants.PRINT_RECEIPT_PROMPT);

                PressEnter(wVStoreMainWindow);
            }
            else
            {
                LoggerUtility.WriteLog("Fail: Failed to tender the transaction with Cash");
            }
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

        public bool IsTransactionOnHoldAtCustForm(string sTransactionNumber)
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










