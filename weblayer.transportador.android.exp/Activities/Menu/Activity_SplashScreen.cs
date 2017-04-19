
using Android.App;
using Android.OS;
using Android.Views;

namespace weblayer.transportador.android.exp.Activities
{
    [Activity(MainLauncher = true, NoHistory = true)]
    public class Activity_SplashIntro : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.SplashLayout);

            if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
            else
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            }

            System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());
        }

        private void LoadActivity()
        {
            System.Threading.Thread.Sleep(5000);
            RunOnUiThread(() => StartActivity(typeof(Activity_Menu)));
        }
    }
}