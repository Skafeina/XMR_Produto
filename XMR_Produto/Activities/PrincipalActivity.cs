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
        ToolbarV7 tbrPrincipal;
        ListView lstProdutos;


        List<string> produtos;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_principal);

            tbrPrincipal = FindViewById<ToolbarV7>(Resource.Id.tbrPrincipal);
            lstProdutos = FindViewById<ListView>(Resource.Id.lstProdutos);
            
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

        //Método responsável por inserir o menu (.xml) na toolbar (definida no SetSupportActionBar).
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_principal, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Método responsável por capturar um item selecionado na toolbar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_addProduto:
                    Intent telaAddProduto = new Intent(this, typeof(CadastraProdutoActivity));
                    StartActivity(telaAddProduto);
                    break;
                case Resource.Id.menu_alterarSenha:
                    Toast.MakeText(this, "Tocou em alterar senha!", ToastLength.Long).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}