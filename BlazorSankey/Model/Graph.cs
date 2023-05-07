namespace Codeus.BlazorSankey.Model
{
    public class Graph
    {
        public Graph(List<Node> nodes, List<Link> links, double x0, double y0, double x1, double y1, Func<Node, int, double> align, double nodeWidth = 24, double nodePadding = 8)
        {
            Nodes = nodes;
            Links = links;
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
            dx = nodeWidth;
            dy = nodePadding;
            Align = align;
        }

        public Func<Node, int, double> Align { get; set; }

        internal double x0, y0 = 0;
        internal double x1, y1 = 1;
        internal double dx = 24; // nodeWidth
        internal double dy = 8, py; // nodePadding

        public List<Node> Nodes { get; private set; } = new List<Node>();
        public List<Link> Links { get; private set; } = new List<Link>();
    }
}
