using System;
using System.Collections.Generic;
using System.Linq;

namespace EpicSeatingAssignment
{
    public class Program
    {
        private static Random rng = new Random();

        public enum Devs
        {
            Chester,
            Eric,
            Fahad,
            Jason,
            Mayur,
            Susan
        }

        static void Main(string[] args)
        {
            var current = new List<string> {"Mayur", "Fahad", "Susan", "Jason", "Eric", "Chester"};
            var seating = current.Clone();

            var sameSpot = true;
            var enemiesExist = true;
            var i = 0;
            while (enemiesExist || sameSpot)
            {
                Console.WriteLine($"Iteration {++i}");

                seating = Shuffle<string>(seating);

                sameSpot = SameSpot(current, seating);
                enemiesExist = EnemiesExist(seating);
            }
            
            Display(seating);
        }

        private static void Display(IList<string> seating)
        {
            Console.WriteLine($"{seating[3]} {seating[2]} {seating[1]}");
            Console.WriteLine($"{seating[4]} {seating[5]} {seating[0]}");
            Console.ReadLine();
        }

        public static bool SameSpot(IList<string> current, IList<string> seating)
        {
            for (var i = 0; i < current.Count; i++)
            {
                if (current[i] == seating[i]) return true;
            }

            return false;
        }

        private static bool EnemiesExist(IList<string> seating)
        {
            if (seating[5] == seating[2]) return true;
            if (seating[1] == seating[2]) return true;
            if (seating[0] == seating[1]) return true;
            if (seating[3] == seating[4]) return true;

            return false;
        }

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
