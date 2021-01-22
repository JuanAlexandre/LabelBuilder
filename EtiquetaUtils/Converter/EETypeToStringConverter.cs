using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.Converter
{
    public class EETypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EEType)value)
            {
                case EEType.Texto:
                    return "Texto";
                case EEType.BarCode:
                    return "Código de barras";
                case EEType.Linha:
                    return "Linha";
                case EEType.PrecoMascarado:
                    return "Preço mascarado";
                default:
                    throw new NotImplementedException("Tipo não reconhecido");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
