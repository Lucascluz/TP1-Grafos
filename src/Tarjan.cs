namespace Tarjan;

class Node
{
  public int LowLink { get; set; }
  public int Index { get; set; }
  public int N { get; }

  public Node(int n)
  {
    N = n;
    Index = -1;
    LowLink = 0;
  }
}

class Graph
{
  public HashSet<Node> Nodes { get; } = new HashSet<Node>();
  public Dictionary<Node, HashSet<Node>> Adj { get; } = new Dictionary<Node, HashSet<Node>>();

  public void Add(Node key, HashSet<Node> value)
  {
    Nodes.Add(key);

    foreach (var item in value)
    {
      Nodes.Add(item);
      Adj.Add(item, new HashSet<Node>());
    }

    Adj.Add(key, value);
  }

  public LinkedList<Node> BreadthFirstSearch(Node s)
  {
    var size = Nodes.Count;

    // Mark all the vertices as not
    // visited(By default set as false)
    var visited = new bool[size];
    for (int i = 0; i < size; i++)
    {
      visited[i] = false;
    }

    // Create a queue for BFS
    var queue = new LinkedList<Node>();

    // Mark the current node as
    // visited and enqueue it
    visited[s.N] = true;
    queue.AddLast(s);

    while (queue.Any())
    {
      // Dequeue a vertex from queue
      // and print it
      s = queue.First();
      Console.Write(s + " ");
      queue.RemoveFirst();

      // Get all adjacent vertices of the
      // dequeued vertex s. If a adjacent
      // has not been visited, then mark it
      // visited and enqueue it
      var list = Adj[s];

      foreach (var val in list)
      {
        if (!visited[val.N])
        {
          visited[val.N] = true;
          queue.AddLast(val);
        }
      }
    }

    return queue;
  }

  public void Tarjan()
  {
    var index = 0; // number of nodes
    var s = new Stack<Node>();

    Action<Node> strongConnect = default!;

    strongConnect = (v) =>
    {
      // Set the depth index for v to the smallest unused index
      v.Index = index;
      v.LowLink = index;

      index++;
      s.Push(v);

      // Consider successors of v
      foreach (var w in Adj[v])
        if (w.Index < 0)
        {
          // Successor w has not yet been visited; recurse on it
          strongConnect(w);
          v.LowLink = Math.Min(v.LowLink, w.LowLink);
        }
        else if (s.Contains(w))
        {
          // Successor w is in stack S and hence in the current SCC
          v.LowLink = Math.Min(v.LowLink, w.Index);
        }

      // If v is a root node, pop the stack and generate an SCC
      if (v.LowLink == v.Index)
      {
        Console.Write("SCC: ");

        Node w;
        do
        {
          w = s.Pop();
          Console.Write(w.N + " ");
        } while (w != v);

        Console.WriteLine();
      }
    };

    foreach (var v in Nodes)
    {
      if (v.Index < 0)
      {
        strongConnect(v);
      }
    }
  }
}
