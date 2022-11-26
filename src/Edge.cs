// Classe que representa uma aresta do grafo.
class Edge
{
  public Node pai = default!;
  public int flow = default!;
  public int C = default!;
  public Node filho = default!;

  public Edge(Node filho, int flow, int C, Node pai)
  {
    this.filho = filho;
    this.flow = flow;
    this.C = C;
    this.pai = pai;
  }

  public Edge(Node filho, Node pai)
  {
    this.filho = filho;
    this.flow = 0;
    this.C = 1;
    this.pai = pai;
  }
}
