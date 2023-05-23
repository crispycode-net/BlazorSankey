using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CrispyCode.BlazorSankeyInput.Shared
{
    public partial class InputContainer
    {
        [Inject] IJSRuntime? JS { get; set; }
        private Diagram? Diagram { get; set; }
        private bool initComplete;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await Task.Delay(100);

                var module = await JS!.InvokeAsync<IJSObjectReference>("import", "./_content/Z.Blazor.Diagrams/script.js");

                initComplete = true;
                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var options = new DiagramOptions
            {
                DeleteKey = "Delete", // What key deletes the selected nodes/links
                DefaultNodeComponent = null, // Default component for nodes
                AllowMultiSelection = true, // Whether to allow multi selection using CTRL
                Links = new DiagramLinkOptions
                {
                    // Options related to links
                },
                Zoom = new DiagramZoomOptions
                {
                    Minimum = 0.5, // Minimum zoom value
                    Inverse = false, // Whether to inverse the direction of the zoom when using the wheel
                                     // Other
                }
            };

            Diagram = new Diagram(options);
            Setup();
        }

        private void Setup()
        {
            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            var node3 = NewNode(300, 50);
            Diagram?.Nodes.Add(new[] { node1, node2, node3 });
            Diagram?.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left)));
        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            foreach (PortAlignment portAlignment in Enum.GetValues(typeof(PortAlignment)))
            {
                if (node.GetPort(portAlignment) == null)
                {
                    node.AddPort(portAlignment);
                }
            }

            return node;
        }

        private void AddNodeClicked()
        {
            var node = NewNode(50, 50);
            Diagram?.Nodes.Add(node);
        }
    }
}