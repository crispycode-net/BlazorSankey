namespace BlazorSankey.Model
{
    public class Alignments
    {
        public static double AlignJustify(Node node, int n)
        {
            return node.SourceLinks.Any() ? node.Depth : n - 1;
        }
    }
}
