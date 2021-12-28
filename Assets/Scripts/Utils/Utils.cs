using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class Time
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
}