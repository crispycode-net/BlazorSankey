using Codeus.BlazorSankey;
using Codeus.BlazorSankey.Model;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanInitialize()
        {
            var nodes = new List<Node> { 
                new Node(1, "Node 1"),
                new Node(2, "Node 2"),
                new Node(3, "Node 3")
            };
            var links = new List<Link>
            {
                new Link(1, 2, 10),
                new Link(1, 3, 5),
                new Link(2, 3, 3),
            };

            var graphBuilder = new GraphBuilder();
            var graph = graphBuilder.Build(nodes, links, 200, 100, Alignments.AlignJustify);
        }
    }
}