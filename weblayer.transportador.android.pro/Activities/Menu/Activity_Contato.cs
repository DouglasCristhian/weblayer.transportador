using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace weblayer.transportador.android.pro.Activities.Menu
{
    [Activity(Label = "Contato")]
    public class Activity_Contato : Activity_Base
    {
        private LinearLayout layoutEmail;
        private LinearLayout layoutTelefone;
        private LinearLayout layoutSite;
        private ImageView imgEmail;
        private ImageView imgTel;
        private ImageView imgSite;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Contato;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindData();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;
            };
            return base.OnOptionsItemSelected(item);
        }

        private void FindViews()
        {
            layoutEmail = FindViewById<LinearLayout>(Resource.Id.layoutEmail);
            layoutTelefone = FindViewById<LinearLayout>(Resource.Id.layoutTelefone);
            layoutSite = FindViewById<LinearLayout>(Resource.Id.layoutSite);
            imgEmail = FindViewById<ImageView>(Resource.Id.ic_message);
            imgTel = FindViewById<ImageView>(Resource.Id.ic_phone);
            imgSite = FindViewById<ImageView>(Resource.Id.ic_site);
        }

        private void BindData()
        {
            imgEmail.SetImageResource(Resource.Mipmap.ic_contatoemail);
            imgTel.SetImageResource(Resource.Mipmap.ic_contatotel);
            imgSite.SetImageResource(Resource.Mipmap.ic_contatosite);
            layoutEmail.Click += LayoutEmail_Click;
            layoutTelefone.Click += LblTelefone_Click;
            layoutSite.Click += LayoutSite_Click;

        }

        private void LayoutEmail_Click(object sender, System.EventArgs e)
        {
            Intent email_intent = new Intent(Intent.ActionSend);
            email_intent.PutExtra(Intent.ExtraSubject, "Gostaria de falar com um consultor Weblayer!");
            email_intent.PutExtra(Intent.ExtraEmail, new string[] { "contato@weblayer.com.br" });
            email_intent.SetType("message/rfc822");
            Intent.CreateChooser(email_intent, "Enviar Email Via");

            try
            {
                StartActivity(email_intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Email não enviado devido à um erro:" + ex.Message, ToastLength.Long).Show();
            }
        }

        private void LblTelefone_Click(object sender, System.EventArgs e)
        {
            string phone = "0114063-5420";
            Intent intent = new Intent(Intent.ActionDial, Uri.FromParts("tel", phone, null));
            StartActivity(intent);
        }

        private void LayoutSite_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(Intent.ActionView, Uri.Parse("http://weblayer.com.br/contato/"));
            StartActivity(i);
        }
    }
}