using NUnit.Framework;
using Jesta.VStore.Automation.Framework.CommonLibrary;
using Jesta.VStore.Automation.Framework.Configuration;
using System.Xml.Linq;

namespace Jesta.Automation.VisionStore.Tests
{

    [SetUpFixture]
    public class TestInitializeHook : CommonUtility
    {
        
        [OneTimeSetUp] [PreTest]
        public void RunBeforeAnySuite()
        {
            string sTestSuiteName = CommonData.sDefaultSuite;
            string sGetParameterName = TestContext.Parameters["Suite"];

            if (sGetParameterName != null)
              {
                sTestSuiteName = sGetParameterName;
              }
            LoggerUtility.WriteLog("<Info> : The Name Of The TestSuite Passed - " + sTestSuiteName);
            base.ConfigXMLWithTestSuiteName(sTestSuiteName);
            LoggerUtility.SetupReportConfig(sTestSuiteName);
        }

        [OneTimeTearDown][PostTest]
        public void RunAfterAnySuite()
        {
            LoggerUtility.WriteLog("sdivahar");
            LoggerUtility.FlushResultsAndClose();
        }
    }
}
