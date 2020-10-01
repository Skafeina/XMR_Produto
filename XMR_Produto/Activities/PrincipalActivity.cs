using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using ToolbarV7 = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;

namespace XMR_Produto.Activities
{
    [Activity(Theme = "@style/TemaSemActionBar")]
    public class PrincipalActivity : AppCompatActivity
    {
        ListView lstProdutos;

        ToolbarV7 tbrPrincipal; 

        List<string> produtos;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_principal);

            lstProdutos = FindViewById<ListView>(Resource.Id.lstProdutos);
            tbrPrincipal = FindViewById<ToolbarV7>(Resource.Id.tbrPrincipal);

            //Definindo qual a toolbar desta tela (muitos métodos serão referenciados à toolbar)
            SetSupportActionBar(tbrPrincipal);
            //Definindo um título para a toolbar
            SupportActionBar.Title = "Lista de Produtos";


            //Inserindo nomes de produtos de maneira "HardCoded"
            produtos = new List<string>();
            produtos.Add("Leite");
            produtos.Add("Pão Francês");

            //Adaptando a lista para "encaixar" na listView
            ArrayAdapter<string> adaptador = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, produtos);

            //Atribuindo os dados adaptados para o componente da tela ListView
            lstProdutos.Adapter = adaptador;
        }
    }
}