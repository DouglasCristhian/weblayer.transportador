using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using weblayer.transportador.android.pro.Activities;
using Android;
using weblayer.transportador.android.pro.Fragments;

namespace weblayer.transportador.android.pro.Activities
{
    [Activity(Label = "Ajuda", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Activity_Ajuda : Activity_Base
    {
        Android.Support.V7.Widget.Toolbar toolbar;
        ViewPager pager;
        GenericFragmentPagerAdaptor adapter;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_ManualUsuario;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Ajuda";

            pager = FindViewById<ViewPager>(Resource.Id.pager);
            adapter = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            adapter.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Fragment_UtilizandoApp, v, false);
                return view;
            });



            pager.Adapter = adapter;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_ajuda);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_sincronizar);
            menu.RemoveItem(Resource.Id.action_legenda);

            return base.OnCreateOptionsMenu(menu);
        }
    }
}