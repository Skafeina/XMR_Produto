
using System;

namespace XMR_Produto.Classes
{
    class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Administrador { get; set; }
        public bool Ativo { get; set; }


        public static Usuario RealizaLogin(string login, string senha)
        {
            string query = string.Format("SELECT * FROM usuario WHERE Login = '{0}'", login);
            Conexao cn = new Conexao(query);
            Usuario usuarioLogado = new Usuario();
            try
            {
                cn.AbreConexao();
                cn.dr = cn.comando.ExecuteReader();
                if (cn.dr.HasRows)
                {
                    while (cn.dr.Read())
                    {
                        usuarioLogado.Id = Convert.ToInt32(cn.dr[0]);
                        usuarioLogado.Nome = cn.dr[1].ToString();
                        usuarioLogado.Login = cn.dr[2].ToString();
                        usuarioLogado.Senha = cn.dr[3].ToString();
                        usuarioLogado.Administrador = Convert.ToBoolean(cn.dr[4]);
                        usuarioLogado.Ativo = Convert.ToBoolean(cn.dr[5]);
                    }

                    if (usuarioLogado.Senha == senha)
                    {
                        if (usuarioLogado.Ativo)
                            return usuarioLogado;
                        else
                            throw new Exception("Usuário bloqueado.");
                    }
                    else
                        throw new Exception("Senha não confere.");
                }
                else
                    throw new System.Exception("Usuário não encontrado.");
            }
            catch (System.Exception)
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