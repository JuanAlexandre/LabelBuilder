using EtiquetaUtils.Model;
using EtiquetaUtils.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EtiquetaUtils
{
    /// <summary>
    /// Lógica interna para Preview.xaml
    /// </summary>
    public partial class Preview : Window
    {
        PreviewViewModel viewModel;
        public Preview(List<Produto> prods)
        {
            InitializeComponent();
            viewModel = new PreviewViewModel(CANVAS_ETIQUETA, prods);
            DataContext = viewModel;            
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

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Print();
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

        private void btnRefreshEtiquetas_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadEtiquetas();
        }

        private void btnOpenEtiquetasFolder_Click(object sender, RoutedEventArgs e)
        {
            Funcoes.OpenEtiquetasFolder();
        }
    }
}
