class Edge
{
  public Node a = default!;
  public Node b = default!;
  public int h = default!;
  public int l = default!;

  public Edge(Node node, int l, int h)
  {
    a = node;
    this.h = h;
    this.l = l;
  }

  public Edge(Node nodeA, Node nodeB)
  {
    a = nodeA;
    b = nodeB;
  }
}
