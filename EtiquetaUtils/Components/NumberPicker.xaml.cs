using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EtiquetaUtils.Components
{
    /// <summary>
    /// Interação lógica para NumberPicker.xam
    /// </summary>
    public partial class NumberPicker : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
        nameof(Number), typeof(decimal), typeof(NumberPicker),
        new FrameworkPropertyMetadata(
        1m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public decimal Number
        {
            get { return (decimal)GetValue(NumberProperty); }
            set
            {
                if ((decimal)GetValue(NumberProperty) != value)
                    SetValue(NumberProperty, value);
                CanAddValue = MaxValue > Number;
                CanLowerValue = MinValue < Number;
            }
        }

        private bool canAddValue;
        public bool CanAddValue
        {
            get => canAddValue;
            set
            {
                if (value == canAddValue) return;
                canAddValue = value;
                OnPropertyChanged(nameof(CanAddValue));
            }
        }
        private bool canLowerValue;
        public bool CanLowerValue
        {
            get => canLowerValue;
            set
            {
                if (value == canLowerValue) return;
                canLowerValue = value;
                OnPropertyChanged(nameof(CanLowerValue));
            }
        }
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
        nameof(MaxValue), typeof(decimal), typeof(NumberPicker),
        new FrameworkPropertyMetadata(
        10m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set
            {
                if ((decimal)GetValue(MaxValueProperty) != value)
                    SetValue(MaxValueProperty, value);
                CanAddValue = MaxValue > Number;
                CanLowerValue = MinValue < Number;
            }
        }
        public static readonly DependencyProperty TickProperty = DependencyProperty.Register(
        nameof(Tick), typeof(decimal), typeof(NumberPicker),
        new FrameworkPropertyMetadata(
        1m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public decimal Tick
        {
            get { return (decimal)GetValue(TickProperty); }
            set
            {
                if ((decimal)GetValue(TickProperty) != value)
                    SetValue(TickProperty, value);
            }
        }
        
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
        nameof(MinValue), typeof(decimal), typeof(NumberPicker),
        new FrameworkPropertyMetadata(
        0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set
            {
                if ((decimal)GetValue(MinValueProperty) != value)
                    SetValue(MinValueProperty, value);
                CanAddValue = MaxValue > Number;
                CanLowerValue = MinValue < Number;
            }
        }
        public NumberPicker()
        {
            InitializeComponent();
            CanAddValue = MaxValue > Number;
            CanLowerValue = MinValue < Number;
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Number>MinValue)
            {
                Number=Number-Tick;
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (Number<MaxValue)
            {
                Number=Number+Tick;
            }
        }
    }
}
