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

namespace XMR_Produto.Activities
{
    [Activity(Theme = "@style/Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //TODO - Carregar a lista de produtos aqui e passar a lista para as telas seguintes.

            //Aqui pode ser colocado todo o carregamento do app
            //ou seja, buscas no banco, imagens, mapas, listas...

            Intent telaLogin = new Intent(this, typeof(LoginActivity));
            //PutExtra... Passar todos os parâmetros para a tela de login
            StartActivity(telaLogin);
        }
    }
}