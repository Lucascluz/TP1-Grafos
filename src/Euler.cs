namespace Euler;

// Grafo simples e não direcionado representado por meio da lista de adjacência
class Graph
{
  static readonly Random random = new Random();

  int vertexCount; // Numero de Vertices
  List<int>[] adj = default!; // Lista de adjacência
  Graph(int numOfVertices)
  {
    // initialise vertex count
    this.vertexCount = numOfVertices;

    // Inicializa Lista de adjacência
    initGraph();
  }

  // Metodo que inicializa Lista de adjacência
  void initGraph()
  {
    adj = new List<int>[vertexCount];
    for (int i = 0; i < vertexCount; i++)
    {
      adj[i] = new List<int>();
    }
  }

  // Adiciona a aresta u-v
  void addAresta(int u, int v)
  {
    adj[u].Add(v);
    adj[v].Add(u);
  }

  // Remove aresta u-v do grafo.
  void removeAresta(int u, int v)
  {
    adj[u].Remove(v);
    adj[v].Remove(u);
  }

  bool VerificaArestaExiste(int u, int v)
  {
    if (adj[u].Contains(v))
    {
      return true;
    }
    return false;
  }

  bool VerificaArestaPar(int u, int v)
  {
    if ((adj[u].Count) % 2 == 0 && (adj[v].Count) % 2 == 0)
    {
      return true;
    }
    return false;
  }
  /* 
  Função principal que imprime o caminho Eureliano. Primeiro
  ele acha um vertice com grau impar (caso exista), e depois
  chama o metodo printEulerUtil() para printar o caminho
  */
  public bool printEulerTour()
  {
    // Acha o vertice de grau impar
    var impares = new List<int>();
    int u = 0;
    for (int i = 0; i < vertexCount; i++)
    {
      if (adj[i].Count % 2 == 1)
      {
        impares.Add(i);
        if (impares.Count > 2)
        {
          return false;
        }
      }
    }

    // Printa tour a partir do impar V
    Console.Write("{");
    if (impares.Count != 0)
    {
      printEulerUtil(impares[0]);
    }
    else
    {
      printEulerUtil(u);
    }
    Console.Write("}");
    Console.WriteLine();

    return true;
  }

  // Printa o caminho Eureliano começando do Vertice U
  void printEulerUtil(int u)
  {
    // Percorre todos os vertices adjacentes a u
    for (int i = 0; i < adj[u].Count; i++)
    {
      int v = adj[u][i];

      // Se aresta u-v é valida, passa para a proxima
      if (isValidNextAresta(u, v))
      {
        Console.Write("(" + u + "-" + v + "), ");

        // Essa aresta é utlizada então é removida agora
        removeAresta(u, v);
        printEulerUtil(v);
      }
    }
  }

  // Função que checa se a aresta u-v pode ser considerada proxima aresta do caminho Eureliano
  bool isValidNextAresta(int u, int v)
  {
    // A Aresta u-v é valida em um dos casos a seguir

    // 1) Se v é o unico vertice adjacente a u ou o tamanho da lista de adjacencia é 1 
    if (adj[u].Count == 1)
    {
      return true;
    }

    // 2) Se houverem multiplos adjacentes, então u-v não e uma ponte.
    // Siga o passo a passo para saber de u-v é uma ponte
    // 2.a) conte os vertices alcançaveis a partir de u 
    bool[] isVisited = new bool[this.vertexCount];
    int count1 = dfsCount(u, isVisited);

    // 2.b) Remove Aresta (u, v) e depois de remover 
    // conte os vertices alcançaveis a partir de u
    removeAresta(u, v);
    isVisited = new bool[this.vertexCount];
    int count2 = dfsCount(u, isVisited);

    // 2.c) Coloca a aresta de volta no grafo
    addAresta(u, v);
    return (count1 > count2) ? false : true;
  }

  static int RandomNumber(int min, int max)
  {
    return random.Next(min, max);
  }

  public static Graph Eureliano(int totalvertice)
  {

    Graph grafoEureliano = new Graph(totalvertice);
    totalvertice--;
    int vertice1 = 0;
    grafoEureliano.addAresta(0, totalvertice);
    while (vertice1 < totalvertice)
    {
      grafoEureliano.addAresta(vertice1, vertice1 + 1);
      vertice1++;
    }

    return grafoEureliano;
  }

  public static Graph SemiEureliano(int totalvertice)
  {
    Graph grafoSemiEureliano = Eureliano(totalvertice);

    grafoSemiEureliano.addAresta(0, totalvertice / 2 + 1);

    return grafoSemiEureliano;
  }

  public static Graph NaoEureliano(int totalvertice)
  {
    Graph grafoNaoEureliano = Eureliano(totalvertice);
    for (int i = 0; i < 2;)
    {
      var vertice1 = RandomNumber(0, totalvertice);
      var vertice2 = RandomNumber(0, totalvertice);
      if (vertice1 != vertice2 && !grafoNaoEureliano.VerificaArestaExiste(vertice1, vertice2) && grafoNaoEureliano.VerificaArestaPar(vertice1, vertice2))
      {
        grafoNaoEureliano.addAresta(vertice1, vertice2);
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
      //se tamnho = impar não pode ser eureliano nem semi
      default:
        Graph g1 = new Graph(tamanho);
        return g1;
    }
  }

  // Uma função baseada em DFS pra contar os vertices alcançaveis a partir de V
  int dfsCount(int v, bool[] isVisited)
  {
    // Marca o nó visitado atualmente
    isVisited[v] = true;
    int count = 1;

    // Recorre por todos os vertices adjacentes a este vertice
    foreach (int i in adj[v])
    {
      if (!isVisited[i])
      {
        count = count + dfsCount(i, isVisited);
      }
    }
    return count;
  }
}
