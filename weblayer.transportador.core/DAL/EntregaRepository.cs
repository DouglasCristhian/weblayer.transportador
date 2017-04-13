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

        public List<Entrega> ListFiltro(int dataInclusao)
        {
            DateTime intervalo_inicio = new DateTime(1900, 01, 01);
            DateTime intervalo_fim = new DateTime(2020, 01, 01);


            if (dataInclusao == 1)
            {
                intervalo_inicio = DateHelper.GetStartOfDay(DateTime.Today);
                intervalo_fim = DateHelper.GetEndOfDay(DateTime.Today);
            }

            if (dataInclusao == 2)
            {
                intervalo_inicio = DateHelper.GetStartOfCurrentWeek();
                intervalo_fim = DateHelper.GetEndOfCurrentWeek();
            }

            if (dataInclusao == 3)
            {
                intervalo_inicio = DateHelper.GetStartOfCurrentMonth();
                intervalo_fim = DateHelper.GetEndOfCurrentMonth();
            }

            var result_data = Database.GetConnection().Query<Entrega>($@"SELECT * FROM Entrega where 
            dt_inclusao>=@intervalo_inicio and dt_inclusao<=@intervalo_fim", intervalo_inicio, intervalo_fim);

            return result_data.OrderBy(x => -x.id).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Entrega { ds_NFE = "35160903703339000142550000000594611002684151", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/04/07"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Tudo certo", fl_status = 0 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 2, dt_inclusao = DateTime.Parse("2017/04/03"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 3, dt_inclusao = DateTime.Parse("2017/03/08"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 4, dt_inclusao = DateTime.Parse("2017/04/02"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 1, dt_inclusao = DateTime.Parse("2017/03/02"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Tudo errado", fl_status = 1 });
            Save(new Entrega { ds_NFE = "35160972456809001709550010000114581062554946", id_ocorrencia = 3, dt_inclusao = DateTime.Parse("2017/03/12"), dt_entrega = DateTime.Parse("2016/05/01"), ds_observacao = "Nada a declarar", fl_status = 0, ds_geolocalizacao = "15,8741688, -53,5272828", });
        }
    }
}
