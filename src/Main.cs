using System;
using System.Collections.Generic;

// Grafico simples e não direcionado representado por meio da lista de adjacência
class Graph
{
    private int vertices; // Numero de Vertices
    private List<int>[] adj; // Lista de adjacência
    Graph(int numOfVertices)
    {
        // initialise vertex count
        this.vertices = numOfVertices;

        // Inicializa Lista de adjacência
        initGraph();
    }

    // Metodo que inicializa Lista de adjacência
    private void initGraph()
    {
        adj = new List<int>[vertices];
        for (int i = 0; i < vertices; i++)
        {
            adj[i] = new List<int>();
        }
    }

    // Adiciona a aresta u-v
    private void addAresta(int u, int v)
    {
        adj[u].Add(v);
        adj[v].Add(u);
    }

    // Remove aresta u-v do grafo.
    private void removeAresta(int u, int v)
    {
        adj[u].Remove(v);
        adj[v].Remove(u);
    }

    /* 
    Função principal que imprime o caminho Eureliano. Primeiro
    ele acha um vertice com grau impar (caso exista), e depois
    chama o metodo printEulerUtil() para printar o caminho
    */
    private void printEulerTour()
    {
        // Acha o vertice de grau impar
        int u = 0;
        for (int i = 0; i < vertices; i++)
        {
            if (adj[i].Count % 2 == 1)
            {
                u = i;
                break;
            }
        }

        // Printa tour a partir do impar V
        printEulerUtil(u);
        Console.WriteLine();
    }

    // Printa o caminho Eureliano começando do Vertice U
    private void printEulerUtil(int u)
    {
        // Percorre todos os vertices adjacentes a u
        for (int i = 0; i < adj[u].Count; i++)
        {
            int v = adj[u][i];

            // Se aresta u-v é valida, passa para a proxima
            if (isValidNextAresta(u, v))
            {
                Console.Write(u + "-" + v + " ");

                // Essa aresta é utlizada então é removida agora
                removeAresta(u, v);
                printEulerUtil(v);
            }
        }
    }

    // Função que checa se a aresta u-v pode ser considerada proxima aresta do caminho Eureliano
    private bool isValidNextAresta(int u, int v)
    {
        // A Aresta u-v é valida em um dos casos a seguir

        // 1) Se v é o unico vertice adjacente a u ou o tamanho da lista de adjacencia é 1 
        if (adj[u].Count == 1)
        {
            return true;
        }

        // 2) Se houverem multiplos adjacentes, então u-v não e uma ponte.
        // Siga o passo a passo para saber de u-v é uma ponte
        // 2.a) conte os vertices alcançaveis a partir de u 
        bool[] isVisited = new bool[this.vertices];
        int count1 = dfsCount(u, isVisited);

        // 2.b) Remove Aresta (u, v) e depois de remover 
        // conte os vertices alcançaveis a partir de u
        removeAresta(u, v);
        isVisited = new bool[this.vertices];
        int count2 = dfsCount(u, isVisited);

        // 2.c) Coloca a aresta de volta no grafo
        addAresta(u, v);
        return (count1 > count2) ? false : true;
    }

    // Uma função baseada em DFS pra contar os vertices alcançaveis a partir de V
    private int dfsCount(int v, bool[] isVisited)
    {
        // Marca o nó visitado atualmente
        isVisited[v] = true;
        int count = 1;

        // Recorre por todos os vertices adjacentes a este vertice
        foreach (int i in adj[v])
        {
            if (!isVisited[i])
            {
                count = count + dfsCount(i, isVisited);
            }
        }
        return count;
    }

    public static void Main(String[] a)
    {
        Random rand = new Random();

        Console.WriteLine("\tGerando grafo aleatorio de 100 Vértices");
        Graph g100 = new Graph(100);
        for (int i = 0; i < g100.vertices; i++)
        {
            g100.addAresta(rand.Next(1, 100), rand.Next(1, 100));
        }
        Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
        g100.printEulerTour();

        Console.WriteLine("\tGerando grafo aleatorio de 1000 Vértices");
        Graph g1k = new Graph(1000);
        for (int i = 0; i < g1k.vertices; i++)
        {
            g1k.addAresta(rand.Next(1, 1000), rand.Next(1, 1000));
        }
        Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
        g1k.printEulerTour();

        Console.WriteLine("\tGerando grafo aleatorio de 10000 Vértices");
        Graph g10k = new Graph(10000);
        for (int i = 0; i < g10k.vertices; i++)
        {
            g10k.addAresta(rand.Next(1, 10000), rand.Next(1, 10000));
        }
        Console.WriteLine("\t Imprimindo Caminho Euleriano do Grafo");
        g10k.printEulerTour();

    }
}