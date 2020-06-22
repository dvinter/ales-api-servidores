using apiDadosServidores.Models.DAO;
using apiDadosServidores.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace apiDadosServidores.Controller
{
    public class ConsultaCargosController : ApiController
    {
        [HttpGet]
        public object consultaCargosExistentes(string nome="")
        {
            List<Cargos> cargos = new CargoDAO().obterDadosSetores(nome);
            return cargos;
        }
    }
}
