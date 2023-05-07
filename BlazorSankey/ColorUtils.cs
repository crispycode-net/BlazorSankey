namespace Codeus.BlazorSankey
{    
    public static class ColorUtils
    {
        public static string GetRainbowColor(int index, int count)
        {
            return ColorUtils.InterpolateRainbow((float)index / (count));
        }

        private static string InterpolateRainbow(float value)
        {
            float h = value * 360f;
            float s = 1.0f;
            float l = 0.5f;

            return HslToRgb(h, s, l);
        }

        private static string HslToRgb(float h, float s, float l)
        {
            float c = (1 - Math.Abs(2 * l - 1)) * s;
            float x = c * (1 - Math.Abs((h / 60f) % 2 - 1));
            float m = l - c / 2;

            float r = 0, g = 0, b = 0;

            if (0 <= h && h < 60)
            {
                r = c; g = x; b = 0;
            }
            else if (60 <= h && h < 120)
            {
                r = x; g = c; b = 0;
            }
            else if (120 <= h && h < 180)
            {
                r = 0; g = c; b = x;
            }
            else if (180 <= h && h < 240)
            {
                r = 0; g = x; b = c;
            }
            else if (240 <= h && h < 300)
            {
                r = x; g = 0; b = c;
            }
            else if (300 <= h && h < 360)
            {
                r = c; g = 0; b = x;
            }

            r = Math.Clamp((r + m) * 255, 0, 255);
            g = Math.Clamp((g + m) * 255, 0, 255);
            b = Math.Clamp((b + m) * 255, 0, 255);

            return $"rgb({(int)r}, {(int)g}, {(int)b})";
        }
    }

}
