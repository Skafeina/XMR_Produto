using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using XMR_Produto.Classes;
using System.Collections.Generic;

namespace XMR_Produto.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class CadastraProdutoActivity : AppCompatActivity
    {
        EditText edtDescricao, edtPreco, edtQuantidade;
        Button btnCadastrar;

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

            btnCadastrar.Click += BtnCadastrar_Click;

        }

        private void BtnCadastrar_Click(object sender, System.EventArgs e)
        {
            Produto novoProduto = new Produto
            {
                Descricao = edtDescricao.Text,
                Preco = decimal.Parse(edtPreco.Text),
                Quantidade = int.Parse(edtQuantidade.Text)
            };

            _produtos.Add(novoProduto);

            edtDescricao.Text = "";
            edtPreco.Text = "";
            edtQuantidade.Text = "";
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}