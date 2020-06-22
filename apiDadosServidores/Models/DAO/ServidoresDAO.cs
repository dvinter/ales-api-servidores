using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Data.SqlClient;
using apiDadosServidores.Models.Entidade;


namespace apiDadosServidores.Models.DAO
{
    public class ServidoresDAO
    {
        // OK
        public List<Servidores> ObterDadosServidor(string txPesquisa)
        {
            List<Servidores> resultado = new List<Servidores>();
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);

            //Caso não seja informado nenhum texto para pesquisa
            //Força a pesquisa a retornar vazio otimizando o tempo de busca.
            if (string.IsNullOrEmpty(txPesquisa)) txPesquisa = "999999999999";


            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            txPesquisa = txPesquisa.ToUpper();
            txPesquisa = txPesquisa.Replace(' ', '%');
            String sql =

                               " SELECT DISTINCT " + Environment.NewLine +

                    "  vw_fl_funcionario.codigo_cadfu, " + Environment.NewLine +
                    " (substring((vw_fl_funcionario.codigo_cadfu),1,6)) AS matricula, " + Environment.NewLine +
                    "  vw_fl_funcionario.nome_g, " + Environment.NewLine +
                    "  gg_codau.nome_codau as regime, " + Environment.NewLine +
                    "  vw_padrao.nomepad as padrao, " + Environment.NewLine +
                    "  substring((fl_horario.horas_diarias),2,4) as carga_horaria, " + Environment.NewLine +
                    "  gg_profissoes.nome_profi as cargo, " + Environment.NewLine +
                    "  gg_local.nome_local as setor, " + Environment.NewLine +
                    "  to_char(vw_fl_funcionario.data_admissao, 'DD/MM/YYYY')as data_admissao, " + Environment.NewLine +
                    "  to_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')as data_demissao " + Environment.NewLine +

                    "  from vw_fl_funcionario " + Environment.NewLine +

                    "  inner join gg_local on " + Environment.NewLine +
                    "  gg_local.codigo_local = vw_fl_funcionario.codigo_local " + Environment.NewLine +
                    "  inner join gg_profissoes on " + Environment.NewLine +
                    "  gg_profissoes.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_profissoes.codigo_profi = vw_fl_funcionario.codigo_cargo " + Environment.NewLine +
                    "  inner join gg_codau on " + Environment.NewLine +
                    "  gg_codau.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_codau.codigo_tipco = vw_fl_funcionario.codigo_tipco_regime and " + Environment.NewLine +
                    "  gg_codau.codigo_codau = vw_fl_funcionario.codigo_codau_regime " + Environment.NewLine +
                    "  inner join vw_padrao on " + Environment.NewLine +
                    "  vw_padrao.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  vw_padrao.codigo_fil = vw_fl_funcionario.codigo_fil and " + Environment.NewLine +
                    "  vw_padrao.codigo_padrao = vw_fl_funcionario.codigo_padrao " + Environment.NewLine +
                    "  inner join fl_horario on " + Environment.NewLine +
                    "  fl_horario.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  fl_horario.codigo_horar = vw_fl_funcionario.codigo_horar " + Environment.NewLine +

                    "  where (( to_ascii(vw_fl_funcionario.nome_g) Like to_ascii(@LikeTxPesquisa) OR  substring(vw_fl_funcionario.codigo_cadfu,1,6) = @txPesquisa)) " + Environment.NewLine +
                    "  and (vw_fl_funcionario.data_demissao is null OR vw_fl_funcionario.data_demissao > '01/01/2012') " + Environment.NewLine +
                    "  order by vw_fl_funcionario.nome_g ";

            try
            {
                conexao.Open();


                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);

                command.Parameters.Add("@LikeTxPesquisa", NpgsqlTypes.NpgsqlDbType.Varchar);
                command.Parameters.Add("@txPesquisa", NpgsqlTypes.NpgsqlDbType.Varchar);

                command.Parameters["@LikeTxPesquisa"].Value = "%" + txPesquisa + "%";
                command.Parameters["@txPesquisa"].Value = txPesquisa;


                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        resultado.Add(new Servidores()
                        {
                            nomeServidor = reader["nome_g"].ToString(),
                            matriculaVinculo = reader["codigo_cadfu"].ToString(),
                            cargo = reader["cargo"].ToString(),
                            setor = reader["setor"].ToString(),
                            regime = reader["regime"].ToString(),
                            padraoSalarial = reader["padrao"].ToString(),
                            cargaHoraria = reader["carga_horaria"].ToString(),
                            dataAdmissao = reader["data_admissao"].ToString(),
                            dataDemissao = reader["data_demissao"].ToString()

                        });
                    }
                    reader.Close();
                }
                conexao.Close();

            }
            catch (NpgsqlException ex) { }

            return resultado;
        }
         // OK
        public List<Remuneracao> ObterRemuneracao(string txVinculo)
        {
            List<Remuneracao> resultado = new List<Remuneracao>();
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);

            //Caso não seja informado nenhum texto para pesquisa
            //Força a pesquisa a retornar vazio otimizando o tempo de busca.
            if (string.IsNullOrEmpty(txVinculo)) txVinculo = "999999999999";

            String sql =


" SELECT " + Environment.NewLine +
" codigo_cadfu, nome_g, " + Environment.NewLine +
" DATA_LIBERADO as DATA_ORDENA, " + Environment.NewLine +
" to_char(DATA_LIBERADO, 'DD/MM/YYYY') as DATA_LIBERADO, " + Environment.NewLine +
" MES_REMUNERACAO, " + Environment.NewLine +
" COALESCE(SALARIO_BASE,0) as SALARIO_BASE, " + Environment.NewLine +
" COALESCE(OUTRAS_REMUNERACOES,0) as OUTRAS_REMUNERACOES, " + Environment.NewLine +
" COALESCE(IMPOSTO_RENDA,0) as IMPOSTO_RENDA, " + Environment.NewLine +
" COALESCE(DESCONTO_PREVIDENCIA,0) as DESCONTO_PREVIDENCIA , " + Environment.NewLine +
" COALESCE(OUTRAS_DEDUCOES,0) as OUTRAS_DEDUCOES, " + Environment.NewLine +
" (COALESCE(SALARIO_BASE, 0) + COALESCE(OUTRAS_REMUNERACOES, 0) - COALESCE(IMPOSTO_RENDA, 0) - COALESCE(DESCONTO_PREVIDENCIA, 0) - COALESCE(OUTRAS_DEDUCOES, 0)) AS VALOR_LIQUIDO " + Environment.NewLine +
" FROM ( " + Environment.NewLine +
"         SELECT " + Environment.NewLine +
"         codigo_cadfu, " + Environment.NewLine +
"         nome_g, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT inicio_publicacao FROM dti_fl_transparencia tbl_data " + Environment.NewLine +
"                where tbl_data.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   tbl_data.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_data.inicio_publicacao " + Environment.NewLine +
"         ) as DATA_LIBERADO, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT ( mes || '/' || ano) FROM dti_fl_transparencia tbl_mes_ano " + Environment.NewLine +
"                where tbl_mes_ano.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   tbl_mes_ano.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_mes_ano.inicio_publicacao, tbl_mes_ano.mes, tbl_mes_ano.ano " + Environment.NewLine +
"         ) as MES_REMUNERACAO, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT sum(valor) FROM dti_fl_transparencia tbl_salario_base " + Environment.NewLine +
"                where tbl_salario_base.codigo_cadfu  = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   id_visao_campo = 'A7DBC70FAAF5C5DDE2016A47F1C347A9' " + Environment.NewLine +
"                and   tbl_salario_base.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_salario_base.inicio_publicacao " + Environment.NewLine +
"         ) AS SALARIO_BASE, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT sum(valor) FROM dti_fl_transparencia tbl_outras_remuneracoes " + Environment.NewLine +
"                where tbl_outras_remuneracoes.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                /* *** Quando ID Visão for Outras Remunerações, Folha Complementar Salário base ou Folha Complementar Outras Remunerações *** */ " + Environment.NewLine +
"                and   id_visao_campo in ('2DD33B0D45B1BC8E525249A0B0D4E805','A57A5F804D32E9307F60AB400E7D29E6','EA5E41E968465E9CFB3D3BE218BF2198') " + Environment.NewLine +
"                and   tbl_outras_remuneracoes.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                and   codigo_vende <> '02922' /* ***NÃO CONTABILIZA O TICKET PAGO EM CARTÃO*** */ " + Environment.NewLine +
"                group by tbl_outras_remuneracoes.inicio_publicacao " + Environment.NewLine +
"         ) AS OUTRAS_REMUNERACOES, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT sum(valor) FROM dti_fl_transparencia tbl_imposto_renda " + Environment.NewLine +
"                where tbl_imposto_renda.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   id_visao_campo = '43855ADDF576B9E3D303B187B4E43CDD' " + Environment.NewLine +
"                and   tbl_imposto_renda.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_imposto_renda.inicio_publicacao " + Environment.NewLine +
"         ) AS IMPOSTO_RENDA, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT sum(valor) FROM dti_fl_transparencia tbl_previdencia " + Environment.NewLine +
"                where tbl_previdencia.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   id_visao_campo = 'C9494F2F4C4505CCA288B2D34382B11B' " + Environment.NewLine +
"                and   tbl_previdencia.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_previdencia.inicio_publicacao " + Environment.NewLine +
"         ) AS DESCONTO_PREVIDENCIA, " + Environment.NewLine +
"         ( " + Environment.NewLine +
"         SELECT sum(valor) FROM dti_fl_transparencia tbl_outras_deducoes " + Environment.NewLine +
"                where tbl_outras_deducoes.codigo_cadfu = dti_fl_transparencia.codigo_cadfu " + Environment.NewLine +
"                and   id_visao_campo in ('34A620BDB23903DE58EF100E0BD5E0B9','56F62E681A4056204A3E71F9D9D503BC') " + Environment.NewLine +
"                and   tbl_outras_deducoes.inicio_publicacao = dti_fl_transparencia.inicio_publicacao " + Environment.NewLine +
"                group by tbl_outras_deducoes.inicio_publicacao " + Environment.NewLine +
"         ) AS OUTRAS_DEDUCOES " + Environment.NewLine +
"         FROM dti_fl_transparencia " + Environment.NewLine +
"         where " + Environment.NewLine +
"         codigo_cadfu = @txVinculo " + Environment.NewLine +
"         group by codigo_cadfu, nome_g, inicio_publicacao, ano, mes " + Environment.NewLine +
"         order by ano desc, mes desc " + Environment.NewLine +
 ") TAB01 ";
            try
            {
                conexao.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);

                command.Parameters.Add("@txVinculo", NpgsqlTypes.NpgsqlDbType.Varchar);

                command.Parameters["@txVinculo"].Value = txVinculo;


                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    int count = reader.FieldCount;
                    while (reader.Read())
                    {

                        resultado.Add(new Remuneracao()
                        {

                            matriculaVinculo = reader["codigo_cadfu"].ToString(),
                            nomeServidor = reader["nome_g"].ToString(),
                            mesRemuneracao = reader["MES_REMUNERACAO"].ToString(),
                            salarioBase = reader["SALARIO_BASE"].ToString(),
                            outrasRemuneracoes = reader["OUTRAS_REMUNERACOES"].ToString(),
                            impostoRenda = reader["IMPOSTO_RENDA"].ToString(),
                            descontoPrevidencia = reader["DESCONTO_PREVIDENCIA"].ToString(),
                            outrasDeducoes = reader["OUTRAS_DEDUCOES"].ToString(),
                            valorLiquido = reader["VALOR_LIQUIDO"].ToString()

                        });

                    }
                    reader.Close();
                }
                conexao.Close();

            }
            catch (NpgsqlException ex) { }

            return resultado;
        }
        public int ObterQuantidadeServidor(string regime = "", string padraoSalarial = "", string cargaHoraria = "", string cargo = "", string setor = "", string dataAdmissao = "", string dataDemissao = "")
        {
            int resultado = 0 ;
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);


            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            String sqlWhere = "  where (vw_fl_funcionario.data_demissao is null OR vw_fl_funcionario.data_demissao > '01/01/2012') and gg_local.nome_local != 'AUXILIO SAUDE INATIVOS' " + Environment.NewLine;
            String sql =

            " SELECT COUNT(DISTINCT vw_fl_funcionario.codigo_cadfu) " + Environment.NewLine +
                    "  from vw_fl_funcionario " + Environment.NewLine +

                    "  inner join gg_local on " + Environment.NewLine +
                    "  gg_local.codigo_local = vw_fl_funcionario.codigo_local " + Environment.NewLine +
                    "  inner join gg_profissoes on " + Environment.NewLine +
                    "  gg_profissoes.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_profissoes.codigo_profi = vw_fl_funcionario.codigo_cargo " + Environment.NewLine +
                    "  inner join gg_codau on " + Environment.NewLine +
                    "  gg_codau.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_codau.codigo_tipco = vw_fl_funcionario.codigo_tipco_regime and " + Environment.NewLine +
                    "  gg_codau.codigo_codau = vw_fl_funcionario.codigo_codau_regime " + Environment.NewLine +
                    "  inner join vw_padrao on " + Environment.NewLine +
                    "  vw_padrao.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  vw_padrao.codigo_fil = vw_fl_funcionario.codigo_fil and " + Environment.NewLine +
                    "  vw_padrao.codigo_padrao = vw_fl_funcionario.codigo_padrao " + Environment.NewLine +
                    "  inner join fl_horario on " + Environment.NewLine +
                    "  fl_horario.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  fl_horario.codigo_horar = vw_fl_funcionario.codigo_horar " + Environment.NewLine;

            sqlWhere +=
            "  and (@REGIME = '' OR UPPER(gg_codau.nome_codau) = UPPER( @REGIME )) " + Environment.NewLine +
            "  and (@PADRAOSALARIAL = '' OR UPPER(vw_padrao.nomepad) = UPPER( @PADRAOSALARIAL )) " + Environment.NewLine +
            "  and (@CARGAHORARIA = '' OR UPPER(substring((fl_horario.horas_diarias),2,4)) = UPPER( @CARGAHORARIA )) " + Environment.NewLine +
            "  and (@CARGO = '' OR UPPER(gg_profissoes.nome_profi) = UPPER( @CARGO )) " + Environment.NewLine +
            "  and (@SETOR = '' OR UPPER(gg_local.nome_local) = UPPER( @SETOR )) " + Environment.NewLine +
            "  and (@DATAADIMISSAO = '' OR UPPER(to_char(vw_fl_funcionario.data_admissao, 'DD/MM/YYYY')) = UPPER( @DATAADIMISSAO )) " + Environment.NewLine +
            "  and (@DATADEMISSAO = '' OR UPPER(to_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')) = UPPER( @DATADEMISSAO ))" + Environment.NewLine +
            "  and (UPPER(to_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')) is null);";
            sql = sql + sqlWhere;
            try
            {
                conexao.Open();


                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);

                command.Parameters.AddWithValue("@REGIME", regime);
                command.Parameters.AddWithValue("@PADRAOSALARIAL", padraoSalarial);
                command.Parameters.AddWithValue("@CARGAHORARIA", cargaHoraria);
                command.Parameters.AddWithValue("@CARGO", cargo);
                command.Parameters.AddWithValue("@SETOR", setor);
                command.Parameters.AddWithValue("@DATAADIMISSAO", dataAdmissao);
                command.Parameters.AddWithValue("@DATADEMISSAO", dataDemissao);


                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    resultado = Int32.Parse(reader["count"].ToString());
                    reader.Close();
                }
                conexao.Close();

            }
            catch (NpgsqlException ex) { }

            return resultado;
        }
    
    public List<Servidores> ObterDadosServidorPeloCargo(string regime="", string padraoSalarial="", string cargaHoraria="", string cargo="", string setor="", string dataAdmissao="", string dataDemissao="")
        {
            List<Servidores> resultado = new List<Servidores>();
            NpgsqlConnection conexao = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDeldadosPostgres"].ConnectionString);


            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            String sqlWhere = "  where (vw_fl_funcionario.data_demissao is null OR vw_fl_funcionario.data_demissao > '01/01/2012') " + Environment.NewLine;
            String sql =

            " SELECT DISTINCT " + Environment.NewLine +

                    "  vw_fl_funcionario.codigo_cadfu, " + Environment.NewLine +
                    " (substring((vw_fl_funcionario.codigo_cadfu),1,6)) AS matricula, " + Environment.NewLine +
                    "  vw_fl_funcionario.nome_g, " + Environment.NewLine +
                    "  gg_codau.nome_codau as regime, " + Environment.NewLine +
                    "  vw_padrao.nomepad as padrao, " + Environment.NewLine +
                    "  substring((fl_horario.horas_diarias),2,4) as carga_horaria, " + Environment.NewLine +
                    "  gg_profissoes.nome_profi as cargo, " + Environment.NewLine +
                    "  gg_local.nome_local as setor, " + Environment.NewLine +
                    "  to_char(vw_fl_funcionario.data_admissao, 'DD/MM/YYYY')as data_admissao, " + Environment.NewLine +
                    "  to_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')as data_demissao " + Environment.NewLine +

                    "  from vw_fl_funcionario " + Environment.NewLine +

                    "  inner join gg_local on " + Environment.NewLine +
                    "  gg_local.codigo_local = vw_fl_funcionario.codigo_local " + Environment.NewLine +
                    "  inner join gg_profissoes on " + Environment.NewLine +
                    "  gg_profissoes.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_profissoes.codigo_profi = vw_fl_funcionario.codigo_cargo " + Environment.NewLine +
                    "  inner join gg_codau on " + Environment.NewLine +
                    "  gg_codau.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  gg_codau.codigo_tipco = vw_fl_funcionario.codigo_tipco_regime and " + Environment.NewLine +
                    "  gg_codau.codigo_codau = vw_fl_funcionario.codigo_codau_regime " + Environment.NewLine +
                    "  inner join vw_padrao on " + Environment.NewLine +
                    "  vw_padrao.codigo_emp = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  vw_padrao.codigo_fil = vw_fl_funcionario.codigo_fil and " + Environment.NewLine +
                    "  vw_padrao.codigo_padrao = vw_fl_funcionario.codigo_padrao " + Environment.NewLine +
                    "  inner join fl_horario on " + Environment.NewLine +
                    "  fl_horario.codigo_emp   = vw_fl_funcionario.codigo_emp and " + Environment.NewLine +
                    "  fl_horario.codigo_horar = vw_fl_funcionario.codigo_horar " + Environment.NewLine;

            sqlWhere +=
            "  and (@REGIME = '' OR UPPER(gg_codau.nome_codau) = UPPER( @REGIME )) " + Environment.NewLine +
            "  and (@PADRAOSALARIAL = '' OR UPPER(vw_padrao.nomepad) = UPPER( @PADRAOSALARIAL )) " + Environment.NewLine +
            "  and (@CARGAHORARIA = '' OR UPPER(substring((fl_horario.horas_diarias),2,4)) = UPPER( @CARGAHORARIA )) " + Environment.NewLine +
            "  and (@CARGO = '' OR UPPER(gg_profissoes.nome_profi) = UPPER( @CARGO )) " + Environment.NewLine +
            "  and (@SETOR = '' OR UPPER(gg_local.nome_local) = UPPER( @SETOR )) " + Environment.NewLine +
            "  and (@DATAADIMISSAO = '' OR UPPER(to_char(vw_fl_funcionario.data_admissao, 'DD/MM/YYYY')) = UPPER( @DATAADIMISSAO )) " + Environment.NewLine +
            "  and (@DATADEMISSAO = '' OR UPPER(to_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')) = UPPER( @DATADEMISSAO ))";

            string sqlOrder =

            "  order by vw_fl_funcionario.nome_g ;";
            sql = sql + sqlWhere + sqlOrder;
            try
            {
                conexao.Open();


                NpgsqlCommand command = new NpgsqlCommand(sql, conexao);

                command.Parameters.AddWithValue("@REGIME", regime);
                command.Parameters.AddWithValue("@PADRAOSALARIAL", padraoSalarial);
                command.Parameters.AddWithValue("@CARGAHORARIA", cargaHoraria);
                command.Parameters.AddWithValue("@CARGO", cargo);
                command.Parameters.AddWithValue("@SETOR", setor);
                command.Parameters.AddWithValue("@DATAADIMISSAO", dataAdmissao);
                command.Parameters.AddWithValue("@DATADEMISSAO", dataDemissao);


                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        resultado.Add(new Servidores()
                        {
                            nomeServidor = reader["nome_g"].ToString(),
                            matriculaVinculo = reader["codigo_cadfu"].ToString(),
                            cargo = reader["cargo"].ToString(),
                            setor = reader["setor"].ToString(),
                            regime = reader["regime"].ToString(),
                            padraoSalarial = reader["padrao"].ToString(),
                            cargaHoraria = reader["carga_horaria"].ToString(),
                            dataAdmissao = reader["data_admissao"].ToString(),
                            dataDemissao = reader["data_demissao"].ToString()

                        });
                    }
                    reader.Close();
                }
                conexao.Close();

            }
            catch (NpgsqlException ex) { }

            return resultado;
        }
        // OK


    }
}