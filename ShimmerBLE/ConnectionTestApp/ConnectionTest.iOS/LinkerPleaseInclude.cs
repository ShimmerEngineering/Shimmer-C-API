using System.Collections.Specialized;
using System.Windows.Input;
using Foundation;
using UIKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin;

namespace ConnectionTest.iOS
{
    // This class is never actually executed, but when Xamarin linking is enabled it does ensure types and properties
    // are preserved in the deployed app
    [Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
		public void Include(MvxTaskBasedBindingContext c)
        {
            c.Dispose();
            var c2 = new MvxTaskBasedBindingContext();
            c2.Dispose();
        }

        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                                      uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) =>
                                 barButton.Title = barButton.Title + "";
        }

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + "";
            textField.EditingChanged += (sender, args) => { textField.Text = ""; };
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + "";
            textView.Changed += (sender, args) => { textView.Text = ""; };
        }

        public void Include(UILabel label)
        {
            label.Text = label.Text + "";
            label.AttributedText = new NSAttributedString(label.AttributedText.ToString() + "");
        }

        public void Include(UIImageView imageView)
        {
            imageView.Image = new UIImage(imageView.Image.CGImage);
        }

        public void Include(UIDatePicker date)
        {
            date.Date = date.Date.AddSeconds(1);
            date.ValueChanged += (sender, args) => { date.Date = NSDate.DistantFuture; };
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UIProgressView progress)
        {
            progress.Progress = progress.Progress + 1;
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
        }

        public void Include(MvxViewController vc)
        {
            vc.Title = vc.Title + "";
        }

        public void Include(UIStepper s)
        {
            s.Value = s.Value + 1;
            s.ValueChanged += (sender, args) => { s.Value = 0; };
        }

        public void Include(UIPageControl s)
        {
            s.Pages = s.Pages + 1;
            s.ValueChanged += (sender, args) => { s.Pages = 0; };
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}"; };
        }

        public void Include(ICommand command)
        {
           command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        public void Include(MvvmCross.IoC.MvxPropertyInjector injector)
        {
            injector = new MvvmCross.IoC.MvxPropertyInjector();
        } 

        public void Include(System.ComponentModel.INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) => { var test = e.PropertyName; };
        }

        //it seems that neight PreserveAttribute or [assembly: Preserve] is working. The only way to load it is this LinkerPleaseInclude file.
        public void Include(out IMvxPlugin plugin)
        {
            plugin = new MvvmCross.Plugins.BLE.iOS.Plugin();//Make sure the plugin is loaded.
        }
    }
}
