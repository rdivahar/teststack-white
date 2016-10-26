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

namespace Jesta.VStore.Automation.Framework.AppLibrary
{
    public class Employee : CommonUtility
    {
        
        WindowBase wBase = new WindowBase();
        WindowActions wAction = new WindowActions();

        public void EnterSalesAdvisor(string sSalesAdvisorID)
        {

            // Checks the AppState[1415]
            wVStoreMainWindow.WaitWhileBusy();
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            wVStoreMainWindow.WaitWhileBusy();
            Label appState_1415 = GetAppState(StateConstants.STATE_1415);
               
            if (appState_1415.NameMatches(StateConstants.STATE_1415) && majorPromptLabel.NameMatches(AppConstants.SALES_ADVISOR_LABEL))
            {
                try
                {
                    //Verify the State Transition to [1000]
                    this.EnterCredential(sSalesAdvisorID);                 
                    Label appState_1000 = GetAppState(StateConstants.STATE_1000);
                    Assert.True(appState_1000.Enabled);
                }
                catch (AssertionException ex)
                {
                    Console.WriteLine("Failure Message: " + ex.StackTrace);
                    throw;
                }
            }
        }

        public bool EnterEmployeeNumber(string sEmpNmbr)
        {
            Boolean bResult = false;

            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            if (majorPromptLabel.NameMatches(AppConstants.ENTER_EMPNMBR_PROMPT))
            {
                EnterCredential(sEmpNmbr);
                return true;
            }
            else
            {
                LoggerUtility.WriteLog("The Employee Number Prompt DOESNOT Exist");
                return bResult;
            }
        }

        public void AuthenticateEmployee(string sEmpNmbr, string sPasscode)
        {
            EnterEmployeeNumber(sEmpNmbr);
            EnterEmployeePasscode(sPasscode);
            Thread.Sleep(CommonData.iLoadingTime);
            wVStoreMainWindow.WaitWhileBusy();
        }

        public void EnterEmployeePasscode(string sPasscode)
        {
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            if (majorPromptLabel.NameMatches(AppConstants.ENTER_EMPPWD_PROMPT))
            {
                Thread.Sleep(1500);
                wVStoreMainWindow.Keyboard.Enter(sPasscode);
                PressEnter(wVStoreMainWindow);
                Thread.Sleep(2000);
            }
        }

        public void AuthenticateUser(string sEmpNmbr, string sPasscode)
        {
            EnterEmployeeNumber(sEmpNmbr);
            EnterEmployeePasscode(sPasscode);
            Thread.Sleep(3000);
            wVStoreMainWindow.WaitWhileBusy();
        }
    }
}

        
 





