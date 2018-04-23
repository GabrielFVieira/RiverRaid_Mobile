using Android.App;
using Android.Widget;
using Android.OS;

namespace XamarinDemo
{
    [Activity(Label = "XamarinDemo", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Set our view from the "main" layout resource
            SetContentView(new AwesomeView(this));
        }
    }
}

