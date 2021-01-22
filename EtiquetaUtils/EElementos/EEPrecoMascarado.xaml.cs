using EtiquetaUtils.Intarface;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZXing;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.EElementos
{
    /// <summary>
    /// Interação lógica para PrecoMascarado.xam
    /// </summary>
    public partial class EEPrecoMascarado : UserControl,IEtiquetaElement
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
        public EEPrecoMascarado(EEModel model = null)
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
                    Tipo = EEType.PrecoMascarado,
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

        public bool IsReadOnly => true;

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

        private bool hasText = true;
        public bool HasText
        {
            get => hasText;
        }

        private bool isLine = false;
        public bool IsLine
        {
            get => isLine;

        }

        public static readonly DependencyProperty TextoValidoProperty = DependencyProperty.Register(
        nameof(TextoValido), typeof(bool), typeof(EEPrecoMascarado),
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
        nameof(IsSelected), typeof(bool), typeof(EEPrecoMascarado),
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
                "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EEPrecoMascarado));

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
            if (prod == null)
            {
                Texto = string.Empty;
                return;
            }
            Texto = $"{DateTime.Now.ToString("yyMM")}" +
                $"000" +
                $"{prod.PrecoAtacado.ToString("N2").Replace(",", "").Replace(".", "")}" +
                $"000" +
                $"{prod.PrecoVarejo.ToString("N2").Replace(",", "").Replace(".", "")}";
        }

        private void MouseClickDown(object sender, MouseButtonEventArgs e)
        {
            RaiseSelectedEvent();
        }
    }
}
