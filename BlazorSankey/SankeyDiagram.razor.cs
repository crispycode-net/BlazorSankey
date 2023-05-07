using Codeus.BlazorSankey.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Codeus.BlazorSankey
{
    public partial class SankeyDiagram
    {
        [Parameter] public string Width { get; set; } = "100%";
        [Parameter] public string Height { get; set; } = "100%";
        [Parameter] public List<Node> Nodes { get; set; } = new List<Node>();
        [Parameter] public List<Link> Links { get; set; } = new List<Link>();

        [Inject] IJSRuntime? JSRuntime { get; set; }

        private Graph? graph;
        private ElementReference svgElement;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public string StyleString => $"width:{Width}; height:{Height};";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                SankeyJsInterop exampleJsInterop = new SankeyJsInterop(JSRuntime!);
                var dimensions = await exampleJsInterop.GetDimensions(svgElement);

                var graphBuilder = new GraphBuilder();
                graph = graphBuilder.Build(Nodes, Links, dimensions.Width, dimensions.Height, Alignments.AlignJustify, 20, 8);

                StateHasChanged();
            }
        }
    }
}