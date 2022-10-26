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
  static readonly Random random = new Random();
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

  public Graph CopyWithoutAresta(Node a, Node b)
  {
    var graph = this.Copy();

    graph.removeAresta(a, b);

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

  void removeAresta(Node a, Node b)
  {
    Adj[a].Remove(b);
    Adj[b].Remove(a);
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
  public void Tarjan2()
  {
    var index = 0; // number of nodes
    var s = new Stack<Node>();
    /* Encontre uma árvore de extensão mínima de {\displaystyle G}G
    Crie uma árvore enraizada {\displaystyle T}T a partir da árvore de extensão mínima
    Atravesse a árvore {\displaystyle T}T em pós-ordem e numere os nodos. Nodos pai na árvore agora tem números mais altos do que os nodos filho.
    Para cada nodo de 1 a {\displaystyle v_{1}}v_{1} (o nodo raiz da árvore) faça:
    Calcule o número de descendentes {\displaystyle ND(v)}{\displaystyle ND(v)} para este nodo.
    Calcule {\displaystyle L(v)}{\displaystyle L(v)} e {\displaystyle H(v)}{\displaystyle H(v)}
    para cada {\displaystyle w}w tal que {\displaystyle v\to w}{\displaystyle v\to w}: se {\displaystyle H(w)\leq w}{\displaystyle H(w)\leq w} e {\displaystyle L(w)>w-ND(w)}{\displaystyle L(w)>w-ND(w)} então {\displaystyle (v,w)}{\displaystyle (v,w)} é uma ponte. */

  }

  bool VerificaArestaExiste(Node a, Node b)
  {
    if (Adj[a].Contains(b))
    {
      return true;
    }
    return false;
  }

  bool VerificaArestaPar(Node a, Node b)
  {
    if ((Adj[a].Count) % 2 == 0 && (Adj[b].Count) % 2 == 0)
    {
      return true;
    }
    return false;
  }

  public bool printEulerTourNaive(bool naiveTarjan)
  {

    // Acha o vertice de grau impar
    var impares = new List<Node>();
    Node u = Nodes.ElementAt(0);
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

    // for (int i = 0; i < Nodes.Count; i++)
    // {
    //   if (Adj[i].Count % 2 == 1)
    //   {
    //     impares.Add(i);
    //     if (impares.Count > 2)
    //     {
    //       return false;
    //     }
    //   }
    // }

    // Printa tour a partir do impar V
    // Console.Write("{");

    //Naive = false, tarjan = true
    if (!naiveTarjan)
    {
      if (impares.Count != 0)
      {
        printEulerUtilNaive(impares[0]);
      }
      else
      {
        printEulerUtilNaive(u);
      }
    }
    else
    {
      if (impares.Count != 0)
      {
        printEulerUtilTarjan(impares[0]);
      }
      else
      {
        printEulerUtilTarjan(u);
      }
    }
    // Console.Write("}");
    // Console.WriteLine();

    return true;
  }

  void printEulerUtilNaive(Node newU)
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
        foreach (var v in Adj[node])
        {
          // Se aresta u-v é valida, passa para a proxima
          if (NaiveHasBridge(next, v)) ///MECHER AQUI PARA JUNTAR Fleury
          {
            // Console.Write("(" + next + "-" + v + "), ");

            // Essa aresta é utlizada então é removida agora
            removeAresta(next, v);

            us.Enqueue(v);
          }
        }
      }
      // for (int i = 0; i < Adj[next].Count; i++)
      // {
      //   int v = Adj[next][i];

      //   // Se aresta u-v é valida, passa para a proxima
      //   if (isValidNextAresta(next, v)) ///MECHER AQUI PARA JUNTAR Fleury
      //   {
      //     Console.Write("(" + next + "-" + v + "), ");

      //     // Essa aresta é utlizada então é removida agora
      //     removeAresta(next, v);

      //     us.Enqueue(v);
      //   }
      // }
    }
  }

  void printEulerUtilTarjan(Node newU)
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
        foreach (var v in Adj[node])
        {
          // Se aresta u-v é valida, passa para a proxima
          if (NaiveHasBridge(next, v)) ///MECHER AQUI PARA JUNTAR Fleury
          {
            // Console.Write("(" + next + "-" + v + "), ");

            // Essa aresta é utlizada então é removida agora
            removeAresta(next, v);

            us.Enqueue(v);
          }
        }
      }
      // for (int i = 0; i < Adj[next].Count; i++)
      // {
      //   int v = Adj[next][i];

      //   // Se aresta u-v é valida, passa para a proxima
      //   if (isValidNextAresta(next, v)) ///MECHER AQUI PARA JUNTAR Fleury
      //   {
      //     Console.Write("(" + next + "-" + v + "), ");

      //     // Essa aresta é utlizada então é removida agora
      //     removeAresta(next, v);

      //     us.Enqueue(v);
      //   }
      // }
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
    var newGraph = this.CopyWithoutAresta(a, b);

    // Verificar se o grafo copiado é conexo.
    var isConnected = newGraph.IsConnected();

    if (!isConnected)
    {
      return true;
    }

    return false;
  }

  static int RandomNumber(int min, int max)
  {
    return random.Next(min, max);
  }
  public static Graph IniciaVertices(int totalvertice)
  {
    Graph grafo = new Graph();
    for (int i = 0; i < totalvertice; i++)
    {
      grafo.Add(new Node(i));
    }

    return grafo;
  }

  public static Graph Eureliano(int totalvertice)
  {
    Graph grafoEureliano = IniciaVertices(totalvertice);
    totalvertice--;
    int vertice1 = 0;
    var a = grafoEureliano.Nodes.ElementAt(0);
    var b = grafoEureliano.Nodes.ElementAt(totalvertice);

    grafoEureliano.Add(a, b);
    while (vertice1 < totalvertice)
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
    Graph grafoSemiEureliano = Eureliano(totalvertice);

    grafoSemiEureliano.Add(grafoSemiEureliano.Nodes.ElementAt(0), grafoSemiEureliano.Nodes.ElementAt(totalvertice / 2 + 1));

    return grafoSemiEureliano;
  }

  public static Graph NaoEureliano(int totalvertice)
  {
    Graph grafoNaoEureliano = Eureliano(totalvertice);
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

  // Eureliano = 0, SemiEureliano = 1, NãoEureliano = 2
  public static Graph PreecherGrafo(int tamanho, int tipoGrafo)
  {
    switch (tipoGrafo)
    {
      case 0:
        return Eureliano(tamanho);
      case 1:
        return SemiEureliano(tamanho);
      case 2:
        return NaoEureliano(tamanho);
      //se tamanho = impar não pode ser eureliano nem semi
      default:
        Graph g1 = new Graph();
        return g1;
    }
  }
}
