﻿/* DictionaryUtility.cs -- dictionary manipulation helpers
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// <see cref="Dictionary{Key,Value}" /> manipulation
    /// helper methods.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DictionaryUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeWithConflicts<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeWithoutConflicts<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    if (!result.ContainsKey(pair.Key))
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }
            return result;
        }

        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeLastValues<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    result[pair.Key] = pair.Value;
                }
            }
            return result;
        }

        #endregion
    }
}