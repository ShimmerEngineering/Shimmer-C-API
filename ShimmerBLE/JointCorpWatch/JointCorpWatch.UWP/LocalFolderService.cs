
using JointCorpWatch.UWP;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace JointCorpWatch.UWP
{
    public class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}