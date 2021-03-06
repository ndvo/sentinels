using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class Time
    {
        /// <summary>
        ///     Returns the current time in unix timestamp as a double.
        ///     This method is based on an answer provided in StackOverflow by Bartłomiej Mucha
        ///     <see>https://stackoverflow.com/a/21055459/935645</see>
        /// </summary>
        public static int UnixNow()
        {
            return (int) (
                DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)
            ).TotalSeconds;
        }
    }

    public static class Iterables
    {
        /// <summary>
        ///     Apply a given action to an iterable in a symmetrical fashion.
        ///     By "symmetrical" we mean that the mirror transformation will applied for elements in the second half.
        ///     If the iterable has odd length, the transformation will be applied normally to the last element.
        /// </summary>
        /// <param name="iterable">
        ///     Iterable is the collection of elements you wish to apply a function in a symmetrical way to.
        /// </param>
        /// <param name="transformation">
        ///     The action to be done to each element of the first half of the collection.
        /// </param>
        /// <param name="mirror">
        ///     The action to be done to each element of the second half of the collection.
        /// </param>
        public static void SymmetricalApply<T>(
            IEnumerable<T> iterable,
            Action<T> transformation,
            Action<T> mirror
        )
        {
            var enumerable = iterable as T[] ?? iterable.ToArray();
            var n = enumerable.Count();
            if (n == 0) return;
            var odds = n % 2;
            var half = odds % 2 == 0 ? n / 2 : (n - 1) / 2;
            for (var i = 0; i < n - odds; i++)
            {
                var el = enumerable[i];
                if (i < half) transformation(el);
                else mirror(el);
            }

            if (odds > 0) transformation(enumerable[n - 1]);
        }

        /// <summary>
        ///     Creates an array of integers starting from floor(-n/2) up to n.
        ///     Creates an array of integers with numbers balanced as evenly as possible around zero.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a balanced range</returns>
        public static int[] BalancedRange(int items)
        {
            return (from num in Enumerable.Range(0, items) select num - items / 2).ToArray();
        }

        /// <summary>
        ///     Creates a grid from two different lists
        ///     The grid will have A.Length rows and B.length columns.
        ///     Each cell will have an element from A and an element from B.
        ///     Example: A = [a, b, c], B = [d, e, f]
        ///     returns: [[[a,d], [a,e], [a,f]], [[b,d], [b,e], [b,f]], [[c,d], [c,e], [c,f]]]
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <typeparam name="T">the type of the element in the array</typeparam>
        /// <returns></returns>
        public static T[,][] CreateGrid<T>(T[] listA, T[] listB)
        {
            var sizeA = listA.Length;
            var sizeB = listB.Length;
            var grid = new T[sizeA, sizeB][];
            for (var x = 0; x < sizeA; x++)
            for (var y = 0; y < sizeB; y++)
                grid[x, y] = new[] {listA[x], listB[y]};
            return grid;
        }
    }

    /// <summary>
    ///     Defines project extension methods as described here:
    ///     https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     This method is taken from https://www.techiedelight.com/get-subarray-of-array-csharp/
        ///     It creates an extension method that can be used with objects of type IEnumerable, easing the task of taking
        ///     a SubArray from an Array.
        /// </summary>
        /// <param name="array">The array from which to take a subset</param>
        /// <param name="offset">The index of the first position in the SubArray</param>
        /// <param name="length">The size of the SubArray</param>
        /// <typeparam name="T">They type of the object contained in the array</typeparam>
        /// <returns></returns>
        public static T[] SubArray<T>(this IEnumerable<T> array, int offset, int length)
        {
            return array.Skip(offset).Take(length).ToArray();
        }
    }

    /// <summary>
    ///     Position struct is a x,y pair that allow representing positions as if this was a 2d game.
    ///     This struct allows for easy comparison and operations.
    /// </summary>
    public readonly struct Position
    {
        private bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public readonly int X;
        public readonly int Y;

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
    }

    /// <summary>
    ///     Direction provides an easy to referent set of directions.
    ///     Directions are represented as unity Positions.
    /// </summary>
    internal static class Direction
    {
        public static readonly Position North = new Position(0, -1);
        public static readonly Position South = new Position(0, 1);
        public static readonly Position East = new Position(1, 0);
        public static readonly Position West = new Position(-1, 0);

        public static string ToString(Position pos)
        {
            if (pos == North) return "North";
            if (pos == South) return "South";
            if (pos == West) return "West";
            if (pos == East) return "East";
            throw new Exception("Unknown direction");
        }

        public static float ToAngle(Position pos)
        {
            if (pos == North) return 0f;
            if (pos == South) return 180f;
            if (pos == West) return 90f;
            if (pos == East) return -90f;
            return 0f;
        }
    }
}