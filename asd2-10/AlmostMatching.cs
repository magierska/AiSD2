using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace lab10
{

    public struct AlmostMatchingSolution
    {
        public AlmostMatchingSolution(int edgesCount, List<Edge> solution)
        {
            this.edgesCount = edgesCount;
            this.solution = solution;
        }

        public readonly int edgesCount;
        public readonly List<Edge> solution;
    }



    public class AlmostMatching
    {
        public static int maxrisk;
        public static int risk;
        public static int maxedges;
        public static double minweight;
        public static double currentweight;
        public static int usededgescount;
        public static int edgescount;
        public static Edge[] edges;
        public static bool[] usededges;
        public static bool[] bestusededges;
        public static int[] usedvertices;
        /// <summary>
        /// Zwraca najliczniejszy możliwy zbiór krawędzi, którego poziom
        /// ryzyka nie przekracza limitu. W ostatnim etapie zwracać
        /// zbiór o najmniejszej sumie wag ze wszystkich najliczniejszych.
        /// </summary>
        /// <returns>Liczba i lista linek (krawędzi)</returns>
        /// <param name="g">Graf linek</param>
        /// <param name="allowedCollisions">Limit ryzyka</param>
        public static AlmostMatchingSolution LargestS(Graph g, int allowedCollisions)
        {
            List<Edge> solution = new List<Edge>();
            maxrisk = allowedCollisions;
            risk = 0;
            maxedges = 0;
            usededgescount = 0;
            minweight = Double.MaxValue;
            currentweight = 0;
            edgescount = g.EdgesCount;
            edges = new Edge[g.EdgesCount];
            usededges = new bool[g.EdgesCount];
            bestusededges = new bool[g.EdgesCount];
            usedvertices = new int[g.VerticesCount];
            int i = 0;

            for (int v = 0; v < g.VerticesCount; v++)
            {
                usedvertices[v] = 0;
                foreach (Edge e in g.OutEdges(v))
                {
                    bool flag = false;
                    for (int j = 0; j < i; j++)
                        if ((e.From == edges[j].From && e.To == edges[j].To) || (e.From == edges[j].To && e.To == edges[j].From))
                            flag = true;
                    if (!flag)
                    {
                        usededges[i] = false;
                        edges[i] = e;
                        i++;
                    }
                }
            }

            LargestSRec(0);

            for (i = 0; i < edgescount; i++)
                if (bestusededges[i])
                    solution.Add(edges[i]);

            return new AlmostMatchingSolution(maxedges, solution);
        }

        public static void LargestSRec(int edge)
        {
            while (edge < edgescount)
            {
                if (usededgescount + edgescount - edge < maxedges)
                    return;

                int addedrisk = 0;
                if (usedvertices[edges[edge].To] > 0)
                {
                    addedrisk++;
                }
                if (usedvertices[edges[edge].From] > 0)
                {
                    addedrisk++;
                }

                if (risk + addedrisk <= maxrisk)
                {
                    usedvertices[edges[edge].To]++;
                    usedvertices[edges[edge].From]++;
                    currentweight += edges[edge].Weight;
                    usededgescount++;
                    risk += addedrisk;
                    usededges[edge] = true;

                    if (usededgescount > maxedges || (usededgescount == maxedges && currentweight < minweight))
                    {
                        minweight = currentweight;
                        maxedges = usededgescount;
                        bestusededges = (bool[])usededges.Clone();
                    }

                    LargestSRec(edge + 1);

                    usedvertices[edges[edge].To]--;
                    usedvertices[edges[edge].From]--;
                    currentweight -= edges[edge].Weight;
                    usededges[edge] = false;
                    risk -= addedrisk;
                    usededgescount--;
                }
                edge++;
            }

            return;
        }
    }

}


