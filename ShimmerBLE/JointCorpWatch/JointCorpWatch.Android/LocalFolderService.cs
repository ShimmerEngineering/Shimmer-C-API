using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JointCorpWatch.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace JointCorpWatch.Droid
{
    public class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
        }
    }
}