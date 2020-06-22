using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiDadosServidores.Models.Entidade
{
    public class Servidores
    {
        //DADOS DO SERVIDOR
        public string matriculaVinculo { get; set; }
        public string nomeServidor { get; set; }
        public string regime { get; set; }
        public string padraoSalarial { get; set; }
        public string cargaHoraria { get; set; }
        public string cargo { get; set; }
        public string setor { get; set; }
        public string dataAdmissao { get; set; }
        public string dataDemissao { get; set; }


    }
}
