using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtiquetaUtils
{
    public static class UnitConverter
    {
        public static int CmToPoints(this double cm)
        {
            //return (int)(cm * 75.63);
            return (int)(cm * 80);
            //return (int)(cm * 79.238);
        }

        public static double PixelsToCm(this double pixels)
        {
            return pixels * 0.02645;
        }

        public static int CmToPixels(this double cm)
        {
            return (int)(cm / 0.02645);
        }

        public static int PixelsToPoints(this double pixels)
        {
            return pixels.PixelsToCm().CmToPoints();
        }

        public static int PointsToPixels(this int points)
        {
            return (int)(points * (96d / 72d));
        }
    }
}
