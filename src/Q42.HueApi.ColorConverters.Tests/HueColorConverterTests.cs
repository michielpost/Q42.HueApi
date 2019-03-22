using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Q42.HueApi.Models.Gamut;
using Q42.HueApi.ColorConverters.Gamut;

namespace Q42.HueApi.ColorConverters.Tests
{
    [TestClass]
    public class HueColorConverterTests
    {
        [TestMethod]
        public void ColorConversionDrawBitmaps()
        {
            // Draw some bitmaps so we can manually confirm the output is as expected.
            DrawBitmap("LST001").Save("GamutA.png");
            DrawBitmap("LCT001").Save("GamutB.png");
            DrawBitmap("LST002").Save("GamutC.png");
        }

        private Bitmap DrawBitmap(string model)
        {
            var gamut = CIE1931Gamut.ForModel(model);

            int dimension = 500;
            Bitmap b = new Bitmap(dimension, dimension);
            for (int x = 0; x < dimension; x++)
            {
                for (int y = 0; y < dimension; y++)
                {
                    CIE1931Point point = new CIE1931Point(x / (dimension * 1.0), y / (dimension * 1.0));
                    var rgb = HueColorConverter.XYToRgb(point, CIE1931Gamut.ForModel(model));

                    Color c;
                    if (point.x + point.y > 1.0)
                    {
                        c = Color.Black;
                    }
                    else if (gamut.Contains(point))
                    {
                        c = Color.FromArgb((int)(rgb.R * 255.999), (int)(rgb.G * 255.999), (int)(rgb.B * 255.999));
                    }
                    else
                    {
                        c = Color.FromArgb((int)(rgb.R * 127.999), (int)(rgb.G * 127.999), (int)(rgb.B * 127.999));
                    }

                    // CIE1931 charts are drawn with y-increasing being upwards, not downwards as in bitmaps.
                    b.SetPixel(x, (dimension - 1) - y, c);
                }
            }
            return b;
        }
    }
}
