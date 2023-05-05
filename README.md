# BlazorSankey

This repo contains a sankey diagram component that can be used in Blazor applications. At the moment the implementation is very basic but should already be useful for some cases.

The BlazorSankeyTest project shows a sample - the usage should be qiet straight forward:

```
@code {
    private List<Node> nodes = new List<Node> {
        new Node(1, "Client Devices"),
        new Node(2, "Load Balancer"),
        new Node(3, "Web Server 1"),
        new Node(4, "Web Server 2"),
        new Node(5, "Database Server"),
        new Node(6, "Cache Server"),
        new Node(7, "Storage Server"),
        new Node(8, "API Server"),
        new Node(9, "External Services")
    };

    private List<Link> links = new List<Link> {
        new Link(1, 2, 5000),
        new Link(2, 3, 2500),
        new Link(2, 4, 2500),
        new Link(3, 5, 1000),
        new Link(3, 6, 1500),
        new Link(4, 5, 1000),
        new Link(4, 6, 1500),
        new Link(5, 7, 2000),
        new Link(6, 7, 3000),
        new Link(3, 8, 800),
        new Link(4, 8, 800),
        new Link(8, 9, 1600)
    };
}
```
