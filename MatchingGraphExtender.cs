using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASD.Graphs;

namespace ASD
{
    public static class MatchingGraphExtender
    {
        /// <summary>
        /// Podział grafu na cykle. Zakładamy, że dostajemy graf nieskierowany i wszystkie wierzchołki grafu mają parzyste stopnie
        /// (nie trzeba sprawdzać poprawności danych).
        /// </summary>
        /// <param name="G">Badany graf</param>
        /// <returns>Tablica cykli; krawędzie każdego cyklu powinny być uporządkowane zgodnie z kolejnością na cyklu, zaczynając od dowolnej</returns>
        /// <remarks>
        /// Metoda powinna działać w czasie O(m)
        /// </remarks>
        public static Edge[][] cyclePartition(this Graph G)
        {
            Edge[][] result = null;
            Queue<Edge> edges = new Queue<Edge>();
            Graph g = G.Clone();
            int cycles = 0;
            Edge[][] tmp = new Edge[g.EdgesCount][];

            Stack<int> vertstack = new Stack<int>();
            int v = 0;
            vertstack.Push(v);
            while (v < g.VerticesCount)
            {
                Edge edge = g.OutEdges(v).First();
                v = edge.To;
                vertstack.Push(v);
                edges.Enqueue(edge);
                g.DelEdge(edge);
                if (g.OutDegree(v) <= 0)
                {
                    tmp[cycles] = edges.ToArray();
                    cycles++;
                    edges = new Queue<Edge>();
                    v = 0;
                    while (v < g.VerticesCount && g.OutDegree(v) <= 0)
                        v++;
                }
            }

            result = new Edge[cycles][];
            for (int i = 0; i < cycles; i++)
                result[i] = tmp[i];

            return result;
        }

        /// <summary>
        /// Szukanie skojarzenia doskonałego w grafie nieskierowanym o którym zakładamy, że jest dwudzielny i 2^r-regularny
        /// (nie trzeba sprawdzać poprawności danych)
        /// </summary>
        /// <param name="G">Badany graf</param>
        /// <returns>Skojarzenie doskonałe w G</returns>
        /// <remarks>
        /// Metoda powinna działać w czasie O(m), gdzie m jest liczbą krawędzi grafu G
        /// </remarks>
        public static Graph perfectMatching(this Graph G)
        {
            Graph g = G.Clone();
            int p = G.OutDegree(0);

            while (p >= 2)
            {
                Edge[][] edges = g.cyclePartition();
                for (int i = 0; i < edges.Length; i++)
                    for (int j = 0; j < edges[i].Length; j += 2)
                        g.DelEdge(edges[i][j]);
                p /= 2;
            }

            return g;
        }
    }
}
