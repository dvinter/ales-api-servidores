using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiDadosServidores.Models.Entidade
{
    public class Legislacao
    {
        public string idNormaLegislativa { get; set; } //Código da norma (Lei, ato, etc)

        public string txTipoLegislacao { get; set; }

        public string strNumero { get; set; }

        public string strAno { get; set; }

        public string strEmenta { get; set; }

        public string strAnexo { get; set; }


    }
}
