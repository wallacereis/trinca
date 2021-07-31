using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Android.Views;

namespace ChurrasTrincaAPP.Droid
{
    [Activity
        (Label = "Trinca"
        , Icon = "@drawable/icon"
        , Theme = "@style/MainTheme"
        , MainLauncher = false
        , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize 
    )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //UserDialogs Init
            UserDialogs.Init(this);

            //Set Transparent status bar and navigation bar
            SetStatusBarColor();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [System.Obsolete]
        public void SetStatusBarColor()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                // for covering the full screen in android..
                Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

                // clear FLAG_TRANSLUCENT_STATUS flag:
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

                Window.DecorView.SystemUiVisibility = 0;

                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}