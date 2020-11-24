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
    [Activity(Theme = "@style/Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        private List<Produto> _produtos;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //TODO - Carregar a lista de produtos aqui e passar a lista para as telas seguintes.

            //Aqui pode ser colocado todo o carregamento do app
            //ou seja, buscas no banco, imagens, mapas, listas...

            try
            {
                //Buscar todos os produtos do banco
                _produtos = Produto.BuscarProdutos();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                _produtos = new List<Produto>();
            }

            Intent telaLogin = new Intent(this, typeof(LoginActivity));
            telaLogin.PutExtra("produtos", JsonConvert.SerializeObject(_produtos));
            StartActivity(telaLogin);
        }
    }
}