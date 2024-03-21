using BLE.Client.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;

using System.ComponentModel;
using Xamarin.Forms;
using System.ComponentModel;
using Xamarin.Forms;
using ShimmerBLEAPI;
using OxyPlot.Xamarin.Forms;
using OxyPlot;

namespace BLE.Client.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class DeviceListPage : MvxTabbedPage<DeviceListViewModel>
    {
        public DeviceListPage()
        {
            InitializeComponent();
        }

        public void OnStartDateSelected(object sender, DateChangedEventArgs args)
        {
            // date set in SelectedStartDate
        }

        public void OnStartTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                DeviceListViewModel.StartTimeSpan = _startTimePicker.Time;
            }
        }

        public void OnEndDateSelected(object sender, DateChangedEventArgs args)
        {
            // date set in SelectedEndDate
        }

        public void OnEndTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                DeviceListViewModel.EndTimeSpan = _endTimePicker.Time;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //when uwp top left back button is pressed

            //Device.BeginInvokeOnMainThread(async () => {
            //    var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
            //    if (result)
            //    {
            //        await Application.Current.MainPage.Navigation.PopAsync();
            //    }
            //    else
            //    {

            //    }
            //});

            return true;
        }
    }
}
