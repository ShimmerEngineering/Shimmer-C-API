using DisconnectTest.UWP;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace DisconnectTest.UWP
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}
