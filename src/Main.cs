namespace TP1Grafos;

static class Program
{
  // TODO: fix, arrumar método
  static void RunEuler()
  {
    var g100Eureliano = Euler.Graph.PreecherGrafo(100, 0);
    var g100SemiEureliano = Euler.Graph.PreecherGrafo(100, 1);
    var g100NaoEureliano = Euler.Graph.PreecherGrafo(100, 2);

    g100Eureliano.printEulerTour();

    g100SemiEureliano.printEulerTour();

    g100NaoEureliano.printEulerTour();

    // Random rand = new Random();

    // Console.WriteLine("\tGerando grafo aleatorio de 100 Vértices");
    // Graph g100 = new Graph(100);
    // for (int i = 0; i < g100.vertices; i++)
    // {
    //   g100.addAresta(rand.Next(1, 100), rand.Next(1, 100));
    // }
    // Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
    // g100.printEulerTour();

    // Console.WriteLine("\tGerando grafo aleatorio de 1000 Vértices");
    // Graph g1k = new Graph(1000);
    // for (int i = 0; i < g1k.vertices; i++)
    // {
    //   g1k.addAresta(rand.Next(1, 1000), rand.Next(1, 1000));
    // }
    // Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
    // g1k.printEulerTour();

    // Console.WriteLine("\tGerando grafo aleatorio de 10000 Vértices");
    // Graph g10k = new Graph(10000);
    // for (int i = 0; i < g10k.vertices; i++)
    // {
    //   g10k.addAresta(rand.Next(1, 10000), rand.Next(1, 10000));
    // }
    // Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
    // g10k.printEulerTour();
  }

  static void RunTarjan()
  {
    var g = new Tarjan.Graph();

    var _0 = new Tarjan.Node(0);
    var _1 = new Tarjan.Node(1);
    var _2 = new Tarjan.Node(2);
    var _3 = new Tarjan.Node(3);
    var _4 = new Tarjan.Node(4);

    g.Add(_0, _1);
    g.Add(_0, _2);
    g.Add(_0, _3);
    g.Add(_3, _4);

    // g.Tarjan();

    var hasBridge = g.NaiveHasBridge();

    Log(hasBridge.ToString());
  }

  static void Log(String value)
  {
    Console.WriteLine(value);
  }

  public static void Main(String[] args)
  {
    // RunEuler();

    RunTarjan();
  }
}
