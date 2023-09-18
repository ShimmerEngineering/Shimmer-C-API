using Foundation;
using MultiVerisenseSpeedTest.iOS;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace MultiVerisenseSpeedTest.iOS
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}