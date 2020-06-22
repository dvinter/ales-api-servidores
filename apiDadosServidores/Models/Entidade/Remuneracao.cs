using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiDadosServidores.Models.Entidade
{
    public class Remuneracao
    {
        //DADOS DA REMUNERAÇÃO
        public string matriculaVinculo { get; set; }
        public string nomeServidor { get; set; }
        public string mesRemuneracao { get; set; }
        public string salarioBase { get; set; }
        public string outrasRemuneracoes { get; set; }
        public string impostoRenda { get; set; }
        public string descontoPrevidencia { get; set; }
        public string outrasDeducoes { get; set; }
        public string valorLiquido { get; set; }

    }
}
