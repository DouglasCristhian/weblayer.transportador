using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace weblayer.transportador.android.exp.Activities
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

            var Novidades = " 1.5 (23/03/2017):"
            + "\n[Melhoria] Implementação do foco automático no escaneamento da NFe"
            + "\n\n";

            Novidades = Novidades + " 1.4 (15/03/2017):"
            + "\n[Novo] Filtro de entregas por data de inclusão"
            + "\n\n";

            Novidades = Novidades + " 1.3 (10/02/2017):"
                + "\n[Melhorias] Correções no texto de ajuda"
                + "\n\n";

            Novidades = Novidades + " 1.2 (23/01/2017):"
                + "\n[Novo] Instruções de uso do aplicativo (Via menu 'Ajuda')"
                + "\n[Melhorias] Atualização dos ícones do menu"
                + "\n\n";

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
            menu.RemoveItem(Resource.Id.action_filtrar);
            menu.RemoveItem(Resource.Id.action_contato);

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