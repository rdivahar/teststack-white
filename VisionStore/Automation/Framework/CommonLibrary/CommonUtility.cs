using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Automation;
using NUnit.Framework;
using System;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
    {
    public class CommonUtility : WindowActions
    {

        WindowBase wBase = new WindowBase();
        WindowActions wAction = new WindowActions();

        public void OpenTerminal()
        {
            Label appState = wBase.GetLabel(wVStoreMainWindow, StateConstants.APPSTATE_LABEL_ID);
            VStoreApp.WaitWhileBusy();

            if (appState.NameMatches(StateConstants.STATE_130))
            {
                wAction.ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_OPENTERMINAL);
                Assert.True(appState.NameMatches(StateConstants.STATE_140));             
            }
            else
            {
                Console.WriteLine("Failure Message: Failed to Open the Terminal");
            }                
        }

        public void EnterCredential(string sID)
        {
            wVStoreMainWindow.Keyboard.Enter(sID);
            wVStoreMainWindow.WaitWhileBusy();
            Thread.Sleep(1000);
            PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(CommonData.iLoadingTime);
        }

        public bool VerifyAppStateAndLabel(string sAppStateText, string sIdentificationLabel)
        {
            Boolean bResults = false;
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            wVStoreMainWindow.WaitWhileBusy();
            Label appState = GetAppState(sAppStateText);
            Thread.Sleep(CommonData.iMinWait);

            if (majorPromptLabel.NameMatches(sIdentificationLabel) && appState.NameMatches(sAppStateText))
            {
                return true; 
            }else
            {
                Console.WriteLine("Failure: The Applicated failed to load the expected Appstate: " + sAppStateText);
                return bResults;
            }
        }

        public void OpenTerminalUsingKeyStrokes(string sEmpName, string sPasscode)
        {
            //InvokeVStore();
            PressSpecialKey(KeyboardInput.SpecialKeys.F1);

            wVStoreMainWindow.Keyboard.Enter(sEmpName);
            PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            
            wVStoreMainWindow.Keyboard.Enter(sPasscode);
            PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            wVStoreMainWindow.Close();
            wVStoreMainWindow.Dispose();
            Thread.Sleep(CommonData.iLoadingTime);
        }
        }  
      }
    
 





