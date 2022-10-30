// Classe que representa um vértice do grafo.
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

  public override int GetHashCode()
  {
    return N;
  }

  public override bool Equals(object? obj)
  {
    if (obj is Node)
    {
      return ((Node)obj).N == N;
    }

    return false;
  }
}
