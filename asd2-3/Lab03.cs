
using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace ASD
{

public static class Lab03GraphExtender
    {

    /// <summary>
    /// Wyszukiwanie cykli w grafie
    /// </summary>
    /// <param name="g">Badany graf</param>
    /// <param name="cycle">Znaleziony cykl</param>
    /// <returns>Informacja czy graf jest acykliczny</returns>
    /// <remarks>
    /// 1) Algorytm powinien dzia³aæ zarówno dla grafów skierowanych, jak i nieskierowanych
    /// 2) Grafu nie wolno zmieniaæ
    /// 3) Jeœli graf zawiera cykl to parametr cycle powinien byæ tablic¹ krawêdzi tworz¹cych dowolny z cykli.
    ///    Krawêdzie musz¹ byæ umieszczone we w³aœciwej kolejnoœci (tak jak w cyklu, mo¿na rozpocz¹æ od dowolnej)
    /// 4) Jeœli w grafie nie ma cyklu to parametr cycle ma wartoœæ null.
    /// </remarks>
    public static bool FindCycle(this Graph g, out Edge[] cycle)
        {
            cycle = null;
            int cc;
            g.DFSearchAll(null, null, out cc);
            if (!g.Directed && g.EdgesCount + cc <= g.VerticesCount)
                return false;
            bool jestcykl = false;
            for (int v = 0; v < g.VerticesCount; v++)
            {
                Edge[] edges = new Edge[2*g.EdgesCount];
                int count = 0;
                g.GeneralSearchFrom<EdgesStack>(v,null,null,e => 
                {
                        edges[count] = e;
                        count++;
                        if (e.To == v)
                        {
                            if (!g.Directed)
                                for (int i = 0; i < count - 1; i++)
                                    if (edges[i].From == e.To && edges[i].To == e.From)
                                        return true;
                            jestcykl = true;
                            return false;
                        }
                    return true;
                });
                if (jestcykl)
                {
                    Edge[] p = new Edge[count];
                    int[] vert = new int[g.VerticesCount];
                    for (int j = 0; j < g.VerticesCount; j++)
                        vert[j] = -1;
                    vert[edges[count - 1].From] = 0;       
                    p[0] = edges[count - 1];
                    int k = 1;
                    for (int i = 1; i < count; i++)
                        if (p[k - 1].From == edges[count - i - 1].To)
                        {
                            if (vert[edges[count - i - 1].From] >= 0)
                            {
                                for (int j = vert[edges[count - i - 1].From] + 1; j < k; j++)
                                    vert[p[j].From] = -1;
                                k = vert[edges[count - i - 1].From]+1;
                            }
                            else
                            { 
                                p[k] = edges[count - i - 1];
                                vert[edges[count - i - 1].From] = k;
                                k++;
                            }
                        }
                    cycle = new Edge[k];
                    for (int i = 0; i < k; i++)
                        cycle[i] = p[k - i - 1];
                    return true;
                }
            }
            return false;
        }

    /// <summary>
    /// Wyznaczanie centrum drzewa
    /// </summary>
    /// <param name="g">Badany graf</param>
    /// <param name="center">Znalezione centrum</param>
    /// <returns>Informacja czy badany graf jest drzewem</returns>
    /// <remarks>
    /// 1) Dla grafów skierowanych metoda powinna zg³aszaæ wyj¹tek ArgumentException
    /// 2) Grafu nie wolno zmieniaæ
    /// 3) Parametr center to 1-elementowa lub 2-elementowa tablica zawieraj¹ca numery wierzcho³ków stanowi¹cych centrum.
    ///    (w przypadku 2 wierzcho³ków ich kolejnoœæ jest dowolna)
    /// </remarks>
    public static bool TreeCenter(this Graph g, out int[] center)
        {
            center = null;
            if (g.Directed)
                throw new ArgumentException("Graf skierowany");
            int cc;
            g.DFSearchAll(null, null, out cc);
            if (cc != 1)
                return false;
            if (g.VerticesCount != g.EdgesCount + 1)
                return false;
            Graph t = g.Clone();
            int[] vert = new int[t.VerticesCount];
            int count = t.VerticesCount;
            for (int v = 0; v < t.VerticesCount; v++)
                vert[v] = 1;
            while (count > 2)
            {
                for (int v = 0; v < t.VerticesCount; v++)
                {
                    if (vert[v] == 1)
                    {
                        int k = 0;
                        foreach (Edge e in t.OutEdges(v))
                            if (vert[e.To] != -1)
                                k++;
                        if (k == 1)
                            vert[v] = 0;
                    }
                }
                for (int v = 0; v < t.VerticesCount; v++)
                    if (vert[v] == 0)
                    {
                        vert[v] = -1;
                        count--;
                    }
            }
            int i = 0;
            int[] p = new int[2];
            for (int v = 0; v < t.VerticesCount; v++)
                if (vert[v] == 1)
                {
                    p[i] = v;
                    i++;
                }
            center = new int[i];
            for (int j = 0; j < i; j++)
                center[j] = p[j];
            return true;
        }

    }

}
