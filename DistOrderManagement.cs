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
    public class DistOrderManagement : WindowActions
    {

        public bool SelectDistOrderManagement()
        {
            Boolean bResults = false;
            Button btnDistOrderMgmt = GetButton(ButtonConstants.BTN_DIST_ORDER_MANAGEMENT);
            
            if (btnDistOrderMgmt.Enabled)
            {
                btnDistOrderMgmt.Focus();
                btnDistOrderMgmt.Click();
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.TITLE_DIST_ORDERMANAGEMENT).Enabled;
            }
            else
            {
                return bResults;
            }
        }
    }
}
      
    
 





