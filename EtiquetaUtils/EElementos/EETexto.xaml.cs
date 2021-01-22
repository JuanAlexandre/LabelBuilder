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
using ZXing;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.EElementos
{    
    public partial class EETexto : UserControl, IEtiquetaElement
    {
        Dictionary<int, int> FontSizes = new Dictionary<int, int>
        {
            {1,6 },
            {2,7 },
            {3,10},
            {4,12},
            {5,24}
        };

        public EEModel Model { get; set; }
        public EETexto(EEModel model = null)
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
                    Tipo = EEType.Texto,
                };
            }
            ValidaTexto();
            updateTamanhoFonte();
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

        private void ValidaTexto()
        {
            TextoValido = !string.IsNullOrEmpty(Texto);
            if (!TextoValido)
            {
                LastValidationError = "Conteúdo vazio";
            }
        }

        public string Texto
        {
            get => Model.Texto;
            set
            {
                if (value == Model.Texto) return;
                Model.Texto = value;
                ValidaTexto();
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
                updateTamanhoFonte();
            }
        }

        private void updateTamanhoFonte()
        {
            tbTexto.FontSize = (FontSizes[TamanhoFonte]).PointsToPixels();
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

        public BarcodeFormat TipoCodBarras
        {
            get => Model.TipoCodBarras;
            set
            {
                return;
            }
        }
        private bool isBarCode = false;
        public bool IsBarCode
        {
            get => isBarCode;

        }
        private bool isLine = false;
        public bool IsLine
        {
            get => isLine;

        }

        public static readonly DependencyProperty TextoValidoProperty = DependencyProperty.Register(
        nameof(TextoValido), typeof(bool), typeof(EETexto),
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

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected), typeof(bool), typeof(EETexto),
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

        private bool adicionado;
        public bool Adicionado
        {
            get => adicionado;
            set
            {
                if (value == adicionado) return;
                adicionado = value;
                OnPropertyChanged(nameof(Adicionado));
            }
        }

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
                "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EETexto));

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

        public void SetText(Produto prod)
        {
            if (prod == null) return;
            if (Model.UsarPrecoVarejo)
            {
                Texto = prod.PrecoVarejo.ToString("c");
            }
            else if (Model.UsarPrecoAtacado)
            {
                Texto = prod.PrecoAtacado.ToString("c");
            }
            else if (Model.UsarCodProduto)
            {
                Texto = string.IsNullOrEmpty(prod.CodBarras) ? string.Empty : prod.CodBarras;
            }
            else if (Model.UsarDescProduto)
            {
                Texto = string.IsNullOrEmpty(prod.Descricao) ? string.Empty : prod.Descricao;
            }
            else if (Model.UsarIdProduto)
            {
                Texto = prod.IdProd.ToString();
            }
            else if (Model.UsarUnidadeProduto)
            {
                Texto = string.IsNullOrEmpty(prod.Unidade) ? string.Empty : prod.Unidade;
            }
            else if (Model.UsarGTINProduto)
            {
                Texto = string.IsNullOrEmpty(prod.GTIN) ? string.Empty : prod.GTIN;
            }
            else if (Model.UsarCorProduto)
            {
                Texto = string.IsNullOrEmpty(prod.Cor) ? string.Empty : prod.Cor;
            }
            else if (Model.UsarTamanhoProduto)
            {
                Texto = string.IsNullOrEmpty(prod.Tamanho) ? string.Empty : prod.Tamanho;
            }
            else if (Model.UsarMarcaProduto)
            {
                Texto = string.IsNullOrEmpty(prod.Marca) ? string.Empty : prod.Marca;
            }
        }

        private void MouseClickDown(object sender, MouseButtonEventArgs e)
        {
            RaiseSelectedEvent();
        }
    }
}
