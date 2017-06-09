using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using weblayer.transportador.android.exp.Adapters;
using weblayer.transportador.android.exp.Fragments;
using weblayer.transportador.android.core.Helpers;
using weblayer.transportador.core.BLL;
using weblayer.transportador.core.DAL;
using weblayer.transportador.core.Model;
using ZXing.Mobile;
using static Android.Widget.AdapterView;
using JavaUri = Android.Net.Uri;

namespace weblayer.transportador.android.exp.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class Activity_InformaEntrega : Activity_Base
    {
        #region variaveis
        Android.Support.V7.Widget.Toolbar toolbar;
        public static string MyPREFERENCES = "MyPrefs";
        MobileBarcodeScanner scanner;
        private Java.IO.File imagefile;
        private Spinner spinnerOcorrencia;
        private List<mySpinner> ocorr;
        private EditText txtCodigoNF;
        private TextView lblCNPJ;
        private TextView lblNumeroNF;
        private EditText txtGeolocalizacao;
        private EditText txtObservacao;
        private TextView txtDataEntrega;
        private TextView txtHoraEntrega;
        private TextView lblObservacao;
        private TextView lblGeolocalizacao;
        private Button btnEscanearNF;
        private Button btnSalvar;
        private Button btnAnexarImagem;
        private Button btnEnviar;
        private Button btnEnviarViaEmail;
        private ImageView imageView;
        private CheckBox checkBoxGeolocalizacao;
        private byte[] bytes;
        private Android.Graphics.Bitmap bitmap;
        private Entrega entrega;
        private string operacao;
        private string spinOcorrencia;
        private string descricaoocorrencia;
        private int count;
        Android.Net.Uri contentUri;
        private bool camcheck;
        private bool lercheck;
        public bool PROSSEGUIR;
        #endregion variaveis

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_InformaEntrega;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RestoreForm();

            var jsonnota = Intent.GetStringExtra("JsonEntrega");
            if (jsonnota == null)
            {
                entrega = null;
            }
            else
            {
                entrega = new EntregaRepository().Get(int.Parse(jsonnota));
                operacao = "selecionado";
            }

            FindViews();
            BindData();
            BindViews();

            ocorr = PopulateOcorrenciaList();
            spinnerOcorrencia.Adapter = new ArrayAdapter<mySpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ocorr);

            if (entrega != null)
                spinnerOcorrencia.SetSelection(getIndexByValue(spinnerOcorrencia, entrega.id_ocorrencia));

        }

        //DEFININDO OBJETOS E EVENTOS
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);

            if (operacao != "selecionado")
            {
                menu.RemoveItem(Resource.Id.action_deletar);
                menu.RemoveItem(Resource.Id.action_adicionar);
                menu.RemoveItem(Resource.Id.action_ajuda);
                menu.RemoveItem(Resource.Id.action_sobre);
                menu.RemoveItem(Resource.Id.action_sair);
                menu.RemoveItem(Resource.Id.action_filtrar);
            }
            else
            {
                menu.RemoveItem(Resource.Id.action_adicionar);
                menu.RemoveItem(Resource.Id.action_ajuda);
                menu.RemoveItem(Resource.Id.action_sobre);
                menu.RemoveItem(Resource.Id.action_sair);
                menu.RemoveItem(Resource.Id.action_filtrar);
            }
            menu.RemoveItem(Resource.Id.action_contato);

            return base.OnCreateOptionsMenu(menu);
        }

        private void FindViews()
        {
            #region FindViewsById
            lblGeolocalizacao = FindViewById<TextView>(Resource.Id.lblGeolocalizacao);
            txtCodigoNF = FindViewById<EditText>(Resource.Id.txtCodigoNF);
            spinnerOcorrencia = FindViewById<Spinner>(Resource.Id.spinnerOcorrencia);
            spinnerOcorrencia.ItemSelected += new EventHandler<ItemSelectedEventArgs>(SpinnerOcorrencia_ItemSelected);
            txtDataEntrega = FindViewById<TextView>(Resource.Id.txtDataEntrega);
            txtHoraEntrega = FindViewById<TextView>(Resource.Id.txtHoraEntrega);
            txtObservacao = FindViewById<EditText>(Resource.Id.txtObservacao);
            lblObservacao = FindViewById<TextView>(Resource.Id.lblObservacao);
            lblCNPJ = FindViewById<TextView>(Resource.Id.lblCNPJ);
            lblNumeroNF = FindViewById<TextView>(Resource.Id.lblNumeroNF);
            btnAnexarImagem = FindViewById<Button>(Resource.Id.btnAnexarImagem);
            btnEscanearNF = FindViewById<Button>(Resource.Id.btnEscanearNF);
            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            btnEnviar = FindViewById<Button>(Resource.Id.btnEnviar);
            btnEnviarViaEmail = FindViewById<Button>(Resource.Id.btnEnviarViaEmail);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            checkBoxGeolocalizacao = FindViewById<CheckBox>(Resource.Id.checkBoxGeolocalizacao);
            txtGeolocalizacao = FindViewById<EditText>(Resource.Id.txtGeolocalizacao);
            #endregion

            //txtGeolocalizacao.Visibility = ViewStates.Gone;
            lblGeolocalizacao.Visibility = ViewStates.Gone;

            if (operacao == "selecionado")
            {
                spinnerOcorrencia.Enabled = false;
                txtObservacao.Focusable = false;
                txtCodigoNF.Focusable = false;

                btnEscanearNF.Visibility = ViewStates.Gone;
                btnAnexarImagem.Visibility = ViewStates.Gone;
                btnEnviar.Visibility = ViewStates.Gone;
                btnSalvar.Visibility = ViewStates.Gone;
                checkBoxGeolocalizacao.Visibility = ViewStates.Gone;
            }

            imageView = FindViewById<ImageView>(Resource.Id.imageView);

            MobileBarcodeScanner.Initialize(Application);
            scanner = new MobileBarcodeScanner();

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            this.Title = "Informar Ocorrência";
            toolbar.MenuItemClick += Toolbar_MenuItemClick;
        }

        private void BindViews()
        {
            if (entrega == null)
                return;

            txtCodigoNF.Text = entrega.ds_NFE;
            spinOcorrencia = entrega.id_ocorrencia.ToString();
            txtDataEntrega.Text = entrega.dt_entrega.Value.ToString("dd/MM/yyyy");
            txtHoraEntrega.Text = entrega.dt_entrega.Value.ToString("HH:mm");
            txtObservacao.Text = entrega.ds_observacao.ToString();

            Substring_Helper sub = new Substring_Helper();
            lblCNPJ.Text = "CNPJ Emissor: " + sub.Substring_CNPJ(entrega.ds_NFE);
            lblNumeroNF.Text = "Número NF: " + sub.Substring_NumeroNF(entrega.ds_NFE) + "/" + sub.Substring_SerieNota(entrega.ds_NFE);

            if (entrega.Image != null)
            {
                ByteHelper helper = new ByteHelper();
                bitmap = helper.ByteArrayToImage(entrega.Image);
                imageView.SetImageBitmap(bitmap);
            }

            if (entrega.ds_geolocalizacao == null || entrega.ds_geolocalizacao == "")
            {
                txtGeolocalizacao.Visibility = ViewStates.Gone;
                lblGeolocalizacao.Visibility = ViewStates.Gone;
            }
            else if (entrega.ds_geolocalizacao != null || entrega.ds_geolocalizacao != "")
            {
                lblGeolocalizacao.Visibility = ViewStates.Visible;
                txtGeolocalizacao.Visibility = ViewStates.Visible;
                txtGeolocalizacao.Text = entrega.ds_geolocalizacao.ToString();
            }
            checkBoxGeolocalizacao.Visibility = ViewStates.Gone;
        }

        private void BindModel()
        {
            if (entrega == null)
                entrega = new Entrega();

            entrega.ds_NFE = txtCodigoNF.Text.ToString();

            string data = (txtDataEntrega.Text + " " + txtHoraEntrega.Text);
            var datahora = DateTime.Parse(data, CultureInfo.CreateSpecificCulture("pt-BR"));

            entrega.dt_entrega = datahora;//DateTime.Parse(datahora);
            entrega.dt_inclusao = DateTime.Now;
            var minhaocorrencia = ocorr[spinnerOcorrencia.SelectedItemPosition];
            entrega.id_ocorrencia = minhaocorrencia.Id();
            entrega.ds_observacao = txtObservacao.Text.ToString();
            if (bytes != null)
            {
                entrega.Image = bytes;
                entrega.ds_ImageUri = imagefile.AbsolutePath;
            }
            entrega.ds_geolocalizacao = txtGeolocalizacao.Text;
        }

        private void BindData()
        {
            if (entrega == null)
            {
                txtDataEntrega.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtHoraEntrega.Text = DateTime.Now.ToString("HH:mm");
                btnEnviarViaEmail.Visibility = ViewStates.Gone;
            }

            if (entrega != null)
            {
                if (entrega.ds_observacao == "")
                {
                    txtObservacao.Visibility = ViewStates.Gone;
                    lblObservacao.Visibility = ViewStates.Gone;
                }

                if (entrega.ds_geolocalizacao == "")
                {
                    txtGeolocalizacao.Visibility = ViewStates.Gone;
                    checkBoxGeolocalizacao.Visibility = ViewStates.Gone;
                }
            }

            if (operacao != "selecionado")
            {
                txtDataEntrega.Click += TxtDataEntrega_Click;
                txtHoraEntrega.Click += TxtHoraEntrega_Click;
            }

            lblGeolocalizacao.Visibility = ViewStates.Gone;
            checkBoxGeolocalizacao.CheckedChange += CheckBoxGeolocalizacao_CheckedChange;
            btnEscanearNF.Click += BtnEscanearNF_Click;
            btnAnexarImagem.Click += BtnAnexarImagem_Click;
            btnEnviar.Click += BtnEnviar_Click;
            btnSalvar.Click += BtnSalvar_Click;
            btnEnviarViaEmail.Click += BtnEnviarViaEmail_Click;
            txtCodigoNF.FocusChange += TxtCodigoNF_FocusChange;
            imageView.Click += ImageView_Click;
        }

        private void BtnAnexarImagem_Click(object sender, EventArgs e)
        {
            PermissoesGarantidas(0);

            if (PROSSEGUIR == true)
            {
                TirarFoto();
            }
        }

        private void CheckBoxGeolocalizacao_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (checkBoxGeolocalizacao.Checked)
            {
                GetGeolocalizacao(this);
            }

            if (!checkBoxGeolocalizacao.Checked)
            {
                txtGeolocalizacao.Text = "";
            }
        }

        private void ImageView_Click(object sender, EventArgs e)
        {
            if (entrega != null)
            {
                if (entrega.Image != null)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutByteArray("imagem", entrega.Image);

                    Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    FragmentImageView dialog = new FragmentImageView();
                    dialog.Arguments = bundle;
                    dialog.Show(transaction, "dialog");
                }
            }
            else
            {
                if (bytes != null)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutByteArray("imagem", bytes);

                    Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    FragmentImageView dialog = new FragmentImageView();
                    dialog.Arguments = bundle;
                    dialog.Show(transaction, "dialog");
                }
            }
        }

        private void TxtCodigoNF_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (txtCodigoNF.Text.Length == 0)
            {
                return;
            }
            else if (!e.HasFocus && txtCodigoNF.Text.Length > 0 && (txtCodigoNF.Text.Length < 44 || txtCodigoNF.Text.Length > 44))
            {
                txtCodigoNF.Error = "Código inválido! O código de barras deve ter 44 caracteres!";
                lblCNPJ.Text = "CNPJ Emissor: ";
                lblNumeroNF.Text = "Número NF: ";
            }
            else if ((!e.HasFocus && txtCodigoNF.Text.Length == 44))
            {
                txtCodigoNF.Error = null;
                Substring_Helper sub = new Substring_Helper();
                lblCNPJ.Text = "CNPJ Emissor: " + sub.Substring_CNPJ(txtCodigoNF.Text.ToString());
                string numero_serie = sub.Substring_NumeroNF(txtCodigoNF.Text.ToString());
                if (numero_serie != null)
                {
                    lblNumeroNF.Text = "Número NF: " + sub.Substring_NumeroNF(txtCodigoNF.Text.ToString()) + " / " + sub.Substring_SerieNota(txtCodigoNF.Text.ToString());
                }
            }
        }

        private List<mySpinner> PopulateOcorrenciaList()
        {
            List<mySpinner> lista = new List<mySpinner>();
            var listaOcorrencias = new OcorrenciaRepository().List();

            lista.Add(new mySpinner(0, "Selecione.."));

            foreach (var item in listaOcorrencias)
            {
                lista.Add(new mySpinner(item.id, item.ds_descricao));
            }

            return lista;
        }

        private bool ValidateViews()
        {
            var validacao = true;
            if (txtCodigoNF.Length() == 0 || txtCodigoNF.Length() < 44 || txtCodigoNF.Length() > 44)
            {
                validacao = false;
                txtCodigoNF.Error = "Código inválido! O código de barras deve ter 44 caracteres!";
            }

            if (spinnerOcorrencia.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, selecione a ocorrência", ToastLength.Short).Show();
            }

            //TODO: TERMINAR VALIDAÇÕES
            return validacao;
        }

        private void SpinnerOcorrencia_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            txtCodigoNF.ClearFocus();
            spinOcorrencia = spinnerOcorrencia.SelectedItem.ToString();
        }

        //EVENTOS CLICK
        private void BtnEnviarViaEmail_Click(object sender, EventArgs e)
        {
            //Enviar após a inserção
            SendByEmail();
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            VerificaGeolocalizacao();
            if (ValidateViews())
                SendByEmail();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            VerificaGeolocalizacao();
            //Save();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void TxtDataEntrega_Click(object sender, EventArgs e)
        {
            DatePickerHelper frag = DatePickerHelper.NewInstance(delegate (DateTime time)
            {
                //var teste = DateTime.Now.ToString("hh:mm:ss");
                txtDataEntrega.Text = time.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));
            });

            frag.Show(FragmentManager, DatePickerHelper.TAG);
        }

        private void TxtHoraEntrega_Click(object sender, EventArgs e)
        {
            TimePickerHelper frag = TimePickerHelper.NewInstance(delegate (DateTime time)
            {
                txtHoraEntrega.Text = time.ToString("HH:mm");
            });

            frag.Show(FragmentManager, TimePickerHelper.TAG);
        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_deletar:
                    Delete();
                    break;
            }
        }

        private void BtnEscanearNF_Click(object sender, EventArgs e)
        {
            PermissoesGarantidas(0);
            if (PROSSEGUIR == true)
            {
                Scanner();
            }
        }

        private async void Scanner()
        {
            ZXing.Result result = null;
            //scanner.UseCustomOverlay = false;
            scanner.TopText = "Aguarde o escaneamento do código de barras";

            new Thread(new ThreadStart(delegate
            {
                while (result == null)
                {
                    scanner.AutoFocus();
                    Thread.Sleep(2000);
                }
            })).Start();

            result = await scanner.Scan();
            HandleScanResult(result);
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

        private void TirarFoto()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            //imagefile = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures),
            //Java.Lang.String.ValueOf(count++) + ".jpeg");

            var directory = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory, "W Transportador - Canhotos/").ToString();
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            imagefile = new Java.IO.File(directory + "/" + Java.Lang.String.ValueOf(count++) + ".jpeg");

            JavaUri tempuri = JavaUri.FromFile(imagefile);
            SaveForm();
            intent.PutExtra(MediaStore.ExtraOutput, tempuri);
            StartActivityForResult(intent, 0);
        }


        //EVENTOS RESULTADOS
        private void GetGeolocalizacao(Context context)
        {
            if (ActivityCompat.CheckSelfPermission(context, Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                IntentGeolocalizacao();
            }
            else
            {
                string[] permissionRequest = { Manifest.Permission.AccessFineLocation };
                RequestPermissions(permissionRequest, 333);
            }
        }

        private void IntentGeolocalizacao()
        {
            Intent intent = new Intent(this, typeof(Activity_Geolocalizacao));
            StartActivityForResult(intent, 0);
        }

        public void PermissoesGarantidas(int valor)
        {
            if (valor == 0)
            {
                if ((ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == Permission.Granted))
                {
                    camcheck = true;
                    if (lercheck == true && camcheck == true)
                    {
                        PROSSEGUIR = true;
                    }
                }
                else
                {
                    string[] permissionRequest = { Manifest.Permission.Camera };
                    RequestPermissions(permissionRequest, 111);
                }

                if ((ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Granted))
                {
                    lercheck = true;
                    if (lercheck == true && camcheck == true)
                    {
                        PROSSEGUIR = true;
                    }
                }
                else
                {
                    string[] permissionRequest = { Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(permissionRequest, 222);
                }
            }
        }

        private void DefinirOcorrencia()
        {
            if (entrega.id_ocorrencia == 1)
                descricaoocorrencia = "ENTREGA";
            if (entrega.id_ocorrencia == 2)
                descricaoocorrencia = "INFORMATIVO";
            if (entrega.id_ocorrencia == 3)
                descricaoocorrencia = "REENTREGA";
            if (entrega.id_ocorrencia == 4)
                descricaoocorrencia = "DEVOLUÇÃO";
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if ((requestCode == 111))
            {
                if (grantResults[0] == Permission.Granted)
                {
                    //camcheck = true;
                    if ((camcheck == true) && (lercheck == true))
                    {
                        PROSSEGUIR = true;
                        TirarFoto();
                    }
                }
                else
                    Toast.MakeText(this, "Não é possível usar a câmera sem as devidas permissões", ToastLength.Long).Show();
                return;
            }

            if ((requestCode == 222))
            {
                if (grantResults[0] == Permission.Granted)
                {
                    if ((camcheck == true) && (lercheck == true))
                    {
                        PROSSEGUIR = true;
                        Scanner();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Não é possível usar a câmera sem as devidas permissões", ToastLength.Long).Show();
                    return;
                }
            }

            if ((requestCode == 333))
            {
                if (grantResults[0] == Permission.Granted)
                {
                    IntentGeolocalizacao();
                }
                else
                {
                    Toast.MakeText(this, "Não é possível usar o GPS sem as devidas permissões", ToastLength.Long).Show();
                    checkBoxGeolocalizacao.Checked = false;
                    return;
                }
            }
        }

        public void HandleScanResult(ZXing.Result result)
        {
            if (result == null)
            {
                Toast.MakeText(this, "Ocorreu um erro durante o escaneamento. Por favor, tente novamente", ToastLength.Short).Show();
                txtCodigoNF.Error = null;
            }

            if (result != null && !string.IsNullOrEmpty(result.Text) && result.Text.Length == 44)
            {
                txtCodigoNF.Text = result.Text;

                Substring_Helper sub = new Substring_Helper();
                lblCNPJ.Text = "CNPJ Emissor: " + sub.Substring_CNPJ(result.Text.ToString());
                lblNumeroNF.Text = "Número NF: " + sub.Substring_NumeroNF(result.Text.ToString()) + "/" + sub.Substring_SerieNota(result.Text.ToString());
                txtCodigoNF.Error = null;
            }
            else
            {
                txtCodigoNF.Error = "Código inválido! O código de barras deve ter 44 caracteres!";
                lblCNPJ.Text = "CNPJ Emissor: ";
                lblNumeroNF.Text = "Número NF: ";
                txtCodigoNF.Text = "";
            }

        }

        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            if (requestCode == 0)
            {
                switch (resultCode)
                {
                    case Android.App.Result.Ok:
                        if (imagefile.Exists())
                        {
                            //Salvar imagem na galeria
                            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                            contentUri = Android.Net.Uri.FromFile(imagefile);
                            mediaScanIntent.SetData(contentUri);
                            SendBroadcast(mediaScanIntent);

                            //Converter image para byte
                            Java.Net.URI juri = new Java.Net.URI(contentUri.ToString());
                            ByteHelper helper = new ByteHelper();
                            bytes = helper.imageToByteArray(juri, bytes);

                            System.IO.Stream stream = ContentResolver.OpenInputStream(contentUri);
                            imageView.SetImageBitmap(BitmapFactory.DecodeStream(stream));

                            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
                            BitmapFactory.DecodeFile(contentUri.ToString(), options);
                            //options.InJustDecodeBounds = false;

                            Matrix mtx = new Matrix();
                            ExifInterface exif = new ExifInterface(contentUri.ToString());
                            string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                            switch (orientation)
                            {
                                case "6":
                                    mtx.PreRotate(90);
                                    mtx.Dispose();
                                    mtx = null;
                                    break;
                                case "1":
                                    break;
                                default:
                                    mtx.PreRotate(90);
                                    mtx.Dispose();
                                    mtx = null;
                                    break;
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, "O arquivo não foi salvo devido à um erro", ToastLength.Short).Show();
                        }
                        break;


                    case Result.FirstUser:
                        string Lat = data.GetStringExtra("Lat");
                        string Long = data.GetStringExtra("Lon");
                        string mensagem = data.GetStringExtra("mensagem");
                        string endereco = data.GetStringExtra("Endereco");

                        if ((Lat == null || Lat == "") && (Long == null || Long == ""))
                        {
                            Toast.MakeText(this, "Endereço não encontrado. Verifique se seu GPS está ativado e tente novamente", ToastLength.Long).Show();
                            txtGeolocalizacao.Visibility = ViewStates.Visible;
                            txtGeolocalizacao.Text = "";
                            checkBoxGeolocalizacao.Checked = false;
                        }
                        else
                        {
                            txtGeolocalizacao.Visibility = ViewStates.Visible;
                            txtGeolocalizacao.Text = Lat + ", " + Long;
                        }
                        break;


                    case Result.Canceled:
                        break;

                    default:
                        break;
                }
            }
        }

        public void VerificaGeolocalizacao()
        {
            if (checkBoxGeolocalizacao.Checked && (txtGeolocalizacao.Text == null || txtGeolocalizacao.Text == ""))
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);

                alert.SetTitle("Não foi possível adquirir a geolocalização. Salvar assim mesmo?");
                alert.SetPositiveButton("Sim", (senderAlert, args) =>
                {
                    try
                    {
                        Save();
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
            else
                Save();
        }

        public void SendByEmail()
        {
            var email = new Intent(Android.Content.Intent.ActionSend);
            //email.PutExtra(Android.Content.Intent.ExtraCc, "Testando 123");

            if (entrega.Image != null || entrega.ds_ImageUri != null)
            {
                imagefile = new Java.IO.File(entrega.ds_ImageUri);

                if (!imagefile.Exists())
                {
                    ByteHelper helper = new ByteHelper();
                    bitmap = helper.ByteArrayToImage(entrega.Image);

                    var stream = new FileStream(imagefile.AbsolutePath, FileMode.Create);
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                    stream.Close();

                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri uri = Android.Net.Uri.FromFile(imagefile);
                    mediaScanIntent.SetData(uri);
                    SendBroadcast(mediaScanIntent);
                }
                else if (imagefile.Exists())
                {
                    ByteHelper helper = new ByteHelper();
                    bitmap = helper.ByteArrayToImage(entrega.Image);

                    var stream = new FileStream(imagefile.AbsolutePath, FileMode.Create);
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                    stream.Close();
                }

                Android.Net.Uri contentUri = JavaUri.FromFile(imagefile);
                email.PutExtra(Intent.ExtraStream, contentUri);
            }

            DefinirOcorrencia();
            email.PutExtra(Intent.ExtraSubject, "NFE: " + entrega.ds_NFE + "; Ocorrência: " + entrega.id_ocorrencia.ToString() + "; Data: " + DateTime.Parse(entrega.dt_entrega.ToString()));
            email.PutExtra(Intent.ExtraText, "NFe: " + entrega.ds_NFE +
                                             "\nOcorrência: " + descricaoocorrencia +
                                             "\nData de Inclusão: " + entrega.dt_inclusao +
                                             "\nData de Entrega: " + entrega.dt_entrega +
                                             "\nGeoposicionamento: " + entrega.ds_geolocalizacao +
                                             "\nObservação: " + entrega.ds_observacao);
            email.SetType("application/image");
            Intent.CreateChooser(email, "Enviar Email Via");

            try
            {
                StartActivityForResult(email, 0);
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Email não enviado devido à um erro:" + e.Message, ToastLength.Long).Show();
            }

        }

        private void Delete()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Tem certeza que deseja excluir este registro?");
            alert.SetPositiveButton("Sim", (senderAlert, args) =>
            {
                try
                {
                    var Ent = new EntregaManager();
                    Ent.Delete(entrega);

                    Intent myIntent = new Intent(this, typeof(Activity_Menu));
                    myIntent.PutExtra("mensagem", Ent.mensagem);
                    SetResult(Android.App.Result.Ok, myIntent);
                    Finish();
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

        private void Save()
        {
            if (!ValidateViews())
                return;
            try
            {
                BindModel();

                var Ent = new EntregaManager();
                Ent.Save(entrega);

                Intent myIntent = new Intent(this, typeof(Activity_Menu));
                myIntent.PutExtra("mensagem", Ent.mensagem);
                SetResult(Result.Ok, myIntent);
                //SendByEmail();

                Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void SaveForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();
            prefEditor.PutInt("Soma", count);
            prefEditor.Commit();
        }

        private void RestoreForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
            var somePref = prefs.GetInt("Soma", 0);
            count = somePref;
        }
    }
}
