using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EpicSeatingAssignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var previousArrangement = GetPreviousSeatingArrangement();

            var newArrangement = previousArrangement.Clone();

            var i = 0;
            do
            {
                Console.WriteLine($"Iteration {++i}");
                newArrangement = Extensions.Shuffle<string>(newArrangement);
            } while (SamePosition(previousArrangement, newArrangement) || EnemiesExist(previousArrangement, newArrangement));

            SaveArrangement(newArrangement);
            DisplayArrangement(newArrangement, i);
        }

        private static IList<string> GetPreviousSeatingArrangement()
        {
            return File.ReadLines("../../../arrangement.txt").Last().Split(',').ToList();
        }

        private static void SaveArrangement(IList<string> newArrangement)
        {
            using (var sw = new StreamWriter("../../../arrangement.txt", append: true))
            {
                sw.WriteLine('\n' + string.Join(',', newArrangement).Trim());
                sw.Close();
            }
        }

        private static void DisplayArrangement(IList<string> newArragement, int i)
        {
            Console.WriteLine($"\n{newArragement[3]}\t{newArragement[2]}\t{newArragement[1]}\t|");
            Console.WriteLine($"{newArragement[4]}\t{newArragement[5]}\t{newArragement[0]}\t|");
            Console.WriteLine($"\nSolved in {i} iterations");
        }

        public static bool SamePosition(IList<string> previousArrangement, IList<string> newArrangement)
        {
            return previousArrangement.Where((t, i) => t == newArrangement[i]).Any();
        }

        private static bool EnemiesExist(IList<string> previousArrangement, IList<string> newArrangement)
        {
            return HaveSwapped(previousArrangement, newArrangement, 0, 5) ||
                   HaveSwapped(previousArrangement, newArrangement, 1, 2) ||
                   HaveSwapped(previousArrangement, newArrangement, 2, 3) ||
                   HaveSwapped(previousArrangement, newArrangement, 4, 5);
        }

        private static bool HaveSwapped(IList<string> previousArrangement, IList<string> newArrangement, int i, int j)
        {
            return previousArrangement[i] == newArrangement[j] && previousArrangement[j] == newArrangement[i];
        }
    }

    public static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        private static readonly Random Seed = new Random();

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            var n = list.Count;

            while (n > 1)
            {
                n--;
                var k = Seed.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}