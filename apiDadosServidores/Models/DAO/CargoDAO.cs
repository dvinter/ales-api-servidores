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
    class CargoDAO
    {
        public List<Cargos> obterDadosSetores(string nomeCargo)
        {
            nomeCargo = nomeCargo.ToUpper();
            nomeCargo = nomeCargo.Replace(' ', '%');
            List<Cargos> resultado = new List<Cargos>();
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);
            String sqlSelect =
                " SELECT DISTINCT " +
                " gg_profissoes.codigo_profi, " +
                " gg_profissoes.nome_profi ";
            String sqlFrom =
                " FROM " +
                " gg_profissoes " +
                " INNER JOIN vw_fl_funcionario ON " +
                " vw_fl_funcionario.codigo_cargo = gg_profissoes.codigo_profi ";
            String sqlWhere =
                " WHERE vw_fl_funcionario.data_demissao IS NULL " +
                " and gg_profissoes.nome_profi != 'AUXILIO SAUDE INATIVO' " +
                " and gg_profissoes.nome_profi != 'BENEFICIARIOS IPDE' " +
                " and gg_profissoes.nome_profi != 'PENSIONISTAS IPDE' " +
                " and gg_profissoes.nome_profi != 'PM DA RESERVA' "+
                " and gg_profissoes.nome_profi != 'PM DA RESERVA OFICIAL' ";
            if (!string.IsNullOrEmpty(nomeCargo)) sqlWhere += " and gg_profissoes.nome_profi like @NOMECARGO";
            String sqlOrder =
                " ORDER BY gg_profissoes.nome_profi ASC ";


            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            String sql = sqlSelect + sqlFrom + sqlWhere + sqlOrder;
            try
            {
                conexao.Open();


                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);
                command.Parameters.Add("@NOMECARGO", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters["@NOMECARGO"].Value = "%" + nomeCargo + "%";
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        resultado.Add(new Cargos()
                        {
                            codigo = reader["codigo_profi"].ToString(),
                            nome = reader["nome_profi"].ToString(),

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
