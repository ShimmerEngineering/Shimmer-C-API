using ShimmerBLEAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerisenseBLEDemoApp.UWP;

[assembly: Xamarin.Forms.Dependency(typeof(UIHelperUWP))]
namespace VerisenseBLEDemoApp.UWP
{
    public class UIHelperUWP : INativeUIService
    {
        public async void Invoke(Action action)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { action(); });
        }
    }
}
