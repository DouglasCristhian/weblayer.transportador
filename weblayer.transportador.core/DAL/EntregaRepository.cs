using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.transportador.core.Model;

namespace weblayer.transportador.core.DAL
{
    [Activity(Label = "EntregaRepository")]
    public class EntregaRepository
    {
        public string Mensage { get; set; }

        public Entrega Get(int id)
        {
            return Database.GetConnection().Table<Entrega>().Where(x => x.id == id).FirstOrDefault();
        }

        public Entrega GetImage(int id, byte[] imagem)
        {
            return Database.GetConnection().Table<Entrega>().Where(x => x.id == id && x.Image == imagem).FirstOrDefault();
        }

        public void Save(Entrega entidade)
        {
            try
            {
                if (entidade.id > 0)
                    Database.GetConnection().Update(entidade);
                else
                    Database.GetConnection().Insert(entidade);
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(Entrega entidade)
        {
            Database.GetConnection().Delete(entidade);
        }

        public List<Entrega> List()
        {
            return Database.GetConnection().Table<Entrega>().ToList();
        }

        public List<Entrega> ListFiltro(int dataEmissao)
        {
            DateTime intervalo_inicio = DateTime.Today;
            DateTime intervalo_fim = DateTime.Today;
            IEnumerable<Entrega> result_data;

            if (dataEmissao != 0)
            {
                #region FiltroData
                if (dataEmissao == 1)
                {
                    intervalo_inicio = DateHelper.GetStartOfDay(DateTime.Today);
                    intervalo_fim = DateHelper.GetEndOfDay(DateTime.Today);
                }

                if (dataEmissao == 2)
                {
                    intervalo_inicio = DateHelper.GetStartOfCurrentWeek();
                    intervalo_fim = DateHelper.GetEndOfCurrentWeek();

                    //result_data = from m in Database.GetConnection().Table<Entrega>().Where(x => x.dt_entrega >= intervalo_inicio && x.dt_entrega <= intervalo_fim) select m;
                }

                if (dataEmissao == 3)
                {
                    intervalo_inicio = DateHelper.GetStartOfCurrentMonth();
                    intervalo_fim = DateHelper.GetEndOfCurrentMonth();
                }

                result_data = from m in Database.GetConnection().Table<Entrega>().
                          Where(x => x.dt_inclusao >= intervalo_inicio && x.dt_inclusao <= intervalo_fim)
                              select m;
            }
            else
            {
                result_data = from m in Database.GetConnection().Table<Entrega>().ToList() select m;
            }

            return result_data.ToList();
            #endregion

            //return Database.GetConnection().Table<Entrega>().ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Entrega { ds_NFE = "35160903703339000142550000000594611002684151", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/02/05"), dt_entrega = DateTime.Parse("2017/02/05"), ds_observacao = "Tudo certo", fl_status = 0 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 2, dt_inclusao = DateTime.Parse("2017/01/10"), dt_entrega = DateTime.Parse("2017/01/10"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160903703339000142550000000594611002684151", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/03/01"), dt_entrega = DateTime.Parse("2017/03/17"), ds_observacao = "Tudo certo", fl_status = 0 });
            Save(new Entrega { ds_NFE = "35160903703339000142550000000594611002684151", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/03/07"), dt_entrega = DateTime.Parse("2017/03/16"), ds_observacao = "Tudo certo", fl_status = 0 });
            Save(new Entrega { ds_NFE = "35160903703339000142550000000594611002684151", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/03/08"), dt_entrega = DateTime.Parse("2017/03/18"), ds_observacao = "Tudo certo", fl_status = 0 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 2, dt_inclusao = DateTime.Parse("2017/03/13"), dt_entrega = DateTime.Parse("2017/03/21"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 2, dt_inclusao = DateTime.Parse("2017/04/22"), dt_entrega = DateTime.Parse("2017/04/27"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 2, dt_inclusao = DateTime.Parse("2017/01/18"), dt_entrega = DateTime.Parse("2017/02/18"), ds_observacao = "Tudo errado", fl_status = 1 });
        }
    }
}
