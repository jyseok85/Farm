using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Threading.Tasks;
using Android.Util;
using Android.Content;
using Farm.Models;
using Android.Widget;

namespace Farm.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "X:" + typeof (SplashActivity).Name;

        //public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        //{
        //    base.OnCreate(savedInstanceState, persistentState);

        //    SetContentView(Resource.Layout.Splash);
        //    FindViewById<TextView>(Resource.Id.txtAppVersion).Text = $"Version {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";
        //}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);

            FindViewById<TextView>(Resource.Id.txtDescription).Text = $"Version {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";

        }
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            Loading page = new Loading();
            //await page.InitDataSetting();

            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(5000); // Simulate a bit of startup work.
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}