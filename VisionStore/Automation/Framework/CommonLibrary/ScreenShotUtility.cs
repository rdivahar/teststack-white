using Jesta.VStore.Automation.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;

namespace Jesta.VStore.Automation.Framework.CommonLibrary
{
    public class ScreenShotUtility
    {

        private string TakeScreenshot1(string screenshotName)
        {
            
           
            var imagename = screenshotName + ".png";
            var imagePath = Path.Combine(CommonData.screenshotDir, imagename);
            try
            {
                Desktop.CaptureScreenshot().Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                Trace.WriteLine(String.Format("Screenshot taken: {0}", imagePath));
            }
            catch (Exception)
            {
                Trace.TraceError(String.Format("Failed to save screenshot to directory: {0}, filename: {1}", CommonData.screenshotDir, imagePath));
            }
            return imagePath;
        }

        public static Bitmap CaptureScreenshot()
        {
            var screenCapture = new ScreenCapture();
            return screenCapture.CaptureScreenShot();
        }

        public static void TakeScreenshot(string screenshotName)
        {
           

            var imagename = screenshotName + ".png";
            var filename = Path.Combine(CommonData.screenshotDir, imagename);
            var bitmap = CaptureScreenshot();
            bitmap.Save(filename);
        }

        public static string GetScreenshot(string ssName)
        {
            
            var imagename = ssName + ".png";
            var filepath = Path.Combine(CommonData.screenshotDir, imagename);

            TakeScreenshot(ssName);
            return filepath;
        }

    }
}
