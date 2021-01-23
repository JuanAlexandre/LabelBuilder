using Etiquetas40.Model;
using Etiquetas40.ModelView;
using EtiquetaUtils;
using EtiquetaUtils.EElementos;
using EtiquetaUtils.Intarface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Etiquetas40.View
{
    /// <summary>
    /// Lógica interna para EtiquetaBuilder.xaml
    /// </summary>
    public partial class EtiquetaBuilder : Window
    {

        private void lvOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((((ListView)sender).SelectedItem) != null)
            {
                IEtiquetaElement x = (IEtiquetaElement)((ListView)sender).SelectedItem;
                viewModel.ElementoSelecionado = viewModel.ElementOptions[viewModel.ElementOptions.IndexOf((IEtiquetaElement)((ListView)sender).SelectedItem)];
                lvOptions.UnselectAll();
            }
        }

        EtiquetaBuilderViewModel viewModel;
        public EtiquetaBuilder()
        {
            InitializeComponent();
            viewModel = new EtiquetaBuilderViewModel(CANVAS_ETIQUETA);
            DataContext = viewModel;
        }
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            bool adicionado = viewModel.AdicionarElemento();
            if (viewModel.ElementoSelecionado != null && adicionado)
            {
                if (viewModel.ElementoSelecionado.GetType() == typeof(EEPrecoMascarado))
                {
                    Storyboard sb = (this.Resources["MaskedAdded"] as Storyboard);
                    sb.Begin();
                }
            }
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoverElemento();
        }

        private void tbTexto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (viewModel.ElementoSelecionado.Model.UsarCodProduto
                || viewModel.ElementoSelecionado.Model.UsarGTINProduto
                || viewModel.ElementoSelecionado.Model.UsarIdProduto
                || viewModel.ElementoSelecionado.Model.UsarUnidadeProduto
                || viewModel.ElementoSelecionado.Model.UsarDescProduto
                || viewModel.ElementoSelecionado.Model.UsarCorProduto
                || viewModel.ElementoSelecionado.Model.UsarTamanhoProduto
                || viewModel.ElementoSelecionado.Model.UsarPrecoVarejo
                || viewModel.ElementoSelecionado.Model.UsarMarcaProduto
                || viewModel.ElementoSelecionado.Model.UsarPrecoAtacado)
            {
                e.Handled = true;
                return;
            }
        }

        private void tbTexto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            viewModel.ValidaTexto(e);
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Print();
        }

        private void btnRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RotateElementLeft();
        }

        private void btnRotateRight_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RotateElementRight();
        }

        private void allow_double(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void CheckBoxTextType_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.SetElementText();
        }

        private void tbQuantidade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void btnRefreshPrinters_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RefreshPrinters();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SalvarEtiqueta();
        }

        private void btnOpenEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OpenEtiqueta();
        }
    }
}
