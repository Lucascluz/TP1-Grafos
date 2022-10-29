namespace Tarjan;

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
    var visitedNodeIndices = new bool[nodeCount];

    foreach (var item in Nodes)
    {
      item.Visitado = false;
    }

    // for (int i = 0; i < nodeCount; i++)
    // {
    //   visitedNodeIndices[i] = false;
    // }

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

  // Tuple<Graph, List<Node>> BuscaEmLaguraNumeraArvoreEnraizada(Node node)
  // {
  //   var grafo = new Graph();
  //   grafo.Nodes = Nodes;
  //   var nodeCount = Nodes.Count;

  //   // Marcar todos os vértices como não visitados.
  //   var visitedNodeIndices = new bool[nodeCount];
  //   for (int i = 0; i < nodeCount; i++)
  //   {
  //     visitedNodeIndices[i] = false;
  //   }

  //   // Criar fila.
  //   var queue = new LinkedList<Node>();

  //   // Marcar e colocar na fila o vértice atual.
  //   visitedNodeIndices[node.N] = true;
  //   queue.AddLast(node);

  //   while (queue.Any())
  //   {
  //     // Retirar o vértice da fila.
  //     node = queue.First();
  //     queue.RemoveFirst();

  //     // Recuperar os vértices adjacentes ao vértice atual.
  //     foreach (var val in Adj[node])
  //     {
  //       // Caso o vértice não tenha sido visitado, marcá-lo e colocá-lo na fila.
  //       if (!visitedNodeIndices[val.N])
  //       {
  //         grafo.Add(node, val);
  //         visitedNodeIndices[val.N] = true;
  //         queue.AddLast(val);
  //       }
  //     }
  //   }

  //   // Criar uma lista com apenas os vértices visitados.
  //   var visitedNodes = new List<Node>();
  //   for (int i = 0; i < visitedNodeIndices.Length; i++)
  //   {
  //     if (visitedNodeIndices[i])
  //     {
  //       visitedNodes.Add(Nodes.First((node) => node.N == i));
  //       visitedNodes[i].Index = i;
  //     }
  //   }

  //   // Retornar a lista de vértices visitados.
  //   return new Tuple<Graph, List<Node>>(grafo, visitedNodes);
  // }

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
  // public Edge? TarjanCiclo()
  // {
  //   var index = 0; // number of nodes
  //   var s = new Stack<Node>();

  //   Func<Node, Edge?> strongConnect = default!;

  //   strongConnect = (v) =>
  //   {
  //     // Set the depth index for v to the smallest unused index
  //     v.Index = index;
  //     v.LowLink = index;

  //     index++;
  //     s.Push(v);

  //     // Consider successors of v
  //     foreach (var w in Adj[v])
  //       if (w.Index < 0)
  //       {
  //         // Successor w has not yet been visited; recurse on it
  //         var a = strongConnect(w);
  //         if (a != null)
  //           return a;
  //         v.LowLink = Math.Min(v.LowLink, w.LowLink);
  //       }
  //       else if (s.Contains(w))
  //       {
  //         // Successor w is in stack S and hence in the current SCC
  //         v.LowLink = Math.Min(v.LowLink, w.Index);
  //       }

  //     // If v is a root node, pop the stack and generate an SCC
  //     if (v.LowLink == v.Index)
  //     {
  //       Console.Write("SCC: ");
  //       var edge = new Edge();

  //       Node? w = null;
  //       do
  //       {
  //         if (w == null)
  //         {
  //           w = s.Pop();
  //           edge.a = w;
  //         }
  //         else
  //         {
  //           w = s.Pop();
  //         }
  //         Console.Write(w.N + " ");
  //       } while (w != v);
  //       edge.b = w;
  //       return edge;
  //     }

  //     return null;
  //   };

  //   foreach (var v in Nodes)
  //   {
  //     if (v.Index < 0)
  //     {
  //       var a = strongConnect(v);
  //       if (a != null)
  //         return a;
  //     }
  //   }

  //   return null;
  // }
  public Tuple<Graph, Stack<Node>> ArvoreEnraizada(Node pai, Node node/* , int quantVertices */)
  {
    var grafo = new Graph();
    // var q = new Queue<Node>();
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
    // q.Enqueue(pai);
    //// q.Enqueue(node);
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
          // q.Enqueue(pai);
          // q.Enqueue(filho);
          // pai = filho;
          filho.Visitado = true;
          break;
        }
        filho.Visitado = true;
      }


      if (grafo.Nodes.Count == Nodes.Count/* quantVertices */ || q.Count == 0)
      {
        return new Tuple<Graph, Stack<Node>>(grafo, s);
      }
    }
    //   foreach (var filho in Adj[pai])
    //   {
    //     if (filho.Visitado == false)
    //     {
    //       filho.Index = n--;
    //       grafo.Add(filho);
    //       grafo.Add(pai, filho);
    //       s.Push(filho);

    //       q.Enqueue(pai);
    //       // q.Enqueue(filho);
    //       pai = filho;
    //     }
    //     filho.Visitado = true;
    //   }
    //   pai = q.Dequeue();

    //   if (/* grafo.Nodes.Count == Nodes.Count || */ q.Count == 0)
    //   {
    //     return new Tuple<Graph, Stack<Node>>(grafo, s);
    //   }
    // }
  }


  // public Tuple<int, List<Edge>> GrauNos(Node pai, int pular,)
  // {
  //   if (Adj[pai].Count != 0)
  //   {
  //     foreach (var filho in Adj[pai])
  //     {
  //       if (filho.N != pular)
  //       {
  //         pai.LowLink += 1 + GrauNos(filho, pai.N, pontes).Item1;

  //         if (Math.Min(pai.Index - pai.LowLink + 1, filho.Index - filho.LowLink + 1) > pai.Index - pai.LowLink && Math.Max(pai.Index, filho.Index) >= pai.Index)
  //         {
  //           var ed = new Edge();
  //           ed.a = pai;
  //           ed.b = filho;
  //           pontes.Add(ed);
  //         }
  //       }
  //     }
  //     return new Tuple<int, List<Edge>>(pai.LowLink, pontes);
  //   }
  //   return new Tuple<int, List<Edge>>(0, pontes);
  // }

  //   int dfsBR(int u, int p) {
  //   low[u] = disc[u] = ++Time;
  //   for (int& v : adj[u]) {
  //     if (v == p) continue; // we don't want to go back through the same path.
  //                           // if we go back is because we found another way back
  //     if (!disc[v]) { // if V has not been discovered before
  //       dfsBR(v, u);  // recursive DFS call
  //       if (disc[u] < low[v]) // condition to find a bridge
  //         br.push_back({u, v});
  //       low[u] = min(low[u], low[v]); // low[v] might be an ancestor of u
  //     } else // if v was already discovered means that we found an ancestor
  //       low[u] = min(low[u], disc[v]); // finds the ancestor with the least discovery time
  //   }
  // }
  // int DfsBR(Node pai, Node filho, int time, List<Edge> pontes)
  // {
  //   pai.Index = pai.LowLink = ++time;

  //   // var conexoes = new List<Edge>();
  //   // conexoes.Add(new Edge(pai, 1, 1));

  //   foreach (var no in Adj[pai])
  //   {
  //     if (no == filho) continue;

  //     if (no.Index == -1)
  //     {
  //       DfsBR(no, pai);
  //       if (pai.Index < no.LowLink)
  //       {
  //         pontes.Add(new Edge(pai, no));
  //       }
  //       pai.LowLink = Math.Min()
  //     }
  //   }

  // }

  public bool TarjanPontes(Node pai, Node filho/* , int quantVertices */)
  {
    var conexoes = new HashSet<Edge>();

    // Remover ciclos para formar uma árvore de árvore enraizada 
    // var item = BuscaEmLaguraNumeraArvoreEnraizada(pai);

    var item = ArvoreEnraizada(pai, filho/* , quantVertices */);
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
        var alo = conexoes.ElementAt(conexoes.Count - 2).l > filho.Index - filho.LowLink && conexoes.ElementAt(conexoes.Count - 2).h <= filho.Index;
        // var alo = false;
        return alo;
      }
      node = s.Pop();

    }



    // var grauFilhos = -1;
    // s.Push(pai);
    // var j = 0;

    // while (true)
    // {
    //   // for (var k = 0; k < Nodes.Count; k++)
    //   // {
    //   if (Adj[pai].Count == 0)
    //   {
    //     pai = s.Pop();
    //     j -= 2;
    //   }
    //   pai.LowLink = 0;
    //   grauFilhos = 0;
    //   foreach (var filho in Adj[pai])
    //   {
    //     if (filho.LowLink != -1)
    //     {
    //       grauFilhos += filho.LowLink;
    //     }
    //     if (pai.Index > filho.Index)
    //     {
    //       s.Push(filho);
    //     }
    //   }
    //   if (grauFilhos != 0)
    //     pai.LowLink = grauFilhos;
    //   pai = s.ElementAt(++j);
    // }






    /* Encontre uma árvore de extensão mínima de G
    Crie uma árvore enraizada T a partir da árvore de extensão mínima
    Atravesse a árvore T em pós-ordem e numere os nodos. Nodos pai na árvore agora tem números mais altos do que os nodos filho.
    Para cada nodo de 1 a v1 (o nodo raiz da árvore) faça:
    Calcule o número de descendentes ND(v) para este nodo.
    Calcule L(v) e H(v)
    para cada {\displaystyle w}w tal que {\displaystyle v\to w}{\displaystyle v\to w}: se {\displaystyle H(w)\leq w}{\displaystyle H(w)\leq w} e {\displaystyle L(w)>w-ND(w)}{\displaystyle L(w)>w-ND(w)} então {\displaystyle (v,w)}{\displaystyle (v,w)} é uma ponte. */

  }
  // public bool TarjanPontes(Node pai, Node filho)
  // {
  //   var conexoes = new HashSet<Edge>();

  //   // Remover ciclos para formar uma árvore de árvore enraizada 
  //   var item = ArvoreEnraizada(pai, filho);
  //   Nodes = item.Item1.Nodes;
  //   var s = item.Item2;
  //   // var pai = NumeraNodos();
  //   var node = s.Pop();

  //   while (true)
  //   {
  //     var menorNo = node;
  //     var quantFilhos = 0;
  //     foreach (var no in item.Item1.Adj[node])
  //     {

  //       if (no.Index < node.Index)
  //       {
  //         quantFilhos += 1 + no.LowLink;
  //       }
  //       if (Adj[node].Count - 1 == 0)
  //       {
  //         conexoes.Add(new Edge(node, node.Index, node.Index));
  //       }
  //       if (no.Index < menorNo.Index)
  //       {
  //         menorNo = no;
  //       }

  //     }
  //     if (menorNo != node)
  //     {
  //       conexoes.Add(new Edge(node, Math.Min(node.Index - node.LowLink + 1, menorNo.Index - (1 + menorNo.LowLink) + 1), Math.Max(node.Index, menorNo.Index)));
  //     }
  //     node.LowLink = quantFilhos;
  //     if (node == pai)
  //     {
  //       var alo = conexoes.ElementAt(conexoes.Count - 1).l > filho.Index - filho.LowLink && conexoes.ElementAt(conexoes.Count - 1).h <= filho.Index;
  //       // var alo = false;
  //       return alo;
  //     }
  //     node = s.Pop();

  //   }



  //   // var grauFilhos = -1;
  //   // s.Push(pai);
  //   // var j = 0;

  //   // while (true)
  //   // {
  //   //   // for (var k = 0; k < Nodes.Count; k++)
  //   //   // {
  //   //   if (Adj[pai].Count == 0)
  //   //   {
  //   //     pai = s.Pop();
  //   //     j -= 2;
  //   //   }
  //   //   pai.LowLink = 0;
  //   //   grauFilhos = 0;
  //   //   foreach (var filho in Adj[pai])
  //   //   {
  //   //     if (filho.LowLink != -1)
  //   //     {
  //   //       grauFilhos += filho.LowLink;
  //   //     }
  //   //     if (pai.Index > filho.Index)
  //   //     {
  //   //       s.Push(filho);
  //   //     }
  //   //   }
  //   //   if (grauFilhos != 0)
  //   //     pai.LowLink = grauFilhos;
  //   //   pai = s.ElementAt(++j);
  //   // }






  //   /* Encontre uma árvore de extensão mínima de G
  //   Crie uma árvore enraizada T a partir da árvore de extensão mínima
  //   Atravesse a árvore T em pós-ordem e numere os nodos. Nodos pai na árvore agora tem números mais altos do que os nodos filho.
  //   Para cada nodo de 1 a v1 (o nodo raiz da árvore) faça:
  //   Calcule o número de descendentes ND(v) para este nodo.
  //   Calcule L(v) e H(v)
  //   para cada {\displaystyle w}w tal que {\displaystyle v\to w}{\displaystyle v\to w}: se {\displaystyle H(w)\leq w}{\displaystyle H(w)\leq w} e {\displaystyle L(w)>w-ND(w)}{\displaystyle L(w)>w-ND(w)} então {\displaystyle (v,w)}{\displaystyle (v,w)} é uma ponte. */

  // }

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

    // Garantir que não existe mais nenhuma aresta
    // Console.Write("}");
    // Console.WriteLine();

    return true;
  }
  public bool VerificaAdj()
  {

    return !(Nodes.Count == 0);
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
        if (Adj[next].Count > 1) ///MECHER AQUI PARA JUNTAR Fleury
        {
          // foreach (var v in Adj[node])
          // {
          // Se aresta u-v é valida, passa para a proxima
          if (!NaiveHasBridge(next, node))
          { ///MECHER AQUI PARA JUNTAR Fleury

            // Console.Write("(" + next.N + "-" + node.N + "), ");

            // Essa aresta é utlizada então é removida agora
            removeAresta(next, node);

            us.Enqueue(node);
            break;
          }
          // }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), ");
          removeAresta(next, node);
          Remove(next);
          us.Enqueue(node);
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
    var quantVertices = Nodes.Count;
    us.Enqueue(newU);

    while (true)
    {
      if (us.Count == 0) break;

      var next = us.Dequeue();

      // Percorre todos os vertices adjacentes a u
      foreach (var node in Adj[next])
      {
        if (Adj[next].Count > 1) ///MECHER AQUI PARA JUNTAR Fleury
        {
          // Se aresta u-v é valida, passa para a proxima
          if (!TarjanPontes(next, node/*,  quantVertices */))
          {

            // Console.Write("(" + next.N + "-" + node.N + "), ");

            // Essa aresta é utlizada então é removida agora
            removeAresta(next, node);

            us.Enqueue(node);
            break;
          }
          // }
        }
        else
        {
          // Console.Write("(" + next.N + "-" + node.N + "), ");
          removeAresta(next, node);
          Remove(next);
          // quantVertices--;
          us.Enqueue(node);
        }
      }

      // foreach (var node in Nodes)
      // {
      //   if (Adj[node].Count != 0)
      //     Console.Write("QUant adj: " + Adj[node].Count);
      //   // foreach (var no in Adj[node])
      //   // {
      //   //   Console.Write("nao usou todos" + false);
      //   // }
      // }

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
