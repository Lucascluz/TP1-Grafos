class Graph
{
  static readonly Random random = new Random();
  public HashSet<Node> Nodes { get; set; } = new HashSet<Node>();
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
    var visitedNodeIndices = new Dictionary<int, bool>();

    foreach (var item in Nodes)
    {
      item.Visitado = false;
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

        var exists = visitedNodeIndices.ContainsKey(val.N);
        var notVisited = !exists || !visitedNodeIndices[val.N];

        if (notVisited)
        {
          visitedNodeIndices[val.N] = true;
          queue.AddLast(val);
        }
      }
    }

    // Criar uma lista com apenas os vértices visitados.
    var visitedNodes = new List<Node>();

    foreach (var item in visitedNodeIndices)
    {
      if (item.Value)
      {
        visitedNodes.Add(Nodes.First((node) => node.N == item.Key));
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

  public Graph CopyWithoutEdge(Node a, Node b)
  {
    var graph = this.Copy();

    graph.RemoveEdge(a, b);

    return graph;
  }

  public static Graph IniciaVertices(int totalVertice)
  {
    var grafo = new Graph();

    for (int i = 0; i < totalVertice; i++)
    {
      grafo.Add(new Node(i));
    }

    return grafo;
  }

  // Realiza uma busca em largura a partir de algum vértice para definir se o grafo é conexo.
  public bool IsConnected()
  {
    return BreadthFirstSearch(Nodes.ElementAt(0)).Count == Nodes.Count;
  }

  public Tuple<Graph, Stack<Node>> SpanningTree(Node pai, Node node)
  {
    var grafo = new Graph();

    var q = new Stack<Node>();
    var s = new Stack<Node>();

    foreach (var item in Nodes)
    {
      item.Visitado = false;
      item.Index = -1;
      item.LowLink = 0;
    }

    pai.Visitado = true;
    node.Visitado = true;

    grafo.Add(pai);
    grafo.Add(node);
    grafo.Add(pai, node);

    q.Push(pai);
    q.Push(node);

    s.Push(pai);
    s.Push(node);

    var n = Nodes.Count;

    pai.Index = n--;
    node.Index = n--;

    pai = node;
    while (true)
    {
      pai = q.Pop();

      foreach (var filho in Adj[pai])
      {
        if (filho.Visitado == false)
        {
          filho.Index = n--;
          grafo.Add(filho);
          grafo.Add(pai, filho);
          s.Push(filho);

          q.Push(pai);
          q.Push(filho);

          filho.Visitado = true;

          break;
        }

        filho.Visitado = true;
      }

      if (grafo.Nodes.Count == Nodes.Count || q.Count == 0)
      {
        return new Tuple<Graph, Stack<Node>>(grafo, s);
      }
    }
  }

  public bool TarjanPontes(Node pai, Node filho)
  {
    var conexoes = new HashSet<Edge>();

    // Remover ciclos para formar uma árvore de árvore enraizada 

    var item = SpanningTree(pai, filho);
    Nodes = item.Item1.Nodes;

    var s = item.Item2;
    var i = s.Count - 1;
    var node = s.Pop();

    while (true)
    {
      var menorNo = node;
      var quantFilhos = 0;

      foreach (var no in item.Item1.Adj[node])
      {
        if (no.Index < node.Index)
        {
          quantFilhos += 1 + no.LowLink;
        }

        if (item.Item1.Adj[node].Count - 1 == 0)
        {
          var max = node.Index;

          foreach (var noo in Adj[node])
          {
            if (noo.Index != node.Index + 1)
            {
              max = Math.Max(max, noo.Index);
            }
          }

          conexoes.Add(new Edge(node, node.Index + 1, max));
        }

        if (no.Index < menorNo.Index)
        {
          menorNo = no;
        }
      }

      if (menorNo != node)
      {
        conexoes.Add(new Edge(node, Math.Min(node.Index - node.LowLink + 1, menorNo.Index - menorNo.LowLink + 1), Math.Max(node.Index, conexoes.ElementAt(conexoes.Count - 1).h)));
      }

      node.LowLink = quantFilhos;

      if (node == pai)
      {
        return conexoes.ElementAt(conexoes.Count - 2).l > filho.Index - filho.LowLink && conexoes.ElementAt(conexoes.Count - 2).h <= filho.Index; ;
      }

      node = s.Pop();
    }
  }

  bool VerificaArestaExiste(Node a, Node b)
  {
    return Adj[a].Contains(b);
  }

  bool VerificaArestaPar(Node a, Node b)
  {
    return (Adj[a].Count) % 2 == 0 && (Adj[b].Count) % 2 == 0;
  }

  public bool PrintEulerTourNaive(bool naiveTarjan)
  {
    // Acha o vertice de grau impar
    var impares = new List<Node>();

    var u = Nodes.ElementAt(0);

    foreach (var node in Nodes)
    {
      if (Adj[node].Count % 2 == 1)
      {
        impares.Add(node);

        if (impares.Count > 2)
        {
          return false;
        }
      }
    }

    // Naive = false, tarjan = true
    if (!naiveTarjan)
    {
      if (impares.Count != 0)
      {
        PrintEulerUtilNaive(impares[0]);
      }
      else
      {
        PrintEulerUtilNaive(u);
      }
    }
    else
    {
      if (impares.Count != 0)
      {
        PrintEulerUtilTarjan(impares[0]);
      }
      else
      {
        PrintEulerUtilTarjan(u);
      }
    }

    return true;
  }

  void PrintEulerUtilNaive(Node newU)
  {
    var us = new Queue<Node>();

    us.Enqueue(newU);

    while (true)
    {
      if (us.Count == 0) break;

      var next = us.Dequeue();

      // Percorre todos os vertices adjacentes a u
      foreach (var node in Adj[next])
      {
        if (Adj[next].Count > 1)
        {
          // Se aresta u-v é valida, passa para a proxima
          if (!NaiveHasBridge(next, node))
          {
            // Console.Write("(" + next.N + "-" + node.N + "), "); // TODO: fix

            // Essa aresta é utlizada então é removida agora
            RemoveEdge(next, node);

            us.Enqueue(node);

            break;
          }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), "); // TODO: fix
          RemoveEdge(next, node);
          RemoveNode(next);

          us.Enqueue(node);
        }
      }
    }
  }

  void PrintEulerUtilTarjan(Node newU)
  {
    var us = new Queue<Node>();
    var quantVertices = Nodes.Count;

    us.Enqueue(newU);

    while (true)
    {
      if (us.Count == 0) break;

      var next = us.Dequeue();

      // Percorre todos os vertices adjacentes a u
      foreach (var node in Adj[next])
      {
        if (Adj[next].Count > 1)
        {
          // Se aresta u-v é valida, passa para a proxima
          if (!TarjanPontes(next, node))
          {
            // Console.Write("(" + next.N + "-" + node.N + "), "); // TODO: fix

            // Essa aresta é utlizada então é removida agora
            RemoveEdge(next, node);

            us.Enqueue(node);

            break;
          }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), "); // TODO: fix
          RemoveEdge(next, node);
          RemoveNode(next);

          us.Enqueue(node);
        }
      }
    }
  }

  public bool NaiveHasBridge(Node a, Node b)
  {
    // Caso não seja conexo, lançar exceção.
    if (!IsConnected())
    {
      throw new Exception("Expected a connected graph.");
    }

    // Criar uma cópia sem esse vértice do grafo atual.
    var newGraph = this.CopyWithoutEdge(a, b);

    // Verificar se o grafo copiado é conexo.
    var isConnected = newGraph.IsConnected();

    return !isConnected;
  }

  // Eureliano = 0, SemiEureliano = 1, NãoEureliano = 2
  public static Graph PreecherGrafo(int tamanho, GraphType type)
  {
    switch (type)
    {
      case GraphType.euler:
        return Eureliano(tamanho);
      case GraphType.semiEuler:
        return SemiEureliano(tamanho);
      case GraphType.nonEuler:
        return NaoEureliano(tamanho);
      // Se tamanho = impar não pode ser eureliano nem semi
      default:
        return new Graph();
    }
  }

  static int RandomNumber(int min, int max)
  {
    return random.Next(min, max);
  }

  // Remover um determinado vértice do grafo.
  public void RemoveNode(Node node)
  {
    var neighbours = Adj[node];

    foreach (var item in neighbours)
    {
      Adj[item].Remove(node);
    }

    Nodes.Remove(node);
    Adj.Remove(node);
  }

  void RemoveEdge(Node a, Node b)
  {
    Adj[a].Remove(b);
    Adj[b].Remove(a);
  }

  public static Graph Eureliano(int totalVertice)
  {
    var grafoEureliano = IniciaVertices(totalVertice);

    totalVertice--;

    int vertice1 = 0;
    var a = grafoEureliano.Nodes.ElementAt(0);
    var b = grafoEureliano.Nodes.ElementAt(totalVertice);

    grafoEureliano.Add(a, b);

    while (vertice1 < totalVertice)
    {
      a = grafoEureliano.Nodes.ElementAt(vertice1);
      b = grafoEureliano.Nodes.ElementAt(vertice1 + 1);

      grafoEureliano.Add(a, b);

      vertice1++;
    }

    return grafoEureliano;
  }

  public static Graph SemiEureliano(int totalvertice)
  {
    var grafoSemiEureliano = Eureliano(totalvertice);

    grafoSemiEureliano.Add(grafoSemiEureliano.Nodes.ElementAt(0), grafoSemiEureliano.Nodes.ElementAt(totalvertice / 2 + 1));

    return grafoSemiEureliano;
  }

  public static Graph NaoEureliano(int totalvertice)
  {
    var grafoNaoEureliano = Eureliano(totalvertice);

    for (int i = 0; i < 2;)
    {
      var vertice1 = RandomNumber(0, totalvertice);
      var vertice2 = RandomNumber(0, totalvertice);

      if (vertice1 != vertice2 && !grafoNaoEureliano.VerificaArestaExiste(grafoNaoEureliano.Nodes.ElementAt(vertice1), grafoNaoEureliano.Nodes.ElementAt(vertice2)) && grafoNaoEureliano.VerificaArestaPar(grafoNaoEureliano.Nodes.ElementAt(vertice1), grafoNaoEureliano.Nodes.ElementAt(vertice2)))
      {
        grafoNaoEureliano.Add(grafoNaoEureliano.Nodes.ElementAt(vertice1), grafoNaoEureliano.Nodes.ElementAt(vertice2));
        i++;
      }
    }

    return grafoNaoEureliano;
  }
}
