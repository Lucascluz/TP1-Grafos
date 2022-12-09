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
      stopwatch.Restart();
      KConexo(test).BuscaLigação();
      Print("K-conexo", test, stopwatch);

      stopwatch.Restart();
      Direcionado(test).BuscaLigação();
      Print("Direcionado", test, stopwatch);

      stopwatch.Restart();
      Circular(test).BuscaLigação();
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
