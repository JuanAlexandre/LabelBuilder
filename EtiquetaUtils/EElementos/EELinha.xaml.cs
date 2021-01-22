using EtiquetaUtils.Intarface;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public partial class EELinha : UserControl, IEtiquetaElement
    {
        public EEModel Model { get; set; }
        public EELinha(EEModel model = null)
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
                    Tipo = EEType.Linha,
                    Altura = 0.1,
                    Width = 20,
                };
            }
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

        public bool HasText => false;        
        public bool IsReadOnly => true;        

        public string Texto { get => string.Empty; set { } }
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
        public bool IsBarCode => false;
        public bool IsLine => true;
        public bool TextoValido { get => true; set { } }
        public string LastValidationError { get => null; set { } }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
                nameof(IsSelected), typeof(bool), typeof(EELinha),
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

        public BarcodeFormat TipoCodBarras
        {
            get => Model.TipoCodBarras;
            set
            {
                if (value == Model.TipoCodBarras) return;
                Model.TipoCodBarras = value;
                OnPropertyChanged(nameof(TipoCodBarras));
            }
        }


        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
                        "Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EELinha));

        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }
        private void MouseClickDown(object sender, MouseButtonEventArgs e)
        {
            RaiseSelectedEvent();
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
            return;
        }
    }
}
