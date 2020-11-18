using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XMR_Produto.Classes
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; } = true;


        public static List<Produto> BuscarProdutos()
        {
            string query = string.Format("SELECT Id, Descricao, Preco, Quantidade, Ativo FROM produto WHERE Ativo = 1");
            Conexao cn = new Conexao(query);
            try
            {
                cn.AbreConexao();
                cn.dr = cn.comando.ExecuteReader();
                if (cn.dr.HasRows)
                {
                    List<Produto> produtos = new List<Produto>();
                    while (cn.dr.Read())
                    {
                        produtos.Add(new Produto
                        {
                            Id = Convert.ToInt32(cn.dr[0]),
                            Descricao = cn.dr[1].ToString(),
                            Preco = Convert.ToDecimal(cn.dr[2]),
                            Quantidade = Convert.ToInt32(cn.dr[3]),
                            Ativo = Convert.ToBoolean(cn.dr[4])
                        });
                    }
                    return produtos;
                }
                else
                {
                    return new List<Produto>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.FechaConexao();
            }
        }

        public void Cadastrar()
        {
            string query = string.Format("INSERT INTO produto (Descricao, Preco, Quantidade, Ativo) VALUES ('{0}', {1}, {2}, 1); SELECT LAST_INSERT_ID();", Descricao, Preco, Quantidade);
            Conexao cn = new Conexao(query);
            try
            {
                cn.AbreConexao();
                cn.dr = cn.comando.ExecuteReader();
                if (cn.dr.HasRows)
                {
                    while (cn.dr.Read())
                    {
                        Id = Convert.ToInt32(cn.dr[0]);
                    }
                }
                else
                {
                    throw new Exception("Houve um erro ao recuperar o Id do produto inserido.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.FechaConexao();
            }
        }

        public void Alterar()
        {
            string query = string.Format("UPDATE produto SET Descricao = '{0}', Preco = {1}, Quantidade = {2} WHERE Id = {3}", Descricao, Preco, Quantidade, Id);
            Conexao cn = new Conexao(query);
            try
            {
                cn.AbreConexao();
                cn.comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.FechaConexao();
            }
        }

        public void Excluir()
        {
            string query = string.Format("UPDATE produto SET Ativo = 0 WHERE Id = {0}", Id);
            Conexao cn = new Conexao(query);
            try
            {
                cn.AbreConexao();
                cn.comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.FechaConexao();
            }
        }

    }
}