using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apiDadosServidores.Models.Entidade;
using apiDadosServidores.Models.DAO;
using System.Web.Http;

namespace apiDadosServidores.Controller
{
    public class ConsultaServidorController : ApiController
    {

        [HttpGet]
        public object consultaDadosServidor(string txPesquisa)
        {
            try
            {
                List<Servidores> servidores = new List<Servidores>();
                Int32.Parse(txPesquisa);
                string txPesquisaAux = txPesquisa.Substring(0, 6);
                List<Servidores> servidoresAux = new ServidoresDAO().ObterDadosServidor(txPesquisaAux);
                foreach (Servidores servidor in servidoresAux)
                {
                    if (servidor.matriculaVinculo == txPesquisa)
                    {
                        servidores.Add(servidor);
                        return servidores;
                    }
                }
                return servidoresAux;

            }
            catch (Exception e)
            {
                List<Servidores> servidores = new ServidoresDAO().ObterDadosServidor(txPesquisa);
                return servidores;
            }
        }
        [HttpGet]
        public object consultaDadosPeloCargo(string regime= "", string padraoSalarial = "", string cargaHoraria = "", string cargo = "", string setor = "", string dataAdmissao = "", string dataDemissao = "", string count= "")
        {
            if(string.IsNullOrEmpty(count))
            {
                List<Servidores> servidoresAux = new ServidoresDAO().ObterDadosServidorPeloCargo(regime, padraoSalarial, cargaHoraria, cargo, setor, dataAdmissao, dataDemissao);
                return servidoresAux;
            }else
            {
                int qntidade = new ServidoresDAO().ObterQuantidadeServidor(regime, padraoSalarial, cargaHoraria, cargo, setor, dataAdmissao, dataDemissao);
                return qntidade;
            }

        }

    }
}
