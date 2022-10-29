class Node
{
  public int LowLink { get; set; }
  public int Index { get; set; }
  public int N { get; }
  public bool Visitado { get; set; }

  public Node(int n)
  {
    N = n;
    Index = -1;
    LowLink = 0;
    Visitado = false;
  }

  public override string ToString()
  {
    return $"{{Tarjan.Graph n = {N}}}";
  }
}
