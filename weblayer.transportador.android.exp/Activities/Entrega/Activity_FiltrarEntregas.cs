using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.transportador.android.exp.Activities.Menu;
using weblayer.transportador.android.exp.Adapters;

namespace weblayer.transportador.android.exp.Activities
{
    [Activity(MainLauncher = false, Label = "")]
    public class Activity_FiltrarEntregas : Activity_Base
    {
        public string MyPREFERENCES = "MyPrefs";
        private Spinner spinnerDataEmissao;
        private List<mySpinner> spinnerDatas;
        private Button btnApagarFiltros;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_FiltrarEntregas;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindData();
            RestoreForm();

            spinnerDatas = PopulateSpinner();
            spinnerDataEmissao.Adapter = new ArrayAdapter<mySpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, spinnerDatas);

            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
            int resultado = prefs.GetInt("Id_DataEmissao", 0);
            spinnerDataEmissao.SetSelection(getIndexByValue(spinnerDataEmissao, resultado));
        }

        private void FindViews()
        {
            btnApagarFiltros = FindViewById<Button>(Resource.Id.btnApagarFiltros);
            spinnerDataEmissao = FindViewById<Spinner>(Resource.Id.spinnerDataEmissao);
            spinnerDataEmissao.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BindData()
        {
            btnApagarFiltros.Click += BtnApagarFiltros_Click;
        }

        private void BtnApagarFiltros_Click(object sender, System.EventArgs e)
        {
            spinnerDataEmissao.SetSelection(0);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_Filtro, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_aceitar:
                    SaveForm();
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

            }

            return base.OnOptionsItemSelected(item);
        }

        private int getIndexByValue(Spinner spinner, long myId)
        {
            int index = 0;

            var adapter = (ArrayAdapter<mySpinner>)spinner.Adapter;
            for (int i = 0; i < spinner.Count; i++)
            {
                if (adapter.GetItemId(i) == myId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private List<mySpinner> PopulateSpinner()
        {
            List<mySpinner> minhalista = new List<mySpinner>();

            minhalista.Add(new mySpinner(0, "Todas as ocorrências"));
            minhalista.Add(new mySpinner(1, "Ocorrências incluídas hoje"));
            minhalista.Add(new mySpinner(2, "Ocorrências incluídas esta semana"));
            minhalista.Add(new mySpinner(3, "Ocorrências incluídas este mês"));

            return minhalista;
        }

        private void SaveForm()
        {
            var prefs = Application.Context.GetSharedPreferences("MyPrefs", FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();

            prefEditor.PutInt("Id_DataEmissao", spinnerDataEmissao.SelectedItemPosition);
            prefEditor.Commit();

            #region DataEmissao
            int retorno_Data = 0;
            if (spinnerDataEmissao.SelectedItemPosition == 1)
            {
                retorno_Data = 1;
            }
            else if (spinnerDataEmissao.SelectedItemPosition == 2)
            {
                retorno_Data = 2;
            }
            else if (spinnerDataEmissao.SelectedItemPosition == 3)
            {
                retorno_Data = 3;
            }
            #endregion

            Intent intent = new Intent();
            intent.PutExtra("DataEmissao", retorno_Data);
            intent.PutExtra("mensagem", "Preferências de filtro atualizadas");
            SetResult(Result.Ok, intent);

            Finish();
        }

        private void RestoreForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
        }
    }
}