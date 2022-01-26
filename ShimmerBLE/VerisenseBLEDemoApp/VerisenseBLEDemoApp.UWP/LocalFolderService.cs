using BLE.Client.UWP;
using shimmer.Services;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace BLE.Client.UWP
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}
