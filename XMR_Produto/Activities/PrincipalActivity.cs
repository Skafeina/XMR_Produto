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
using XMR_Produto.Classes;
using Newtonsoft.Json;
using XMR_Produto.Adapters;

namespace XMR_Produto.Activities
{
    [Activity(Theme = "@style/TemaSemActionBar")]
    public class PrincipalActivity : AppCompatActivity
    {
        ToolbarV7 tbrPrincipal;
        ListView lstProdutos;

        private Usuario _usuarioLogado;

        private List<Produto> _produtos;

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

            _usuarioLogado = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));

            //Instanciar a lista para poder usá-la
            _produtos = new List<Produto>();

            //Adicionar 2 produtos com todas as informações (HardCode)
            _produtos.Add(new Produto() { Id = 1, Descricao = "Leite", Preco = 3.89m, Quantidade = 233, Ativo = true });
            _produtos.Add(new Produto() { Id = 2, Descricao = "Pão Francês", Preco = 1.07m, Quantidade = 467, Ativo = true });

            //Adaptando a lista para "encaixar" na listView
            AdapterProduto adaptador = new AdapterProduto(this, _produtos);

            //Atribuindo os dados adaptados para o componente da tela ListView
            lstProdutos.Adapter = adaptador;

            //Criar um evento de toque longo em um item no listView
            lstProdutos.ItemLongClick += LstProdutos_ItemLongClick;

        }

        private void LstProdutos_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //TODO - Exibir um AlertDialog perguntando se o usuário realmente deseja remover aquele produto tocado.

            //Sabendo a posição do item tocado -> e.Position

            //Criando o AlertDialog
            Android.Support.V7.App.AlertDialog.Builder alertExcluir;
            //Instanciar o objeto
            alertExcluir = new Android.Support.V7.App.AlertDialog.Builder(this);
            //Definir o título
            alertExcluir.SetTitle("Confirmação");
            //Definir a mensagem (corpo)
            alertExcluir.SetMessage("Você deseja realmente excluir o produto: " + _produtos[e.Position].Descricao + "?");
            //Definir um ícone padrão android
            alertExcluir.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            //Definir os botões e suas programações (3 botões possíveis: Positive, Negative e Neutral)
            alertExcluir.SetNegativeButton("Não", delegate { });
            alertExcluir.SetPositiveButton("Sim", delegate {
                //Remover o produto selecionado da List
                _produtos.RemoveAt(e.Position);
                //Adaptar novamente
                AdapterProduto adp = new AdapterProduto(this, _produtos);
                //Atribuir o adaptador no ListView
                lstProdutos.Adapter = adp;
                //Toast pra falar que foi excluído mesmo
                Toast.MakeText(this, "Excluído com sucesso!", ToastLength.Short).Show();
            });
            //Exibir o alert na tela
            alertExcluir.Show();
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
                    telaAddProduto.PutExtra("usuario", JsonConvert.SerializeObject(_usuarioLogado));
                    telaAddProduto.PutExtra("produtos", JsonConvert.SerializeObject(_produtos));

                    //"ForResult" quer dizer que a PrincipalActivity (this) espera por um resultado da CadastraProdutoActivity
                    //Se foi criado ForResult, precisa criar o método "OnActivityResult"
                    StartActivityForResult(telaAddProduto, 1);
                    break;
                case Resource.Id.menu_alterarSenha:
                    Toast.MakeText(this, "Tocou em alterar senha!", ToastLength.Long).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok && data != null)
            {
                _produtos = JsonConvert.DeserializeObject<List<Produto>>(data.GetStringExtra("produtos"));
                //Adaptando a lista para "encaixar" na listView
                AdapterProduto adaptador = new AdapterProduto(this, _produtos);
                //Atribuindo os dados adaptados para o componente da tela ListView
                lstProdutos.Adapter = adaptador;
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

    }
}