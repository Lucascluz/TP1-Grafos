using System.Diagnostics;

namespace TP1Grafos;

static class Program
{
  static void RunEuler()
  {
    var stopwatch = new Stopwatch();

    Console.WriteLine("Eureliano");
    for (int i = 10; i < 100_001; i *= 10)
    {
      var gEurelianoNaive = Graph.PreecherGrafo(i, 0);
      var gEurelianoTarjan = Graph.PreecherGrafo(i, 0);

      stopwatch.Restart();
      var grafo = gEurelianoTarjan.printEulerTourNaive(true);
      stopwatch.Stop();

      var adj = gEurelianoTarjan.VerificaAdj();

      Console.WriteLine("Tarjan: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      stopwatch.Restart();
      grafo = gEurelianoNaive.printEulerTourNaive(false);
      stopwatch.Stop();

      adj = gEurelianoNaive.VerificaAdj();

      Console.WriteLine("Naive: é eureliano/semi = " + grafo + " Tamanho: " + i + " Tempo em ms: " + stopwatch.ElapsedMilliseconds);

      Console.WriteLine();
    }

    Console.WriteLine("Semi-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var gSemiEurelianoNaive = Graph.PreecherGrafo(i, 1);
      var gSemiEurelianoTarjan = Graph.PreecherGrafo(i, 1);

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
      var gNaoEurelianoNaive = Graph.PreecherGrafo(i, 2);
      var gNaoEurelianoTarjan = Graph.PreecherGrafo(i, 2);

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
  }

  static void Test()
  {
    var g = new Graph();

    var _0 = new Node(0);
    var _1 = new Node(1);
    var _2 = new Node(2);
    var _3 = new Node(3);
    var _4 = new Node(4);
    var _5 = new Node(5);

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

    g.Add(_0, _1);
    g.Add(_1, _2);
    g.Add(_1, _3);
    g.Add(_2, _3);
    g.Add(_0, _4);
    g.Add(_0, _5);
    g.Add(_4, _5);

    // g.Add(_0, _1);
    // g.Add(_0, _2);
    // g.Add(_0, _3);
    // g.Add(_3, _2);

    // g.Add(_0, _4);
    // g.Add(_0, _5);
    // g.Add(_4, _5);

    var grafo = g.printEulerTourNaive(false);
  }

  public static void Main(String[] args)
  {
    RunEuler();

    // Test();
  }
}
