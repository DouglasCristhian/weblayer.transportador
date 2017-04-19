using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace weblayer.transportador.android.pro.Activities.Menu
{
    [Activity(Label = "Novidades")]
    public class Activity_Novidades : Activity_Base
    {
        Android.Support.V7.Widget.Toolbar toolbar;
        private TextView txtNovidades;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Novidades;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Novidades";

            FindViews();
            BindData();
        }

        private void FindViews()
        {
            txtNovidades = FindViewById<TextView>(Resource.Id.txtNovidades);
        }

        private void BindData()
        {
            txtNovidades.Text = Novidades();
        }

        private string Novidades()
        {
            string Novidades;
            Novidades = " 1.2 (23/01/2017):"
                                     + "\n\n    [Novo] Instru��es de uso do aplicativo (Via menu 'Ajuda')"
                                     + "\n    [Melhorias] Atualiza��o dos �cones do menu";
            return Novidades;

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
            menu.RemoveItem(Resource.Id.action_filtrar);

            return base.OnCreateOptionsMenu(menu);
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

        private string GetVersion()
        {
            return Application.Context.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
        }
    }
}