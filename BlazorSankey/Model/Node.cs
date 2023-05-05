using System.Globalization;

namespace BlazorSankey.Model
{
    public class Node
    {
        public Node(object id, string? name = null, double? fixedValue = null, string? color = null, double opacity = 0.6, string? hoverText = null)
        {
            Id = id;
            Name = name;
            FixedValue = fixedValue;
            Color = color;
            Opacity = opacity;
            HoverText = hoverText;
        }

        public string? Name { get; set; }
        public object Id { get; set; }
        public double? FixedValue { get; set; }
        public string? Color { get; set; }
        public double Opacity { get; }
        public string? HoverText { get; set; }
        public int Index { get; internal set; }
        public List<Link> SourceLinks { get; internal set; } = new List<Link>();
        public List<Link> TargetLinks { get; internal set; } = new List<Link>();
        public double? Value { get; internal set; }
        public int Depth { get; internal set; }
        public int Height { get; internal set; }
        public int Layer { get; internal set; }
        public double X0 { get; internal set; }
        public double X1 { get; internal set; }
        public double Y0 { get; internal set; }
        public double Y1 { get; internal set; }

        public string pX0 => $"{X0.ToString(CultureInfo.InvariantCulture)}";
        public string pX1 => $"{X1.ToString(CultureInfo.InvariantCulture)}";        
        public string pY0 => $"{Y0.ToString(CultureInfo.InvariantCulture)}";
        public string pY1 => $"{Y1.ToString(CultureInfo.InvariantCulture)}";
        public string pYMiddle => $"{(Y0 + (Y1 - Y0) / 2).ToString(CultureInfo.InvariantCulture)}";
        public string pWidth => $"{(X1 - X0).ToString(CultureInfo.InvariantCulture)}";
        public string pHeight => $"{(Y1 - Y0).ToString(CultureInfo.InvariantCulture)}";
        public string pOpacity => $"{Opacity.ToString(CultureInfo.InvariantCulture)}";

        public bool IsInLastLayer { get; internal set; }
        public string pXText => $"{ (IsInLastLayer ? X0 - 4 : X0 + (X1 - X0) + 4).ToString(CultureInfo.InvariantCulture)}";
        public string pTextAnchor => $"{ (IsInLastLayer ? "end" : "start") }";
    }
}
