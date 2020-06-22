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
    public class ConsultaGestoresController : ApiController
    {
        [HttpGet]
        public object consultaSetoresExistentes(string cargo="")
        {
            List<Gestores> gestores = new GestoresDAO().CarregarGestores(cargo);
            return gestores;
        }
    }
}
