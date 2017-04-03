using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.transportador.core.Model;

namespace weblayer.transportador.android.pro.Adapters
{
    [Activity(Label = "Adapter_EntregaListView")]
    public class Adapter_EntregaListView : BaseAdapter<Entrega>
    {
        public List<Entrega> mItems;
        public Context mContext;
        private string descricaoocorrencia;

        public Adapter_EntregaListView(Context context, List<Entrega> items)
        {
            mItems = items;
            mContext = context;
        }

        public override Entrega this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_Entrega_ListView, null, false);

            descricaoocorrencia = "";
            if (mItems[position].id_ocorrencia == 1)
                descricaoocorrencia = "ENTREGA";
            if (mItems[position].id_ocorrencia == 2)
                descricaoocorrencia = "INFORMATIVO";
            if (mItems[position].id_ocorrencia == 3)
                descricaoocorrencia = "REENTREGA";
            if (mItems[position].id_ocorrencia == 4)
                descricaoocorrencia = "DEVOLUÇÃO";

            row.FindViewById<TextView>(Resource.Id.ds_NFE).Text = "NFe: " + mItems[position].ds_NFE;
            row.FindViewById<TextView>(Resource.Id.id_ocorrencia).Text = "Ocorrência: " + descricaoocorrencia;
            row.FindViewById<TextView>(Resource.Id.dt_inclusao).Text = "Data de Inclusão: " + mItems[position].dt_inclusao.Value.ToString("dd/MM/yyyy HH:mm");
            row.FindViewById<TextView>(Resource.Id.dt_entrega).Text = "Data de Entrega: " + mItems[position].dt_entrega.Value.ToString("dd/MM/yyyy HH:mm");

            if (mItems[position].fl_status == 0)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.Cinza);
            }

            if (mItems[position].fl_status == 1)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.Azul);
            }

            return row;
        }
    }
}