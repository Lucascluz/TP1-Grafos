// Classe que representa o grafo com as implementações necessárias.
class Graph
{
  static readonly Random random = new Random();

  // Hash set que representa os vértices do grafo.
  public HashSet<Node> Nodes { get; set; } = new HashSet<Node>();

  // Dictionary que representa a lista de adjacência dos vértices.
  public Dictionary<Node, HashSet<Edge>> Adj { get; set; } = new Dictionary<Node, HashSet<Edge>>();

  // Adicionar apenas um vértice ao grafo.
  public void Add(Node a)
  {
    if (!Adj.ContainsKey(a))
    {
      Nodes.Add(a);
      Adj.Add(a, new HashSet<Edge>());
    }
  }

  // Adicionar uma aresta ao grafo.
  public void Add(Node a, Node b)
  {
    if (!Adj.ContainsKey(a))
    {
      Nodes.Add(a);
      Adj.Add(a, new HashSet<Edge>());
    }

    if (!Adj.ContainsKey(b))
    {
      Nodes.Add(b);
      Adj.Add(b, new HashSet<Edge>());
    }

    var edge = new Edge(b, a);

    Adj[a].Add(edge);
    Adj[b].Add(edge);
  }

  public bool BreadthFirstSearch(Node s, Node f)
  {
    // var grafo = Copy();
    var nodeCount = Nodes.Count;

    // Marcar todos os vértices como não visitados.
    foreach (var item in Nodes)
    {
      item.Index = -1;
      item.Visitado = false;
    }

    // Criar fila.
    var queue = new LinkedList<Node>();

    // Marcar e colocar na fila o vértice atual.
    s.Index = 0;
    queue.AddLast(s);

    while (queue.Any())
    {
      // Retirar o vértice da fila.
      var node = queue.First();
      queue.RemoveFirst();

      // Recuperar os vértices adjacentes ao vértice atual.
      foreach (var val in Adj[node])
      {
        // Caso o vértice não tenha sido visitado, marcá-lo e colocá-lo na fila.
        if (val.flow == 1)
        {
          if ((val.pai.Index == -1 || val.pai.Index > node.Index + 1) && val.pai != node)
          {
            val.pai.Index = node.Index + 1;
            queue.AddLast(val.pai);
          }
        }
        else
        {
          if (val.filho.Index == -1 || val.filho.Index > node.Index + 1)
          {
            val.filho.Index = node.Index + 1;
            queue.AddLast(val.filho);
          }
        }
      }

      if (queue.Contains(f))
      {
        break;
      }
    }

    if (queue.Count == 0)
    {
      return false;
    }

    var arestaMenorNivel = 0;
    foreach (var item in Adj[f])
    {
      if (item.pai.Index == f.Index - 1 && item.flow == 0)
      {
        arestaMenorNivel++;
      }
    }

    while (arestaMenorNivel > 0)
    {
      var visitedEdges = new List<Edge>();

      queue.Clear();
      queue.AddLast(f);

      int i = f.Index;

      while (queue.Any() && !queue.Contains(s))
      {
        // Retirar o vértice da fila.
        var node = queue.First();
        queue.RemoveFirst();

        // Recuperar os vértices adjacentes ao vértice atual.
        foreach (var val in Adj[node])
        {
          // Caso o vértice não tenha sido visitado, marcá-lo e colocá-lo na fila.
          if (val.pai.Index == i - 1 && val.flow == 0 && val.pai.Visitado == false)
          {
            val.pai.Visitado = true;
            i--;
            visitedEdges.Add(val);
            queue.AddLast(val.pai);

            break;
          }
          else if (val.filho.Index == i - 1 && val.flow == 1 && val.filho.Visitado == false)
          {
            val.filho.Visitado = true;

            i--;

            visitedEdges.Add(val);
            queue.AddLast(val.filho);

            break;
          }
        }

        if (queue.Contains(s))
        {
          s.Visitado = false;
          queue.Clear();

          PreencheArestas(visitedEdges);

          visitedEdges = new List<Edge>();

          queue.AddLast(f);

          i = f.Index;

          arestaMenorNivel--;
        }
      }

      arestaMenorNivel--;
    }

    // Retornar a lista de vértices visitados.
    return true;
  }

  void PreencheArestas(List<Edge> visitedNodes)
  {
    foreach (var item in visitedNodes)
    {
      if (item.flow == 0)
      {
        item.flow = 1;
      }
      else
      {
        item.flow = 0;
      }
    }
  }

  // Cria uma cópia do grafo.
  public Graph Copy()
  {
    var graph = new Graph();

    // Processar a lista de adjacência.
    var conexoes = new HashSet<Edge>();

    foreach (var node in Nodes)
    {
      graph.Add(new Node(node.N));

      foreach (var entry in Adj[node])
      {
        conexoes.Add(entry);
      }
    }

    foreach (var entry in conexoes)
    {
      var edge = new Edge(graph.Nodes.ElementAt(entry.filho.N), graph.Nodes.ElementAt(entry.pai.N));

      graph.Adj[graph.Nodes.ElementAt(entry.pai.N)].Add(edge);
      graph.Adj[graph.Nodes.ElementAt(entry.filho.N)].Add(edge);
    }

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
        if (neighbour.filho.N != node.Key.N)
        {
          export += $"{node.Key.N} {neighbour.filho.N}\n";
        }
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

  // Verifica se a aresta existe.
  bool VerificaArestaExiste(Node a, Node b)
  {
    foreach (var item in Adj[a])
    {
      if (item.filho == b)
      {
        return true;
      }
    }

    return false;
  }

  // Verifica se a aresta é par.
  bool VerificaArestaPar(Node a, Node b)
  {
    return (Adj[a].Count) % 2 == 0 && (Adj[b].Count) % 2 == 0;
  }

  // Gerar um número aleatório entre o mínimo e o máximo.
  static int RandomNumber(int min, int max)
  {
    return random.Next(min, max);
  }

  // Gerar um grafo Euleriano.
  public static Graph Euleriano(int vertexCount)
  {
    var g = InitVertices(vertexCount);

    vertexCount--;

    int vertice1 = 0;
    var a = g.Nodes.ElementAt(0);
    var b = g.Nodes.ElementAt(vertexCount);

    g.Add(b, a);

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

  public static Graph KConexo(int vertexCount)
  {
    var g = InitVertices(vertexCount);

    int vertice1 = 0;

    while (vertice1 < vertexCount)
    {
      for (int i = 0; i < vertexCount; i++)
      {
        if (i != vertice1)
        {
          var a = g.Nodes.ElementAt(vertice1);
          var b = g.Nodes.ElementAt(i);
          var edge = new Edge(b, a);

          g.Adj[a].Add(edge);
          g.Adj[b].Add(edge);
        }
      }

      vertice1++;
    }

    return g;
  }

  public static Graph KConexo(int vertexCount, int quantK, int ligacao)
  {
    var g = InitVertices(vertexCount * quantK);

    int vertice1 = 0;
    int i = 0;

    for (int j = 0; j < quantK; j++)
    {
      while (vertice1 < vertexCount + (vertexCount * j))
      {
        for (i = 0 + (vertexCount * j); i < vertexCount + (vertexCount * j); i++)
        {
          if (i != vertice1)
          {
            var a = g.Nodes.ElementAt(vertice1);
            var b = g.Nodes.ElementAt(i);
            var edge = new Edge(b, a);

            g.Adj[a].Add(edge);
            g.Adj[b].Add(edge);
          }
        }

        vertice1++;
      }
    }

    for (int l = 0; l < quantK - 1; l++)
    {
      for (int k = 0; k < ligacao; k++)
      {
        var n1 = g.Nodes.ElementAt(k + (vertexCount * l));
        var n2 = g.Nodes.ElementAt(k + (vertexCount * (l + 1)));

        var edge = new Edge(n2, n1);

        g.Adj[n1].Add(edge);
        g.Adj[n2].Add(edge);
      }
    }

    return g;
  }

  public static Graph DirecaoGrafo(int vertexCount, int linhaQuant)
  {
    var quantVerticesMeio = vertexCount * linhaQuant;
    var g = InitVertices(quantVerticesMeio + 2);
    var nInicial = g.Nodes.ElementAt(0);

    for (int i = 1; i <= linhaQuant; i++)
    {
      var n2 = g.Nodes.ElementAt(i);

      var edge = new Edge(n2, nInicial);

      g.Adj[nInicial].Add(edge);
      g.Adj[n2].Add(edge);
    }

    for (int i = 1; i <= quantVerticesMeio - linhaQuant; i++)
    {
      var n1 = g.Nodes.ElementAt(i);

      var n2 = g.Nodes.ElementAt(i + linhaQuant);

      var edge = new Edge(n2, n1);

      g.Adj[n1].Add(edge);
      g.Adj[n2].Add(edge);

      if (i % 3 != 0)
      {
        var n3 = g.Nodes.ElementAt(i + linhaQuant + 1);
        var newEdge = new Edge(n3, n1);

        g.Adj[n3].Add(newEdge);
        g.Adj[n1].Add(newEdge);
      }
    }

    var nFinal = g.Nodes.ElementAt(quantVerticesMeio + 1);

    for (int i = 0; i < linhaQuant; i++)
    {
      var n = g.Nodes.ElementAt(quantVerticesMeio - i);
      var edg = new Edge(nFinal, n);

      g.Adj[n].Add(edg);
      g.Adj[nFinal].Add(edg);
    }

    return g;
  }

  public List<List<Edge>> MaxCaminhosDisjuntos(Node s, Node f)
  {
    while (true)
    {
      var m = BreadthFirstSearch(s, f);

      if (m == false)
      {
        break;
      }
    }

    var queue = new LinkedList<Node>();

    queue.AddFirst(s);

    var caminhos = new List<List<Edge>>();

    var i = 0;

    caminhos.Add(new List<Edge>());

    while (queue.Any())
    {
      var node = queue.First();

      queue.RemoveFirst();

      foreach (var item in Adj[node])
      {
        if (item.flow == item.C)
        {
          caminhos[i].Add(item);
          item.flow = 0;

          if (item.filho == f)
          {
            queue.Clear();
            queue.AddFirst(s);

            i++;

            caminhos.Add(new List<Edge>());
          }
          else
          {
            queue.AddFirst(item.filho);
          }

          break;
        }
      }
    }

    return caminhos;
  }

  public List<List<Edge>> BuscaLigação(int s, int f)
  {
    return MaxCaminhosDisjuntos(Nodes.ElementAt(s), Nodes.ElementAt(f));
  }

  public List<List<Edge>> BuscaLigação()
  {
    return MaxCaminhosDisjuntos(Nodes.ElementAt(0), Nodes.ElementAt(Nodes.Count - 1));
  }
}
