using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.Converter
{
    public class ElementRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ElementRotation)value)
            {
                case ElementRotation.ROT90:
                    return 90;
                case ElementRotation.ROT180:
                    return 180;
                case ElementRotation.ROT270:
                    return 270;

            };
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
