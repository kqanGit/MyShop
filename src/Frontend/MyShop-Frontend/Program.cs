using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MyShop_Frontend
{
    public class Program
    {
        [DllImport("Microsoft.ui.xaml.dll")]
        private static extern void XamlCheckProcessRequirements();

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                XamlCheckProcessRequirements();
                WinRT.ComWrappersSupport.InitializeComWrappers();

                Application.Start((p) =>
                {
                    var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);
                    new App();
                });
            }
            catch (Exception)
            {
                // In a real app, you might want to log this to a file or event viewer
            }
        }
    }
}
