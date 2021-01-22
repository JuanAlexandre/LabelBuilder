using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtiquetaUtils
{
    public class Enums
    {
        public enum Dpi
        {
           DPI200=200,
           DPI300=300
        }
        public enum ElementRotation
        {
            NORMAL = 0,
            ROT90 = 1,
            ROT180 = 2,
            ROT270 = 3
        }
        public enum EEType
        {
            Linha,
            Texto,
            BarCode,
            PrecoMascarado,
        }
    }
}
