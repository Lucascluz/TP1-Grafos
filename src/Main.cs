namespace TP1Grafos;
using System.Diagnostics;

static class Program
{
  // TODO: fix, arrumar método
  static void RunEuler()
  {
    var stopwatch = new Stopwatch();

    Console.WriteLine("Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var g100Eureliano = Tarjan.Graph.PreecherGrafo(i, 0);

      stopwatch.Restart();
      var grafo = g100Eureliano.printEulerTourNaive(false);
      stopwatch.Stop();

      Console.WriteLine("È eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);
    }

    Console.WriteLine("Semi-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var g100SemiEureliano = Tarjan.Graph.PreecherGrafo(i, 1);

      stopwatch.Restart();
      var grafo = g100SemiEureliano.printEulerTourNaive(false);
      stopwatch.Stop();

      Console.WriteLine("È eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);
    }
    Console.WriteLine("Não-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var g100NaoEureliano = Tarjan.Graph.PreecherGrafo(i, 2);

      stopwatch.Restart();
      var grafo = g100NaoEureliano.printEulerTourNaive(false);
      stopwatch.Stop();

      Console.WriteLine("È eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);
    }

    // g100SemiEureliano.printEulerTourNaive();

    // g100NaoEureliano.printEulerTourNaive();

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
    var _5 = new Tarjan.Node(5);

    g.Add(_0, _1);
    g.Add(_1, _2);
    g.Add(_2, _3);
    g.Add(_3, _0);
    g.Add(_0, _4);
    g.Add(_2, _5);

    g.Tarjan();

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
