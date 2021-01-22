using EtiquetaUtils.EElementos;
using EtiquetaUtils.Intarface;
using Etiquetas40.Model;
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
using EtiquetaUtils;
using static EtiquetaUtils.Enums;

namespace Etiquetas40.ModelView
{
    public class EtiquetaBuilderViewModel : INotifyPropertyChanged
    {
        private List<IEtiquetaElement> elementos = new List<IEtiquetaElement>();
        public List<IEtiquetaElement> Elementos { get => elementos; }

        public Dictionary<string, Dpi> DpiOptions { get; } = new Dictionary<string, Dpi>()
        {
            {"200",Dpi.DPI200 },
            {"300",Dpi.DPI300 }
        };

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

        private double zoom = 1.5;
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

        private ICommand removeElementCommand;
        public ICommand RemoveElementCommand
        {
            get
            {
                return removeElementCommand
                    ?? (removeElementCommand = new BasicCommand(() =>
                    {
                        RemoverElemento();
                    }));
            }
        }

        private ICommand removeSelectionCommand;
        public ICommand RemoveSelectionCommand
        {
            get
            {
                return removeSelectionCommand
                    ?? (removeSelectionCommand = new BasicCommand(() =>
                    {
                        ElementoSelecionado = null;
                    }));
            }
        }

        public void SetElementText()
        {
            if (ProdutoSelecionado != null && ElementoSelecionado != null)
            {
                ElementoSelecionado.SetText(ProdutoSelecionado);
            }
        }

        private IEtiquetaElement elementoSelecionado;
        public IEtiquetaElement ElementoSelecionado
        {
            get => elementoSelecionado;
            set
            {
                if (value == elementoSelecionado) return;
                elementoSelecionado = value;
                this.Elementos.ForEach(x => x.IsSelected = false);
                ElementOptions.ToList().ForEach(x => x.IsSelected = false);
                IsElementSelected = value != null;
                if (ElementoSelecionado != null)
                {
                    ElementoSelecionado.IsSelected = true;
                }
                OnPropertyChanged(nameof(ElementoSelecionado));
            }
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
            SetElementText();
            foreach (IEtiquetaElement element in Elementos)
            {
                element.SetText(ProdutoSelecionado);
            }
            foreach (IEtiquetaElement element in ElementOptions)
            {
                element.SetText(ProdutoSelecionado);
            }
        }

        public ObservableCollection<ZXing.BarcodeFormat> OpcoesCodBarras { get; set; }

        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public ObservableCollection<IEtiquetaElement> ElementOptions { get; set; }
        public ObservableCollection<string> Printers { get; set; } = new ObservableCollection<string>();
        public string SelectedPrinter { get; set; }

        private Canvas CANVAS;

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
        public EtiquetaBuilderViewModel(Canvas canvas)
        {
            CANVAS = canvas;
            CANVAS.MouseDown += CANVAS_MouseDown;
            CANVAS.MouseUp += CANVAS_MouseUp;
            CANVAS.MouseMove += CANVAS_MouseMove;
            ElementOptions = new ObservableCollection<IEtiquetaElement>();
            Etiqueta = new EtiquetaModel();
            OpcoesCodBarras = new ObservableCollection<ZXing.BarcodeFormat>() {
                ZXing.BarcodeFormat.CODE_128,
                ZXing.BarcodeFormat.EAN_13,
            };
            RefreshPrinters();

            Produtos.Add(new Produto() { CodBarras = "123123123123", Descricao = "CAMISA DE MALHA RALPH LAUREN", IdProd = 100, PrecoVarejo = 25.90M, PrecoAtacado = 15M, Unidade = "Un", GTIN = "999888777666", Cor = "Verde", Tamanho = "M", Marca = "Ralph lauren" });
            Produtos.Add(new Produto() { CodBarras = "CODIGO128", Descricao = "CALCA JEANS", IdProd = 200, PrecoVarejo = 60M, PrecoAtacado = 25M, Unidade = "Un", GTIN = "888888888888", Cor = "Preto", Tamanho = "38", Marca="Hering" });
            Produtos.Add(new Produto() { CodBarras = "CODIGODEBARRAS", Descricao = "CINTO DE COURO DE COBRA DA MALASIA", IdProd = 65432132, PrecoVarejo = 459.99M, PrecoAtacado = 220M, Unidade = "Un", GTIN = "888888888888", Cor = "Verde", Tamanho = "M", Marca="Tabajara" });
            Produtos.Add(new Produto() { CodBarras = "427654359866", Descricao = "SAPATO MASCULINO SOCIAL", IdProd = 998, PrecoVarejo = 199.99M, PrecoAtacado = 130M, Unidade = "Un", GTIN = "888888888888", Cor = "Preto", Tamanho = "40", Marca="Reserva" });

            EETexto ele = new EETexto();
            ele.Selected += Ele_Selected;
            ElementOptions.Add(ele);

            EEZBarCode barcodeZ = new EEZBarCode();
            barcodeZ.Selected += Ele_Selected;
            ElementOptions.Add(barcodeZ);

            EELinha linha = new EELinha();
            linha.Selected += Ele_Selected;
            ElementOptions.Add(linha);

            EEPrecoMascarado maskedprice = new EEPrecoMascarado();
            maskedprice.Selected += Ele_Selected;
            ElementOptions.Add(maskedprice);
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

        System.Windows.Point startPosition;
        System.Windows.Point mouseStart;

        private void CANVAS_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                UIElement ele = (UIElement)e.Source;
                System.Windows.Point current = e.GetPosition(CANVAS);
                double deltaX = startPosition.X + (current.X - mouseStart.X);
                double deltaY = startPosition.Y + (current.Y - mouseStart.Y);
                double newX = getNewX(deltaX, ele);
                double newY = getNewY(deltaY, ele);
                ((IEtiquetaElement)ele).X = (float)newX;
                ((IEtiquetaElement)ele).Y = (float)newY;
            }
        }

        private double getNewX(double desiredX, UIElement ele)
        {

            if (desiredX < Margin)
            {
                return Margin;
            }
            else if (desiredX + Margin > CANVAS.ActualWidth)
            {
                return CANVAS.ActualWidth - Margin;
            }
            else
            {
                return desiredX;
            }
        }

        private double getNewY(double desiredY, UIElement ele)
        {
            if (desiredY < Margin)
            {
                return Margin;
            }
            else if (desiredY + Margin > CANVAS.ActualHeight)
            {
                return CANVAS.ActualHeight - Margin;
            }
            else
            {
                return desiredY;
            }
        }

        private void CANVAS_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((IInputElement)e.Source != null)

                ((IInputElement)e.Source).ReleaseMouseCapture();
        }

        private void CANVAS_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            startPosition = new System.Windows.Point(Canvas.GetLeft((UIElement)e.Source), Canvas.GetTop((UIElement)e.Source));
            mouseStart = e.GetPosition(CANVAS);
            ((IInputElement)e.Source).CaptureMouse();
        }

        private void SizeChanged(object sender, RoutedEventArgs e)
        {
            foreach (IEtiquetaElement element in Elementos)
            {
                if (element.Y >= Etiqueta.Height.CmToPixels())
                {
                    element.Y = Etiqueta.Height.CmToPixels();
                }
                if (element.X >= Etiqueta.Width.CmToPixels())
                {
                    element.X = Etiqueta.Width.CmToPixels();
                }
            }
        }

        private void Ele_Selected(object sender, RoutedEventArgs e)
        {
            IEtiquetaElement ele = (IEtiquetaElement)sender;
            ElementoSelecionado = ele;
        }

        private bool isElementSelected;
        public bool IsElementSelected
        {
            get => isElementSelected;
            set
            {
                if (value == isElementSelected) return;
                isElementSelected = value;
                OnPropertyChanged(nameof(IsElementSelected));
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

        private int margin = 0;
        public int Margin
        {
            get => margin;
            set
            {
                if (margin == value) return;
                foreach (IEtiquetaElement element in Elementos)
                {
                    if (element.Y <= margin)
                    {
                        element.Y = value;
                    }
                    if (element.Y + margin + 1 >= Etiqueta.Height.CmToPixels())
                    {
                        element.Y = (float)(Etiqueta.Height.CmToPixels() - value);
                    }
                    if (element.X <= margin)
                    {
                        element.X = value;
                    }
                    if (element.X + margin + 1 >= Etiqueta.Width.CmToPixels())
                    {
                        element.X = (float)(Etiqueta.Width.CmToPixels() - value);
                    }
                }
                margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }

        private enum UpdType
        {
            ADICIONAR,
            REMOVER,
            REFRESH,
        }

        private void UpdateCanvas(UpdType operacao = UpdType.REFRESH, IEtiquetaElement ele = null)
        {
            UIElement elemento = (UIElement)ele;
            switch (operacao)
            {
                case UpdType.ADICIONAR:
                    CANVAS.Children.Add(elemento);
                    Canvas.SetLeft(elemento, ele.X + Margin);
                    Canvas.SetTop(elemento, ele.Y + Margin);
                    break;
                case UpdType.REMOVER:
                    CANVAS.Children.Remove((UIElement)ele);
                    ele = null;
                    IsElementSelected = false;
                    break;
                case UpdType.REFRESH:
                    break;
            }
        }

        public void SalvarEtiqueta()
        {
            if (Etiqueta.ElementModels.Count < 1)
            {
                MessageBox.Show("Não é possível salvar uma etiqueta vazia!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            EtiquetaModel copia = Etiqueta.DeepClone();
            foreach (EEModel item in copia.ElementModels)
            {
                if (item.UsarCodProduto)
                {
                    item.Texto = "@CODIGO";
                }
                else if (item.UsarDescProduto)
                {
                    item.Texto = "@DESCRICAO";
                }
                else if (item.UsarGTINProduto)
                {
                    item.Texto = "@GTIN";
                }
                else if (item.UsarIdProduto)
                {
                    item.Texto = "@ID";
                }
                else if (item.UsarPrecoVarejo)
                {
                    item.Texto = "@PRECOVAREJO";
                }
                else if (item.UsarPrecoAtacado)
                {
                    item.Texto = "@PRECOATACADO";
                }
                else if (item.UsarCorProduto)
                {
                    item.Texto = "@CORPRODUTO";
                }
                else if (item.UsarTamanhoProduto)
                {
                    item.Texto = "@TAMANHOPRODUTO";
                }
                else if (item.UsarMarcaProduto)
                {
                    item.Texto = "@MARCAPRODUTO";
                }
                else if (item.UsarUnidadeProduto)
                {
                    item.Texto = "@UN";
                }
            }
            System.Windows.Forms.SaveFileDialog explorer = new System.Windows.Forms.SaveFileDialog();
            explorer.InitialDirectory = @"C:\";
            explorer.RestoreDirectory = true;
            explorer.Title = "QUER SALVAR ONDE CIDADÃO?";
            explorer.Filter = "Etiqueta LabelBuilder (*.lblb)|*.lblb";
            explorer.CheckPathExists = true;
            if (explorer.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string json = copia.SerializeToJSON();
            string encrypted = StringCipher.Encrypt(json, "");
            File.WriteAllText(explorer.FileName, encrypted);
            MessageBox.Show("Etiqueta salva!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool AdicionarElemento()
        {
            if (elementoSelecionado == null) return false;
            if (!ElementoSelecionado.TextoValido)
            {
                MessageBox.Show("Conteúdo inválido: " + ElementoSelecionado.LastValidationError, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            IEtiquetaElement novoElemento = Funcoes.NovoElemento(ElementoSelecionado.Model);
            novoElemento.Selected += Ele_Selected;
            Elementos.Add(novoElemento);
            Etiqueta.ElementModels.Add(novoElemento.Model);
            UpdateCanvas(UpdType.ADICIONAR, novoElemento);
            novoElemento.Adicionado = true;
            return true;
        }


        public void RemoverElemento()
        {
            if (elementoSelecionado == null || !elementoSelecionado.Adicionado) return;
            Etiqueta.ElementModels.Remove(elementoSelecionado.Model);
            Elementos.Remove(elementoSelecionado);
            UpdateCanvas(UpdType.REMOVER, elementoSelecionado);
        }

        public void ValidaTexto(TextCompositionEventArgs e)
        {
            if (elementoSelecionado.Model.UsarCodProduto
                || elementoSelecionado.Model.UsarDescProduto
                || elementoSelecionado.Model.UsarIdProduto
                || elementoSelecionado.Model.UsarUnidadeProduto
                || elementoSelecionado.Model.UsarGTINProduto
                || elementoSelecionado.Model.UsarCorProduto
                || elementoSelecionado.Model.UsarTamanhoProduto
                || elementoSelecionado.Model.UsarPrecoVarejo
                || elementoSelecionado.Model.UsarPrecoAtacado)
            {
                e.Handled = true;
                return;
            }
            string numbers = "0123456789";
            string barcodeCharacteres = numbers + "ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";
            if (ElementoSelecionado.GetType() == typeof(EEZBarCode))
            {
                if (elementoSelecionado.Model.TipoCodBarras == ZXing.BarcodeFormat.EAN_13)
                {
                    if (!(numbers.Contains(e.Text)))
                    {
                        e.Handled = true;
                        return;
                    }
                }
                CultureInfo culture = new CultureInfo("pt-BR");
                if (culture.CompareInfo.IndexOf(barcodeCharacteres, e.Text, CompareOptions.IgnoreCase) >= 0)
                {
                    e.Handled = false;
                    return;
                }
                e.Handled = true;
                return;
            }
        }

        public void RotateElementLeft()
        {
            if (elementoSelecionado == null) return;
            ElementoSelecionado.Model.Rotacao = ElementoSelecionado.Model.Rotacao.Previous();
        }
        public void RotateElementRight()
        {
            if (elementoSelecionado == null) return;
            ElementoSelecionado.Model.Rotacao = ElementoSelecionado.Model.Rotacao.Next();
        }

        public void OpenEtiqueta()
        {
            System.Windows.Forms.OpenFileDialog explorerwindow = new System.Windows.Forms.OpenFileDialog();
            explorerwindow.InitialDirectory = @"C:\";
            explorerwindow.RestoreDirectory = true;
            explorerwindow.Title = "Carregar etiqueta";
            explorerwindow.Filter = "Etiqueta LabelBuilder (*.lblb)|*.lblb";
            explorerwindow.CheckPathExists = true;
            explorerwindow.Multiselect = false;
            if (explorerwindow.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            try
            {
                Etiqueta = Funcoes.LoadEtiquetaFromFile(explorerwindow.FileName);
            }
            catch (Exception ex)
            {
                Funcoes.CriaLog(ex);
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEtiqueta()
        {
            if (Etiqueta == null) return;
            Elementos.Clear();
            CANVAS.Children.Clear();
            foreach (EEModel ele in Etiqueta.ElementModels)
            {
                IEtiquetaElement novoElemento = Funcoes.LoadElement(ele);
                novoElemento.Adicionado = true;
                novoElemento.Selected += Ele_Selected;
                Elementos.Add(novoElemento);
                CANVAS.Children.Add((UIElement)novoElemento);
                Canvas.SetLeft((UIElement)novoElemento, ele.X);
                Canvas.SetTop((UIElement)novoElemento, ele.Y);
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
            if (string.IsNullOrEmpty(SelectedPrinter))
            {
                MessageBox.Show("Selecione uma impressora. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Etiqueta.ElementModels.Count < 1)
            {
                MessageBox.Show("Adicione elementos à etiqueta para imprimir. ", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            foreach (IEtiquetaElement element in Elementos)
            {
                if (!element.TextoValido)
                {
                    MessageBox.Show("Existem elementos com o conteúdo invalido na etiqueta. " + element.LastValidationError, "Conteúdo inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsBusy = false;
                    return;
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
