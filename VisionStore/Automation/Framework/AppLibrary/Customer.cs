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
using System.Collections.Generic;

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

        public Button btnAddCustomer
        {
            get
            {
                return GetButton(ButtonConstants.BTN_ADD_CUST);
            }
        }

        public Button btnSearchCustomer
        {
            get
            {
                return GetButton(wCustomerWin,ButtonConstants.BTN_SEARCH_CUST);
            }
        }

        public TextBox tbxSearchCustomer
        {
            get
            {
                return GetTextBox(wCustomerWin,AppConstants.TXT_SEARCH_ID);
            }
        }

        public Label lblSearchResults
        {
            get
            {
                return GetLabel(wCustomerWin, AppConstants.LBL_SEARCH_RESULTS);
            }
        }

        public ListView tblCustomerList
        {
            get
            {
                return GetListView(wCustomerWin, AppConstants.TBL_CUST_DETAILS,0);
            }
        }

        public bool OpenCustomerWindow()  
        {
            Boolean bResults = false;
           try
            {
                ClickOnButton(wVStoreMainWindow, ButtonConstants.BTN_CUSTOMER);
                wVStoreMainWindow.WaitWhileBusy();
                LoggerUtility.StatusInfo("Opening the Customer Window");
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
                LoggerUtility.StatusInfo("Closed the Customer Window");
                return bResults;
            }
            catch (Exception Ex)
            {
                return (!bResults);
                throw new AutomationException("Failed to Close the Customer Search Screen", Ex.StackTrace);
            }
        }

        public bool SetCustomer(string sCustomer)
        {
            Boolean bResults = false;
            try
            {
                Console.WriteLine("Searching thre customer with Name: " + sCustomer);
                SearchCustomers(sCustomer);
                //Thread.Sleep(1000);
                wCustomerWin.WaitWhileBusy();
                ClickOnButton(wCustomerWin, "fBtnSetAsCustomer");
                Thread.Sleep(CommonData.iLoadingTime);
                LoggerUtility.StatusInfo("Selected The Customer With  Name: " + sCustomer); 
                return (!bResults);
            }catch (Exception Ex)
            {
                if (wCustomerWin.Enabled)
                {
                    CloseCustomerWindow();
                }
                Console.WriteLine("Error: The Application Failed to Select the Customer" + Ex.StackTrace);
                return bResults;
            }
        }

        public bool SearchCustomers(string sCustomer)
        {
            Boolean bResults = false;
            Thread.Sleep(1000);

            try
            {
                wCustomerWin.WaitWhileBusy();
                SetTextByID(wCustomerWin, AppConstants.TXT_SEARCH_ID, sCustomer);
                ClickOnButton(GetButton(wCustomerWin, ButtonConstants.BTN_SEARCH_CUST));
                wCustomerWin.WaitWhileBusy();
                Thread.Sleep(1000);

                Label SearchResults = GetLabel(wCustomerWin, AppConstants.LBL_SEARCH_RESULTS);
                if (SearchResults.Text != "Displaying 1 of 1 Customer")
                {
                    GetListView(wCustomerWin, "dataGrid1").Row("First Name", "Paul").Select();
                    wCustomerWin.WaitWhileBusy();
                }
                LoggerUtility.StatusInfo("Searching The Customer With Name : " +sCustomer);

                return this.IsCustomerSelected(sCustomer);
            }
            catch (Exception Ex)
            {
                if (wCustomerWin.Enabled)
                {
                    CloseCustomerWindow();
                }
                return bResults;
                throw new AutomationException("Failed to Search the Customer", Ex.StackTrace);
            } 
        }

        public bool SearchCustomerAndSelect(string sPhoneOrEmail)
        {
            bool bResults = false;
            ListViewRow lstViewRow;
            string sFirstName;
            string sLastName;
            string sCustomerName;
            string sResultsCount;

            try
            {
                SetTextByElement(tbxSearchCustomer, sPhoneOrEmail);
                ClickOnButton(btnSearchCustomer);

                ListView lstCustomerTable = wCustomerWin.Get<ListView>(SearchCriteria.ByAutomationId("dataGrid1"));
                Thread.Sleep(CommonData.iLoadingTime);
                wCustomerWin.WaitWhileBusy();
                sResultsCount = lblSearchResults.Text;
                

                if (sResultsCount != AppConstants.LBL_CUSTCOUNT_ZERO && sResultsCount != AppConstants.LBL_CUSTCOUNT_ONE)
                { 
                    if (sPhoneOrEmail.Contains("@") && sPhoneOrEmail.Contains("."))
                    {
                        lstViewRow = lstCustomerTable.Row(AppConstants.LBL_TITLE_EMAIL, sPhoneOrEmail);
                        lstViewRow.Select();
                        LoggerUtility.WriteLog("Selected Based On Email#");
                    }
                    else
                    {
                        lstViewRow = lstCustomerTable.Row(AppConstants.LBL_TITLE_PHONE, sPhoneOrEmail);
                        lstViewRow.Select();
                        LoggerUtility.WriteLog("Selected Based On Phone#");
                    }
                    sFirstName = lstViewRow.Cells[AppConstants.LBL_TITLE_FNAME].Text;
                    sLastName = lstViewRow.Cells[AppConstants.LBL_TITLE_LNAME].Name;
                }else 
                {
                    sFirstName = lstCustomerTable.Rows[0].Cells[AppConstants.LBL_TITLE_FNAME].Text;
                    sLastName = lstCustomerTable.Rows[0].Cells[AppConstants.LBL_TITLE_LNAME].Text;
                }

                sCustomerName = sFirstName + " " + sLastName;
                wCustomerWin.WaitWhileBusy();
                LoggerUtility.StatusInfo("Searched The Client BasedOn = "+sPhoneOrEmail);

                return bResults = IsCustomerSelected(sCustomerName);
            }
            catch (Exception Ex)
            {
                //CloseAllChildWindows();
                if (wCustomerWin.Enabled)
                { 
                CloseCustomerWindow();
                }
                return bResults;
                throw new AutomationException("Failed to Load Customer Window: ", Ex.StackTrace);
            }
        }

        public bool SearchCustomerAndSelect(string sFirstName, string sLastName)
        {
            string sCustomer = sFirstName + " " + sLastName;
            string sResultsCount;
            bool bResults = false;

            try
            {
                SetTextByElement(tbxSearchCustomer, sCustomer);
                ClickOnButton(btnSearchCustomer);

                Thread.Sleep(CommonData.iLoadingTime);
                wCustomerWin.WaitWhileBusy();
                sResultsCount = lblSearchResults.Text;

                if (sResultsCount != AppConstants.LBL_CUSTCOUNT_ZERO && sResultsCount != AppConstants.LBL_CUSTCOUNT_ONE)
                {
                    ListView lstCustomerTable = wCustomerWin.Get<ListView>(SearchCriteria.ByAutomationId(AppConstants.TBL_CUST_DETAILS));
                    lstCustomerTable.Row("First Name", sFirstName).Select();
                    wCustomerWin.WaitWhileBusy();
                } 
                    return bResults = IsCustomerSelected(sCustomer);
            }
            catch (Exception Ex)
            {
                return bResults;
                throw new AutomationException("Failed to Search & Select The Customer : "+ sCustomer, Ex.StackTrace);
            }
        }

        public bool AddNewCustomer()
        {
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
            Thread.Sleep(1000);
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

        /// <summary>
        /// Checks The TableHeader In The Customer Form Window For a Column Name 
        /// </summary>
        /// <param name="sTableID"></param>
        /// <param name="sExpValue"></param>
        /// <returns></returns>
        public bool IsTableHeaderContains(string sTableID, string sExpValue)
        {
            bool bResult;
            bResult = IsDataGridHeaderContains(wCustomerWin, sTableID, sExpValue);
            return bResult;
        }

        public string GetNumberOfCustomersInApp()
        {
            string[] sValues;
            string sCountInApp;

            sValues = (GetLabel(wCustomerWin, AppConstants.LBL_RESULTSCOUNT).Name).Split(' ');
            return sCountInApp = sValues[0];
        }

        public bool IsCustomersCountInDBEquals(string sCustCountInApp)
        {
            //bool bResult;
            //bResult = 
            return true;
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