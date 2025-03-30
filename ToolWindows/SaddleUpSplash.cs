using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace VitoExtensions.SaddleUp.ToolWindows
{
    [Guid("a1f7c438-5c45-4bde-8f11-02d1dcbf5617")]
    public class SaddleUpSplash : ToolWindowPane
    {
        public SaddleUpSplash() : base(null)
        {
            Caption = "Saddle Up!";
            BitmapResourceID = 301;
            BitmapIndex = 1;
            Content = new SaddleUpSplashControl();
        }
    }
}
