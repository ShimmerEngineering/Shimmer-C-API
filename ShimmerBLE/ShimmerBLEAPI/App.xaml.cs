using Newtonsoft.Json;
using shimmer.Helpers;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace shimmer
{
    public partial class App : Application
    {
        public static double DisplayScreenWidth = 0f;
        public static double DisplayScreenHeight = 0f;
        public static double DisplayScaleFactor = 0f;

        public static Guid ServiceID = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        public static Guid TxID = Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");
        public static Guid RxID = Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");
        public static Guid RSCServiceID = Guid.Parse("00001814-0000-1000-8000-00805f9b34fb");
        public static Guid RSCMeasurementID = Guid.Parse("00002a53-0000-1000-8000-00805f9b34fb");
        public static long RealmDBSizeLimitInMB = 500;
        public static readonly long MinIntervalRealmDBSizeLimitInMB = 86400000;
        //public static bool CompactDB = RealmService.CompactAndMigrateDatabase(); //first DB operation when you open the app
        public static readonly int TRUE = 1;
        public static readonly int FALSE = 0;
        public static int ASMConnectionTimeoutInMs = 75000; //75 seconds until the device disconnects, actually this is 60 seconds, but to be safe
        public static int BLERefreshServicesWait = 10000; //wait 10 seconds for the refresh
        public static readonly int MaxSensorStorageCapacityMB = 512;
        public static readonly int MaxSensorStorageCapacityKB = MaxSensorStorageCapacityMB * 1024;
        public static readonly int SensorClockFrequency = 32768;
        public App()
        {
            //InitializeComponent();
            //string ScreenDetails = Device.OS.ToString() + " Device Screen Size:\n" +
            //    $"Width: {DisplayScreenWidth}\n" +
            //    $"Height: {DisplayScreenHeight}\n" +
            //    $"Scale Factor: {DisplayScaleFactor}";

            ProjectSettings.Data.DisplayScaleFactor = DisplayScaleFactor;
            if (DisplayScaleFactor <= 2)
            {
                ProjectSettings.Data.ScaleFontSize = 0.9;
            }

            //NavigationService.SetMenuPage();

        }

        //public static void SendSyncMessage(SyncEvent currentEvent, string AsmID)
        //{
        //    MessagingCenter.Send(Current, "sync" + AsmID, currentEvent);
        //}

        protected async override void OnResume()
        {
            await PostFinalizer(false);
            
        }

        protected async override void OnStart()
        {
            await PostFinalizer();
        }

        protected async override void OnSleep() 
        {
            await PostFinalizer(true); 
        }

        private async Task PostFinalizer()
        {
            bool postFinalizer;

            if (App.Current.Properties.ContainsKey("PostFinalizer"))
                postFinalizer = (bool)App.Current.Properties["PostFinalizer"];
            else
            {
                App.Current.Properties.Add("PostFinalizer", false);
                return;
            }

            if (postFinalizer)       //app is killed previously
            {
                
            }

            await PostFinalizer(true);
        }
        /// <summary>
        /// The idea behind this is the app is marked as force stopped when it goes to onsleep/background, if the app is put back in the foreground onresume is called and it is no longer marked as a force stopped, however if the app is indeed killed oncreate will create the log because it was marked prior as a force stop by onsleep
        /// </summary>
        /// <param name="ForceStop"></param>
        /// <returns></returns>
        private async Task PostFinalizer(bool ForceStop)
        {
            if (!App.Current.Properties.ContainsKey("PostFinalizer"))   //does nothing if the value doesn't exit
                return;
            App.Current.Properties["PostFinalizer"] = ForceStop;
            if(ForceStop)
            {
                
            }
            else
            {
                
            }
            await App.Current.SavePropertiesAsync();
        }
    }
}
