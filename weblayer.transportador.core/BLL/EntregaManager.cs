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
using weblayer.transportador.core.DAL;
using weblayer.transportador.core.Model;

namespace weblayer.transportador.core.BLL
{
    public class EntregaManager
    {
        public string mensagem;

        public List<Entrega> GetEntrega()
        {
            return new EntregaRepository().List();
        }

        public void Save(Entrega obj)
        {
            var erros = "";

            if (obj.ds_NFE.Length < 5)
                erros = erros + "\n O c�digo da nota � inv�lido! Ele deve ter, no m�nimo, 6 caracteres";

            var Repository = new EntregaRepository();
            Repository.Save(obj);

            mensagem = $"Entrega {obj.id} atualizada com sucesso";
        }

        public void Delete(Entrega obj)
        {
            var Repository = new EntregaRepository();
            Repository.Delete(obj);

            mensagem = $"Entrega {obj.id} exclu�da com sucesso";
        }
    }
}