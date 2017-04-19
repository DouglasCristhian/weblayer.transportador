using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace weblayer.transportador.android.pro.Activities.Menu
{
    [Activity]
    public abstract class Activity_Base : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResource);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
        }

        protected abstract int LayoutResource
        {
            get;
        }
    }
}