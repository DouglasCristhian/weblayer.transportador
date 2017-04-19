using Android.OS;

namespace weblayer.transportador.android.exp.Activities.Menu
{

    public abstract class Activity_Base : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
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