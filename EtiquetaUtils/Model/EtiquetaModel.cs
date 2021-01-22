using EtiquetaUtils.Intarface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static EtiquetaUtils.Enums;


namespace EtiquetaUtils.Model
{
    [Serializable]
    public class EtiquetaModel : IEtiquetaBase, INotifyPropertyChanged
    {
        public EtiquetaModel()
        {
            defineEtiquetaSize();
        }

        public ObservableCollection<EEModel> ElementModels { get; set; } = new ObservableCollection<EEModel>();

        private Dpi dpi = Dpi.DPI200;
        public Dpi Dpi
        {
            get => dpi;
            set
            {
                if (value == dpi) return;
                dpi = value;                
                OnPropertyChanged(nameof(Dpi));
            }
        }

        private int colunas = 3;
        public int Colunas
        {
            get => colunas;
            set
            {
                if (value == colunas) return;
                colunas = value;
                if (colunas <= 1)
                {
                    Espacamento = 0;
                }
                defineEtiquetaSize();
                OnPropertyChanged(nameof(Colunas));
            }
        }

        private double larguraPagina = 10.5;
        public double LarguraPagina
        {
            get => larguraPagina;
            set
            {
                if (value == larguraPagina) return;
                larguraPagina = value;
                defineEtiquetaSize();
                OnPropertyChanged(nameof(LarguraPagina));
            }
        }


        private double espacamento = 0.1;
        public double Espacamento
        {
            get => espacamento;
            set
            {
                if (value == espacamento) return;
                espacamento = value;
                defineEtiquetaSize();
                OnPropertyChanged(nameof(Espacamento));
            }
        }

        private void defineEtiquetaSize()
        {
            Width = (LarguraPagina / Colunas) - (((Colunas - 1) * Espacamento) / Colunas);
        }

        private double height = 5;
        public double Height
        {
            get => height;
            set
            {
                if (height == value) return;
                height = value;
                OnSizeChanged();
                OnPropertyChanged(nameof(Height));
            }
        }

        private double width;
        public double Width
        {
            get => width;
            set
            {
                if (width == value) return;
                width = value;
                OnSizeChanged();
                OnPropertyChanged(nameof(Width));
            }
        }        

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized()]
        public event RoutedEventHandler SizeChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, new RoutedEventArgs());
        }

    }
}
