using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class Time
    {
        /// <summary>
        /// Returns the current time in unix timestamp as a double.
        /// 
        /// This method is based on an answer provided in StackOverflow by Bartłomiej Mucha
        /// <see>https://stackoverflow.com/a/21055459/935645</see>
        /// </summary>
        public static int UnixNow()
        {
            return (int)(
                    DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)
                    ).TotalSeconds;
        }
    }

    public static class Iterables
    {
        
        /// <summary>
        /// Apply a given action to an iterable in a symmetrical fashion.
        ///
        /// By "symmetrical" we mean that the mirror transformation will applied for elements in the second half.
        /// If the iterable has odd length, the transformation will be applied normally to the last element.
        ///
        /// </summary>
        /// <param name="iterable">
        /// Iterable is the collection of elements you wish to apply a function in a symmetrical way to.
        /// </param>
        /// <param name="transformation">
        /// The action to be done to each element of the first half of the collection.
        /// </param>
        /// <param name="mirror">
        /// The action to be done to each element of the second half of the collection.
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
            var half = odds % 2 == 0 ? n/2: (n - 1) /2;
            for (var i = 0; i < n - odds; i++)
            {
                var el = enumerable[i];
                if (i < half) transformation(el);
                else mirror(el);
            }
            if (odds > 0) transformation(enumerable[n - 1]);
        }
        
        /// <summary>
        /// Creates an array of integers starting from floor(-n/2) up to n.
        ///
        /// Creates an array of integers with numbers balanced as evenly as possible around zero.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a balanced range</returns>
        public static int[] BalancedRange(int items)
        {
            return (from num in Enumerable.Range(0, items) select (num - items / 2)).ToArray();
        }
        
        /// <summary>
        /// Creates a grid from two different lists
        ///
        /// The grid will have A.Length rows and B.length columns.
        /// Each cell will have an element from A and an element from B.
        ///
        /// Example: A = [a, b, c], B = [d, e, f]
        /// returns: [[[a,d], [a,e], [a,f]], [[b,d], [b,e], [b,f]], [[c,d], [c,e], [c,f]]]
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
            {
                for (var y = 0; y < sizeB; y++)
                {
                    grid[x, y] = new T[] {listA[x], listB[y]};
                }
            }
            return grid;
        }
    }

    /// <summary>
    /// Defines project extension methods as described here:
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// This method is taken from https://www.techiedelight.com/get-subarray-of-array-csharp/
        /// It creates an extension method that can be used with objects of type IEnumerable, easing the task of taking
        /// a SubArray from an Array.
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
    
    public readonly struct Position
        {
            private bool Equals(Position other)
            {
                return x == other.x && y == other.y;
            }

            public override bool Equals(object obj)
            {
                return obj is Position other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (x * 397) ^ y;
                }
            }

            public readonly int x;
            public readonly int y;
    
            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
    
            public static Position operator +(Position a, Position b)
            {
                return new Position(a.x + b.x, a.y + b.y);
            }
    
            public static bool operator ==(Position a, Position b)
            {
                return a.x == b.x && a.y == b.y;
            }
    
            public static bool operator !=(Position a, Position b)
            {
                return !(a == b);
            }
        }

    internal static class Direction
    {
        public static readonly Position North = new Position(1, 0);
        public static readonly Position South = new Position(-1, 0);
        public static readonly Position East = new Position(0, 1);
        public static readonly Position West = new Position(0, -1);
    }
}