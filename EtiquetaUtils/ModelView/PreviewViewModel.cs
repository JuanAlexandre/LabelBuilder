using EtiquetaUtils.EElementos;
using EtiquetaUtils.Intarface;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EtiquetaUtils.ModelView
{
    public class PreviewViewModel : INotifyPropertyChanged
    {
        private List<IEtiquetaElement> elementos = new List<IEtiquetaElement>();
        public List<IEtiquetaElement> Elementos { get => elementos; }

        private Dictionary<string, EtiquetaModel> etiquetas;
        public Dictionary<string, EtiquetaModel> Etiquetas
        {
            get => etiquetas;
            set
            {
                if (value == etiquetas) return;
                etiquetas = value;
                OnPropertyChanged(nameof(Etiquetas));
            }
        }

        private decimal npAlturaMinValue = 0;
        public decimal NpAlturaMinValue
        {
            get => npAlturaMinValue;
            set
            {
                if (value == npAlturaMinValue) return;
                npAlturaMinValue = value;
                OnPropertyChanged(nameof(NpAlturaMinValue));
            }
        }
        private decimal npAlturaMaxValue = 0;
        public decimal NpAlturaMaxValue
        {
            get => npAlturaMaxValue;
            set
            {
                if (value == npAlturaMaxValue) return;
                npAlturaMaxValue = value;
                OnPropertyChanged(nameof(NpAlturaMaxValue));
            }
        }

        private decimal npLarguraPaginaMaxValue = 0;
        public decimal NpLarguraPaginaMaxValue
        {
            get => npLarguraPaginaMaxValue;
            set
            {
                if (value == npLarguraPaginaMaxValue) return;
                npLarguraPaginaMaxValue = value;
                OnPropertyChanged(nameof(NpLarguraPaginaMaxValue));
            }
        }
        private decimal npLarguraPaginaMinValue = 0;
        public decimal NpLarguraPaginaMinValue
        {
            get => npLarguraPaginaMinValue;
            set
            {
                if (value == npLarguraPaginaMinValue) return;
                npLarguraPaginaMinValue = value;
                OnPropertyChanged(nameof(NpLarguraPaginaMinValue));
            }
        }
        private decimal npEspacamentoMaxValue = 0;
        public decimal NpEspacamentoMaxValue
        {
            get => npEspacamentoMaxValue;
            set
            {
                if (value == npEspacamentoMaxValue) return;
                npEspacamentoMaxValue = value;
                OnPropertyChanged(nameof(NpEspacamentoMaxValue));
            }
        }
        private decimal npEspacamentoMinValue = 0;
        public decimal NpEspacamentoMinValue
        {
            get => npEspacamentoMinValue;
            set
            {
                if (value == npEspacamentoMinValue) return;
                npEspacamentoMinValue = value;
                OnPropertyChanged(nameof(NpEspacamentoMinValue));
            }
        }

        private int quantidade = 3;
        public int Quantidade
        {
            get => quantidade;
            set
            {
                if (value == quantidade) return;
                quantidade = value;
                OnPropertyChanged(nameof(Quantidade));
            }
        }

        private double zoom = 1;
        public double Zoom
        {
            get => zoom;
            set
            {
                if (value == zoom) return;
                zoom = value;
                OnPropertyChanged(nameof(Zoom));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Produto produtoSelecionado;
        public Produto ProdutoSelecionado
        {
            get => produtoSelecionado;
            set
            {
                if (value == produtoSelecionado) return;
                produtoSelecionado = value;
                LoadProduto();
                OnPropertyChanged(nameof(ProdutoSelecionado));
            }
        }

        private void LoadProduto()
        {
            foreach (IEtiquetaElement element in Elementos)
            {
                element.SetText(ProdutoSelecionado);
            }
        }

        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public ObservableCollection<string> Printers { get; set; } = new ObservableCollection<string>();
        public string SelectedPrinter { get; set; }

        private Canvas CANVAS;

        public PreviewViewModel(Canvas canvas, List<Produto> prods)
        {
            CANVAS = canvas;
            Produtos = prods;
            RefreshPrinters();
            LoadEtiquetas();
        }

        private void SizeChanged(object sender, RoutedEventArgs e)
        {
            foreach (IEtiquetaElement element in Elementos)
            {
                if (element.Y>=Etiqueta.Height.CmToPixels())
                {
                    element.Y = Etiqueta.Height.CmToPixels();
                }
                if (element.X>=Etiqueta.Width.CmToPixels())
                {
                    element.X = Etiqueta.Width.CmToPixels();
                }
            }
        }

        public void LoadEtiquetas()
        {
            try
            {
                Etiquetas = Funcoes.LoadEtiquetas();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Não foi possível localizar as etiquetas, certifique-se de colocar as etiquetas na pasta .../Etiquetas", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void LoadEtiqueta()
        {
            if (Etiqueta == null) return;
            NpAlturaMinValue = (Etiqueta.Height - 0.5) > 0 ? (decimal)Etiqueta.Height - 0.5m : 0;
            NpAlturaMaxValue = (decimal)Etiqueta.Height + 0.5m;

            NpLarguraPaginaMinValue = Etiqueta.LarguraPagina - 0.5 > 0 ? (decimal)Etiqueta.LarguraPagina - 0.5m : 0;
            NpLarguraPaginaMaxValue = (decimal)Etiqueta.LarguraPagina + 0.5m;

            NpEspacamentoMinValue = Etiqueta.Espacamento - 0.3 > 0 ? (decimal)Etiqueta.Espacamento - 0.3m : 0;
            NpEspacamentoMaxValue = (decimal)Etiqueta.Espacamento + 0.3m;
            Elementos.Clear();
            CANVAS.Children.Clear();
            foreach (EEModel ele in Etiqueta.ElementModels)
            {
                IEtiquetaElement novoElemento = Funcoes.LoadElement(ele);
                Elementos.Add(novoElemento);
                CANVAS.Children.Add((UIElement)novoElemento);
                Canvas.SetLeft((UIElement)novoElemento, ele.X);
                Canvas.SetTop((UIElement)novoElemento, ele.Y);
            }
        }

        public void RefreshPrinters()
        {
            SelectedPrinter = null;
            Printers.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Printers.Add(printer);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (value == isBusy) return;
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private EtiquetaModel etiqueta;
        public EtiquetaModel Etiqueta
        {
            get => etiqueta;
            set
            {
                if (value == etiqueta) return;
                EtiquetaModel nova = value.DeepClone();
                nova.SizeChanged += SizeChanged;
                etiqueta = nova;
                LoadEtiqueta();
                LoadProduto();
                OnPropertyChanged(nameof(Etiqueta));
            }
        }

        public void Print()
        {
            if (IsBusy) return;
            if (Quantidade < 1)
            {
                MessageBox.Show("Informe a quantidade de etiquetas. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Etiqueta == null)
            {
                MessageBox.Show("Selecione uma etiqueta. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrEmpty(SelectedPrinter))
            {
                MessageBox.Show("Selecione uma impressora. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            foreach (IEtiquetaElement item in Elementos)
            {
                if (ProdutoSelecionado == null
                    &&
                    (
                    item.Model.UsarCodProduto
                    || item.Model.UsarDescProduto
                    || item.Model.UsarGTINProduto
                    || item.Model.UsarIdProduto
                    || item.Model.UsarPrecoAtacado
                    || item.Model.UsarPrecoVarejo
                    || item.Model.UsarUnidadeProduto
                    )
                    )
                {
                    MessageBox.Show("Selecione um produto. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            foreach (IEtiquetaElement element in Elementos)
            {
                if (!element.TextoValido)
                {
                    if (MessageBoxResult.No == MessageBox.Show("Existe um elemento com o conteúdo invalido na etiqueta. " + Environment.NewLine + element.LastValidationError + Environment.NewLine + Environment.NewLine + "Continuar mesmo assim?", "Conteúdo inválido", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    {
                        IsBusy = false;
                        return;
                    }
                }
            }
            IsBusy = true;
            try
            {
                Funcoes.PrintEtiqueta(Etiqueta, Quantidade, SelectedPrinter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            IsBusy = false;
        }
    }
}
