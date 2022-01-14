using shimmer.Services;
using System;
using VerisenseBLEDemoApp.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace VerisenseBLEDemoApp.iOS
{
    class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
