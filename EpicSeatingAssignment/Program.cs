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
            Console.ReadLine();
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

        private static void DisplayArrangement(IList<string> newArrangement, int i)
        {
            Console.WriteLine($"\n{newArrangement[3]}\t{newArrangement[2]}\t{newArrangement[1]}\t|");
            Console.WriteLine($"{newArrangement[4]}\t{newArrangement[5]}\t{newArrangement[0]}\t|");
            Console.WriteLine($"\nSolved in {i} iterations");
        }

        public static bool SamePosition(IList<string> previousArrangement, IList<string> newArrangement)
        {
            return previousArrangement.Where((t, i) => t == newArrangement[i]).Any();
        }

        private static bool EnemiesExist(IList<string> previousArrangement, IList<string> newArrangement)
        {
            return WerePreviousEnemies(newArrangement[1], newArrangement[2], previousArrangement) ||
                   WerePreviousEnemies(newArrangement[2], newArrangement[3], previousArrangement) ||
                   WerePreviousEnemies(newArrangement[4], newArrangement[5], previousArrangement) ||
                   WerePreviousEnemies(newArrangement[0], newArrangement[5], previousArrangement);
        }

        private static bool WerePreviousEnemies(string p1, string p2, IList<string> previousArrangement)
        {
            return previousArrangement[1] == p1 && previousArrangement[2] == p2 ||
                previousArrangement[2] == p1 && previousArrangement[1] == p2 ||
                previousArrangement[2] == p1 && previousArrangement[3] == p2 ||
                previousArrangement[3] == p1 && previousArrangement[2] == p2 ||
                previousArrangement[4] == p1 && previousArrangement[5] == p2 ||
                previousArrangement[5] == p1 && previousArrangement[4] == p2 ||
                previousArrangement[0] == p1 && previousArrangement[5] == p2 ||
                previousArrangement[5] == p1 && previousArrangement[0] == p2;
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