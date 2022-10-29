using System.Diagnostics;

public enum GraphType
{
  euler,
  nonEuler,
  semiEuler,
}

static class Program
{
  static void Print(String type, bool grafo, int i, Stopwatch stopwatch)
  {
    var euler = grafo ? "eureliano/semi" : "não eureliano/semi";

    Console.WriteLine($"{type}:\t{euler}\tTamanho:\t{i}\tTempo:\t{stopwatch.ElapsedMilliseconds}ms");
  }

  static void RunEuler()
  {
    var stopwatch = new Stopwatch();

    Console.WriteLine($"\nEureliano");
    for (int i = 10; i < 100_001; i *= 10)
    {
      // Naïve

      var naive = Graph.PreecherGrafo(i, GraphType.euler);

      stopwatch.Restart();
      var euler = naive.PrintEulerTourNaive(false);
      stopwatch.Stop();

      Print("Naïve", euler, i, stopwatch);

      var tarjan = Graph.PreecherGrafo(i, GraphType.euler);

      stopwatch.Restart();
      euler = tarjan.PrintEulerTourNaive(true);
      stopwatch.Stop();

      Print("Tarjan", euler, i, stopwatch);

      Console.WriteLine();
    }

    Console.WriteLine($"\nSemi-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var naive = Graph.PreecherGrafo(i, GraphType.semiEuler);

      stopwatch.Restart();
      var euler = naive.PrintEulerTourNaive(false);
      stopwatch.Stop();

      Print("Naïve", euler, i, stopwatch);

      var tarjan = Graph.PreecherGrafo(i, GraphType.semiEuler);

      stopwatch.Restart();
      euler = tarjan.PrintEulerTourNaive(true);
      stopwatch.Stop();

      Print("Tarjan", euler, i, stopwatch);

      Console.WriteLine();
    }

    Console.WriteLine($"\nNão-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      var naive = Graph.PreecherGrafo(i, GraphType.nonEuler);

      stopwatch.Restart();
      var euler = naive.PrintEulerTourNaive(false);
      stopwatch.Stop();

      Print("Naïve", euler, i, stopwatch);

      var tarjan = Graph.PreecherGrafo(i, GraphType.nonEuler);

      stopwatch.Restart();
      euler = tarjan.PrintEulerTourNaive(true);
      stopwatch.Stop();

      Print("Tarjan", euler, i, stopwatch);

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

    // Grafos para teste.

    // g.Add(_0, _1);
    // g.Add(_1, _2);
    // g.Add(_2, _0);

    // g.Add(_3, _4);
    // g.Add(_4, _5);
    // g.Add(_5, _3);

    // Árvore.
    // g.Add(_0, _1);
    // g.Add(_0, _4);
    // g.Add(_1, _2);
    // g.Add(_1, _3);
    // g.Add(_2, _3);
    // g.Add(_4, _5);

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

    var grafo = g.PrintEulerTourNaive(false);
  }

  public static void Main(String[] args)
  {
    RunEuler();

    Test();
  }
}
