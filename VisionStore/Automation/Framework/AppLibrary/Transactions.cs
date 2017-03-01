using System;
using TestStack.White;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using TestStack.White.UIItems;
using NUnit.Framework;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using TestStack.White.UIItems.TabItems;

namespace Jesta.VStore.Automation.Framework.AppLibrary
{

    public class Transactions : CommonUtility
    {
        /// <summary>
        /// Returns Transaction Window 
        /// </summary>
        public Window wResumeTransactionWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_RESUME_TRANSACTIONS);
            }
        }

        /// <summary>
        /// Defenition for Transaction Window 
        /// </summary>
        private Window wHoldExpiryDateWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_HOLD_TRANSEXPIRY);
            }
        }

        public Window wMergeTransactionWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_MERGE_TRANSACTIONS);
            }
        }
        
        /// <summary>
        /// Select Sell/Return Button 
        /// </summary>
        /// <returns>True or False based on the Expected App State</returns>
        public bool StartTransaction()
        {
            try
            {
                VerifyAppState(StateConstants.STATE_200);
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_SELL_RETURN);
                LoggerUtility.StatusInfo("Started the Transaction");
                return VerifyAppState(StateConstants.STATE_900);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application failed to Start a Transaction" , ex.StackTrace);
            }
        }

        /// <summary>
        /// Select Suspend Transaction Button
        /// </summary>
        /// <returns>True Or False based on the Expected Appstate</returns>
        public bool SuspendTransaction()
        {
            try
            {
                ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
                LoggerUtility.WriteLog("Clicked the Next Button");
                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                wVStoreMainWindow.WaitWhileBusy();
                LoggerUtility.StatusInfo("Suspending The Transaction");
                return VerifyAppState(StateConstants.STATE_200);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application failed to select Suspend Transaction", ex.StackTrace);
            }
        }

        /// <summary>
        /// Select the Hold Transaction Button and accepts the Expiry Date Window
        /// </summary>
        /// <returns>True or False based on the Expected State</returns>
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

        /// <summary>
        /// Selects the Recover Transaction Button
        /// </summary>
        /// <returns>True of False based on the expected State</returns>
        public bool SelectRecoverTransactions()
        {
            try
            {
                wVStoreMainWindow.WaitWhileBusy();
                VerifyAppState(StateConstants.STATE_200);
                ClickOnButton(ButtonConstants.BTN_RECOVER_TRANSACTIONS);
                LoggerUtility.StatusInfo("Selected RecoverTransactions");
                return VerifyAppState(StateConstants.STATE_9010);
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application Failed to select Recover Transactions", ex.StackTrace);
            }
        }

        /// <summary>
        /// Selects the Void Transaction Button
        /// </summary>
        /// <returns>True of False based on the expected State</returns>
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

        /// <summary>
        /// Selects the Resume Transaction Button
        /// </summary>
        /// <returns>True of False based on the expected State</returns>
        public bool ResumeTransaction()
        {
            Employee Emp = new Employee();

            try
            {
                ClickOnButton(ButtonConstants.BTN_RESUME);
                Emp.AuthenticateUser(CommonData.EMP_ID, CommonData.EMP_PWD);
                Thread.Sleep(CommonData.iWinLoadingWait);
                LoggerUtility.StatusInfo("Resuming The Transaction");
                return wResumeTransactionWin.Enabled;
            }
            catch (Exception ex)
            {
                return false;
                throw new AutomationException("Error: The Application Failed to Resume Transactions", ex.StackTrace);
            }
        }

        /// <summary>
        /// Enter the Barcode value into the textfield based on the input parameter
        /// </summary>
        /// <param name="sBarCode"></param>
        public void ScanBarCode(string sBarCode) 
        {
            try
            {
                VerifyAppStateAndLabel(StateConstants.STATE_1000, AppConstants.SCAN_BARCODE_LABEL);
                EnterCredential(sBarCode);
                LoggerUtility.StatusInfo("The Barcode Of The Scanned Product # " + sBarCode);
                Assert.True(VerifyAppState(StateConstants.STATE_1000), "The Application is not in " + StateConstants.STATE_1000);
                Console.WriteLine("Info: The Application is in State: " + StateConstants.STATE_1000);
             }
            catch (Exception ex)
            {
                throw new AutomationException("Error: The App Failed to Scan the Barcode", ex.StackTrace);
            }
        }

        /// <summary>
        /// Enter the Barcode value based on the input parameter and tender the transaction with cash
        /// </summary>
        /// <param name="sBarCode"></param>
        public void ScanBarCodeAndTender(string sBarCode)
        {
            //TODO - Modify the method to pass a generic Tender Type

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

        public bool NoTransactionToTransaferMessage()
        {
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            bool bNoTransMessage = (majorPromptLabel.NameMatches("There are no transactions to transfer."));

            //wVStoreMainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_CANCEL);//PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            wVStoreMainWindow.WaitWhileBusy();
            return bNoTransMessage;
        }

        public bool SelectTransactionToTransaferMessage()
        {
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            bool bSelectTransMessage = (majorPromptLabel.NameMatches("Select the terminal to transfer from."));

            //wVStoreMainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_CANCEL);//PressSpecialKey(KeyboardInput.SpecialKeys.F1);
            wVStoreMainWindow.WaitWhileBusy();
            return bSelectTransMessage;
        }


        /// <summary>
        /// Tender the transaction based on the passed Tender Type and the Tender Amount (Optional)
        /// </summary>
        /// <param name="sTenderType"></param>
        /// <param name="sTenderAmount"></param>
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

        /// <summary>
        /// Make a payment for the scanned product using Cash based on the entered amount  
        /// </summary>
        /// <param name="sTenderAmount"></param>
        /// <returns>True or False based on the Expected Appstate</returns>
        public bool PayWithCash(string sTenderAmount = null)
        {
            double dChangeDueValue;
            
            try
            {
                VerifyAppStateAndLabel(StateConstants.STATE_7000, AppConstants.PAYMENT_MODE_PROMPT);
                PressSpecialKey(KeyboardInput.SpecialKeys.F3);
                VerifyAppStateAndLabel(StateConstants.STATE_7040, AppConstants.ENTER_TENDER_AMT_PROMPT);

                if (sTenderAmount == null)
                {
                    //Need to Get the 
                    PressEnter(wVStoreMainWindow);
                    VerifyAppStateAndLabel(StateConstants.STATE_9900, AppConstants.ENTER_CHANGE_DUE_PROMPT);
                    dChangeDueValue = Convert.ToDouble(GetPanel(wVStoreMainWindow, "txtSku").Text.ToString());
                    Assert.True(dChangeDueValue == 0.00, "Change Due Value is not matching with the Product Cost");
                }
                else
                {
                    double dTenderAmount = Convert.ToDouble(sTenderAmount);
                    EnterCredential(sTenderAmount);
                    VerifyAppStateAndLabel(StateConstants.STATE_9900, AppConstants.ENTER_CHANGE_DUE_PROMPT);
                    dChangeDueValue = Convert.ToDouble(GetPanel(wVStoreMainWindow, "txtSku").Text.ToString());
                    //Assert.True(dChangeDueValue != 0.00, "Change Due Value is not matching with the Entered Tender Amount");
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

        /// <summary>
        /// Print, Email, Print Email the receipt based on the Print Option and Email (Optional)
        /// </summary>
        /// <param name="sPrintOption"></param>
        /// <param name="sEmail"></param>
        /// <returns></returns>
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
                LoggerUtility.StatusInfo("Printing the Receipt & The Option is " + sPrintOption);
                return (!bResults);
            }
            catch (Exception ex)
            {
                return bResults;
                throw new AutomationException("Error: The App Failed to Print the Receipt", ex.StackTrace);
            }
        }

        /// <summary>
        /// Tender the Transaction with the Product COst
        /// </summary>
        public void TenderTransactionWithCash()
        {
            ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_TENDER);
            wVStoreMainWindow.WaitWhileBusy();
            Thread.Sleep(1000);

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

        /// <summary>
        /// Select the Tab in the Resume Transaction Child Window - From Suspend Or Hold
        /// </summary>
        /// <param name="iHoldOrSuspend"></param>
        public void SelectResumeTransactionTab(int iHoldOrSuspend)
        { 
            Tab tabCtrlResumeTransactions = GetTab(wResumeTransactionWin, AppConstants.TAB_CTRL_RESUMETRANSACTIONS);
            tabCtrlResumeTransactions.SelectTabPage(iHoldOrSuspend);
            Thread.Sleep(CommonData.iLoadingTime);
        }

        /// <summary>
        /// Get the Current transaction number of the active transaction in Progress
        /// </summary>
        /// <returns>Transaction #</returns>
        public string GetCurrentTransactionNmbr()
        {
            Label lblTransactionNumber = GetLabel(AppConstants.LBL_TRANS_NUM);
            string TransNumber = lblTransactionNumber.Name.ToString();
            LoggerUtility.StatusInfo("The Current Transaction Number Is " + TransNumber);
            return TransNumber;
        }

        /// <summary>
        /// Verify the Trasaction is Suspended, and available in the Suspended Transactions Table
        /// </summary>
        /// <param name="sTransactionNumber"></param>
        /// <returns>True Or False</returns>
        public bool IsTransactionSuspended(string sTransactionNumber)
        {
            this.SelectResumeTransactionTab(AppConstants.TAB_SUSPEND);
            ListView tableResumeTransaction = GetListView(wResumeTransactionWin, AppConstants.TBL_RESUME_TRANS);
            return tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        /// <summary>
        /// Verify the Trasaction is on Hold, and available in the Suspended Transactions Table
        /// </summary>
        /// <param name="sTransactionNumber"></param>
        /// <returns>True Or False</returns>
        public bool IsTransactionOnHold(string sTransactionNumber)
        {
            this.SelectResumeTransactionTab(AppConstants.TAB_HOLD);
            ListView tableResumeTransaction = GetListView(wResumeTransactionWin, AppConstants.TBL_RESUME_TRANS);
            return tableResumeTransaction.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        /// <summary>
        /// Verify the Trasaction is on Hold, and available in the Customer Win Details Area
        /// </summary>
        /// <param name="sTransactionNumber"></param>
        /// <returns>True Or False</returns>
        public bool IsTransactionOnHoldAtCustForm(string sTransactionNumber)
        {
            Customer Cust = new Customer();
            Cust.SelectFromCustomerInfoTab(AppConstants.TAB_CUST_ONHOLD);
            ListView TableOnHoldInfo = GetListView(Cust.wCustomerWin, AppConstants.TBL_CUST_ONHOLD_TRANS, 1);
            Panel pnlDetails = GetPanel(Cust.wCustomerWin, "pnlDetails");
            return TableOnHoldInfo.Row(AppConstants.HEADR_TRAN_NUM, sTransactionNumber.Substring(14)).Enabled;
        }

        /// <summary>
        /// Selects a Suspended Or OnHold Transaction from the Table based on the passed Transaction # as Parameter 
        /// </summary>
        /// <param name="sTranNo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Void a Suspeded Transaction, from the Suspended Transaction Table
        /// </summary>
        /// <param name="sTransactionNumber"></param>
        /// <returns>True Or False</returns>
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

        /// <summary>
        /// Void a OnHold Transaction, from the OnHold Transaction Table
        /// </summary>
        /// <param name="sTransactionNumber"></param>
        /// <returns></returns>
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

        public bool MergeTransaction()
        {
            ClickOnButton(ButtonConstants.BTN_PANEL_NEXT);
            //PressSpecialKey(KeyboardInput.SpecialKeys.F6);
            ClickOnButton(wVStoreMainWindow,ButtonConstants.BTN_TRANSACTION_MERGE);
            Thread.Sleep(1000);
            LoggerUtility.StatusInfo("Merging The Transaction");
            return wMergeTransactionWin.Enabled;
        }

        public void SelectCorrespondingTransaction(string sTransactionNumber)
        {
            String sTranNo = sTransactionNumber.Substring(14);

            SelectFrmResumeTransactionGrid(sTranNo);
            ClickOnButton(wResumeTransactionWin, ButtonConstants.BTN_APPLY);
            Thread.Sleep(CommonData.iLoadingTime);
            wVStoreMainWindow.WaitWhileBusy();

        }
    }
}










