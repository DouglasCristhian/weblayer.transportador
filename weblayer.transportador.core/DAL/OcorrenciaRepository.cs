using System.Collections.Generic;
using System.Linq;
using weblayer.transportador.core.Model;

namespace weblayer.transportador.core.DAL
{
    public class OcorrenciaRepository
    {
        public List<Ocorrencia> List()
        {
            return Database.GetConnection().Table<Ocorrencia>().ToList();
        }

        public void Save(Ocorrencia entidade)
        {
            Database.GetConnection().Insert(entidade);
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Ocorrencia() { ds_descricao = "Entrega" });   //1
            Save(new Ocorrencia() { ds_descricao = "Informativo" });  //2
            Save(new Ocorrencia() { ds_descricao = "Reentrega" }); //3
            Save(new Ocorrencia() { ds_descricao = "Devolução" }); //4
        }
    }
}