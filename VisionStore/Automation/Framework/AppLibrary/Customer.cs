using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using Jesta.VStore.Automation.Framework.ObjectRepository;
using System;
using TestStack.White;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WPFUIItems;
using NUnit.Framework;
using static TestStack.White.UIItems.WindowItems.Window;
using TestStack.White.UIItems.TabItems;

namespace Jesta.VStore.Automation.Framework.AppLibrary
    {
    public class Customer : WindowActions
    {

        public Window wCustomerWin
        {
            get
            {
                return GetChildWindowByID(wVStoreMainWindow, AppConstants.WIN_CUSTOMER_FORM);
            }
        }

        public bool OpenCustomerWindow()
        {
            Boolean bResults = false;
           try
            {
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_CUSTOMER);
                wVStoreMainWindow.WaitWhileBusy();
                return wCustomerWin.Enabled;
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Failed to Open Customer Search Screen", Ex.StackTrace);
            }
        }

        public bool CloseCustomerWindow()
        {
            Boolean bResults = true;
            try
            {
                wCustomerWin.WaitWhileBusy();
                ClickOnButton(wCustomerWin, ButtonConstants.BTN_CLOSE_CUSTOMERWIN);          
                return true;
            }
            catch
            {
                LoggerUtility.WriteLog("Failure Message: Failed to Close the Customer Search Screen");
                return bResults;
                throw;
            }
        }

        public bool SetCustomer(string sCustomer)
        {
            Boolean bResults = false;
            try
            {
                Assert.True(SearchCustomers(sCustomer));
                Thread.Sleep(7000);
                wCustomerWin.WaitWhileBusy();
                ClickOnButton(wCustomerWin, "fBtnSetAsCustomer");
                Thread.Sleep(CommonData.iLoadingTime);
                return true;
            }catch
            {
                Console.WriteLine("Error: The Application Failed to Select the Customer");
                return bResults;
            }
        }

        public bool SearchCustomers(string sCustomer)
        {
            Boolean bResults = false;

            try
            {
                if (wCustomerWin != null)
                {
                    SetTextByClassName(wCustomerWin, AppConstants.TXT_SEARCH_CLASSNAME, sCustomer);
                    ClickOnButton(GetButton(wCustomerWin, ButtonConstants.BTN_SEARCH_CUST));
                    wCustomerWin.WaitWhileBusy();
                    Thread.Sleep(10000);

                    Label SearchResults = GetLabel(wCustomerWin, AppConstants.LBL_SEARCH_RESULTS);
                    if (SearchResults.Text != "Displaying 1 of 1 Customer")
                    {
                        GetListView(wCustomerWin, "dataGrid1").Row("First Name", "Paul").Select();
                        wCustomerWin.WaitWhileBusy();
                    }

                    bResults = this.IsCustomerSelected(sCustomer);
                }
                return bResults;
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Failed to Load Customer Window: ", Ex.StackTrace);
            } 
        }

        public bool SelectCustomerTest(string sCustomer)
        {
            Boolean bResults = false;
            try
            {
                this.OpenCustomerWindow();

                TextBox txtSearchBox = GetTextBox(wCustomerWin, AppConstants.TXT_SEARCH_CLASSNAME);
                txtSearchBox.Enter(CommonData.sCutomerName);

                ClickOnButton(GetButton(wCustomerWin, ButtonConstants.BTN_SEARCH_CUST));
                Thread.Sleep(CommonData.iLoadingTime);
                wCustomerWin.WaitWhileBusy();

                Label SearchResults = GetLabel(wCustomerWin, AppConstants.LBL_SEARCH_RESULTS); 

                if (SearchResults.Text != "Displaying 1 of 1 Customer")
                {
                    ListView lstCustomerTable = wCustomerWin.Get<ListView>(SearchCriteria.ByAutomationId("dataGrid1"));
                    lstCustomerTable.Row("First Name", "Summer").Select();
                    wCustomerWin.WaitWhileBusy();
                }  
                return bResults = IsCustomerSelected(sCustomer);
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Failed to Load Customer Window: ",Ex.StackTrace);
            }
        }

        public bool AddCustomer()
        {
            //TO-DO : Incomplete Methods
        /// <summary>
        /// The Class is used to add a new customer in the Customer Window
        /// </summary>

            Boolean bResults = false;
            try
            {
                this.OpenCustomerWindow();

                ClickOnButton(wCustomerWin, ButtonConstants.BTN_ADD_CUST);
                LoggerUtility.WriteLog("Clicked the Add");

                Window wCustomerAddWin = GetChildWindowByID(wCustomerWin, AppConstants.WIN_CUSTOMER_ADD);
                wCustomerAddWin.WaitWhileBusy();

                PropertyGrid FirstNamePlaceHolder = wCustomerAddWin.Get<PropertyGrid>(SearchCriteria.ByAutomationId("C_CUSTOMERFIELD_1"));
                LoggerUtility.WriteLog("Got the wCustomerAddWin Control, going to Perform Entry");
                TextBox txtAddCust = FirstNamePlaceHolder.Get<TextBox>(SearchCriteria.ByClassName("WindowsForms10.RichEdit20W.app.0.1a8c1fa_r14_ad1"));
                txtAddCust.Focus();
                txtAddCust.Text = "Sahaya";

                LoggerUtility.WriteLog("Jerish Name Entered");
                Thread.Sleep(10000);
                return true;
            }
            catch
            {
                LoggerUtility.WriteLog("Failure Message: Failed to Select the Customer");
                return bResults;
                throw;
            }
        }

        public bool IsCustomerSelected(string sExpectedCust)
        {
            Thread.Sleep(3000);
            Label sActualCust = GetLabel(wCustomerWin, AppConstants.LABEL_CUSTOMER_ID);
            return (sActualCust.Name.Contains(sExpectedCust));
        }

        public bool VerifyCustomerIsSet(string sExpectedCustName)
        {
            Label btmCustNamelabel = GetLabel(AppConstants.LBL_BTM_CUSTNAME);
            Boolean bBottomCustNameAvail = btmCustNamelabel.Text.Contains(sExpectedCustName);

            ClickOnButton(ButtonConstants.BTN_CUST_TAG);
            Boolean bCustTagInfoAvail = true;

            if (bBottomCustNameAvail == true && bCustTagInfoAvail == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifyNoCustomerIsSelected()
        {
            Label btmCustNamelabel = GetLabel(AppConstants.LBL_BTM_CUSTNAME);
            if (btmCustNamelabel.Text == "")
            {
                return true;
            }else
            {
                return false;
            }
        }

        public bool SelectFromCustomerInfoTab(int iCustTabPageIndex)
        {
            try
            {
                SelectTabItem(wCustomerWin, AppConstants.TAB_CTRL_CUST_INFO, iCustTabPageIndex);
                return true;
            }
            catch (Exception Ex)
            {
                return false;
                throw new AutomationException("Failed to Select From Customer Info", Ex.StackTrace);
            }         
        }
    }  
  }


    
 





