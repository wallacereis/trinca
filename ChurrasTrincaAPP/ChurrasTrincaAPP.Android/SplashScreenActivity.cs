using Android.App;
using Android.OS;

namespace ChurrasTrincaAPP.Droid
{
    [Activity(
        Label = "Trinca",
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true
    )]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }
}