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
using Newtonsoft.Json;
using XMR_Produto.Classes;

namespace XMR_Produto.Activities
{
    [Activity(Label = "Alteração de Senha")]
    public class AlteraSenhaActivity : AppCompatActivity
    {
        EditText edtSenhaAtual, edtNovaSenha, edtConfirmarSenha;
        Button btnAlterarSenha;

        private Usuario _usuarioLogado;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_alteraSenha);

            edtSenhaAtual = FindViewById<EditText>(Resource.Id.edtSenhaAtual);
            edtNovaSenha = FindViewById<EditText>(Resource.Id.edtNovaSenha);
            edtConfirmarSenha = FindViewById<EditText>(Resource.Id.edtConfirmarSenha);
            btnAlterarSenha = FindViewById<Button>(Resource.Id.btnAlterarSenha);

            _usuarioLogado = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            btnAlterarSenha.Click += BtnAlterarSenha_Click;
        }

        private void BtnAlterarSenha_Click(object sender, EventArgs e)
        {
            try
            {
                //Preciso saber se a senha do usuário logado é igual a senha digitada no campo edtSenhaAtual
                if (_usuarioLogado.Senha == edtSenhaAtual.Text)
                {
                    //Agora preciso saber se a nova senha digitada no edtNovaSenha é igual ao que foi digitado no edtConfirmarSenha
                    if (edtNovaSenha.Text == edtConfirmarSenha.Text)
                    {
                        //Quer dizer que tudo deu certo!
                        _usuarioLogado.Senha = edtNovaSenha.Text;
                        Toast.MakeText(this, "Senha alterada com sucesso!", ToastLength.Long).Show();

                        Intent voltar = new Intent();
                        voltar.PutExtra("usuario", JsonConvert.SerializeObject(_usuarioLogado));
                        SetResult(Result.Ok, voltar);
                        Finish();
                    }
                    else
                    {
                        throw new Exception("Nova senha não confere.");
                    }
                }
                else
                {
                    throw new Exception("Senha atual não confere.");
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}