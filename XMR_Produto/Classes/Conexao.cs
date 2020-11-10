using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace XMR_Produto.Classes
{
    public class Conexao
    {
        #region Variáveis

        private static string _strConexao = "Server=192.168.15.10;Database=turman11;Uid=admin;Pwd=123";

        public MySqlConnection conexao = new MySqlConnection(_strConexao);
        public MySqlCommand comando;
        public MySqlDataAdapter da;
        public MySqlDataReader dr;
        public DataSet ds;

        #endregion

        #region Construtores

        public Conexao(string query)
        {
            comando = new MySqlCommand(query, conexao);
            da = new MySqlDataAdapter(query, conexao);
            ds = new DataSet();
        }

        #endregion

        #region Métodos

        public void AbreConexao()
        {
            if (conexao.State == ConnectionState.Open)
            {
                conexao.Close();
            }
            conexao.Open();
        }

        public void FechaConexao()
        {
            if (conexao.State == ConnectionState.Open)
            {
                conexao.Close();
            }
        }

        #endregion

    }
}