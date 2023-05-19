namespace CrispyCode.BlazorSankey.Model
{
    public class SankeyData
    {
        public IEnumerable<NodeData>? Nodes { get; set; }
        public IEnumerable<LinkData>? Links { get; set; }
    }
}
