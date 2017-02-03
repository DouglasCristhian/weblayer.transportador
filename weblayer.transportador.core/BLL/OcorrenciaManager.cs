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
using weblayer.transportador.core.Model;
using weblayer.transportador.core.DAL;

namespace weblayer.transportador.core.BLL
{
    class OcorrenciaManager
    {
        public List<Ocorrencia> GetOcorrencia()
        {
            return new OcorrenciaRepository().List();
        }
    }
}