using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using weblayer.transportador.android.exp.Activities.Menu;
using weblayer.transportador.android.exp.Adapters;
using weblayer.transportador.core.BLL;
using weblayer.transportador.core.Model;

namespace weblayer.transportador.android.exp.Activities
{
    [Activity(MainLauncher = false, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class Activity_Menu : AppCompatActivity
    {
        ListView ListViewEntrega;
        List<Entrega> ListaEntregas;
        private TextView txtEntregas;
        Android.Support.V7.Widget.Toolbar toolbar;

        private int dataEmissao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Menu);

            transportador.core.DAL.Database.Initialize();

            FindViews();
            BindData();
            FillList(dataEmissao);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = " W/Transportador Express";
            toolbar.InflateMenu(Resource.Menu.menu_toolbar);
            toolbar.Menu.RemoveItem(Resource.Id.action_deletar);

            toolbar.MenuItemClick += Toolbar_MenuItemClick;
        }

        private void FindViews()
        {
            ListViewEntrega = FindViewById<ListView>(Resource.Id.EntregaListView);
            txtEntregas = FindViewById<TextView>(Resource.Id.edtMensagem);
        }

        private void BindData()
        {
            ListViewEntrega.ItemClick += ListViewEntrega_ItemClick;
            ListViewEntrega.ItemLongClick += ListViewEntrega_ItemLongClick1;
        }

        private void ListViewEntrega_ItemLongClick1(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var ListViewEntregaClick = sender as ListView;
            var t = ListaEntregas[e.Position];

            Delete(t);
        }

        private void ListViewEntrega_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var ListViewEntregaClick = sender as ListView;
            var t = ListaEntregas[e.Position];

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_InformaEntrega));
            //intent.PutExtra("JsonEntrega", Newtonsoft.Json.JsonConvert.SerializeObject(t));
            intent.PutExtra("JsonEntrega", t.id.ToString());
            StartActivityForResult(intent, 0);
        }

        private void FillList(int dataEmissao)
        {
            ListaEntregas = new EntregaManager().GetEntregaFiltro(dataEmissao);
            if (ListaEntregas.Count > 0)
            {
                ListViewEntrega.Adapter = new Adapter_EntregaListView(this, ListaEntregas);
                txtEntregas.Enabled = false;
                txtEntregas.Visibility = ViewStates.Gone;
            }
            else
            {
                txtEntregas.Enabled = true;
                txtEntregas.Visibility = ViewStates.Visible;
                ListViewEntrega.Adapter = new Adapter_EntregaListView(this, ListaEntregas);
            }
        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_adicionar:
                    Intent intent = new Intent(this, typeof(Activity_InformaEntrega));
                    StartActivityForResult(intent, 0);
                    break;

                case Resource.Id.action_ajuda:
                    StartActivity(typeof(Activity_WebView));
                    break;


                case Resource.Id.action_sobre:
                    Intent intent3 = new Intent(this, typeof(Activity_Sobre));
                    StartActivityForResult(intent3, 0);
                    break;

                case Resource.Id.action_filtrar:
                    Intent intent4 = new Intent(this, typeof(Activity_FiltrarEntregas));
                    StartActivityForResult(intent4, 0);
                    break;

                case Resource.Id.action_contato:
                    Intent intent5 = new Intent(this, typeof(Activity_Contato));
                    StartActivityForResult(intent5, 0);
                    break;

                case Resource.Id.action_sair:
                    Finish();
                    break;
            }
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

        private void Delete(Entrega ent)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);

            alert.SetTitle("Tem certeza que deseja excluir esta ocorrência?");
            alert.SetPositiveButton("Sim", (senderAlert, args) =>
            {
                try
                {
                    var entrega = new EntregaManager();
                    entrega.Delete(ent);

                    //Intent myIntent = new Intent(this, typeof(Activity_Menu));
                    ////myIntent.PutExtra("mensagem", entrega.mensagem);
                    //SetResult(Android.App.Result.Ok, myIntent);

                    Toast.MakeText(this, entrega.mensagem, ToastLength.Short).Show();
                    FillList(dataEmissao);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }

            });
            alert.SetNegativeButton("Não", (senderAlert, args) =>
            {
            });

            RunOnUiThread(() =>
            {
                alert.Show();
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                dataEmissao = data.GetIntExtra("DataEmissao", 0);
                FillList(dataEmissao);

                var mensagem = data.GetStringExtra("mensagem");
                Toast.MakeText(this, mensagem, ToastLength.Short).Show();
            }
        }
    }
}