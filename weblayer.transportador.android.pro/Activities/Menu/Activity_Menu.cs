using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading;
using weblayer.transportador.android.pro.Activities.Entrega;
using weblayer.transportador.android.pro.Adapters;
using weblayer.transportador.android.pro.Fragments;
using weblayer.transportador.core.BLL;
using weblayer.transportador.core.DAL;

namespace weblayer.transportador.android.pro.Activities.Menu
{
    [Activity(MainLauncher = false, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation |
        Android.Content.PM.ConfigChanges.ScreenSize)]
    public class Activity_Menu : Activity
    {
        ListView ListViewEntrega;
        List<transportador.core.Model.Entrega> ListaEntregas;
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
            toolbar.Title = " W/Transportador Professional";
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
            ListViewEntrega.ItemLongClick += ListViewEntrega_ItemLongClick;
        }

        private void ListViewEntrega_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
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
                case Resource.Id.action_filtrar:
                    Intent intent0 = new Intent(this, typeof(Activity_FiltrarEntregas));
                    StartActivityForResult(intent0, 0);
                    break;

                case Resource.Id.action_adicionar:
                    Intent intent = new Intent(this, typeof(Activity_InformaEntrega));
                    StartActivityForResult(intent, 0);
                    break;

                case Resource.Id.action_ajuda:
                    Intent intent2 = new Intent(this, typeof(Activity_WebView));
                    StartActivityForResult(intent2, 0);
                    break;


                case Resource.Id.action_sobre:
                    Intent intent3 = new Intent(this, typeof(Activity_Sobre));
                    StartActivityForResult(intent3, 0);
                    break;

                case Resource.Id.action_sair:
                    Finish();
                    break;

                case Resource.Id.action_sincronizar:

                    var progressDialog = ProgressDialog.Show(this, "Por favor aguarde...", "Verificando os dados...", true);
                    new Thread(new ThreadStart(delegate
                    {
                        System.Threading.Thread.Sleep(1000);
                        RunOnUiThread(() => SincronizarTeste());
                        //LOAD METHOD TO GET ACCOUNT INFO
                        RunOnUiThread(() => Toast.MakeText(this, "Sincronizado com sucesso!", ToastLength.Short).Show());

                        //HIDE PROGRESS DIALOG
                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();
                    break;

                case Resource.Id.action_legenda:
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    Fragment_Legendas dialog = new Fragment_Legendas();
                    dialog.Show(transaction, "dialog");
                    break;
            }
        }

        public void SincronizarTeste()
        {
            foreach (transportador.core.Model.Entrega item in ListaEntregas)
            {
                EntregaRepository rep = new EntregaRepository();

                item.fl_status = 1;
                rep.Save(item);

                FillList(dataEmissao);
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

        private void Delete(transportador.core.Model.Entrega ent)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Tem certeza que deseja excluir esta ocorrência?");
            alert.SetPositiveButton("Sim", (senderAlert, args) =>
            {
                try
                {
                    var entrega = new EntregaManager();
                    entrega.Delete(ent);

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