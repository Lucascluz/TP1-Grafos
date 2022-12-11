using System.Diagnostics;

static class Program
{
  static void Print(String type, int i, Stopwatch stopwatch)
  {
    Console.WriteLine($"{type}:\tTamanho:\t{i}\tTempo:\t{stopwatch.ElapsedMilliseconds}ms");
  }

  static void Test()
  {
    var tests = new List<int> { 100, 1_000, 10_000, 100_000 };

    var stopwatch = new Stopwatch();

    foreach (var test in tests)
    {
      var g0 = KConexo(test);
      stopwatch.Restart();
      g0.BuscaLigação();
      Print("K-conexo", test, stopwatch);

      var g1 = Direcionado(test);
      stopwatch.Restart();
      g1.BuscaLigação();
      Print("Direcionado", test, stopwatch);

      var g2 = Circular(test);
      stopwatch.Restart();
      g2.BuscaLigação();
      Print("Circular", test, stopwatch);
    }
  }

  static Graph KConexo(int n)
  {
    return Graph.KConexo(5, n / 5, 2);
  }

  static Graph Direcionado(int n)
  {
    return Graph.Direcionado(n / 3, 3);
  }

  static Graph Circular(int n)
  {
    return Graph.Circular(n);
  }

  public static void Main(String[] args)
  {
    Test();
  }
}
