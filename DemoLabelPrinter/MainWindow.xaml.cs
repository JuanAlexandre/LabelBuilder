using EtiquetaUtils;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoLabelPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Produto> suaListaDeProdutos = new List<Produto>()
            {
                new Produto()
                {
                    CodBarras="12332112332",
                    Cor="Verde",
                    Descricao="Camisa Polo",
                    GTIN="4445556667774",
                    IdProd=00100,
                    Marca="Ralph Lauren",
                    PrecoAtacado=25,
                    PrecoVarejo=89.99M,
                    Tamanho="G",
                    Unidade="UN"
                },
                new Produto()
                {
                    CodBarras="3324576456",
                    Cor="Preto",
                    Descricao="Tênis AllStar cano alto",
                    GTIN="8888888888888",
                    IdProd=55555,
                    Marca="AllStar",
                    PrecoAtacado=60,
                    PrecoVarejo=115.99M,
                    Tamanho="38",
                    Unidade="PAR"
                },
                new Produto()
                {
                    CodBarras="F35SDG674F",
                    Cor="Azul",
                    Descricao="Brinco foleado",
                    GTIN="43563467345",
                    IdProd=12345,
                    Marca="",
                    PrecoAtacado=30,
                    PrecoVarejo=89.99M,
                    Tamanho="P",
                    Unidade="PAR"
                },
                new Produto()
                {
                    CodBarras="F35SDG674F",
                    Cor="Azul",
                    Descricao="Produto com o nome bemmmmmm grande",
                    GTIN="43563467345",
                    IdProd=12345,
                    Marca="",
                    PrecoAtacado=345.50M,
                    PrecoVarejo=500,
                    Tamanho="",
                    Unidade="UN"
                },
            };
            new Preview(suaListaDeProdutos).ShowDialog();
        }
    }
}
