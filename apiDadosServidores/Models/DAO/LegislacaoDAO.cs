using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using apiDadosServidores.Models.Entidade;


namespace apiDadosServidores.Models.DAO
{
    public class LegislacaoDAO
    {
        public List<Legislacao> ObterLegislacaoPorAssunto(String assuntoNorma)
        {
            List<Legislacao> resultado = new List<Legislacao>();

            String sqlWhere = String.Empty;

            using (SqlConnection conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["conexaoPancas"].ConnectionString))
            {

                if (!String.IsNullOrEmpty(assuntoNorma))
                    sqlWhere += " AND prl_leis.lei_ementa like @ASSUNTO_NORMA COLLATE Latin1_General_CI_AI " + Environment.NewLine;
                else
                    //Para não retornar valor, caso não seja informado um parâmetro
                    sqlWhere += " AND 1 = 2 " + Environment.NewLine;

                String sql =

                        " SELECT DISTINCT  " + Environment.NewLine +
                        "      prl_leis.cod_lei_tipo " + Environment.NewLine +
                        "     ,prl_leis.cod_lei " + Environment.NewLine +
                        " 	  ,prl_leis_tipos.let_nome as tipo_lei " + Environment.NewLine +
                        "     ,lei_numero as numero_lei	    " + Environment.NewLine +
                        " 	  ,(select year(lei_data)) as numero_ano_lei " + Environment.NewLine +
                        "     ,lei_ementa as ementa_lei " + Environment.NewLine +
                        "     ,lei_data as data_publicacao " + Environment.NewLine +
                        "     ,lei_html as nm_html " + Environment.NewLine +
                        " 	  ,lei_compilada as nm_html_compilado " + Environment.NewLine +
                        " 	  ,CONVERT(VARCHAR, prl_proposicoes.pro_numero) + '/' + CONVERT(VARCHAR, prl_proposicoes.pro_ano) AS numeroAnoProposicao " + Environment.NewLine +
                        " 	  ,prl_leis.cod_proposicao " + Environment.NewLine +
                        " 	  ,prl_proposicoes.pro_numero as numeroProposicao " + Environment.NewLine +
                        " 	  ,prl_proposicoes.pro_ano as anoProposicao " + Environment.NewLine +
                        " 	  ,prl_proposicoes.cod_proposicao_tipo " + Environment.NewLine +
                        " 	  ,prl_leis.cod_lei_situacao " + Environment.NewLine +
                        " 	  ,prl_leis_situacoes.les_nome " + Environment.NewLine +
                        "     ,prl_leis.cod_lei_origem " + Environment.NewLine +
                        "     ,lei_situacao_origem " + Environment.NewLine +

                        "   FROM agape.dbo.prl_leis " + Environment.NewLine +

                        "   INNER JOIN prl_leis_tipos ON " + Environment.NewLine +
                        "   prl_leis_tipos.cod_lei_tipo = prl_leis.cod_lei_tipo " + Environment.NewLine +

                        "   LEFT OUTER JOIN prl_leis_autores ON " + Environment.NewLine +
                        "   prl_leis_autores.cod_lei = prl_leis.cod_lei " + Environment.NewLine +

                        "   LEFT OUTER JOIN prl_autores ON " + Environment.NewLine +
                        "   prl_autores.cod_autor = prl_leis_autores.cod_autor " + Environment.NewLine +

                        "   LEFT OUTER JOIN neg_contatos ON " + Environment.NewLine +
                        "   neg_contatos.cod_contato = prl_autores.cod_contato " + Environment.NewLine +

                        "   LEFT OUTER JOIN prl_proposicoes ON " + Environment.NewLine +
                        "   prl_proposicoes.cod_proposicao = prl_leis.cod_proposicao " + Environment.NewLine +

                        "   LEFT OUTER JOIN prl_leis_situacoes ON " + Environment.NewLine +
                        "   prl_leis_situacoes.cod_lei_situacao = prl_leis.cod_lei_situacao " + Environment.NewLine +

                        "   WHERE 0 = 0 " + Environment.NewLine +
                            sqlWhere + Environment.NewLine +

                        " ORDER BY prl_leis_tipos.let_nome, lei_data DESC, lei_numero DESC ";

                SqlDataReader reader = null;

                try
                {
                    // Abre a conexão
                    conexao.Open();

                    // Passa a conexão para o objeto do comando
                    SqlCommand cmd = new SqlCommand(sql, conexao);

                    // Passa os parametros
                    if (!String.IsNullOrEmpty(assuntoNorma))
                        cmd.Parameters.AddWithValue("@ASSUNTO_NORMA", "%" + assuntoNorma + "%");


                    // Usa a Conexão
                    // Obtém a consulta com os resultados
                    using (reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                resultado.Add(new Legislacao()
                                {
                                    txTipoLegislacao = reader["tipo_lei"].ToString(),
                                    strNumero = reader["numero_lei"].ToString(),
                                    strAno = reader["numero_ano_lei"].ToString(),
                                    strEmenta = reader["ementa_lei"].ToString(),
                                    idNormaLegislativa = reader["cod_lei"].ToString(),
                                    strAnexo = "http://www3.al.es.gov.br/" + reader["NM_HTML"].ToString()

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

            return resultado;
        }

    }
}