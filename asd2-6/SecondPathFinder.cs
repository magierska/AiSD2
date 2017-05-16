using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    public static class Lab06GraphExtender
    {

        /// <summary>
        /// Algorytm znajdujący drugą pod względem długości najkrótszą ścieżkę między a i b.
        /// Możliwe, że jej długość jest równa najkrótszej (jeśli są dwie najkrótsze ścieżki,
        /// algorytm zwróci jedną z nich).
        /// Dopuszczamy, aby na ścieżce powtarzały się wierzchołki/krawędzie.
        /// Można założyć, że a!=b oraz że w grafie nie występują pętle.
        /// </summary>
        /// <remarks>
        /// Wymagana złożoność do O(D), gdzie D jest złożonością implementacji alogorytmu Dijkstry w bibliotece Graph.
        /// </remarks>
        /// <param name="g"></param>
        /// <param name="path">null jeśli druga ścieżka nie istnieje, wpp ściezka jako ciąg krawędzi</param>
        /// <returns>null jeśli druga ścieżka nie istnieje, wpp długość znalezionej ścieżki</returns>
        public static double? FindSecondShortestPath(this Graph g, int a, int b, out Edge[] path)
        {
            path = null;
            double? min = Double.MaxValue;

            PathsInfo[] d;
            Graph G;
            Edge edge = new Edge();
            int index = 0;

            if (g.Directed)
                G = g.Reverse();
            else
                G = g;

            G.DijkstraShortestPaths(b, out d);
            if (d[a].Dist.IsNaN())
                return null;

            Edge[] firstpath = PathsInfo.ConstructPath(b, a, d);

            for (int i = 0; i < firstpath.Length; i++)
            {
                int v = firstpath[i].To;
                foreach (Edge e in g.OutEdges(v))
                {
                    if (e.To != firstpath[i].From && d[e.To].Dist + e.Weight + d[a].Dist - d[v].Dist < min)
                    {
                        min = d[e.To].Dist + e.Weight + d[a].Dist - d[v].Dist;
                        edge = e;
                        index = i;
                    }
                }
            }

            if (min == Double.MaxValue)
                return null;

            Edge[] tmp = PathsInfo.ConstructPath(b, edge.To, d);
            Array.Reverse(tmp);
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = new Edge(tmp[i].To, tmp[i].From, tmp[i].Weight);

            path = new Edge[firstpath.Length - index + tmp.Length];
            for (int i = 0; i < firstpath.Length - index - 1; i++)
                path[i] = new Edge(firstpath[firstpath.Length - i - 1].To, firstpath[firstpath.Length - i - 1].From, firstpath[firstpath.Length - i - 1].Weight);
            path[firstpath.Length - index - 1] = edge;
            Array.Copy(tmp, 0, path, firstpath.Length - index, tmp.Length);

            return min;
        }

        /// <summary>
        /// Algorytm znajdujący drugą pod względem długości najkrótszą ścieżkę między a i b.
        /// Możliwe, że jej długość jest równa najkrótszej (jeśli są dwie najkrótsze ścieżki,
        /// algorytm zwróci jedną z nich).
        /// Wymagamy, aby na ścieżce nie było powtórzeń wierzchołków ani krawędzi.  
        /// Można założyć, że a!=b oraz że w grafie nie występują pętle.
        /// </summary>
        /// <remarks>
        /// Wymagana złożoność to O(nD), gdzie D jest złożonością implementacji algorytmu Dijkstry w bibliotece Graph.
        /// </remarks>
        /// <param name="g"></param>
        /// <param name="path">null jeśli druga ścieżka nie istnieje, wpp ściezka jako ciąg krawędzi</param>
        /// <returns>null jeśli druga ścieżka nie istnieje, wpp długość tej ścieżki</returns>
        public static double? FindSecondSimpleShortestPath(this Graph g, int a, int b, out Edge[] path)
        {
            path = null;
            PathsInfo[] d, best = null;
            double min = Double.MaxValue;

            g.DijkstraShortestPaths(a, out d);
            if (d[b].Dist.IsNaN())
                return null;
            Edge[] firstpath = PathsInfo.ConstructPath(a, b, d);

            foreach (Edge e in firstpath)
            {
                g.DelEdge(e);
                g.DijkstraShortestPaths(a, out d);
                if (d[b].Dist < min)
                {
                    min = d[b].Dist;
                    best = d;
                }
                g.AddEdge(e);
            }

            if (min == Double.MaxValue)
                return null;

            path = PathsInfo.ConstructPath(a, b, best);

            return min;
        }
    }
}
