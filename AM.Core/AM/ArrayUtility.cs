/* ArrayUtility.cs -- array manipulation helpers
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// <see cref="Array"/> manipulation helper methods.
    /// </summary>
    [PublicAPI]
    public static class ArrayUtility
    {
        #region Public methods

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="FROM">Type of source array.</typeparam>
        /// <typeparam name="TO">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static TO[] ChangeType<FROM, TO>
            (
                [NotNull] FROM[] sourceArray
            )
        {
            Code.NotNull(sourceArray, "sourceArray");

            TO[] result = new TO[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="TO">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static TO[] ChangeType<TO>
            (
                [NotNull] Array sourceArray
            )
        {
            Code.NotNull(sourceArray, "sourceArray");

            TO[] result = new TO[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Compares two specified arrays by elements.
        /// </summary>
        /// <param name="firstArray">First array to compare.</param>
        /// <param name="secondArray">Second array to compare.</param>
        /// <returns><para>Less than zero - first array is less.</para>
        /// <para>Zero - arrays are equal.</para>
        /// <para>Greater than zero - first array is greater.</para>
        /// </returns>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="firstArray"/> or 
        /// <paramref name="secondArray"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Length of
        /// <paramref name="firstArray"/> is not equal to length of
        /// <paramref name="secondArray"/>.
        /// </exception>
        public static int Compare<T>
            (
                T[] firstArray, 
                T[] secondArray
            )
            where T : IComparable<T>
        {
            Code.NotNull(firstArray, "firstArray");
            Code.NotNull(secondArray, "secondArray");
            if (firstArray.Length
                 != secondArray.Length)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < firstArray.Length; i++)
            {
                int result = firstArray[i].CompareTo(secondArray[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static TO[] Convert<FROM, TO>(FROM[] array)
        {
            Code.NotNull(array, "array");

            TO[] result = new TO[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = ConversionUtility.ConvertTo<TO>(array[i]);
            }

            return result;
        }

        /// <summary>
        /// Creates the array of specified length initializing it with
        /// specified value.
        /// </summary>
        /// <param name="length">Desired length of the array.</param>
        /// <param name="initialValue">The initial value of
        /// array items.</param>
        /// <returns>Created and initialized array.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        public static T[] Create<T>(int length, T initialValue)
        {
            Code.Nonnegative(length, "length");

            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = initialValue;
            }

            return result;
        }

        /// <summary>
        /// Filters the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> or <paramref name="predicate"/>
        /// is <c>null</c>.
        /// </exception>
        public static T[] Filter<T>(T[] array, Predicate<T> predicate)
        {
            Code.NotNull(array, "array");
            Code.NotNull(predicate, "predicate");

            List<T> result = new List<T>(array.Length);
            foreach (T item in array)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Determines whether the specified array is null or empty
        /// (has zero length).
        /// </summary>
        /// <param name="array">Array to check.</param>
        /// <returns><c>true</c> if the array is null or empty;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty
            (
                Array array
            )
        {
            return (ReferenceEquals(array, null)
                     || (array.Length == 0));
        }

        /// <summary>
        /// Maps the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>Converted array.</returns>
        /// <typeparam name="FROM">Type of source array item.
        /// </typeparam>
        /// <typeparam name="TO">Type of destination array item.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> or <paramref name="converter"/>
        /// is <c>null</c>.
        /// </exception>
        public static TO[] Map<FROM, TO>
            (
                FROM[] array,
                Converter<FROM, TO> converter
            )
        {
            Code.NotNull(array, "array");
            Code.NotNull(converter, "converter");

            TO[] result = new TO[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = converter(array[i]);
            }

            return result;
        }

        /// <summary>
        /// Merges the specified arrays.
        /// </summary>
        /// <param name="arrays">Arrays to merge.</param>
        /// <returns>Array that consists of all <paramref name="arrays"/>
        /// items.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// At least one of <paramref name="arrays"/> is <c>null</c>.
        /// </exception>
        public static T[] Merge<T>(params T[][] arrays)
        {
            int resultLength = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                if (arrays[i] == null)
                {
                    throw new ArgumentNullException("arrays");
                }
                resultLength += arrays[i].Length;
            }

            T[] result = new T[resultLength];
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                arrays[i].CopyTo(result, offset);
                offset += arrays[i].Length;
            }

            return result;
        }

        /// <summary>
        /// Reduces the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="func">The func.</param>
        /// <param name="init">The init.</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of array item.
        /// </typeparam>
        /// <typeparam name="V">Type of result.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> or <paramref name="func"/>
        /// is <c>null</c>.
        /// </exception>
        public static V Reduce<T, V>
            (
                T[] array,
                Func<T, V, V> func, 
                V init
            )
        {
            Code.NotNull(array, "array");
            Code.NotNull(func, "func");

            V result = init;
            foreach (T item in array)
            {
                result = func(item, result);
            }

            return result;
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static string[] ToString<T>
            (
                T[] array
            )
        {
            Code.NotNull(array, "array");

            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                object o = array[i];
                if (o != null)
                {
                    result[i] = array[i].ToString();
                }
            }

            return result;
        }

        #endregion
    }
}