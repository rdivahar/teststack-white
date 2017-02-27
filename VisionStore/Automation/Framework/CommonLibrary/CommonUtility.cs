using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using TestStack.White;
using System.Diagnostics;
using System.Threading;
using TestStack.White.UIItems;
using System;
using System.Runtime.CompilerServices;
using RelevantCodes.ExtentReports;
using System.Xml.Linq;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public class CommonUtility : WindowActions
    {
        WindowBase wBase = new WindowBase();
        WindowActions wAction = new WindowActions();
        public ExtentReports extent;
        public ExtentTest test;

        /// <summary>
        /// Open the terminal and validate the Application States
        /// </summary>
        /// <returns>True or False</returns>
        public bool OpenTerminal()
        {
            Boolean bResults = false;
            wVStoreMainWindow.WaitWhileBusy();

            try
            {
                VerifyAppState(StateConstants.STATE_130);
                ClickOnButton(ButtonConstants.BTN_OPENTERMINAL);
                VerifyAppState(StateConstants.STATE_140);
                Console.WriteLine("<Info: Opening the Terminal>");
                return (!bResults);
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Error: The Application failed to Open a Terminal", Ex.StackTrace);
            }               
        }

        /// <summary>
        /// Entering the ke
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public bool EnterCredential(string sID)
        {
            Boolean bResults = false;

            try
            {
                EnterFromKeyborad(wVStoreMainWindow, sID);
                PressEnter(wVStoreMainWindow);
                Console.WriteLine("Info: Entering the Value " + sID);
                return (!bResults);
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Error: The Application failed to Enter the Input "+ sID, Ex.StackTrace);
            }

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public string GetCurrentTestName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Name;
        }

        /// <summary>
        /// Method to get the AppState Value as an Integer in between the Braces []
        /// </summary>
        /// <returns></returns>
        public int tet()
        {
            Label AppState = GetLabel(wVStoreMainWindow, AppConstants.APPSTATE_LABEL_ID);
            string sCurrentAppState = AppState.Text.Split('[', ']')[1];
            int iAppStateValue = Int32.Parse(sCurrentAppState);
            LoggerUtility.StatusInfo("The State Of The Application Is"+ iAppStateValue+"");
            return iAppStateValue;
        }

        public void ChangeXMLNodeValue(string sFilePath, string sRootElement,string sTargetElement, string sNodeValue )
        {
            
            XDocument xDoc = XDocument.Load(sFilePath);
            var element = xDoc.Root.Element(sRootElement).Element(sTargetElement);
            element.Value = sNodeValue;
            xDoc.Save(sFilePath);
        }

        public void ConfigXMLWithTestSuiteName(string sNodeValue)
        {
            string ConfigXMLPath = "C:\\JestaDesktopAutomation\\VisionStore\\Automation\\extent-config.xml";
            string sAppendNodeValue = "--" + sNodeValue.ToUpper();
            this.ChangeXMLNodeValue(ConfigXMLPath,"configuration", "reportHeadline", sAppendNodeValue);
        }

        public bool VerifyAppStateAndLabel(string sAppStateText, string sIdentificationLabel)
        {
            Boolean bResults = false;

            Thread.Sleep(CommonData.iWinLoadingWait);
            Label majorPromptLabel = GetLabel(AppConstants.MAJOR_PROMPT);
            wVStoreMainWindow.WaitWhileBusy();
            Label appState = GetAppState(sAppStateText);
            Thread.Sleep(CommonData.iMinWait);

            if (majorPromptLabel.NameMatches(sIdentificationLabel) && appState.NameMatches(sAppStateText))
            {
                Console.WriteLine("Info: The App has loaded the State " + sAppStateText + " and Label "+ sIdentificationLabel);
                return (!bResults); 
            }else
            {
                return bResults;
            }
        }
        }  
      }
    
 





