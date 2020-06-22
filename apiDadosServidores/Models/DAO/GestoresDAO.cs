using apiDadosServidores.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace apiDadosServidores.Models.DAO
{
    class GestoresDAO
    {
        public List<Gestores> CarregarGestores(string cargo)
        {
            List<Gestores> resultado = new List<Gestores>();

            using (SqlConnection conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["conexaoVitoria"].ConnectionString))
            {
                String sql =


"	SELECT site_gestor_cargo.id_gestor_cargo  " + Environment.NewLine +
"      ,site_gestor_cargo.ds_cargo  " + Environment.NewLine +
"	  , ISNULL((select B.nm_pessoa  " + Environment.NewLine +
"			from site_gestor_ocupante A  " + Environment.NewLine +
"			LEFT JOIN ALE_PESSOA B   " + Environment.NewLine +
"			ON	B.id_pessoa = A.id_pessoa   " + Environment.NewLine +
"			where A.id_cargo = site_gestor_cargo.id_gestor_cargo  " + Environment.NewLine +
"			and A.dt_inicio <= cast(floor(cast(getdate() as float)) as datetime)  " + Environment.NewLine +
"			and (A.dt_fim > cast(floor(cast(getdate() as float)) as datetime) OR A.dt_fim is NULL)  " + Environment.NewLine +
"			and  A.id_gestor_ocupante = (SELECT MAX(C.id_gestor_ocupante)   " + Environment.NewLine +
"											FROM site_gestor_ocupante C  " + Environment.NewLine +
"											where C.id_cargo = site_gestor_cargo.id_gestor_cargo  " + Environment.NewLine +
"											and C.dt_inicio <= cast(floor(cast(getdate() as float)) as datetime)  " + Environment.NewLine +
"											and (C.dt_fim > cast(floor(cast(getdate() as float)) as datetime) OR C.dt_fim is NULL)  )), '') AS nm_pessoa  " + Environment.NewLine +
"      ,site_gestor_cargo.tipo_cargo  " + Environment.NewLine +
"      ,site_gestor_cargo.ic_ativo  " + Environment.NewLine +
"      ,site_gestor_cargo.id_setor  " + Environment.NewLine +
"      ,SPL_SETOR.tx_email_setor  " + Environment.NewLine +
"	   ,SPL_SETOR.tx_funcionamento  " + Environment.NewLine +
"	   ,SPL_SETOR.tx_telefone  " + Environment.NewLine +
"  FROM site_gestor_cargo  " + Environment.NewLine +

" INNER JOIN SPL_SETOR ON	 " + Environment.NewLine +
" SPL_SETOR.id_setor = SITE_GESTOR_CARGO.id_setor  ";
                String sqlWhere = "";
                if (!String.IsNullOrEmpty(cargo))
                {
                    sqlWhere = " where site_gestor_cargo.ds_cargo = @CARGO ";
                }
        String sqlOrder = "  order by site_gestor_cargo.id_gestor_cargo ";
                sql += sqlWhere + sqlOrder;


                //"    SELECT  " + Environment.NewLine +
                //"          site_gestor_ocupante.id_gestor_ocupante " + Environment.NewLine +
                //"    	  ,SITE_GESTOR_CARGO.tipo_cargo " + Environment.NewLine +
                //"    	  ,SITE_GESTOR_CARGO.ds_cargo " + Environment.NewLine +
                //"    	  ,ALE_PESSOA.nm_pessoa " + Environment.NewLine +
                //"         ,site_gestor_ocupante.id_cargo " + Environment.NewLine +
                //"         ,site_gestor_ocupante.id_pessoa " + Environment.NewLine +
                //"         ,site_gestor_ocupante.nr_matricula " + Environment.NewLine +
                //"         ,site_gestor_ocupante.dt_inicio " + Environment.NewLine +
                //"         ,site_gestor_ocupante.dt_fim " + Environment.NewLine +
                //"    	  ,SITE_GESTOR_CARGO.id_setor " + Environment.NewLine +
                //"         ,SPL_SETOR.tx_email_setor " + Environment.NewLine +
                //"         ,SPL_SETOR.tx_funcionamento " + Environment.NewLine +
                //"         ,SPL_SETOR.tx_telefone " + Environment.NewLine +
                //"         ,site_gestor_ocupante.tx_observacao " + Environment.NewLine +

                //"     FROM site_gestor_ocupante " + Environment.NewLine +

                //"    	  INNER JOIN ALE_PESSOA ON	 " + Environment.NewLine +
                //"    	  ALE_PESSOA.id_pessoa = site_gestor_ocupante.id_pessoa " + Environment.NewLine +

                //"    	  INNER JOIN SITE_GESTOR_CARGO ON	 " + Environment.NewLine +
                //"    	  SITE_GESTOR_CARGO.id_gestor_cargo = site_gestor_ocupante.id_cargo " + Environment.NewLine +

                //"    	  INNER JOIN SPL_SETOR ON	 " + Environment.NewLine +
                //"    	  SPL_SETOR.id_setor = SITE_GESTOR_CARGO.id_setor " + Environment.NewLine +

                //"    	WHERE " + Environment.NewLine +
                //"           dt_fim is null " + Environment.NewLine +
                //"    	and dt_inicio < GETDATE() " + Environment.NewLine +

                //"    	ORDER BY  " + Environment.NewLine +
                //"    		SITE_GESTOR_CARGO.tipo_cargo DESC, SITE_GESTOR_CARGO.ds_cargo ";

                SqlDataReader reader = null;

                try
                {

                    // Abre a conexão
                    conexao.Open();

                    // Passa a conexão para o objeto do comando
                    SqlCommand cmd = new SqlCommand(sql, conexao);
                    cmd.Parameters.AddWithValue("@CARGO", cargo);
                    // Obtém a consulta com os resultados
                    using (reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                resultado.Add(new Gestores()
                                {
                                    //id_gestor_ocupante = Convert.ToInt32(reader["id_gestor_ocupante"]),
                                    tipo_cargo = reader["tipo_cargo"].ToString(),
                                    ds_cargo = reader["ds_cargo"].ToString(),
                                    //id_cargo = Convert.ToInt16(reader["id_cargo"]),
                                    nm_pessoa = reader["nm_pessoa"].ToString(),
                                    //nr_matricula = reader["nr_matricula"].ToString(),
                                    //dt_inicio = Convert.ToDateTime(reader["dt_inicio"]),
                                    //dt_fim = reader["dt_fim"] == DBNull.Value ? default(DateTime?) : Convert.ToDateTime(reader["dt_fim"]),
                                    //tx_observacao = reader["tx_observacao"].ToString(),
                                    tx_email_setor = reader["tx_email_setor"].ToString(),
                                    tx_funcionamento = reader["tx_funcionamento"].ToString(),
                                    tx_telefone = reader["tx_telefone"].ToString(),
                                    id_setor = reader["id_setor"].ToString()

                                });
                            }
                            catch (Exception e) { string erro = e.Message; }

                        }

                        reader.Close();
                    }
                    conexao.Close();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (conexao != null)
                        conexao.Close();
                }

            }

            return (resultado);

        }
    }
}
