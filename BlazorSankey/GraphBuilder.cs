using CrispyCode.BlazorSankey.Model;

namespace CrispyCode.BlazorSankey
{
    public class GraphBuilder
    {
        public int Iterations { get; private set; }

        /// <summary>
        /// Sorting function for links of a node. Default is to sort by value.
        /// </summary>
        public Func<IEnumerable<Link>, IOrderedEnumerable<Link>> LinkSort { get; set; }

        /// <summary>
        /// Sorting function for Nodes. Default is ascending breadth.
        /// </summary>
        public Comparison<Node> NodeSort { get; set; }


        public static IOrderedEnumerable<Link> DefaultLinkSort(IEnumerable<Link> links)
        {
            return links.OrderBy(l => l.Value);
        }

        public static int AscendingBreadth(Node a, Node b)
        {
            return (int)a.Y0 - (int)b.Y0;
        }


        public GraphBuilder()
        {
            LinkSort = DefaultLinkSort;
            NodeSort = (a, b) => AscendingBreadth(a, b);
        }

        

        public Graph Build(IEnumerable<Node> nodes, IEnumerable<Link> links, double width, double height, Func<Node, int, double> align, double nodeWidth = 24, double nodePadding = 8, int iterations = 6)
        {
            var graph = new Graph(nodes.ToList(), links.ToList(), 0, 0, width, height, align, nodeWidth, nodePadding);
            Iterations = iterations;

            ComputeNodeLinks(graph);
            ComputeNodeValues(graph.Nodes);
            ComputeNodeDepths(graph.Nodes);
            ComputeNodeHeights(graph.Nodes);
            ComputeNodeBreadths(graph);
            ComputeLinkBreadths(graph.Nodes);
            ComputeLinkColors(graph);
            ComputeNodeProperties(graph);

            return graph;
        }

        private void ComputeLinkColors(Graph graph)
        {
            foreach (var link in graph.Links)
            {
                if (string.IsNullOrEmpty(link.StartColor) && link.Source != null)
                    link.StartColor = ColorUtils.GetRainbowColor(link.Source.Index, graph.Nodes.Count);
                if (string.IsNullOrEmpty(link.EndColor) && link.Target != null)
                    link.EndColor = ColorUtils.GetRainbowColor(link.Target.Index, graph.Nodes.Count);
            }
        }

        private void ComputeNodeProperties(Graph graph) 
        {
            foreach (var node in graph.Nodes)
            {
                if (string.IsNullOrEmpty(node.Color))
                    node.Color = ColorUtils.GetRainbowColor(node.Index, graph.Nodes.Count);

                if (node.HoverText == null)
                    node.HoverText = $"{node.Name}: {node.Value}";
            }            
        }
        private void ComputeLinkBreadths(List<Node> nodes) {
          foreach(var node in nodes) {
            var y0 = node.Y0;
            var y1 = y0;
            foreach(var link in node.SourceLinks) {
              link.Y0 = y0 + link.Width / 2;
              y0 += link.Width;
            }
            foreach(var link in node.TargetLinks) {
              link.Y1 = y1 + link.Width / 2;
              y1 += link.Width;
            }
          }
        }

        /// <summary>
        /// Initialize nodes and links
        /// </summary>
        /// <param name="graph"></param>
        private void ComputeNodeLinks(Graph graph)
        {
            for(int i = 0; i < graph.Nodes.Count; i++)
            {
                var node = graph.Nodes[i];
                node.Index = i;
                node.SourceLinks = new List<Link>();
                node.TargetLinks = new List<Link>();                
            }

            var nodeById = graph.Nodes.ToDictionary(n => n.Id, n => n);

            for (int i = 0; i < graph.Links.Count; i++)
            {
                var link = graph.Links[i];
                link.Index = i;
                link.Source = nodeById[link.SourceId];
                link.Target = nodeById[link.TargetId];

                link.Source.SourceLinks.Add(link);
                link.Target.TargetLinks.Add(link);
            }

            foreach (var node in graph.Nodes)
            {
                node.SourceLinks = LinkSort(node.SourceLinks).ToList();
                node.TargetLinks = LinkSort(node.TargetLinks).ToList();
            }
        }

        /// <summary>
        /// Set all nodes' values to either their given fixed value or the maximum of their source and target links
        /// </summary>
        /// <param name="nodes"></param>
        private void ComputeNodeValues(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.FixedValue != default)
                    node.Value = node.FixedValue;
                else
                    node.Value = Math.Max(node.SourceLinks.Sum(l => l.Value ?? 0), node.TargetLinks.Sum(l => l.Value ?? 0));
            }
        }

        /// <summary>
        /// The function computes the depth of each node in a directed acyclic graph (DAG). 
        /// The depth of a node is the number of steps from the starting node(s) to that node, following the directed links.
        /// The function takes a list of nodes as input. 
        /// Each node has a property sourceLinks, which contains a list of links that originate from that node. 
        /// Each link has a target node.The function uses a breadth-first search (BFS) algorithm to traverse the graph and determine the depth for each node.
        /// The function maintains two sets, current and next. 
        /// The algorithm starts with the current set containing all nodes, and iterates through the nodes, updating their depth and adding their target nodes to the next set. 
        /// The current set is then replaced by the next set, and the next set is cleared. This process repeats until there are no more nodes to process.
        /// </summary>
        /// <param name="nodes">The nodes of the graph</param>
        /// <exception cref="Exception">If the function detects a circular link (i.e., the graph is not acyclic), it throws an error.</exception>
        private void ComputeNodeDepths(IEnumerable<Node> nodes)
        {
            var n = nodes.Count();
            HashSet<Node> current = new HashSet<Node>(nodes);
            HashSet<Node> next = new HashSet<Node>();
            int x = 0;

            while (current.Count > 0)
            {
                foreach (Node node in current)
                {
                    node.Depth = x;
                    foreach (Link link in node.SourceLinks)
                    {
                        next.Add(link.Target!);
                    }
                }
                if (++x > n) throw new Exception("Circular reference detected");
                current = next;
                next = new HashSet<Node>();
            }
        }

        /// <summary>
        /// The function computes the height of each node in a directed acyclic graph (DAG). 
        /// The height of a node is the number of steps from the node to the ending node(s), following the directed links.
        /// The function takes a list of nodes as input. Each node has a property targetLinks, which contains a list of links that terminate at that node.
        /// Each link has a source node. The function uses a breadth-first search (BFS) algorithm to traverse the graph and determine the height for each node.
        /// The function maintains two sets, current and next. 
        /// The algorithm starts with the current set containing all nodes, and iterates through the nodes, updating their height and adding their source nodes to the next set. 
        /// The current set is then replaced by the next set, and the next set is cleared.This process repeats until there are no more nodes to process.
        /// </summary>
        /// <param name="nodes">The nodes of the graph</param>
        /// <exception cref="Exception">If the function detects a circular link (i.e., the graph is not acyclic), it throws an error.</exception>
        private void ComputeNodeHeights(IEnumerable<Node> nodes)
        {
            int n = nodes.Count();
            HashSet<Node> current = new HashSet<Node>(nodes);
            HashSet<Node> next = new HashSet<Node>();
            int x = 0;

            while (current.Count > 0)
            {
                foreach (Node node in current)
                {
                    node.Height = x;
                    foreach (Link link in node.TargetLinks)
                    {
                        next.Add(link.Source!);
                    }
                }
                if (++x > n) throw new Exception("Circular reference detected");
                current = next;
                next = new HashSet<Node>();
            }
        }

        private List<List<Node>> ComputeNodeLayers(Graph graph)
        {
            List<Node> nodes = graph.Nodes;
            double x0 = graph.x0;
            double x1 = graph.x1;
            double dx = graph.dx;
            var align = graph.Align;

            int x = nodes.Max(d => d.Depth) + 1;
            double kx = (x1 - x0 - dx) / (x - 1);
            List<List<Node>> columns = new List<List<Node>>(new List<Node>[x]);

            foreach (Node node in nodes)
            {
                int i = Math.Max(0, Math.Min(x - 1, (int)Math.Floor(align(node, x))));
                node.Layer = i;
                node.IsInLastLayer = node.Layer == columns.Count - 1;
                node.X0 = x0 + i * kx;
                node.X1 = node.X0 + dx;

                if (columns[i] != null)
                    columns[i].Add(node);
                else
                    columns[i] = new List<Node> { node };
            }

            if (NodeSort != null)
            {
                foreach (List<Node> column in columns)
                {
                    column.Sort(NodeSort);
                }
            }

            return columns;
        }

        private void ReorderLinks(List<Node> nodes)
        {
            // our link sort function is never null / undefined

            //if (LinkSort linkSort === undefined)
            //{
            //    for (const { sourceLinks, targetLinks}
            //    of nodes) {
            //        sourceLinks.sort(ascendingTargetBreadth);
            //        targetLinks.sort(ascendingSourceBreadth);
            //    }
            //}
        }

        private void InitializeNodeBreadths(Graph graph, List<List<Node>> columns)
        {
            var ky = columns.Min(c => (graph.y1 - graph.y0 - (c.Count - 1) * graph.py) / c.Sum(c => c.Value!.Value));

            foreach (var nodes in columns)
            {
                var y = graph.y0;
                foreach (var node in nodes)
                {
                    node.Y0 = y;
                    node.Y1 = y + (int)node.Value!.Value * ky;
                    y = node.Y1 + graph.py;
                    foreach (var link in node.SourceLinks)
                    {
                        link.Width = (int)link.Value!.Value * ky;
                    }
                }

                y = (graph.y1 - y + graph.py) / (nodes.Count + 1);
                for (int i = 0; i < nodes.Count; ++i)
                {
                    var node = nodes[i];
                    node.Y0 += y * (i + 1);
                    node.Y1 += y * (i + 1);
                }

                ReorderLinks(nodes);
            }
        }

        // Returns the target.y0 that would produce an ideal link from source to target.
        private double TargetTop(Graph graph, Node source, Node target)
        {
            var y = source.Y0 - (source.SourceLinks.Count - 1) * graph.py / 2;
            foreach (var link in target.SourceLinks)
            {
                var node = link.Target;
                var width = link.Width;
                if (node == target) break;
                y += width + graph.py;
            }
            foreach (var link in target.TargetLinks)
            {
                var node = link.Source;
                var width = link.Width;
                if (node == source) break;
                y -= width;
            }

            return y;
        }

        // Returns the source.y0 that would produce an ideal link from source to target.
        private double SourceTop(Graph graph, Node source, Node target)
        {
            var y = target.Y0 - (target.TargetLinks.Count - 1) * graph.py / 2;
            foreach (var link in target.TargetLinks)
            {
                var node = link.Source;
                var width = link.Width;
                if (node == source) break;
                y += width + graph.py;
            }
            foreach (var link in target.SourceLinks)
            {
                var node = link.Target;
                var width = link.Width;
                if (node == target) break;
                y -= width;
            }

            return y;
        }


        private void ReorderNodeLinks(List<Link> sourceLinks, List<Link> targetLinks)
        {
            /*
              if (linkSort === undefined) {
                  for (const {source: {sourceLinks}} of targetLinks) {
                    sourceLinks.sort(ascendingTargetBreadth);
                  }
                  for (const {target: {targetLinks}} of sourceLinks) {
                    targetLinks.sort(ascendingSourceBreadth);
                  }
                }
           */
        }

        private void ResolveCollisions(Graph graph, List<Node> nodes, double alpha)
        {
            var i = nodes.Count >> 1;
            var subject = nodes[i];
            ResolveCollisionsBottomToTop(graph, nodes, subject.Y0 - graph.py, i - 1, alpha);
            ResolveCollisionsTopToBottom(graph, nodes, subject.Y1 + graph.py, i + 1, alpha);
            ResolveCollisionsBottomToTop(graph, nodes, graph.y1, nodes.Count - 1, alpha);
            ResolveCollisionsTopToBottom(graph, nodes, graph.y0, 0, alpha);
        }

        private void ResolveCollisionsTopToBottom(Graph graph, List<Node> nodes, double y, int i, double alpha)
        {
            for (; i < nodes.Count; ++i)
            {
                var node = nodes[i];
                var dy = (y - node.Y0) * alpha;
                if (dy > 1e-6)
                {
                    node.Y0 += dy;
                    node.Y1 += dy;
                }
                y = node.Y1 + graph.py;
            }
        }

        private void ResolveCollisionsBottomToTop(Graph graph, List<Node> nodes, double y, int i, double alpha)
        {
            for (; i >= 0; --i)
            {
                var node = nodes[i];
                var dy = (node.Y1 - y) * alpha;
                if (dy > 1e-6)
                {
                    node.Y0 -= dy;
                    node.Y1 -= dy;
                }
                y = node.Y0 - graph.py;
            }
        }

        // Reposition each node based on its outgoing (source) links.
        private void RelaxRightToLeft(Graph graph, List<List<Node>> columns, double alpha, double beta)
        {
            for (int n = columns.Count, i = n - 2; i >= 0; --i)
            {
                var column = columns[i];
                foreach (var source in column)
                {
                    double y = 0;
                    double w = 0;

                    foreach (var link in source.SourceLinks)
                    {
                        var value = link.Value!.Value;
                        var target = link.Target!;

                        var v = value * (target.Layer - source.Layer);
                        y += SourceTop(graph, source, target) * v;
                        w += v;
                    }
                    if (!(w > 0)) continue;
                    var dy = (y / w - source.Y0) * alpha;
                    source.Y0 += dy;
                    source.Y1 += dy;
                    ReorderNodeLinks(source.SourceLinks, source.TargetLinks);
                }

                if (NodeSort == null) column.Sort(AscendingBreadth);
                ResolveCollisions(graph, column, beta);
            }
        }

        // Reposition each node based on its incoming (target) links.
        private void RelaxLeftToRight(Graph graph, List<List<Node>> columns, double alpha, double beta)
        {
            int n = columns.Count;
            for (int i = 1; i < n; ++i)            
            {
                var column = columns[i];
                foreach (var target in column)
                {
                    double y = 0;
                    double w = 0;

                    foreach (var link in target.TargetLinks)
                    {
                        var value = link.Value!.Value;
                        var source = link.Source!;

                        var v = value * (target.Layer - source.Layer);
                        y += TargetTop(graph, source, target) * v;
                        w += v;
                    }
                    if (!(w > 0)) continue;
                    var dy = (y / w - target.Y0) * alpha;
                    target.Y0 += dy;
                    target.Y1 += dy;
                    ReorderNodeLinks(target.SourceLinks, target.TargetLinks);
                }

                if (NodeSort == null) column.Sort(AscendingBreadth);
                ResolveCollisions(graph, column, beta);
            }
        }


        private void ComputeNodeBreadths(Graph graph)
        {
            var columns = ComputeNodeLayers(graph);

            graph.py = Math.Min(graph.dy, (graph.y1 - graph.y0) / (columns.Max(c => c.Count) - 1));

            InitializeNodeBreadths(graph, columns);
            for (int i = 0; i < Iterations; ++i)
            {
                var alpha = Math.Pow(0.99, i);
                var beta = Math.Max(1 - alpha, (i + 1) / Iterations);

                RelaxRightToLeft(graph, columns, alpha, beta);
                RelaxLeftToRight(graph, columns, alpha, beta);
            }
        }
    }
}
