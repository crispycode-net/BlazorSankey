namespace CrispyCode.BlazorSankey.Model
{
    public class NodeData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? FixedValue { get; set; }
        public string? Color { get; set; }
        public double? Opacity { get; }
        public string? HoverText { get; set; }
    }
}
