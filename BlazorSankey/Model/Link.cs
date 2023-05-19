using System.Globalization;
using System.Text.Json.Serialization;

namespace CrispyCode.BlazorSankey.Model
{
    public class Link
    {
        public Link(int sourceId, int targetId, double value)
        {
            SourceId = sourceId;
            TargetId = targetId;
            Value = value;
        }

        public Link(object sourceId, object targetId, double value, string? title = null, string? startColor = null, string? endColor = null)
        {
            SourceId = sourceId;
            TargetId = targetId;
            Value = value;
            Title = title;
            StartColor = startColor;
            EndColor = endColor;
        }

        public object SourceId { get; set; }
        public object TargetId { get; set; }
        public double? Value { get; }
        public string? Title { get; }
        public string? StartColor { get; set; }
        public string? EndColor { get; set;  }
        public int Index { get; internal set; }

        [JsonIgnore]
        public Node? Source { get; internal set; }

        [JsonIgnore]
        public Node? Target { get; internal set; }

        public double Width { get; internal set; }
        public double Y0 { get; internal set; }
        public double Y1 { get; internal set; }

        public string pTitle => Title ?? $"{Source?.Name} → {Target?.Name}";
        public string pX1 => $"{Source?.X1.ToString(CultureInfo.InvariantCulture)}";
        public string pX2 => $"{Target?.X0.ToString(CultureInfo.InvariantCulture)}";
        public string pId => $"{SourceId}->{TargetId}";
        public string pPath => HorizontalLink.GenerateHorizontalLinkPathData(Source?.X1, Y0, Target?.X0, Y1);
        public string pWidth => $"{Math.Max(1.0, Width).ToString(CultureInfo.InvariantCulture)}";
    }
}
