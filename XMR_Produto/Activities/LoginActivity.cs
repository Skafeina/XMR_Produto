﻿using System;
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
using Newtonsoft.Json;

namespace XMR_Produto.Activities
{
    [Activity(Theme = "@style/TemaSemActionBar")]
    public class LoginActivity : AppCompatActivity
    {
        EditText edtLogin, edtSenha;
        Button btnLogin;

        Usuario usuario;
        private List<Produto> _produtos;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            edtLogin = FindViewById<EditText>(Resource.Id.edtLogin);
            edtSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            _produtos = JsonConvert.DeserializeObject<List<Produto>>(Intent.GetStringExtra("produtos"));

            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Logaremos o usuário (abriremos uma outra activity)
                usuario = Usuario.RealizaLogin(edtLogin.Text, edtSenha.Text);

                Intent telaPrincipal = new Intent(this, typeof(PrincipalActivity));
                telaPrincipal.PutExtra("usuario", JsonConvert.SerializeObject(usuario)); //Informação que a gente quer enviar para a outra tela
                telaPrincipal.PutExtra("produtos", JsonConvert.SerializeObject(_produtos));
                StartActivityForResult(telaPrincipal, 1);
                Toast.MakeText(this, "Bem-vindo, " + usuario.Nome + "!", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok && data != null)
            {
                usuario = JsonConvert.DeserializeObject<Usuario>(data.GetStringExtra("usuario"));
            }

            try
            {
                //Buscar todos os produtos do banco
                _produtos = Produto.BuscarProdutos();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}