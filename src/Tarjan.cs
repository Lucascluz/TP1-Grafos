namespace Tarjan;

class Node
{
  public int LowLink { get; set; }
  public int Index { get; set; }
  public int N { get; }

  public Node(int n)
  {
    N = n;
    Index = -1;
    LowLink = 0;
  }

  public override string ToString()
  {
    return $"{{Tarjan.Graph n = {N}}}";
  }
}

class Graph
{
  public HashSet<Node> Nodes { get; } = new HashSet<Node>();
  public Dictionary<Node, HashSet<Node>> Adj { get; } = new Dictionary<Node, HashSet<Node>>();

  // Adicionar apenas um vértice ao grafo.
  public void Add(Node a)
  {
    if (!Adj.ContainsKey(a))
    {
      Nodes.Add(a);
      Adj.Add(a, new HashSet<Node>());
    }
  }

  // Adicionar uma aresta ao grafo.
  public void Add(Node a, Node b)
  {
    if (!Adj.ContainsKey(a))
    {
      Nodes.Add(a);
      Adj.Add(a, new HashSet<Node>());
    }

    if (!Adj.ContainsKey(b))
    {
      Nodes.Add(b);
      Adj.Add(b, new HashSet<Node>());
    }

    Adj[a].Add(b);
    Adj[b].Add(a);
  }

  // Realiza uma busca em largura a partir de um vértice.
  List<Node> BreadthFirstSearch(Node node)
  {
    var nodeCount = Nodes.Count;

    // Marcar todos os vértices como não visitados.
    var visitedNodeIndices = new bool[nodeCount];
    for (int i = 0; i < nodeCount; i++)
    {
      visitedNodeIndices[i] = false;
    }

    // Criar fila.
    var queue = new LinkedList<Node>();

    // Marcar e colocar na fila o vértice atual.
    visitedNodeIndices[node.N] = true;
    queue.AddLast(node);

    while (queue.Any())
    {
      // Retirar o vértice da fila.
      node = queue.First();
      queue.RemoveFirst();

      // Recuperar os vértices adjacentes ao vértice atual.
      foreach (var val in Adj[node])
      {
        // Caso o vértice não tenha sido visitado, marcá-lo e colocá-lo na fila.
        if (!visitedNodeIndices[val.N])
        {
          visitedNodeIndices[val.N] = true;
          queue.AddLast(val);
        }
      }
    }

    // Criar uma lista com apenas os vértices visitados.
    var visitedNodes = new List<Node>();
    for (int i = 0; i < visitedNodeIndices.Length; i++)
    {
      if (visitedNodeIndices[i])
      {
        visitedNodes.Add(Nodes.First((node) => node.N == i));
      }
    }

    // Retornar a lista de vértices visitados.
    return visitedNodes;
  }

  // Cria uma cópia do grafo.
  public Graph Copy()
  {
    var graph = new Graph();

    // Processar a lista de adjacência.
    foreach (var entry in Adj)
    {
      var a = entry.Key;

      foreach (var b in entry.Value)
      {
        graph.Add(a, b);
      }
    }

    // Processr a lista de vértices.
    foreach (var node in Nodes)
    {
      graph.Add(node);
    }

    return graph;
  }

  // Cria uma cópia do grafo sem um determinado vértice.
  public Graph CopyWithout(Node node)
  {
    var graph = this.Copy();

    graph.Remove(node);

    return graph;
  }

  // Realiza uma busca em largura a partir de algum vértice para definir se o grafo é conexo.
  public bool IsConnected()
  {
    return BreadthFirstSearch(Nodes.ElementAt(0)).Count == Nodes.Count;
  }

  // Método que utiliza brute force para entrontrar uma ponte, removendo cada vértice e chechando a conectividade do grafo em seguida.
  public bool NaiveHasBridge()
  {
    // Caso não seja conexo, lançar exceção.
    if (!IsConnected())
    {
      throw new Exception("Expected a connected graph.");
    }

    // Para cada vértice, criar uma cópia sem esse vértice do grafo atual.
    foreach (var node in Nodes)
    {
      var newGraph = this.CopyWithout(node);

      // Verificar se o grafo copiado é conexo.
      var isConnected = newGraph.IsConnected();

      if (!isConnected)
      {
        return true;
      }
    }

    return false;
  }

  // Remover um determinado vértice do grafo.
  public void Remove(Node node)
  {
    var neighbours = Adj[node];

    foreach (var item in neighbours)
    {
      Adj[item].Remove(node);
    }

    Nodes.Remove(node);
    Adj.Remove(node);
  }

  // TODO: fix, arrumar método
  public void Tarjan()
  {
    var index = 0; // number of nodes
    var s = new Stack<Node>();

    Action<Node> strongConnect = default!;

    strongConnect = (v) =>
    {
      // Set the depth index for v to the smallest unused index
      v.Index = index;
      v.LowLink = index;

      index++;
      s.Push(v);

      // Consider successors of v
      foreach (var w in Adj[v])
        if (w.Index < 0)
        {
          // Successor w has not yet been visited; recurse on it
          strongConnect(w);
          v.LowLink = Math.Min(v.LowLink, w.LowLink);
        }
        else if (s.Contains(w))
        {
          // Successor w is in stack S and hence in the current SCC
          v.LowLink = Math.Min(v.LowLink, w.Index);
        }

      // If v is a root node, pop the stack and generate an SCC
      if (v.LowLink == v.Index)
      {
        Console.Write("SCC: ");

        Node w;
        do
        {
          w = s.Pop();
          Console.Write(w.N + " ");
        } while (w != v);

        Console.WriteLine();
      }
    };

    foreach (var v in Nodes)
    {
      if (v.Index < 0)
      {
        strongConnect(v);
      }
    }
  }
}
