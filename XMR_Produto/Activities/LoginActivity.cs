using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using XMR_Produto.Classes;

namespace XMR_Produto.Activities
{
    [Activity(Label = "Login", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        EditText edtLogin, edtSenha;
        Button btnLogin;

        Usuario usuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            edtLogin = FindViewById<EditText>(Resource.Id.edtLogin);
            edtSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            usuario = new Usuario
            {
                Id = 1,
                Nome = "Administrator",
                Login = "admin",
                Senha = "123",
                Administrador = true,
                Ativo = true
            };

            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (edtLogin.Text == usuario.Login && edtSenha.Text == usuario.Senha)
                {
                    // Logaremos o usuário (abriremos uma outra activity)
                }
                else
                {
                    // Mensagem de falha de login
                    Toast.MakeText(this, "Usuário e/ou senha incorretos.", ToastLength.Short).Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}