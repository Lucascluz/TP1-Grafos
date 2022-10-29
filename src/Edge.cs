class Edge
{
  public Node a = default!;
  public int h = default!;
  public int l = default!;

  public Edge(Node node, int l, int h)
  {
    a = node;
    this.h = h;
    this.l = l;
  }
}
