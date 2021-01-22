using EtiquetaUtils.Intarface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EtiquetaUtils.Converter
{
    public class ElementoAdicionadoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEtiquetaElement ele = (IEtiquetaElement)value;
            if (ele == null) return false;
            return ele.Adicionado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
