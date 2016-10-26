using Jesta.VStore.Automation.Framework.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public class WindowBase
    {
        public static Application VStoreApp;
        public static Window wVStoreMainWindow;

        public WindowBase()
        {
            VStoreApp = Application.AttachOrLaunch(CreateProcess());
            wVStoreMainWindow = VStoreApp.GetWindow(CommonData.PROG_NAME, InitializeOption.NoCache);
        }

        public ProcessStartInfo CreateProcess()
        {
            return new ProcessStartInfo(CommonData.PROG_PATH);
        }

        public void Init()
        {
            var processInfo = new ProcessStartInfo(CommonData.PROG_PATH);
            VStoreApp = Application.AttachOrLaunch(processInfo);
            VStoreApp.WaitWhileBusy();
            Thread.Sleep(CommonData.iLoadingTime);

            wVStoreMainWindow = VStoreApp.GetWindow(CommonData.PROG_NAME, InitializeOption.NoCache);
            VStoreApp.WaitWhileBusy();

            if (wVStoreMainWindow.Title.ToString() == CommonData.PROG_NAME)
            {
                LoggerUtility.WriteLog("The Application " + CommonData.PROG_NAME + " is Launched");
                VStoreApp.WaitWhileBusy();
            }
            else
            {
                Assert.Fail("Application Error:" + CommonData.PROG_NAME + "failed to Launch");
            }
        }

        public void Exit()
        {
            if (wVStoreMainWindow != null && wVStoreMainWindow.Visible.Equals(true))
            {
                wVStoreMainWindow.Focus();
                wVStoreMainWindow.Close();
                wVStoreMainWindow.WaitWhileBusy();
            }
            else
            {
                LoggerUtility.WriteLog("Application Error" + CommonData.PROG_NAME + " Not Found / Cannot Be Closed");
            }
        }

        public void WaitForWinToLoad(Window wWin)
        {
            wWin.WaitWhileBusy();
        }

        public Label GetLabel(Window wVStoreWin, string sAutomationID)
        {
            wVStoreWin.WaitWhileBusy();
            Label Label = wVStoreWin.Get<Label>(SearchCriteria.ByAutomationId(sAutomationID));
            return Label;
        }

        public Label GetLabel(string sAutomationID)
        {
            wVStoreMainWindow.WaitWhileBusy();
            Label Label = wVStoreMainWindow.Get<Label>(SearchCriteria.ByAutomationId(sAutomationID));
            return Label;
        }

        public Button GetButton(string sAutomationID)
        {
            wVStoreMainWindow.WaitWhileBusy();
            Button Button = wVStoreMainWindow.Get<Button>(SearchCriteria.ByAutomationId(sAutomationID));
            return Button;
        }

        public Button GetButton(Window wVStoreWin, string sAutomationID)
        {
            wVStoreWin.WaitWhileBusy();
            Button Button = wVStoreWin.Get<Button>(SearchCriteria.ByAutomationId(sAutomationID));
            return Button;
        }

        public Button GetButtonUsingText(Window wVStoreWin, string sText)
        {
            wVStoreWin.WaitWhileBusy();
            Button Button = wVStoreWin.Get<Button>(SearchCriteria.ByText(sText));
            return Button;
        }

        public TextBox GetTextBox(Window wVStoreWin, string sClassName)
        {
            wVStoreWin.WaitWhileBusy();
            return wVStoreWin.Get<TextBox>(SearchCriteria.ByClassName(sClassName));
        }

        public Window GetChildWindowByID(Window wParentWindow, string sAutomationID)
        {
            Window childWindow = wParentWindow.ModalWindow(SearchCriteria.ByAutomationId(sAutomationID));
            return childWindow;
        }

        public Label GetAppState(string sText, string sAutomationID = null)
        {
            if (sAutomationID == null)
            {
                string sDefaultID = "lblState";
                wVStoreMainWindow.WaitWhileBusy();
                Thread.Sleep(2000);
                //return wVStoreMainWindow.Get<Label>(SearchCriteria.ByText(sText).AndAutomationId(sDefaultID));
                return wVStoreMainWindow.Get<Label>(SearchCriteria.ByText(sText));
            }
            else
            {
                wVStoreMainWindow.WaitWhileBusy();
                return wVStoreMainWindow.Get<Label>(SearchCriteria.ByText(sText));
                //return wVStoreMainWindow.Get<Label>(SearchCriteria.ByText(sText).AndAutomationId(sAutomationID));
            }
        }
        
        public ListView GetListView(Window wWin, string sAutomationID)
        {
            ListView listView = wWin.Get<ListView>(SearchCriteria.ByAutomationId(sAutomationID));
            return listView;
        }

        public Tab GetTab(Window wWin, string sAutomationID)
        {
            Tab tabControl = wWin.Get<Tab>(SearchCriteria.ByAutomationId(sAutomationID));
            return tabControl;
        }


        //protected new ToolStrip ToolStrip(string title)
        //{
        //    ToolStrip toolStrip = null;
        //    toolStrip = _window.Get<ToolStrip>(SearchCriteria.ByClassName(title));
        //    return toolStrip;
        //}

        //protected TextBox TextBox(int index = 0)
        //{
        //    return _window.Get<TextBox>(SearchCriteria.Indexed(index));
        //}

        //protected TextBox TextBox(VStoreWindow window, int index = 0)
        //{
        //    return window.GetParentWindow.Get<TextBox>(SearchCriteria.Indexed(index));
        //}

        //protected TextBox TextBox(string title)
        //{
        //    return _window.Get<TextBox>(SearchCriteria.ByText(title));
        //}

        //protected TextBox TextBox(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<TextBox>(SearchCriteria.ByText(title));
        //}


        //protected ListView ListView()
        //{
        //    return _window.Get<ListView>();
        //}

        //protected ListBox ListBox()
        //{
        //    return _window.Get<ListBox>();
        //}

        //protected ListBox ListBoxByClassName(string title)
        //{
        //    return _window.Get<ListBox>(SearchCriteria.ByClassName(title));
        //}

        //protected ListBox ListBoxByClassName(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<ListBox>(SearchCriteria.ByClassName(title));
        //}

        //protected Tree TreeByClassName(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<Tree>(SearchCriteria.ByClassName(title));
        //}
        //protected ListBox ListBoxByAutomationId(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<ListBox>(SearchCriteria.ByAutomationId(title));
        //}
        //protected ListItem selectListItem(ListBox itemsList, int index = 0)
        //{
        //    itemsList.Item(index).Select();
        //    ListItem item = itemsList.SelectedItem;
        //    item.Click();
        //    return item;
        //}

        //protected ListItem selectListItem(ListBox itemsList, string itemText)
        //{
        //    itemsList.Item(itemText).Select();
        //    ListItem item = itemsList.SelectedItem;
        //    item.Click();
        //    return item;
        //}

        //protected TreeNode selectTreeItem(Tree tree, string treeItem)
        //{
        //    TreeNodes treeNodes = tree.Nodes;
        //    TreeNode node = treeNodes.GetItem(treeItem);
        //    node.DoubleClick();
        //    return node;
        //}

        //protected ListItem pointToListItem(ListBox itemsList, string keyword)
        //{
        //    itemsList.Item(keyword).Select();
        //    ListItem item = itemsList.SelectedItem;
        //    return item;
        //}

        //protected string getListItemName(ListBox itemsList, int index)
        //{
        //    string itemName = itemsList.Item(index).Name;
        //    return itemName;
        //}

        //protected Label Label(string automationId)
        //{
        //    return _window.Get<Label>(SearchCriteria.ByAutomationId(automationId));
        //}

        //protected RadioButton RadioButton(int index = 0)
        //{
        //    return _window.Get<RadioButton>(SearchCriteria.Indexed(index));
        //}

        //protected RadioButton RadioButton(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<RadioButton>(SearchCriteria.ByText(title));
        //}

        //protected RadioButton RadioButton(Window window, int index = 0)
        //{
        //    return window.Get<RadioButton>(SearchCriteria.Indexed(index));
        //}

        //protected ComboBox ComboBox()
        //{
        //    return _window.Get<ComboBox>();
        //}


        //protected ComboBox ComboBox(string title)
        //{
        //    return _window.Get<ComboBox>(SearchCriteria.ByText(title));
        //}

        //protected CheckBox CheckBox(int index = 0)
        //{
        //    return _window.Get<CheckBox>(SearchCriteria.Indexed(index));
        //}

        //protected CheckBox CheckBox(VStoreWindow window, int index = 0)
        //{
        //    return window.GetParentWindow.Get<CheckBox>(SearchCriteria.Indexed(index));
        //}

        //protected CheckBox CheckBox(VStoreWindow window, string title)
        //{
        //    return window.GetParentWindow.Get<CheckBox>(SearchCriteria.ByText(title));
        //}

        //protected Panel Panel(Window window, string title)
        //{

        //    Panel panel = window.Get<Panel>(SearchCriteria.ByText(title));
        //    return panel;
        //}

        //protected Button Button(Panel parent, string title, int index)
        //{
        //    Button button = parent.Get<Button>(SearchCriteria.ByAutomationId(title).AndIndex(index));
        //    return button;
        //}

        //protected TextBox TextBox(Panel parent, string title, int index = 0)
        //{
        //    TextBox textbox = parent.Get<TextBox>(SearchCriteria.ByAutomationId(title).AndIndex(index));
        //    return textbox;
        //}


        //protected ListBox ListBoxByClassName(Panel window, string title, int index = 0)
        //{
        //    return window.Get<ListBox>(SearchCriteria.ByClassName(title).AndIndex(1));
        //}


        //protected Window ModelWindow(string title)
        //{
        //    return _window.ModalWindows().Find(obj => obj.Title.Contains(title));
        //}

        //public override void Focus()
        //{
        //    _window.Focus();
        //}


        //public override void Click()
        //{
        //    _window.Click();
        //}

        ///*public static Window GetWindow(string title)
        //{
        //    return Retry.For(
        //        () => _application.GetWindows().Find(x => x.Title.Contains(title)),
        //        TimeSpan.FromMilliseconds(LacerteConfiguration.Instance.FindWindowTimeout));
        //}

        //public static Window findWindowByTitle(string windowTitle)
        //{
        //    return Retry.For(
        //        () => Desktop.Instance.Windows().Find(obj => obj.Title.Contains(windowTitle)),
        //        TimeSpan.FromMilliseconds(LacerteConfiguration.Instance.FindWindowTimeout));
        //}

        //private static Window GetCorrectParentWindow()
        //{
        //    Window result = GetWindow(_mainWindowTitle);
        //    if (result == null)
        //    {
        //        result = findWindowByTitle(_mainWindowTitle);
        //    }
        //    return result;
        //}

        //public static Window ParentWindow
        //{
        //    get { return GetCorrectParentWindow(); }
        //}

        //public static Window findModelWindow(Window parent, string title)
        //{
        //    return Retry.For(
        //         () => parent.ModalWindows().Find(obj => obj.Title.Contains(title)),
        //         TimeSpan.FromMilliseconds(LacerteConfiguration.Instance.FindWindowTimeout));

    }
}
