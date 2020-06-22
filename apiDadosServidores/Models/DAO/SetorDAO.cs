using apiDadosServidores.Models.Entidade;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiDadosServidores.Models.DAO
{
    class SetorDAO
    {
        public List<Cargos> obterDadosSetores(string nomeSetor)
        {
            nomeSetor = nomeSetor.ToUpper();
            nomeSetor = nomeSetor.Replace(' ', '%');
            List<Cargos> resultado = new List<Cargos>();
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);
            String sqlSelect =
                " SELECT DISTINCT " +
                " gg_local.codigo_local, " +
                " gg_local.nome_local";
            String sqlFrom =
                " FROM " +
                " gg_local  " +
                " INNER JOIN vw_fl_funcionario ON " +
                " vw_fl_funcionario.codigo_local = gg_local.codigo_local ";
            String sqlWhere = " WHERE vw_fl_funcionario.data_demissao IS NULL " +
                "and gg_local.nome_local != 'AUXILIO SAUDE INATIVOS'" +
                "and gg_local.nome_local != 'CEDIDO OUTRO ÓRGÃO'" +
                "and gg_local.nome_local != 'FUNC.SEM SETOR'" +
                "and gg_local.nome_local != 'IPDE - INATIVOS'" +
                "and gg_local.nome_local != 'Processo Aposentadoria'" +
                "and gg_local.nome_local != 'TRE/ES'";
            if (!string.IsNullOrEmpty(nomeSetor)) sqlWhere += " and gg_local.nome_local like @NOMESETOR";
                String sqlOrder =
                " ORDER BY gg_local.nome_local  ";


            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            String sql = sqlSelect + sqlFrom + sqlWhere + sqlOrder;
            try
            {
                conexao.Open();


                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);

                command.Parameters.Add("@NOMESETOR", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters["@NOMESETOR"].Value = "%" + nomeSetor + "%";
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        resultado.Add(new Cargos()
                        {
                            codigo = reader["codigo_local"].ToString(),
                            nome = reader["nome_local"].ToString(),

                        });
                    }
                    reader.Close();
                }
                conexao.Close();

            }
            catch (NpgsqlException ex) { }

            return resultado;
        }
    }
}
