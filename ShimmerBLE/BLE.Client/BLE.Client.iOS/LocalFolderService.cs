using BLE.Client.iOS;
using shimmer.Services;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace BLE.Client.iOS
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}