using CrispyCode.BlazorSankey.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace CrispyCode.BlazorSankey
{
    public partial class SankeyDiagram
    {
        /// <summary>
        /// The text that will be rendered if no data is available
        /// </summary>
        [Parameter] public string NoDataHint { get; set; } = "No Data";

        /// <summary>
        /// Set this to a css length unit. Default is 100%.
        /// </summary>
        [Parameter] public string Width { get; set; } = "100%";

        /// <summary>
        /// Set this to a css length unit. Default is 100%.
        /// </summary>
        [Parameter] public string Height { get; set; } = "100%";

        /// <summary>
        /// Used to set the data as json string. If this parameter is used, the Nodes and Links parameters will be ignored.
        /// The format of the json string is a serialized SankeyData object.
        /// After serializing the SankeyData object, you need to replace the double quotes with single quotes.
        /// </summary>
        [Parameter] public string? JsonData { get; set; } = null;

        /// <summary>
        /// Used to set the node data directly as a list of Node objects. If the JsonData parameter is used, this parameter will be ignored.
        /// </summary>
        [Parameter] public List<Node> Nodes { get; set; } = new List<Node>();

        /// <summary>
        /// Used to set the link data directly as a list of links. If the JsonData parameter is used, this parameter will be ignored.
        /// </summary>
        [Parameter] public List<Link> Links { get; set; } = new List<Link>();

        /// <summary>
        /// Register a callback function to get notified when a user clicks on a node
        /// </summary>
        [Parameter] public EventCallback<Node> OnNodeClicked { get; set; }

        /// <summary>
        /// Register a callback function to get notified when a user clicks on a link
        /// </summary>
        [Parameter] public EventCallback<Link> OnLinkClicked { get; set; }


        [Inject] IJSRuntime? JSRuntime { get; set; }

        private string StyleString => $"width:{Width}; height:{Height};";
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

                var nodes = Nodes;
                var links = Links;
                if (!string.IsNullOrWhiteSpace(JsonData))
                {
                    var jsonData = JsonData.Replace("'", "\"");
                    var sankeyData = JsonSerializer.Deserialize<SankeyData>(jsonData);
                    if (sankeyData != null)
                    {
                        nodes = sankeyData.Nodes?
                            .Select(x => new Node(x.Id, x.Name, x.Opacity ?? 0.6))
                            .ToList()
                            ?? new List<Node>();

                        links = sankeyData.Links?
                            .Select(x => new Link(x.SourceId, x.TargetId, x.Value!.Value))
                            .ToList()
                            ?? new List<Link>();
                    }
                }

                graph = graphBuilder.Build(nodes, links, dimensions.Width, dimensions.Height, Alignments.AlignJustify, 20, 8);

                StateHasChanged();
            }
        }

        /// <summary>
        /// Will return the actual data as json or an empty json object if no data is available
        /// </summary>
        /// <returns></returns>
        public string GetDataAsJson()
        {
            // if graph is not empty, convert object to json
            if (graph != null)
            {
                var sankeyData = new SankeyData {
                    Nodes = graph.Nodes.Select(n => new NodeData { Id = (int)n.Id, Name = n.Name }),
                    Links = graph.Links.Select(l => new LinkData { SourceId = (int)l.SourceId, TargetId = (int)l.TargetId, Value = l.Value })
                };
                
                return JsonSerializer.Serialize(sankeyData);
            }

            // if graph is empty, return empty json
            return "{}";
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