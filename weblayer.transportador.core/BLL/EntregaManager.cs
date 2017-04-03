using System.Collections.Generic;
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

        public List<Entrega> GetEntregaFiltro(int dataEmissao)
        {
            return new EntregaRepository().ListFiltro(dataEmissao);
        }

        public void Save(Entrega obj)
        {
            var erros = "";

            if (obj.ds_NFE.Length < 5)
                erros = erros + "\n O código da nota é inválido! Ele deve ter 44 caracteres";

            var Repository = new EntregaRepository();
            Repository.Save(obj);

            mensagem = $"Ocorrência atualizada com sucesso";
        }

        public void Delete(Entrega obj)
        {
            var Repository = new EntregaRepository();
            Repository.Delete(obj);

            mensagem = $"Ocorrência excluída com sucesso";
        }
    }
}