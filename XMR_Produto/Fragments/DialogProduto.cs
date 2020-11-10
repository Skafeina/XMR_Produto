using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XMR_Produto.Classes;

namespace XMR_Produto.Fragments
{
    public class AoClicarAlterar : EventArgs
    {
        public Produto ProdutoAlterado { get; set; }

        public AoClicarAlterar(Produto produtoAlterado)
        {
            ProdutoAlterado = produtoAlterado;
        }
    }

    class DialogProduto : Android.Support.V4.App.DialogFragment
    {
        private Produto _produto;
        private Context _contexto;
        private Usuario _usuarioLogado;

        public event EventHandler<AoClicarAlterar> CliqueAlterar;

        public DialogProduto(Produto produto, Context contexto, Usuario usuarioLogado)
        {
            _produto = produto;
            _contexto = contexto;
            _usuarioLogado = usuarioLogado;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View telaAlteraProduto = inflater.Inflate(Resource.Layout.fragment_upProduto, container, false);

            EditText edtDescricaoUp = telaAlteraProduto.FindViewById<EditText>(Resource.Id.edtDescricaoUp);
            EditText edtPrecoUp = telaAlteraProduto.FindViewById<EditText>(Resource.Id.edtPrecoUp);
            EditText edtQuantidadeUp = telaAlteraProduto.FindViewById<EditText>(Resource.Id.edtQuantidadeUp);
            Button btnAlterar = telaAlteraProduto.FindViewById<Button>(Resource.Id.btnAlterar);

            edtDescricaoUp.Text = _produto.Descricao;
            edtPrecoUp.Text = _produto.Preco.ToString();
            edtQuantidadeUp.Text = _produto.Quantidade.ToString();


            edtDescricaoUp.Enabled = _usuarioLogado.Administrador;
            edtPrecoUp.Enabled = _usuarioLogado.Administrador;
            edtQuantidadeUp.Enabled = _usuarioLogado.Administrador;
            btnAlterar.Enabled = _usuarioLogado.Administrador;

            btnAlterar.Click += (s, e) =>
            {
                _produto.Descricao = edtDescricaoUp.Text;
                _produto.Preco = decimal.Parse(edtPrecoUp.Text);
                _produto.Quantidade = int.Parse(edtQuantidadeUp.Text);
                //Toast.MakeText(_contexto, "Produto " + _produto.Descricao + ", alterado!", ToastLength.Short).Show();

                //Invocar o método (evento) criado lá em cima
                CliqueAlterar.Invoke(this, new AoClicarAlterar(_produto));

                //Fechar o DialogFragment
                Dismiss();
            };

            return telaAlteraProduto;
        }

    }
}