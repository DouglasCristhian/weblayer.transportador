
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace weblayer.transportador.android.pro.Activities
{
    [Activity(Label = "Ajuda")]
    public class Activity_WebView : Activity_Base
    {
        private WebView webView;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_WebView;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            webView = FindViewById<WebView>(Resource.Id.webView);

            var view = new ExtendWebViewClient();
            view.SetContextForDialog(this);
            webView.SetWebViewClient(view);

            WebSettings webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;
            webView.LoadUrl("http://kb.weblayer.com.br/w-transportador-express/");
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

        public class ExtendWebViewClient : WebViewClient
        {
            private Context contextForDialog = null;
            ProgressDialog pd = null;

            public void SetContextForDialog(Context _context)
            {
                contextForDialog = _context;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);

                //pd = new ProgressDialog(contextForDialog);
                //pd.SetTitle("Aguarde...");
                //pd.SetMessage("Conteúdo sendo carregado...");
                //pd.Show();
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);

                //if (pd.IsShowing)
                //    pd.Dismiss();
                return true;
            }

            public override void OnPageFinished(WebView view, string url)
            {
                //if (pd.IsShowing)
                //    pd.Dismiss();
            }
        }
    }
}