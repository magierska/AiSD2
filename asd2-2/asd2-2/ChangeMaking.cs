
using System;

namespace ASD
{

class ChangeMaking
    {

    /// <summary>
    /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
    /// bez ograniczeń na liczbę monet danego rodzaju
    /// </summary>
    /// <param name="amount">Kwota reszty do wydania</param>
    /// <param name="coins">Dostępne nominały monet</param>
    /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
    /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
    /// <remarks>
    /// coins[i]  - nominał monety i-tego rodzaju
    /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
    /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to metochange = null,
    /// a metoda również zwraca null
    ///
    /// Wskazówka/wymaganie:
    /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości amount ( czyli rzędu o(amount) ) obliczeniowa amount*n (2 amount2*n)
    /// </remarks>
    public static int? NoLimitsDynamic(int amount, int[] coins, out int[] change)
        {
            int[,] array = new int[amount,2];
            change = null;

            if (coins.GetLength(0) <= 0)
                return null;

            for (int j = 0; j < amount; j++)
            {
                array[j, 0] = Int32.MaxValue;
            }
          
            for (int j = 0; j < amount; j++)
            {
                foreach (int c in coins)
                {
                    if (j + 1 == c)
                    {
                        array[j, 0] = 1;
                        array[j, 1] = -1;
                    }
                    if (j + 1 + c > amount) continue;
                    if (array[j, 0] < Int32.MaxValue && array[j + c,0] > array[j,0] + 1)
                    {
                        array[j + c, 0] = array[j, 0] + 1;
                        array[j + c, 1] = j;
                    }
                }   
            }
            
            if (amount != 0 && array[amount - 1, 0] == Int32.MaxValue) return null;
            change = new int[coins.GetLength(0)];
            for (int i = 0; i < coins.GetLength(0); i++)
                change[i] = 0;

            if (amount == 0) return 0;
            int p = amount - 1, s;
            while (array[p,1] != -1)
            {
                s = p - array[p, 1];
                for (int i = 0; i < coins.GetLength(0); i++)
                    if (coins[i] == s)
                    {
                        change[i]++;
                        break;
                    }
                p -= s;
            }
            s = p - array[p, 1];
            for (int i = 0; i < coins.GetLength(0); i++)
                if (coins[i] == s)
                {
                    change[i]++;
                    break;
                }
            return array[amount-1,0];      // zmienić
        }

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// z uwzględnieniem ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="limits">Liczba dostępnych monet danego nomimału</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// limits[i] - dostepna liczba monet i-tego rodzaju (nominału)
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości iloczynu amount*(liczba rodzajów monet)
        /// ( czyli rzędu o(amount*(liczba rodzajów monet)) )
        /// </remarks>
        public static int? Dynamic(int amount, int[] coins, int[] limits, out int[] change)
        {
            int?[,] arr = new int?[coins.GetLength(0), amount + 1];
            int?[,] arr2 = new int?[coins.GetLength(0), amount + 1];
            change = null;

            if (coins.GetLength(0) <= 0)
                return null;

            for (int i = 0; i < coins.GetLength(0); i++)
                arr2[i, 0] = arr[i, 0] = 0;

            for (int i = 1; i <= limits[0] && i * coins[0] <= amount; i++)
                arr2[0, i * coins[0]] = arr[0, i * coins[0]] = i;

            for (int i = 1; i < coins.GetLength(0); i++)
            {
                for (int j = 1; j <= amount; j++)
                {
                    arr[i, j] = arr[i - 1, j];
                    if (arr[i,j] != null)
                        arr2[i, j] = 0;
                }
                for (int j = 0; j <= amount; j++)
                {
                    if (arr[i - 1, j] != null)
                    {
                        for (int k = 1; k <= limits[i] && k * coins[i] + j <= amount; k++)
                        {
                            if (arr[i, j + k * coins[i]] == null || arr[i, j + k * coins[i]] > arr[i, j] + k)
                            {
                                arr[i, j + k * coins[i]] = arr[i, j] + k;
                                arr2[i, j + k * coins[i]] = k;
                            }
                        }
                    }
                }
            }

            int sum = 0;
            if (arr[coins.GetLength(0) - 1, amount] != null)
            {
                change = new int[coins.GetLength(0)];
                int t = amount;
                for (int i = coins.GetLength(0) - 1; i >= 0; i--)
                {
                    dynamic p = arr2[i, t];
                    change[i] = p == null ? 0 : p;
                    t -= change[i] * coins[i];
                    sum += change[i]; 
                }
            }

            if (amount != 0 && sum == 0)
                return null;

            return sum;      // zmienić
        }

    }

}
