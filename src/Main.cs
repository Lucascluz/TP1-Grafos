using System.Diagnostics;

public enum GraphGeneration
{
  generate,
  import,
}

public enum GraphMethod
{
  naive,
  tarjan,
}

public enum GraphType
{
  euler,
  nonEuler,
  semiEuler,
}

static class Program
{
  static void Export()
  {
    Console.WriteLine("Exporting graphs...");

    for (int i = 10; i < 100_001; i *= 10)
    {
      Console.WriteLine($"  Exporting {i} vertex graphs...");

      File.WriteAllText($"assets/{i}-euler", Graph.FillGraph(i, GraphType.euler).Export());
      Console.WriteLine("  ...Euler Done");
      File.WriteAllText($"assets/{i}-semi-euler", Graph.FillGraph(i, GraphType.semiEuler).Export());
      Console.WriteLine("  ...Semi euler Done");
      File.WriteAllText($"assets/{i}-non-euler", Graph.FillGraph(i, GraphType.nonEuler).Export());
      Console.WriteLine("  ...Non euler Done\n");
    }

    Console.WriteLine("Finished exporting graphs...");
  }

  static Graph Import(int size, GraphType type)
  {
    var typeText = "euler";

    if (type == GraphType.nonEuler)
    {
      typeText = "non-euler";
    }
    else if (type == GraphType.semiEuler)
    {
      typeText = "semi-euler";
    }

    return Graph.Import(File.ReadAllText($"assets/{size}-{typeText}"));
  }

  static void Print(String type, bool grafo, int i, Stopwatch stopwatch)
  {
    var euler = grafo ? "eureliano/semi" : "não eureliano/semi";

    Console.WriteLine($"{type}:\t{euler}\tTamanho:\t{i}\tTempo:\t{stopwatch.ElapsedMilliseconds}ms");
  }

  static void RunEuler(GraphGeneration generation)
  {
    var stopwatch = new Stopwatch();

    Console.WriteLine($"\nEureliano");
    for (int i = 10; i < 100_001; i *= 10)
    {
      RunGraph(stopwatch, i, GraphMethod.naive, GraphType.euler, generation);
      RunGraph(stopwatch, i, GraphMethod.tarjan, GraphType.euler, generation);

      Console.WriteLine();
    }

    Console.WriteLine($"\nSemi-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      RunGraph(stopwatch, i, GraphMethod.naive, GraphType.semiEuler, generation);
      RunGraph(stopwatch, i, GraphMethod.tarjan, GraphType.semiEuler, generation);

      Console.WriteLine();
    }

    Console.WriteLine($"\nNão-Eureliano");
    for (int i = 100; i < 100_001; i *= 10)
    {
      RunGraph(stopwatch, i, GraphMethod.naive, GraphType.nonEuler, generation);
      RunGraph(stopwatch, i, GraphMethod.tarjan, GraphType.nonEuler, generation);

      Console.WriteLine();
    }
  }

  static void RunGraph(Stopwatch stopwatch, int size, GraphMethod method, GraphType type, GraphGeneration generation)
  {
    var g = generation == GraphGeneration.generate ? Graph.FillGraph(size, type) : Import(size, type);

    stopwatch.Restart();
    var isEuler = g.PrintEulerTour(method);
    stopwatch.Stop();

    var label = method == GraphMethod.naive ? "Naïve" : "Tarjan";

    Print(label, isEuler, size, stopwatch);
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
  }

  public static void Main(String[] args)
  {
    // Export();

    // RunEuler(GraphGeneration.generate);

    RunEuler(GraphGeneration.import);

    // Test();
  }
}
