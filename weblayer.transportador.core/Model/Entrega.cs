using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace weblayer.transportador.core.Model
{
    [Table("Entrega")]
    public class Entrega
    {
        [PrimaryKey, AutoIncrement]
        public virtual int id
        { get; set; }

        [MaxLength(200), NotNull]
        public virtual string ds_NFE
        { get; set; }

        [NotNull]
        public virtual int id_ocorrencia
        { get; set; }

        [NotNull]
        public virtual DateTime? dt_inclusao
        { get; set; }

        [NotNull]
        public virtual DateTime? dt_entrega
        { get; set; }

        [MaxLength(200)]
        public virtual string ds_observacao
        { get; set; }

        [MaxLength(200)]
        public virtual string ds_geolocalizacao
        { get; set; }

        public byte[] Image { get; set; }

        [MaxLength(400)]
        public virtual string ds_ImageUri
        { get; set; }

        public int fl_status { get; set; }
    }
}
