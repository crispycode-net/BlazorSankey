using System.Globalization;

namespace Codeus.BlazorSankey
{
    public class HorizontalLink
    {
        public static string GenerateHorizontalLinkPathData(double? x0, double? y0, double? x1, double? y1)
        {
            double? controlPointX = x0 + (x1 - x0) / 2;
            double? controlPointY0 = y0;
            double? controlPointY1 = y1;

            // M: Move to command, C: Cubic Bezier curve command
            return $"M {x0?.ToString(CultureInfo.InvariantCulture)},{y0?.ToString(CultureInfo.InvariantCulture)} C {controlPointX?.ToString(CultureInfo.InvariantCulture)},{controlPointY0?.ToString(CultureInfo.InvariantCulture)} {controlPointX?.ToString(CultureInfo.InvariantCulture)},{controlPointY1?.ToString(CultureInfo.InvariantCulture)} {x1?.ToString(CultureInfo.InvariantCulture)},{y1?.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
