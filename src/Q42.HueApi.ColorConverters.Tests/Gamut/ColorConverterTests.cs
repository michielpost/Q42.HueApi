using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.ColorConverters.Gamut;
using Q42.HueApi.Models.Gamut;
using System;

namespace Q42.HueApi.ColorConverters.Tests
{
    [TestClass]
    public class ColorConverterTests
    {
        [TestMethod]
        public void ColorConversionWhitePoint()
        {
            // These light models are capable of Gamuts A, B and C respectively.
            // See http://www.developers.meethue.com/documentation/supported-lights
            string[] models = new string[] { "LST001", "LCT001", "LST002" };

            foreach (string model in models)
            {
                // Make sure that Philips' white point resolves to #FFFFFF for all lights.
                var rgb = HueColorConverter.XYToRgb(CIE1931Point.PhilipsWhite, CIE1931Gamut.ForModel(model));
                Assert.AreEqual(rgb.R, 1.0, 0.0001);
                Assert.AreEqual(rgb.G, 1.0, 0.0001);
                Assert.AreEqual(rgb.B, 1.0, 0.0001);

                var xy = HueColorConverter.RgbToXY(new RGBColor(1.0, 1.0, 1.0), CIE1931Gamut.ForModel(model));
                AssertAreEqual(CIE1931Point.PhilipsWhite, xy, 0.0001);
            }
        }
        
        [TestMethod]
        public void ColorsOutsideGamutAdjustedToInBeInGamut()
        {
            // This green is in the gamut of LST001, but not LCT001.
            CIE1931Point outsideGreen = new CIE1931Point(0.18, 0.72);

            CIE1931Point gamutAGreen = new CIE1931Point(0.2151, 0.7106);
            CIE1931Point gamutBGreen = new CIE1931Point(0.409, 0.518);
            CIE1931Point gamutCGreen = new CIE1931Point(0.17, 0.7);

            AssertAreEqual(gamutAGreen, CIE1931Gamut.ForModel("LST001").NearestContainedPoint(outsideGreen), 0.0001);
            AssertAreEqual(gamutBGreen, CIE1931Gamut.ForModel("LCT001").NearestContainedPoint(outsideGreen), 0.0001);
            AssertAreEqual(gamutCGreen, CIE1931Gamut.ForModel("LST002").NearestContainedPoint(outsideGreen), 0.0001);
        }
        
        [TestMethod]
        public void ColorsOutsideGamutAdjustedToInBeInGamutOnConversion()
        {
            // The green primary of Gamut A.
            CIE1931Point gamutGreen = new CIE1931Point(0.2151, 0.7106);

            // A color green outside Gamut A.
            CIE1931Point greenOutsideGamut = new CIE1931Point(0.21, 0.75);

            var a = HueColorConverter.XYToRgb(gamutGreen, CIE1931Gamut.ForModel("LST001"));
            var b = HueColorConverter.XYToRgb(greenOutsideGamut, CIE1931Gamut.ForModel("LST001"));

            // Points should be equal, since the green outside the gamut should
            // be adjusted the the nearest green in-gamut.
            Assert.AreEqual(a.R, b.R);
            Assert.AreEqual(a.G, b.G);
            Assert.AreEqual(a.B, b.B);
        }

        [TestMethod]
        public void CompareColorConversionWithReference()
        {
            // Use a consistent seed for test reproducability
            Random r = new Random(0);

            for (int trial = 0; trial < 1000; trial++)
            {
                double red = r.NextDouble();
                double green = r.NextDouble();
                double blue = r.NextDouble();

                var referenceXy = ReferenceColorConverter.XyFromColor(red, green, blue);

                // LCT001 uses Gamut B, which is the gamut used in the reference implementation.
                var actualXy = HueColorConverter.RgbToXY(new RGBColor(red, green, blue), CIE1931Gamut.ForModel("LCT001"));

                AssertAreEqual(referenceXy, actualXy, 0.0001);
            }
        }

        [TestMethod]
        public void GamutContainsWorksCorrectly()
        {
            Random r = new Random(0);

            for (int trial = 0; trial < 1000; trial++)
            {
                var point = new CIE1931Point(r.NextDouble(), r.NextDouble());
                var gamutB = CIE1931Gamut.ForModel("LCT001");

                Assert.AreEqual(ReferenceColorConverter.CheckPointInLampsReach(point), gamutB.Contains(point));
            }
        }

        [TestMethod]
        public void ColorConversionRoundtripInsideGamut()
        {
            // Use a consistent seed for test reproducability
            Random r = new Random(0);

            for (int trial = 0; trial < 1000; trial++)
            {
                CIE1931Point originalXy;

                // Randomly generate a test color that is at a valid CIE1931 coordinate.
                do
                {
                    originalXy = new CIE1931Point(r.NextDouble(), r.NextDouble());
                }
                while (originalXy.x + originalXy.y >= 1.0 
                    || !ReferenceColorConverter.CheckPointInLampsReach(originalXy)
                    || !CIE1931Gamut.PhilipsWideGamut.Contains(originalXy));

                RGBColor rgb = HueColorConverter.XYToRgb(originalXy, CIE1931Gamut.ForModel("LCT001"));
                var xy = HueColorConverter.RgbToXY(rgb, CIE1931Gamut.ForModel("LCT001"));
                
                AssertAreEqual(originalXy, xy, 0.0001);
            }
        }
        
        [TestMethod]
        public void ColorConversionRoundtripAllPoints()
        {
            // Use a consistent seed for test reproducability
            Random r = new Random(0);

            for (int trial = 0; trial < 1000; trial++)
            {
                CIE1931Point originalXy;

                // Randomly generate a test color that is at a valid CIE1931 coordinate.
                do
                {
                    originalXy = new CIE1931Point(r.NextDouble(), r.NextDouble());
                }
                while (originalXy.x + originalXy.y >= 1.0);

                RGBColor rgb = HueColorConverter.XYToRgb(originalXy, CIE1931Gamut.ForModel("LCT001"));
                var xy = HueColorConverter.RgbToXY(rgb, CIE1931Gamut.ForModel("LCT001"));

                // We expect a point that is both inside the lamp's gamut and the "wide gamut" 
                // used for XYZ->RGB and RGB->XYZ conversion. 
                // Conversion from XY to RGB
                var expectedXy = CIE1931Gamut.ForModel("LCT001").NearestContainedPoint(originalXy);
                expectedXy = CIE1931Gamut.PhilipsWideGamut.NearestContainedPoint(expectedXy);

                // RGB to XY
                expectedXy = CIE1931Gamut.ForModel("LCT001").NearestContainedPoint(expectedXy);

                AssertAreEqual(expectedXy, xy, 0.0001);
            }
        }

        private void AssertAreEqual(CIE1931Point expected, CIE1931Point actual, double delta)
        {
            Assert.AreEqual(expected.x, actual.x, delta);
            Assert.AreEqual(expected.y, actual.y, delta);
            Assert.AreEqual(expected.z, actual.z, delta);
        }

    }
    
    /// <summary>
    /// A prior implementation of color converter. Assumes all lights have the gamut of 
    /// the original LCT001 hue lights.
    /// N.B. It has been modified to use the philips matrix "Wide Gamut" for mapping from RGB -> XYZ 
    /// instead of the original matrix, and apply gamma correctly.
    /// </summary>
    internal static partial class ReferenceColorConverter
    {
        private static CIE1931Point Red = new CIE1931Point(0.675F, 0.322F);
        private static CIE1931Point Lime = new CIE1931Point(0.409F, 0.518F);
        private static CIE1931Point Blue = new CIE1931Point(0.167F, 0.04F);
        private static float factor = 10000.0f;
        private static int maxX = 452;
        private static int maxY = 302;
        
        /// <summary>
        ///  Get XY from red,green,blue ints
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static CIE1931Point XyFromColor(double red, double green, double blue)
        {
            double r = (red > 0.04045f) ? Math.Pow((red + 0.055f) / (1.0f + 0.055f), 2.4f) : (red / 12.92f);
            double g = (green > 0.04045f) ? Math.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f) : (green / 12.92f);
            double b = (blue > 0.04045f) ? Math.Pow((blue + 0.055f) / (1.0f + 0.055f), 2.4f) : (blue / 12.92f);

            //double X = r * 0.4360747f + g * 0.3850649f + b * 0.0930804f;
            //double Y = r * 0.2225045f + g * 0.7168786f + b * 0.0406169f;
            //double Z = r * 0.0139322f + g * 0.0971045f + b * 0.7141733f;
            double X = r * 0.664511f + g * 0.154324f + b * 0.162028f;
            double Y = r * 0.283881f + g * 0.668433f + b * 0.047685f;
            double Z = r * 0.000088f + g * 0.072310f + b * 0.986039f;



            double cx = X / (X + Y + Z);
            double cy = Y / (X + Y + Z);

            if (Double.IsNaN(cx))
            {
                cx = 0.0f;
            }

            if (Double.IsNaN(cy))
            {
                cy = 0.0f;
            }

            //Check if the given XY value is within the colourreach of our lamps.
            CIE1931Point xyPoint = new CIE1931Point(cx, cy);
            bool inReachOfLamps = ReferenceColorConverter.CheckPointInLampsReach(xyPoint);

            if (!inReachOfLamps)
            {
                //It seems the colour is out of reach
                //let's find the closes colour we can produce with our lamp and send this XY value out.

                //Find the closest point on each line in the triangle.
                CIE1931Point pAB = ReferenceColorConverter.GetClosestPointToPoint(Red, Lime, xyPoint);
                CIE1931Point pAC = ReferenceColorConverter.GetClosestPointToPoint(Blue, Red, xyPoint);
                CIE1931Point pBC = ReferenceColorConverter.GetClosestPointToPoint(Lime, Blue, xyPoint);

                //Get the distances per point and see which point is closer to our Point.
                double dAB = ReferenceColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pAB);
                double dAC = ReferenceColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pAC);
                double dBC = ReferenceColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pBC);

                double lowest = dAB;
                CIE1931Point closestPoint = pAB;

                if (dAC < lowest)
                {
                    lowest = dAC;
                    closestPoint = pAC;
                }
                if (dBC < lowest)
                {
                    lowest = dBC;
                    closestPoint = pBC;
                }

                //Change the xy value to a value which is within the reach of the lamp.
                cx = closestPoint.x;
                cy = closestPoint.y;
            }

            return new CIE1931Point(cx, cy);
        }

        /// <summary>
        ///  Method to see if the given XY value is within the reach of the lamps.
        /// </summary>
        /// <param name="p">p the point containing the X,Y value</param>
        /// <returns>true if within reach, false otherwise.</returns>
        public static bool CheckPointInLampsReach(CIE1931Point p)
        {
            CIE1931Point v1 = new CIE1931Point(Lime.x - Red.x, Lime.y - Red.y);
            CIE1931Point v2 = new CIE1931Point(Blue.x - Red.x, Blue.y - Red.y);

            CIE1931Point q = new CIE1931Point(p.x - Red.x, p.y - Red.y);

            double s = ReferenceColorConverter.CrossProduct(q, v2) / ReferenceColorConverter.CrossProduct(v1, v2);
            double t = ReferenceColorConverter.CrossProduct(v1, q) / ReferenceColorConverter.CrossProduct(v1, v2);

            if ((s >= 0.0f) && (t >= 0.0f) && (s + t <= 1.0f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates crossProduct of two 2D vectors / points.
        /// </summary>
        /// <param name="p1"> p1 first point used as vector</param>
        /// <param name="p2">p2 second point used as vector</param>
        /// <returns>crossProduct of vectors</returns>
        private static double CrossProduct(CIE1931Point p1, CIE1931Point p2)
        {
            return (p1.x * p2.y - p1.y * p2.x);
        }

        /// <summary>
        /// Find the closest point on a line.
        /// This point will be within reach of the lamp.
        /// </summary>
        /// <param name="A">A the point where the line starts</param>
        /// <param name="B">B the point where the line ends</param>
        /// <param name="P">P the point which is close to a line.</param>
        /// <returns> the point which is on the line.</returns>
        private static CIE1931Point GetClosestPointToPoint(CIE1931Point A, CIE1931Point B, CIE1931Point P)
        {
            CIE1931Point AP = new CIE1931Point(P.x - A.x, P.y - A.y);
            CIE1931Point AB = new CIE1931Point(B.x - A.x, B.y - A.y);
            double ab2 = AB.x * AB.x + AB.y * AB.y;
            double ap_ab = AP.x * AB.x + AP.y * AB.y;

            double t = ap_ab / ab2;

            if (t < 0.0f)
                t = 0.0f;
            else if (t > 1.0f)
                t = 1.0f;

            CIE1931Point newPoint = new CIE1931Point(A.x + AB.x * t, A.y + AB.y * t);
            return newPoint;
        }

        /// <summary>
        /// Find the distance between two points.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>the distance between point one and two</returns>
        private static double GetDistanceBetweenTwoPoints(CIE1931Point one, CIE1931Point two)
        {
            double dx = one.x - two.x; // horizontal difference
            double dy = one.y - two.y; // vertical difference
            double dist = Math.Sqrt(dx * dx + dy * dy);

            return dist;
        }
        
        /// <summary>
        /// Get the RGB color from an XY value
        /// </summary>
        /// <param name="xNumber"></param>
        /// <param name="yNumber"></param>
        /// <returns></returns>
        public static RGBColor RgbFromXy(double xNumber, double yNumber)
        {
            if (xNumber == 0 && yNumber == 0)
            {
                return new RGBColor(1.0, 1.0, 1.0);
            }

            int closestValue = Int32.MaxValue;
            int closestX = 0, closestY = 0;

            double fX = xNumber;
            double fY = yNumber;

            int intX = (int)(fX * factor);
            int intY = (int)(fY * factor);

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    int differenceForPixel = 0;
                    differenceForPixel += Math.Abs(xArray[x, y] - intX);
                    differenceForPixel += Math.Abs(yArray[x, y] - intY);

                    if (differenceForPixel < closestValue)
                    {
                        closestX = x;
                        closestY = y;
                        closestValue = differenceForPixel;
                    }
                }
            }

            int color = cArray[closestX, closestY];
            int red = (color >> 16) & 0xFF;
            int green = (color >> 8) & 0xFF;
            int blue = color & 0xFF;

            return new RGBColor(red / 255.0, green / 255.0, blue / 255.0);
        }
    }
}
