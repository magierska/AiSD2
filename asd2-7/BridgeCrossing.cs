using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class BridgeCrossing
    {
        static int minimum;
        static List<List<int>> minoutlist;
        static int[] people;
        static int sum;
        static List<List<int>> outlist;
        static bool[] onright;
        static int numberonright;

        /// <summary>
        /// Metoda rozwiązuje zadanie optymalnego przechodzenia przez most.
        /// </summary>
        /// <param name="_times">Tablica z czasami przejścia poszczególnych osób</param>
        /// <param name="strategy">Strategia przekraczania mostu: lista list identyfikatorów kolejnych osób,
        /// które przekraczają most (na miejscach parzystych przejścia par przez most,
        /// na miejscach nieparzystych powroty jednej osoby z latarką). Jeśli istnieje więcej niż jedna strategia
        /// realizująca przejście w optymalnym czasie wystarczy zwrócić dowolną z nich.</param>
        /// <returns>Minimalny czas, w jakim wszyscy turyści mogą pokonać most</returns>
        public static int CrossBridge(int[] times, out List<List<int>> strategy)
        {
            minimum = Int32.MaxValue;
            minoutlist = new List<List<int>>();
            people = times;

            if (times.Length == 1)
            {
                List<int> tmp = new List<int>();
                tmp.Add(0);
                strategy = new List<List<int>>();
                strategy.Add(tmp);
                return times[0];
            }

            for (int i = 0; i < times.Length; i++)
                for (int j = i + 1; j < times.Length; j++)
                {
                    sum = 0;
                    numberonright = 0;
                    outlist = new List<List<int>>();
                    onright = new bool[times.Length];
                    crossbridgerec(i, j);
                }

            strategy = minoutlist;
            return minimum;
        }

        public static bool crossbridgerec(int index1, int index2)
        {
            int stillonleft = 0;
            for (int i = 0; i < onright.Length; i++)
                if (!onright[i])
                    stillonleft += people[i];
            if (minimum < sum + stillonleft / 2)
                return false;

            numberonright += 2;
            sum = sum + Math.Max(people[index1],people[index2]);
            onright[index1] = onright[index2] = true;
            List<int> tmp = new List<int>();
            tmp.Add(index1);
            tmp.Add(index2);
            outlist.Add(tmp);

            if (numberonright >= people.Length)
            {
                if (sum < minimum)
                {
                    minimum = sum;
                    minoutlist = new List<List<int>>(outlist);
                    numberonright -= 2;
                    sum = sum - Math.Max(people[index1], people[index2]);
                    onright[index1] = onright[index2] = false;
                    outlist.RemoveRange(outlist.Count - 1, 1);
                    return true;
                }
                numberonright -= 2;
                sum = sum - Math.Max(people[index1], people[index2]);
                onright[index1] = onright[index2] = false;
                outlist.RemoveRange(outlist.Count - 1, 1);
                return false;
            }

            if (sum > minimum)
            {
                numberonright -= 2;
                sum = sum - Math.Max(people[index1], people[index2]);
                onright[index1] = onright[index2] = false;
                outlist.RemoveRange(outlist.Count - 1, 1);
                return false;
            }

            int mintime = Int32.MaxValue, ind = 0;
            for (int i = 0; i < onright.Length; i++)
                if (onright[i] && mintime > people[i])
                {
                    mintime = people[i];
                    ind = i;
                }

            numberonright--;
            sum += people[ind];
            onright[ind] = false;
            tmp = new List<int>();
            tmp.Add(ind);
            outlist.Add(tmp);

            for (int i = 0; i < onright.Length; i++)
                if (!onright[i])
                    for (int j = i + 1; j < onright.Length; j++)
                        if (!onright[j])
                        {
                            crossbridgerec(i, j);
                        }

            numberonright--;
            sum = sum - people[ind] - Math.Max(people[index1], people[index2]);
            onright[ind] = true;
            onright[index1] = onright[index2] = false;
            outlist.RemoveRange(outlist.Count - 2, 2);

            return false;
        }

        // MOŻESZ DOPISAĆ POTRZEBNE POLA I METODY POMOCNICZE
        // MOŻESZ NAWET DODAĆ CAŁE KLASY (ALE NIE MUSISZ)

    }
}