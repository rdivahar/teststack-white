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

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public class WindowActions : WindowBase 
    {      

        public void ClickOnButton(Button btnToClick)
        {
            try
            {
                btnToClick.Focus();
                btnToClick.Click();
                VStoreApp.WaitWhileBusy();
                Thread.Sleep(CommonData.iLoadingTime);
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

        public bool VerifyAppState(string sExpectedAppState)
        {
           string sDefaultAutomationID = StateConstants.APPSTATE_LABEL_ID;
           Thread.Sleep(CommonData.iWinLoadingWait);
           wVStoreMainWindow.WaitWhileBusy();
           Label AppState = GetLabel(wVStoreMainWindow, sDefaultAutomationID);
           return (AppState.NameMatches(sExpectedAppState));    
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

        public void SetTextByElementName(Window wVStoreWin, string sElementName, string sTxtvalue)
        {
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByText(sElementName));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
        }

        public void SetTextByClassName(Window wVStoreWin, string sClassName, string sTxtvalue)
        {
            TextBox txtField = wVStoreWin.Get<TextBox>(SearchCriteria.ByClassName(sClassName));
            txtField.Focus();
            txtField.Enter(sTxtvalue);
            wVStoreWin.WaitWhileBusy();
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

        public void SelectTabItem(Window wWin, String sTabAutomationID, int iTabIndex)
        {
            Tab tbCtrl = GetTab(wWin, sTabAutomationID);
            tbCtrl.SelectTabPage(iTabIndex);
            wWin.WaitWhileBusy();
        }


        //public bool GetItemFromGrid(String HeaderrValue, String ExpectedItem)
        //{

        //}
    }
}
