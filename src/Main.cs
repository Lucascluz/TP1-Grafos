namespace TP1Grafos;
using System.Diagnostics;

static class Program
{
  // TODO: fix, arrumar método
  static void RunEuler()
  {
    var stopwatch = new Stopwatch();

    // Console.WriteLine("Eureliano");
    // for (int i = 10; i < 100_001; i *= 10)
    // {
    //   var gEurelianoNaive = Tarjan.Graph.PreecherGrafo(i, 0);
    //   var gEurelianoTarjan = Tarjan.Graph.PreecherGrafo(i, 0);

    //   stopwatch.Restart();
    //   var grafo = gEurelianoTarjan.printEulerTourNaive(true);
    //   stopwatch.Stop();

    //   var adj = gEurelianoTarjan.VerificaAdj();

    //   Console.WriteLine("Tarjan: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

    //   stopwatch.Restart();
    //   grafo = gEurelianoNaive.printEulerTourNaive(false);
    //   stopwatch.Stop();

    //   adj = gEurelianoNaive.VerificaAdj();

    //   Console.WriteLine("Naive: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

    //   Console.WriteLine();
    // }

    Console.WriteLine("Semi-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var gSemiEurelianoNaive = Tarjan.Graph.PreecherGrafo(i, 1);
      var gSemiEurelianoTarjan = Tarjan.Graph.PreecherGrafo(i, 1);

      stopwatch.Restart();
      var grafo = gSemiEurelianoTarjan.printEulerTourNaive(true);
      stopwatch.Stop();

      var adj = gSemiEurelianoTarjan.VerificaAdj();

      Console.WriteLine("Tarjan: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      stopwatch.Restart();
      grafo = gSemiEurelianoNaive.printEulerTourNaive(false);
      stopwatch.Stop();

      adj = gSemiEurelianoNaive.VerificaAdj();

      Console.WriteLine("Naive: È eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      Console.WriteLine();
    }
    Console.WriteLine("Não-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var gNaoEurelianoNaive = Tarjan.Graph.PreecherGrafo(i, 2);
      var gNaoEurelianoTarjan = Tarjan.Graph.PreecherGrafo(i, 2);

      stopwatch.Restart();
      var grafo = gNaoEurelianoNaive.printEulerTourNaive(false);
      stopwatch.Stop();

      Console.WriteLine(" Naive: È eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      stopwatch.Restart();
      grafo = gNaoEurelianoTarjan.printEulerTourNaive(true);
      stopwatch.Stop();

      Console.WriteLine("Tarjan: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      Console.WriteLine();
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

    // g.Add(_0, _1);
    // g.Add(_1, _2);
    // g.Add(_2, _0);

    // g.Add(_3, _4);
    // g.Add(_4, _5);
    // g.Add(_5, _3);

    //Arvore
    // g.Add(_0, _1);
    // g.Add(_0, _4);
    // g.Add(_1, _2);
    // g.Add(_1, _3);
    // g.Add(_2, _3);
    // g.Add(_4, _5);
    // var grafinho = Tarjan.Graph.PreecherGrafo(10, 0);
    // var grafo = grafinho.printEulerTourNaive(true);

    // g.Add(_0, _1);
    // g.Add(_0, _2);
    // g.Add(_0, _3);
    // g.Add(_0, _4);

    // g.Add(_1, _2);
    // g.Add(_1, _3);
    // g.Add(_1, _4);

    // g.Add(_2, _3);
    // g.Add(_2, _4);

    // g.Add(_3, _4);

    // g.Add(_0, _1);
    // g.Add(_1, _2);
    // g.Add(_1, _3);
    // g.Add(_2, _3);
    // g.Add(_0, _4);
    // g.Add(_0, _5);
    // g.Add(_4, _5);

    g.Add(_0, _1);
    g.Add(_0, _2);
    g.Add(_0, _3);
    g.Add(_3, _2);

    // g.Add(_0, _4);
    // g.Add(_0, _5);
    // g.Add(_4, _5);

    var pontes = new List<Tarjan.Edge>();
    // var alo = g.GrauNos(_0, _0.N, pontes);
    // var b = g.TarjanPontes();
    // var b = g.ArvoreEnraizada(_0);
    var grafooo = g.printEulerTourNaive(true);

    // var hasBridge = g.NaiveHasBridge();

    // Log(hasBridge.ToString());
  }

  // static void Log(String value)
  // {
  //   Console.WriteLine(value);
  // }

  public static void Main(String[] args)
  {
    RunEuler();

    // RunTarjan();
  }
}
