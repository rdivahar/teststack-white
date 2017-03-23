using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using Jesta.VStore.Automation.Framework.Configuration;
using TestStack.White.UIItems.TabItems;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using static Jesta.VStore.Automation.Framework.ObjectRepository.StateConstants;
using TestStack.White.UIItems.ListViewItems;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public class WindowActions : WindowBase 
    {

        /// <summary>
        /// Method to Trigger the VStore Application and Set the App to its Base State 
        /// </summary>
        public void InitializeAndSetToBaseState()
        {
            Init();

            Label AppState = GetLabel(wVStoreMainWindow, "lblState");
            string CurrentAppState = AppState.Text;

            string OutputAppState = CurrentAppState.Split('[', ']')[1];
            int iAppState = Int32.Parse(OutputAppState);

            if (Enum.IsDefined(typeof(AppStatus200), iAppState))
            {
                VStoreApp.WaitWhileBusy();
                Thread.Sleep(CommonData.iLoadingTime);
                PressSpecialKey(KeyboardInput.SpecialKeys.F1);
                wVStoreMainWindow.WaitTill(new Window.WaitTillDelegate(IsState200));
                string NewAppState = AppState.Text;
                string NewOutputAppState = NewAppState.Split('[', ']')[1];
                int iNewAppState = Int32.Parse(NewOutputAppState);
            }
        }

        public void CloseVStoreAndChildWindows()
        {
            //CloseAllChildWindows();
            base.AppExit();
        }

        public void CloseAllChildWindows()
        {
            LoggerUtility.WriteLog("<Info: Closing The Child Windows Method>");
            List<Window> wChildWindows = wVStoreMainWindow.ModalWindows();
            LoggerUtility.WriteLog("dsda"+wChildWindows);

            if (wChildWindows != null)
                LoggerUtility.WriteLog("The Value is Not Equal To Null"+ wChildWindows);
            {
                foreach (Window ChildWindow in wChildWindows)
                {
                    LoggerUtility.WriteLog("The Current Child Window Is " + ChildWindow.Title);
                    ChildWindow.Close();
                }
            } 
        }

        public void ClickOnButton(Button btnToClick)
        {
            try
            {
                btnToClick.Focus();
                btnToClick.Click();
                VStoreApp.WaitWhileBusy();
                Thread.Sleep(CommonData.iWinLoadingWait);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            } 
        }

        public void ClickOnButton(string sAutomationID)
        {
            Button btnToClick = GetButton(wVStoreMainWindow, sAutomationID);
            this.ClickOnButton(btnToClick);
        }

        public void ClickOnButton(Window wWin, string sAutomationID)
        {
            try
            {
              Button btnToClick = GetButton(wWin, sAutomationID);
              this.ClickOnButton(btnToClick);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Checks the Major Prompt Messages based on the input string
        /// </summary>
        /// <param name="sPrmpt">Major Prompt Messages</param>
        /// <returns>Boolean</returns>
        public bool VerifyMajorPrmptMsgs(string sPrmpt)
        {
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            bool bNoTransMessage = (majorPromptLabel.NameMatches(sPrmpt));
            wVStoreMainWindow.WaitWhileBusy();
            return bNoTransMessage;
        }


        /// <summary>
        /// Verify the Application state based on the Expected App State
        /// </summary>
        /// <param name="sExpectedAppState"></param>
        /// <returns>True Or False</returns>
        public bool VerifyAppState(string sExpectedAppState)
        {
           string sDefaultAutomationID = StateConstants.APPSTATE_LABEL_ID;
           Thread.Sleep(CommonData.iMinWait);
           wVStoreMainWindow.WaitWhileBusy();
           Label AppState = GetLabel(wVStoreMainWindow, sDefaultAutomationID);
           return (AppState.NameMatches(sExpectedAppState));    
        }

        public bool VerifyAppStates(string sExpStateOne, string sExpStateTwo)
        {
            Boolean bResults = true;
            if (VerifyAppState(sExpStateOne) || VerifyAppState(sExpStateTwo))
            {
                return bResults;
            } else
            {
                return !bResults;
            }
        }

        public bool IsState200()
        {
            return VerifyAppState(AppConstants.STATE_200);
        }

        public void PressEnter(Window wVStoreWin)
        {
      
            Thread.Sleep(CommonData.iMinWait);
            wVStoreWin.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(CommonData.iMinWait);
            wVStoreWin.WaitWhileBusy();
        }

        public void PressSpecialKey(KeyboardInput.SpecialKeys kSpecialKey)
        {
            Thread.Sleep(CommonData.iMinWait);
            wVStoreMainWindow.Keyboard.PressSpecialKey(kSpecialKey);
            Thread.Sleep(CommonData.iMinWait);
            wVStoreMainWindow.WaitWhileBusy();
        }

        public void EnterFromKeyborad(Window wWin, String sInput)
        {
            wWin.WaitWhileBusy();
            wWin.Keyboard.Enter(sInput);
            Thread.Sleep(CommonData.iWinLoadingWait);
            wWin.WaitWhileBusy();
        }

        public void SetText(Window wVStoreWin, string sAutomationID, string sTxtvalue)
        {
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByText(sAutomationID));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
        }

        public string GetText(UIItem UIObject, Window wWin = null)
        {
            String sContent = UIObject.Name.ToString();
            return sContent;
        }

        public void SetTextByElement(TextBox tbxFieldName, string sTxtvalue)
        {
            tbxFieldName.Focus();
            tbxFieldName.Enter(sTxtvalue);
            VStoreApp.WaitWhileBusy();
        }

        public void SetTextByElementName(Window wVStoreWin, string sElementName, string sTxtvalue)
        {
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByText(sElementName));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
        }

        public void SetTextByClassName(Window wVStoreWin, string sClassName, string sTxtvalue)
        {
            Console.WriteLine("Going to Get teh Text Field");
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByClassName(sClassName));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
            Thread.Sleep(2000);
        }

        public void SetTextByID(Window wVStoreWin, string sAutoID, string sTxtvalue)
        {
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByAutomationId(sAutoID));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
            Thread.Sleep(1500);
        }

        public void WaitForObjectExists(UIItem OBJToWait, int iWaitTime)
        {
           
            for (int i = 0; i <= iWaitTime; i++)
            {
                if (OBJToWait.Visible == true)
                {
                    Console.WriteLine("The Object " + OBJToWait + "is Found after waiting for " + iWaitTime + "mSeconds");
                    break;
                }
                else continue;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wWin"></param>
        /// <param name="sTableID"></param>
        /// <param name="sExpValue"></param>
        /// <returns></returns>
        public bool IsDataGridHeaderContains(Window wWin, string sTableID, string sExpValue)
        {
            ListViewHeader LVHeader = GetListView(wWin, sTableID).Header;
            bool bResult = LVHeader.Column(sExpValue).Enabled;
            return bResult;
        }

        public void SelectTabItem(Window wWin, String sTabAutomationID, int iTabIndex)
        {
            Tab tbCtrl = GetTab(wWin, sTabAutomationID);
            tbCtrl.SelectTabPage(iTabIndex);
            wWin.WaitWhileBusy();
        }
    }
}
