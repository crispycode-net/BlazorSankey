using CrispyCode.BlazorSankey.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CrispyCode.BlazorSankey
{
    public partial class SankeyDiagram
    {
        [Parameter] public string Width { get; set; } = "100%";
        [Parameter] public string Height { get; set; } = "100%";
        [Parameter] public List<Node> Nodes { get; set; } = new List<Node>();
        [Parameter] public List<Link> Links { get; set; } = new List<Link>();
        [Parameter] public EventCallback<Node> OnNodeClicked { get; set; }
        [Parameter] public EventCallback<Link> OnLinkClicked { get; set; }

        public string StyleString => $"width:{Width}; height:{Height};";

        [Inject] IJSRuntime? JSRuntime { get; set; }

        private Graph? graph;
        private ElementReference svgElement;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }


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

        private void NodeClicked(Node node)
        {
            OnNodeClicked.InvokeAsync(node);
        }

        private void LinkClicked(Link link)
        {
            OnLinkClicked.InvokeAsync(link);
        }
    }
}