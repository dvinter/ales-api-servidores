using apiDadosServidores.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace apiDadosServidores.Controller
{
    public class ConsultaRemuneracaoController : ApiController
    {
        [HttpGet]
        public object consultaRemuneracaoPelaMatricula(string txPesquisa)
        {
            string matricula = txPesquisa;
            return new ServidoresDAO().ObterRemuneracao(matricula);
            
        }
    }
}
