using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XMR_Produto.Classes;

namespace XMR_Produto.Adapters
{
    class AdapterProduto : BaseAdapter<Produto>
    {

        private List<Produto> _produtos;
        private Context _contexto;

        public AdapterProduto(Context contexto, List<Produto> produtos)
        {
            _contexto = contexto;
            _produtos = produtos;
        }

        public override Produto this[int position]
        {
            get
            {
                return _produtos[position];
            }
        }

        public override int Count
        {
            get
            {
                return _produtos.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        //Este é o método principal, que realmente adapta a lista à linha que desenhamos
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View linha = convertView;
            if (linha == null)
                linha = LayoutInflater.From(_contexto).Inflate(Resource.Layout.row_produto, null, false);

            //Capturar os itens da linha (os textviews) e mostraremos cada produto por linha
            TextView txtRowDescricao = linha.FindViewById<TextView>(Resource.Id.txtRowDescricao);
            txtRowDescricao.Text = _produtos[position].Descricao;

            TextView txtRowPreco = linha.FindViewById<TextView>(Resource.Id.txtRowPreco);
            txtRowPreco.Text = _produtos[position].Preco.ToString();

            TextView txtRowQuantidade = linha.FindViewById<TextView>(Resource.Id.txtRowQuantidade);
            txtRowQuantidade.Text = "QTD: " + _produtos[position].Quantidade;

            return linha;
        }
    }

}