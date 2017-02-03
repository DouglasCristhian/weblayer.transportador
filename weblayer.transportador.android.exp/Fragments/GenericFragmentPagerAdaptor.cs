using System;
using Android.OS;
using Android.Views;
using Android.App;
using Android.Support.V4.App;
using System.Collections.Generic;

namespace weblayer.transportador.android.exp.Activities
{
    [Activity(Label = "GenericFragmentPagerAdaptor")]
    public class GenericFragmentPagerAdaptor : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();

        public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {

        }

        public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _fragmentList.Add(new GenericViewPagerFragment(view));
        }


        public void AddFragment(GenericViewPagerFragment fragment)
        {
            _fragmentList.Add(fragment);
        }


        public override int Count
        {
            get
            {
                return _fragmentList.Count;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return _fragmentList[position];
        }
    }
}
