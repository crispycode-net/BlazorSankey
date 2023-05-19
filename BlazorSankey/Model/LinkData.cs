namespace CrispyCode.BlazorSankey.Model
{
    public class LinkData
    {
        public int SourceId { get; set; }
        public int TargetId { get; set; }
        public double? Value { get; set; }
        public string? Title { get; }
        public string? StartColor { get; set; }
        public string? EndColor { get; set; }
    }
}
