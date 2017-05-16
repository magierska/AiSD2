using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    class VelocityMeasurements
    {
        /// <summary>
        /// Metoda zwraca możliwą minimalną i maksymalną wartość prędkości samochodu w momencie wypadku.
        /// </summary>
        /// <param name="measurements">Tablica zawierające wartości pomiarów urządzenia zainstalowanego w aucie Mateusza</param>
        /// <param name="isBrakingValue">Tablica zwracająca informację dla każdego z pomiarów z tablicy measurements informację bool czy dla sekwencji dającej 
        /// minimalną prędkość wynikową traktować dany pomiar jako hamujący (true) przy przyspieszający (false)</param>
        /// <returns>Struktura Velocities z informacjami o najniższej i najwyższej możliwej prędkości w momencie wypadku</returns>
        /// 
        /// <remarks>
        /// Złożoność pamięciowa algorytmu powinna być nie większa niż O(sumy_wartości_pomiarów).
        /// Złożoność czasowa algorytmu powinna być nie większa niż O(liczby_pomiarów * sumy_wartości_pomiarów).
        /// </remarks>
        public static Velocities FinalVelocities(int[] measurements, out bool[] isBrakingValue)
        {
            isBrakingValue = new bool[measurements.Length];
            for (int j = 0; j < measurements.Length; j++)
                isBrakingValue[j] = false;
            int max = measurements.Sum();
            int?[] vel = new int?[max + 1];
            vel[0] = -1;
            for (int i = 1; i <= max; i++)
                vel[i] = null;
            for (int i = 0; i < measurements.Length; i++)
            {
                for (int j = 0; j <= max; j++)
                {
                    if (vel[j] != null && vel[j] != i && vel[j + measurements[i]] == null)
                        vel[j + measurements[i]] = i;
                    if (j + measurements[i] > max / 2)
                        break;
                }
            }
            int p = max / 2, min;
            while (vel[p] == null)
                p--;
            min = max - 2 * p;
            while (vel[p] >= 0)
            {
                dynamic t = vel[p];
                isBrakingValue[t] = true;
                p = p - measurements[t];
            }
            return new Velocities(min, max);
        }

        /// <summary>
        /// Metoda zwraca możliwą minimalną i maksymalną wartość prędkości samochodu w trakcie całego okresu trwania podróży.
        /// </summary>
        /// <param name="measurements">Tablica zawierające wartości pomiarów urządzenia zainstalowanego w aucie Mateusza</param>
        /// <param name="isBrakingValue">W tej wersji algorytmu proszę ustawić parametr na null</param>
        /// <returns>Struktura Velocities z informacjami o najniższej i najwyższej możliwej prędkości na trasie</returns>
        /// 
        /// <remarks>
        /// Złożoność pamięciowa algorytmu powinna być nie większa niż O(sumy_wartości_pomiarów).
        /// Złożoność czasowa algorytmu powinna być nie większa niż O(liczby_pomiarów * sumy_wartości_pomiarów).
        /// </remarks>
        public static Velocities JourneyVelocities(int[] measurements, out bool[] isBrakingValue)
        {
            isBrakingValue = null;  // Nie zmieniać !!!

            int max = measurements.Sum(), p;
            int[] vel = new int[max + 1];
            for (int i = 0; i <= max; i++)
                vel[i] = -1;
            vel[measurements[0]] = 0;
            for (int i = 1; i < measurements.GetLength(0); i++)
            {
                int[] pom = new int[max + 1];
                for (int j = 0; j <= max; j++)
                    pom[j] = -1;
                for (int j = 0; j <= max; j++)
                    if (vel[j] == i-1)
                    {
                        p = Math.Abs(j - measurements[i]);
                        pom[p] = i;
                        if (p == 0)
                            return new Velocities(0,max);
                        p = j + measurements[i];
                        pom[p] = i;
                        if (p == 0)
                            return new Velocities(0, max);
                    }
                for (int j = 0; j <= max; j++)
                    if (pom[j] >= 0)
                        vel[j] = pom[j];
            }
            int min = 0;
            while (vel[min] < 0)
                min++;

            return new Velocities(min, max);
        }
    }
}
