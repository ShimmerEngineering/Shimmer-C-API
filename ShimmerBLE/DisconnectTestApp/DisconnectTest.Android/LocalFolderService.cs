
using DisconnectTest.Droid;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace DisconnectTest.Droid
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
            //return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
