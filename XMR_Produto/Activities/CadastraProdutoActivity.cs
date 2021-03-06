﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using XMR_Produto.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Content;

namespace XMR_Produto.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CadastraProdutoActivity : AppCompatActivity
    {
        EditText edtDescricao, edtPreco, edtQuantidade;
        Button btnCadastrar;
        TextView txtInfo, txtNomeUsuario, txtLoginUsuario;

        private List<Produto> _produtos = new List<Produto>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Linha que cria o vínculo entre layout (parte visual) com esta classe (codificação).
            SetContentView(Resource.Layout.activity_cadastraProduto);

            //Capturar os objetos do layout
            edtDescricao = FindViewById<EditText>(Resource.Id.edtDescricao);
            edtPreco = FindViewById<EditText>(Resource.Id.edtPreco);
            edtQuantidade = FindViewById<EditText>(Resource.Id.edtQuantidade);
            btnCadastrar = FindViewById<Button>(Resource.Id.btnCadastrar);
            txtInfo = FindViewById<TextView>(Resource.Id.txtInfo);
            txtNomeUsuario = FindViewById<TextView>(Resource.Id.txtNomeUsuario);
            txtLoginUsuario = FindViewById<TextView>(Resource.Id.txtLoginUsuario);

            //Receber possíveis informações vindas de outra activity
            string jsonUsuario = Intent.GetStringExtra("usuario");
            Usuario usuario = jsonUsuario == null ? new Usuario() : JsonConvert.DeserializeObject<Usuario>(jsonUsuario);

            _produtos = JsonConvert.DeserializeObject<List<Produto>>(Intent.GetStringExtra("produtos"));
            txtInfo.Text = "Produtos cadastrados: " + _produtos.Count;

            //Exibir as informações de nome e login nas textview's correspondentes
            txtNomeUsuario.Text = "Usuário logado: " + usuario.Nome;
            txtLoginUsuario.Text = "Login: " + usuario.Login;

            //Exibir o nome que veio da outra tela em um Toast
            //Toast.MakeText(this, "Bem-vindo, " + usuario.Nome + "!", ToastLength.Long).Show();

            //Criando a chamada do método de click (toque) do botão cadastrar
            btnCadastrar.Click += BtnCadastrar_Click;

        }

        private void BtnCadastrar_Click(object sender, System.EventArgs e)
        {
            //Alimentando as propriedades de um objeto (novoProduto) que acaba de ser criado.
            Produto novoProduto = new Produto
            {
                Id = 0,
                Descricao = edtDescricao.Text,
                Preco = decimal.Parse(edtPreco.Text),
                Quantidade = int.Parse(edtQuantidade.Text)
            };

            try
            {
                //Adicionando este objeto no banco
                novoProduto.Cadastrar();

                //Adicionando este objeto à lista
                _produtos.Add(novoProduto);

                //Exibir toast de sucesso.
                Toast.MakeText(this, novoProduto.Descricao + " adicionado com sucesso!", ToastLength.Long).Show();

                //Atualizar a frase da txtInfo com a quantidade de produtos inseridos na lista
                txtInfo.Text = "Produtos cadastrados: " + _produtos.Count;

                //Limpando os campos
                edtDescricao.Text = "";
                edtPreco.Text = "";
                edtQuantidade.Text = "";
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //Método que é executado quando se toca na setinha pra voltar
        public override void OnBackPressed()
        {
            //Código para devolver algum valor para a activity que abriu a atual
            Intent voltar = new Intent();
            voltar.PutExtra("produtos", JsonConvert.SerializeObject(_produtos));
            SetResult(Result.Ok, voltar);
            Finish();

            base.OnBackPressed();
        }
    }
}