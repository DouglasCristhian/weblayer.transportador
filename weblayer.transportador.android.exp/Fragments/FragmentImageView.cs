
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using UK.CO.Senab.Photoview;
using weblayer.transportador.android.exp.Helpers;

namespace weblayer.transportador.android.exp.Fragments
{
    public class FragmentImageView : DialogFragment
    {
        private ImageView imgView;
        Bitmap imgPassedBitmap;

        static public FragmentImageView newInstance()
        {
            FragmentImageView f = new FragmentImageView();
            return f;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            Activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            var view = inflater.Inflate(Resource.Layout.Fragment_ImageView, container, false);
            this.Dialog.SetCanceledOnTouchOutside(false);
            imgView = view.FindViewById<ImageView>(Resource.Id.img);

            PhotoViewAttacher photoView = new PhotoViewAttacher(imgView);
            photoView.Update();

            byte[] imgPassed = Arguments.GetByteArray("imagem");

            ByteHelper helper = new ByteHelper();
            imgPassedBitmap = helper.ByteArrayToImage(imgPassed);
            imgView.SetImageBitmap(imgPassedBitmap);
            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
        }
    }
}

