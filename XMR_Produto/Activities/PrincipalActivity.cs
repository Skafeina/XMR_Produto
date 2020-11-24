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
using System.Globalization;
using XMR_Produto.Fragments;
using System.Runtime.InteropServices;
using Android.Support.V4.Widget;

namespace XMR_Produto.Activities
{
    [Activity(Theme = "@style/TemaSemActionBar")]
    public class PrincipalActivity : AppCompatActivity
    {
        ToolbarV7 tbrPrincipal;
        ListView lstProdutos;
        Android.Support.V7.Widget.SearchView pesquisar;
        SwipeRefreshLayout srlListaProduto;

        private Usuario _usuarioLogado;

        private List<Produto> _produtos;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_principal);

            tbrPrincipal = FindViewById<ToolbarV7>(Resource.Id.tbrPrincipal);
            srlListaProduto = FindViewById<SwipeRefreshLayout>(Resource.Id.srlListaProduto);
            lstProdutos = FindViewById<ListView>(Resource.Id.lstProdutos);

            //Definindo qual a toolbar desta tela (muitos métodos serão referenciados à toolbar)
            SetSupportActionBar(tbrPrincipal);
            //Definindo um título para a toolbar
            SupportActionBar.Title = "Lista de Produtos";

            _usuarioLogado = JsonConvert.DeserializeObject<Usuario>(Intent.GetStringExtra("usuario"));
            _produtos = JsonConvert.DeserializeObject<List<Produto>>(Intent.GetStringExtra("produtos"));

            //Adaptando a lista para "encaixar" na listView
            AdapterProduto adaptador = new AdapterProduto(this, _produtos);

            //Atribuindo os dados adaptados para o componente da tela ListView
            lstProdutos.Adapter = adaptador;

            //Evento de atualização da listView através do swipeRefreshLayout
            srlListaProduto.Refresh += SrlListaProduto_Refresh;

            //Criar um evento de toque longo em um item no listView
            lstProdutos.ItemLongClick += LstProdutos_ItemLongClick;

            //Criar um evento de toque simples em um item no listView
            lstProdutos.ItemClick += LstProdutos_ItemClick;

        }

        private void SrlListaProduto_Refresh(object sender, EventArgs e)
        {
            //Travar o listView e mostrar o processo de loading
            srlListaProduto.Post(delegate {
                srlListaProduto.Refreshing = true;
                lstProdutos.Clickable = false;
            });

            //Realizar o update da listView
            try
            {
                _produtos = Produto.BuscarProdutos();
                AdapterProduto dp = new AdapterProduto(this, _produtos);
                lstProdutos.Adapter = dp;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

            //Destravar o listView e ocultar o processo de loading
            srlListaProduto.Post(delegate {
                srlListaProduto.Refreshing = false;
                lstProdutos.Clickable = true;
            });
        }

        private void LstProdutos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            //Obtendo qual o produto tocado
            Produto produtoSelecionado = lstProdutos.GetItemAtPosition(e.Position).Cast<Produto>();

            //Criando uma cópia em que um obj não esteja referenciado ao outro
            Produto produtoSelecionadoH = JsonConvert.DeserializeObject<Produto>(JsonConvert.SerializeObject(produtoSelecionado));

            //Abrindo o Dialog Fragment
            Android.Support.V4.App.FragmentTransaction tr = SupportFragmentManager.BeginTransaction();
            DialogProduto dp = new DialogProduto(produtoSelecionado, this, _usuarioLogado);
            dp.Show(tr, "telaAlteraProduto");

            dp.CliqueAlterar += (s, ev) =>
            {
                try
                {
                    //Alterar o produto no banco
                    ev.ProdutoAlterado.Alterar();

                    //Mensagem de sucesso!
                    Toast.MakeText(this, ev.ProdutoAlterado.Descricao + " alterado. Antes: " + produtoSelecionadoH.Descricao, ToastLength.Short).Show();

                    //Existe uma relação de referência entre objetos:
                    //- produtoSelecionado aponta pro mesmo endereço do ev.ProdutoAlterado, ou seja, alterou um, o outro também é alterado.
                    //Que consequentemente, altera o produto da lista _produtos.

                    //Adaptar novamente
                    AdapterProduto adp = new AdapterProduto(this, _produtos);
                    //Atribuir o adaptador no ListView
                    lstProdutos.Adapter = adp;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                    ev.ProdutoAlterado.Descricao = produtoSelecionadoH.Descricao;
                    ev.ProdutoAlterado.Preco = produtoSelecionadoH.Preco;
                    ev.ProdutoAlterado.Quantidade = produtoSelecionadoH.Quantidade;
                }
            };

        }

        private void LstProdutos_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            //Sabendo a posição do item tocado -> e.Position
            Produto produtoSelecionado = lstProdutos.GetItemAtPosition(e.Position).Cast<Produto>();

            //Criando o AlertDialog
            Android.Support.V7.App.AlertDialog.Builder alertExcluir;
            //Instanciar o objeto
            alertExcluir = new Android.Support.V7.App.AlertDialog.Builder(this);
            //Definir o título
            alertExcluir.SetTitle("Confirmação");
            //Definir a mensagem (corpo)
            alertExcluir.SetMessage("Você deseja realmente excluir o produto: " + produtoSelecionado.Descricao + "?");
            //Definir um ícone padrão android
            alertExcluir.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            //Definir os botões e suas programações (3 botões possíveis: Positive, Negative e Neutral)
            alertExcluir.SetNegativeButton("Não", delegate { });
            alertExcluir.SetPositiveButton("Sim", delegate
            {
                try
                {
                    //Remover o produto selecionado do Banco
                    produtoSelecionado.Excluir();
                    //Remover o produto selecionado da List
                    _produtos.Remove(produtoSelecionado);
                    //Adaptar novamente
                    AdapterProduto adp = new AdapterProduto(this, _produtos);
                    //Atribuir o adaptador no ListView
                    lstProdutos.Adapter = adp;
                    //Toast pra falar que foi excluído mesmo
                    Toast.MakeText(this, "Excluído com sucesso!", ToastLength.Short).Show();
                    //Limpa o filtro
                    pesquisar.SetQuery("", false);
                    //pesquisar.ClearFocus();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }
            });
            //Exibir o alert na tela
            alertExcluir.Show();
        }

        //Método responsável por inserir o menu (.xml) na toolbar (definida no SetSupportActionBar).
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_principal, menu);

            //Cria e configura o searchView - Somente após as 3 linhas abaixo será possível programar o "pesquisar"
            IMenuItem item = menu.FindItem(Resource.Id.menu_pesquisar);
            View searchView = (Android.Support.V7.Widget.SearchView)item.ActionView;
            pesquisar = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            //Atualizar a lista conforme escrevemos nele
            //Método de evento que é executado a cada letra que digitamos ou apagamos
            pesquisar.QueryTextChange += (s, e) =>
            {
                List<Produto> listaProdutoFiltrada = _produtos.Where(p => p.Descricao //Buscar produtos ONDE a descrição \/
                                                                                     .ToUpper() //Transformado todas as descrições dos produtos em letras maiúculas
                                                                                     .RemoveDiacritics() //Removendo as acentuações das descrições dos produtos
                                                                                     .Contains( // Verificando se estas Descrições modificadas contém:
                                                                                                e.NewText.ToUpper() //O texto digitado para maiúsculo
                                                                                                         .RemoveDiacritics() //removido as acentuações do que foi digitado
                                                                                                                            )).ToList(); //Convertendo o resultado para uma nova lista
                AdapterProduto adp = new AdapterProduto(this, listaProdutoFiltrada); // Adaptando a lista filtrada
                lstProdutos.Adapter = adp; //Atribuindo a adaptação ao ListView
            };


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

                    Intent telaAlterarSenha = new Intent(this, typeof(AlteraSenhaActivity));
                    telaAlterarSenha.PutExtra("usuario", JsonConvert.SerializeObject(_usuarioLogado));
                    StartActivityForResult(telaAlterarSenha, 2);

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
            else if (requestCode == 2 && resultCode == Result.Ok && data != null)
            {
                string senhaAntiga = _usuarioLogado.Senha;
                _usuarioLogado = JsonConvert.DeserializeObject<Usuario>(data.GetStringExtra("usuario"));
                if (senhaAntiga != _usuarioLogado.Senha)
                {
                    Intent voltarLogin = new Intent();
                    voltarLogin.PutExtra("usuario", JsonConvert.SerializeObject(_usuarioLogado));
                    SetResult(Result.Ok, voltarLogin);
                    Finish();
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }


    }

    //Uma nova classe estática com um nome aleatório (Extension)
    public static class Extension
    {
        //Para receber este método de extensão que "Complementa" o tipo String (string) com o método "RemoveDiacritics"
        //Método este que remove os acentos dos caracteres
        public static string RemoveDiacritics(this String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static T Cast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }

}