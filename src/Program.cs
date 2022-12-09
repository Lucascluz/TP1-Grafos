using System.Diagnostics;

static class Program
{
  // Método utilizado para exportar os grafos para um arquivo.
  // static void Export()
  // {
  //   Console.WriteLine("Exporting graphs...");

  //   for (int i = 10; i < 100_001; i *= 10)
  //   {
  //     Console.WriteLine($"  Exporting {i} vertex graphs...");

  //     File.WriteAllText($"assets/{i}-euler", Graph.FillGraph(i, GraphType.euler).Export());
  //     Console.WriteLine("  ...Euler Done");
  //     File.WriteAllText($"assets/{i}-semi-euler", Graph.FillGraph(i, GraphType.semiEuler).Export());
  //     Console.WriteLine("  ...Semi euler Done");
  //     File.WriteAllText($"assets/{i}-non-euler", Graph.FillGraph(i, GraphType.nonEuler).Export());
  //     Console.WriteLine("  ...Non euler Done\n");
  //   }

  //   Console.WriteLine("Finished exporting graphs...");
  // }

  // // Método utilizado para importar um grafo a partir de um arquivo.
  // static Graph Import(int size, GraphType type)
  // {
  //   var typeText = "euler";

  //   if (type == GraphType.nonEuler)
  //   {
  //     typeText = "non-euler";
  //   }
  //   else if (type == GraphType.semiEuler)
  //   {
  //     typeText = "semi-euler";
  //   }

  //   return Graph.Import(File.ReadAllText($"assets/{size}-{typeText}"));
  // }

  // static void Print(String type, bool grafo, int i, Stopwatch stopwatch)
  // {
  //   var euler = grafo ? "euleriano/semi" : "não euleriano/semi";

  //   Console.WriteLine($"{type}:\t{euler}\tTamanho:\t{i}\tTempo:\t{stopwatch.ElapsedMilliseconds}ms");
  // }

  static void Test()
  {
    var tests = new List<int> { 100, 1_000, 5_000, 10_000 };

    foreach (var test in tests)
    {
      A(test).BuscaLigação();

      B(test).BuscaLigação();

      C(test).BuscaLigação();
    }
  }

  static Graph A(int n)
  {
    return Graph.KConexo(5, n / 5, 2);
  }

  static Graph B(int n)
  {
    return Graph.DirecaoGrafo(n / 3, 3);
  }

  static Graph C(int n)
  {
    return Graph.Euleriano(n);
  }

  public static void Main(String[] args)
  {
    Test();
  }
}
