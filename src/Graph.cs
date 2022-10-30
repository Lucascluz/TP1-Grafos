// Classe que representa o grafo com as implementações necessárias.
class Graph
{
  static readonly Random random = new Random();

  // Hash set que representa os vértices do grafo.
  public HashSet<Node> Nodes { get; set; } = new HashSet<Node>();

  // Dictionary que representa a lista de adjacência dos vértices.
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

  // Copia o graFo sem uma determianda aresta.
  public Graph CopyWithoutEdge(Node a, Node b)
  {
    var graph = this.Copy();

    graph.RemoveEdge(a, b);

    return graph;
  }

  // Exporta as arestas do grafo para uma string.
  public String Export()
  {
    var export = "";

    foreach (var node in Adj)
    {
      foreach (var neighbour in node.Value)
      {
        export += $"{node.Key.N} {neighbour.N}\n";
      }
    }

    return export;
  }

  // Importa um grafo a partir de uma string.
  public static Graph Import(String input)
  {
    var g = new Graph();

    foreach (var item in input.Split("\n").SkipLast(1))
    {
      var x = item.Split(" ");

      var a = new Node(int.Parse(x[0]));
      var b = new Node(int.Parse(x[1]));

      g.Add(a, b);
    }

    return g;
  }

  // Preenche um grafo a partir de uma certa regra de geração.
  public static Graph FillGraph(int size, GraphType type)
  {
    switch (type)
    {
      case GraphType.euler:
        return Euleriano(size);
      case GraphType.semiEuler:
        return SemiEuleriano(size);
      case GraphType.nonEuler:
        return NaoEuleriano(size);
      default:
        return new Graph();
    }
  }

  // Inicializa os vértices de um grafo.
  public static Graph InitVertices(int vertexCount)
  {
    var grafo = new Graph();

    for (int i = 0; i < vertexCount; i++)
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

  // Rotula os nós em orden e cria uma pilha de descoberta dos vértices.
  public Tuple<Graph, Stack<Node>> RootedTree(Node pai, Node node)
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

    // O vértice inicial tem o maior Index.
    pai.Index = n--;
    node.Index = n--;

    pai = node;
    while (true)
    {
      pai = q.Pop();

      foreach (var filho in Adj[pai])
      {
        // Verifica se o vértice foi descoberto.
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

      // Se o novo grafo tem o mesmo número de vértices do anterior, todos vértices foram descobertos e numerados.
      if (grafo.Nodes.Count == Nodes.Count || q.Count == 0)
      {
        return new Tuple<Graph, Stack<Node>>(grafo, s);
      }
    }
  }

  public bool TarjanPontes(Node pai, Node filho)
  {
    // Armazena o L e H de um determiando Node (vértice).
    var conexoes = new HashSet<Edge>();

    // Remover ciclos para formar uma árvore enraizada.
    var item = RootedTree(pai, filho);
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

        // Quando o node não tem filhos, é possivel calcular seu L(node) e H(node).
        if (item.Item1.Adj[node].Count - 1 == 0)
        {
          var max = node.Index;

          // Descobre o maior index no entorno deste nó (sem considerar o do pai).
          foreach (var noo in Adj[node])
          {
            if (noo.Index != node.Index + 1)
            {
              max = Math.Max(max, noo.Index);
            }
          }

          conexoes.Add(new Edge(node, node.Index + 1, max));
        }

        // Descobre o menor index do entorno deste nó.
        if (no.Index < menorNo.Index)
        {
          menorNo = no;
        }
      }

      // Adiciona o L e H dos Nodes necessários às conexões.
      if (menorNo != node)
      {
        conexoes.Add(new Edge(node, Math.Min(node.Index - node.LowLink + 1, menorNo.Index - menorNo.LowLink + 1), Math.Max(node.Index, conexoes.ElementAt(conexoes.Count - 1).h)));
      }

      node.LowLink = quantFilhos;

      // Se H(Filho) <= Filho.Index e L(Filho) > Filho.Index - ND(Filho) então (Pai,Filho) é uma ponte.
      if (node == pai)
      {
        return conexoes.ElementAt(conexoes.Count - 2).l > filho.Index - filho.LowLink && conexoes.ElementAt(conexoes.Count - 2).h <= filho.Index; ;
      }

      node = s.Pop();
    }
  }

  // Verifica se a aresta existe.
  bool VerificaArestaExiste(Node a, Node b)
  {
    return Adj[a].Contains(b);
  }

  // Verifica se a aresta é par.
  bool VerificaArestaPar(Node a, Node b)
  {
    return (Adj[a].Count) % 2 == 0 && (Adj[b].Count) % 2 == 0;
  }

  // Executa o Método de Fleury.
  public bool PrintEulerTourFleury(GraphMethod method)
  {
    var impares = new List<Node>();

    var u = Nodes.ElementAt(0);

    // Acha os vértices de grau ímpar.
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

    // Chama o Método com Naïve.
    if (method == GraphMethod.naive)
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
    else // Chama o Método com Tarjan.
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

  // Percorre o trajeto/caminho euleriano .
  void PrintEulerUtilNaive(Node newU)
  {
    var us = new Queue<Node>();

    us.Enqueue(newU);

    while (true)
    {
      if (us.Count == 0) break;

      var next = us.Dequeue();

      // Percorre todos os vértices adjacentes a u.
      foreach (var node in Adj[next])
      {
        if (Adj[next].Count > 1)
        {
          // Se aresta u-v é válida, passa para a próxima.
          if (!NaiveHasBridge(next, node))
          {
            // Console.Write("(" + next.N + "-" + node.N + "), ");

            // Essa aresta é utilizada então é removida agora.
            RemoveEdge(next, node);

            us.Enqueue(node);

            break;
          }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), "); 
          RemoveEdge(next, node);

          // Esse vértice não tem mais adjacentes, então é excluido.
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

      // Percorre todos os vértices adjacentes a u.
      foreach (var node in Adj[next])
      {
        if (Adj[next].Count > 1)
        {
          // Se aresta u-v é válida, passa para a próxima.
          if (!TarjanPontes(next, node))
          {
            // Console.Write("(" + next.N + "-" + node.N + "), ");

            // Essa aresta é utilizada então é removida agora.
            RemoveEdge(next, node);

            us.Enqueue(node);

            break;
          }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), ");
          RemoveEdge(next, node);

          // Esse vértice não tem mais adjacentes, então é excluido.
          RemoveNode(next);

          us.Enqueue(node);
        }
      }
    }
  }

  // Método Naïve.
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

  // Gerar um número aleatório entre o mínimo e o máximo.
  static int RandomNumber(int min, int max)
  {
    return random.Next(min, max);
  }

  // Remover uma aresta entre dois vértices.
  void RemoveEdge(Node a, Node b)
  {
    Adj[a].Remove(b);
    Adj[b].Remove(a);
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

  // Gerar um grafo Euleriano.
  public static Graph Euleriano(int vertexCount)
  {
    var g = InitVertices(vertexCount);

    vertexCount--;

    int vertice1 = 0;
    var a = g.Nodes.ElementAt(0);
    var b = g.Nodes.ElementAt(vertexCount);

    g.Add(a, b);

    while (vertice1 < vertexCount)
    {
      a = g.Nodes.ElementAt(vertice1);
      b = g.Nodes.ElementAt(vertice1 + 1);

      g.Add(a, b);

      vertice1++;
    }

    return g;
  }

  // Gerar um grafo Semi-euleriano.
  public static Graph SemiEuleriano(int vertexCount)
  {
    var g = Euleriano(vertexCount);

    g.Add(g.Nodes.ElementAt(0), g.Nodes.ElementAt(vertexCount / 2 + 1));

    return g;
  }

  // Gerar um grafo Não-euleriano.
  public static Graph NaoEuleriano(int vertexCount)
  {
    var g = Euleriano(vertexCount);

    for (int i = 0; i < 2;)
    {
      var vertice1 = RandomNumber(0, vertexCount);
      var vertice2 = RandomNumber(0, vertexCount);

      if (vertice1 != vertice2 && !g.VerificaArestaExiste(g.Nodes.ElementAt(vertice1), g.Nodes.ElementAt(vertice2)) && g.VerificaArestaPar(g.Nodes.ElementAt(vertice1), g.Nodes.ElementAt(vertice2)))
      {
        g.Add(g.Nodes.ElementAt(vertice1), g.Nodes.ElementAt(vertice2));
        i++;
      }
    }

    return g;
  }
}
