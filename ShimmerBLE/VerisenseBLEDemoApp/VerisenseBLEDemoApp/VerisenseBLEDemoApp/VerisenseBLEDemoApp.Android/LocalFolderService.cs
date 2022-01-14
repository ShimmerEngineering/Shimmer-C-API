using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using shimmer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerisenseBLEDemoApp.Droid;

[assembly:Xamarin.Forms.Dependency(typeof(LocalFolderService))]
namespace VerisenseBLEDemoApp.Droid
{
    public class LocalFolderService : ILocalFolderService
    {
        public string GetAppLocalFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).Path;
        }
    }
}