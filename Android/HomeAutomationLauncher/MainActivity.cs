using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Content.PM;
using Android.Webkit;
using Android.Views;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter;

namespace HomeAutomationLauncher
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryHome, Intent.CategoryDefault })]
    public class MainActivity : AppCompatActivity
    {

        WebView _webView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppCenter.Start("215b930f-5eff-474f-8cc5-56402a76183e", typeof(Distribute), typeof(Analytics), typeof(Crashes));

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.TranslucentStatus);

            Window.DecorView.SystemUiVisibility |= (StatusBarVisibility)SystemUiFlags.HideNavigation;

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _webView = FindViewById<WebView>(Resource.Id.webView1);
            _webView.Settings.JavaScriptEnabled = true;
            //_webView.SetWebViewClient(new HelloWebViewClient());
            _webView.LoadUrl("http://192.168.2.100/app/devicehome/");
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}