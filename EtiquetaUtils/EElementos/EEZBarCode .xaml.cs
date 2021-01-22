using EtiquetaUtils.Intarface;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing.Common;
using ZXing.Rendering;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.EElementos
{
    public partial class EEZBarCode : UserControl, IEtiquetaElement
    {
        public EEModel Model { get; set; }
        public EEZBarCode(EEModel model = null)
        {
            InitializeComponent();
            if (model != null)
            {
                Model = model;
            }
            else
            {
                Model = new EEModel()
                {
                    Tipo = EEType.BarCode,
                    TipoCodBarras = ZXing.BarcodeFormat.CODE_128,
                    Altura = 2,
                    ShowHumanCode = true,
                    UsarCodProduto = true
                };
            }
            GerarCode();
            DataContext = this;
        }
        public float X
        {
            get => Model.X;
            set
            {
                if (value == Model.X) return;
                Model.X = value;
                Canvas.SetLeft(this, value);
                OnPropertyChanged(nameof(X));
            }
        }

        public float Y
        {
            get => Model.Y;
            set
            {
                if (value == Model.Y) return;
                Model.Y = value;
                Canvas.SetTop(this, value);
                OnPropertyChanged(nameof(Y));
            }
        }

        public string Texto
        {
            get => Model.Texto;
            set
            {
                if (value == Model.Texto) return;
                Model.Texto = value;
                GerarCode();
                OnPropertyChanged(nameof(Texto));
            }
        }

        public bool IsReadOnly => false;
        public bool HasText => true;

        public int TamanhoFonte
        {
            get => Model.TamanhoFonte;
            set
            {
                if (value == Model.TamanhoFonte) return;
                Model.TamanhoFonte = value;
                OnPropertyChanged(nameof(TamanhoFonte));
            }
        }

        private System.Windows.Media.Brush imgBarCode;
        public System.Windows.Media.Brush ImgBarCode
        {
            get => imgBarCode;
            set
            {
                if (value == imgBarCode) return;
                imgBarCode = value;
                OnPropertyChanged(nameof(ImgBarCode));
            }
        }

        private string lastValidationError = string.Empty;
        public string LastValidationError
        {
            get => lastValidationError;
            set
            {
                if (value == lastValidationError) return;
                lastValidationError = value;
                OnPropertyChanged(nameof(LastValidationError));
            }
        }
        private void GerarCode()
        {
            if (string.IsNullOrEmpty(Texto))
            {
                LastValidationError = "Código de barras vazio.";

                TextoValido = false;
                return;
            }
            try
            {
                var bw = new ZXing.BarcodeWriter();
                bw.Format = TipoCodBarras;
                bw.Options.Width = 0;
                BitMatrix matrix = bw.Encode(Texto);
                TextoValido = true;
                LastValidationError = string.Empty;
                Geometry geometry = new GeometryRenderer().Render(matrix, bw.Format, Texto, bw.Options);
                DrawingGroup group = new DrawingGroup();
                using (DrawingContext context = group.Open())
                {
                    context.DrawGeometry(System.Windows.Media.Brushes.Black, null, geometry);
                    this.Width = matrix.Width;
                    ImgBarCode = new DrawingBrush(group);
                }
            }
            catch (Exception ex)
            {
                LastValidationError = ex.Message;
                TextoValido = false;
            }
        }

        public static readonly DependencyProperty TextoValidoProperty = DependencyProperty.Register(
        nameof(TextoValido), typeof(bool), typeof(EEZBarCode),
        new FrameworkPropertyMetadata(
        false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool TextoValido
        {
            get { return (bool)GetValue(TextoValidoProperty); }
            set
            {
                if ((bool)GetValue(TextoValidoProperty) != value)
                {
                    SetValue(TextoValidoProperty, value);
                }

            }
        }

        public bool IsBarCode => true;
        public bool IsLine => false;

        public ZXing.BarcodeFormat TipoCodBarras
        {
            get => Model.TipoCodBarras;
            set
            {
                if (value == Model.TipoCodBarras) return;
                Model.TipoCodBarras = value;
                GerarCode();
                OnPropertyChanged(nameof(TipoCodBarras));
            }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected), typeof(bool), typeof(EEZBarCode),
        new FrameworkPropertyMetadata(
        false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                if ((bool)GetValue(IsSelectedProperty) != value)
                    SetValue(IsSelectedProperty, value);
            }
        }

        public void SetText(Produto prod)
        {
            if (prod == null) return;
            else if (Model.UsarCodProduto)
            {
                Texto = string.IsNullOrEmpty(prod.CodBarras) ? string.Empty : prod.CodBarras.ToUpper();
            }
            else if (Model.UsarGTINProduto)
            {
                Texto = string.IsNullOrEmpty(prod.GTIN) ? string.Empty : prod.GTIN.ToUpper();
            }
        }

        private bool adicionado;
        public bool Adicionado
        {
            get => adicionado;
            set
            {
                if (value == adicionado) return;
                adicionado = value;
                OnPropertyChanged();
            }
        }

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
                "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EEZBarCode));

        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }
        void RaiseSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SelectedEvent);
            RaiseEvent(newEventArgs);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MouseClickDown(object sender, MouseButtonEventArgs e)
        {
            RaiseSelectedEvent();
        }

    }
}
