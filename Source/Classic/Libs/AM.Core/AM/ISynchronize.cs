﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ISynchronize.cs -- general interface of synchronizable objects
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// General interface of synchronizable objects.
    /// </summary>
    [PublicAPI]
    public interface ISynchronize
    {
        /// <summary>
        /// Synchronization root for the object.
        /// </summary>
        /// <remarks><para>Notes for implementers:</para>
        /// <list type="bullet">
        /// <item><see cref="SyncRoot"/> must be non-<c>null</c>.</item>
        /// <item><see cref="SyncRoot"/> must be unique for each
        /// synchronizable object.</item>
        /// </list>
        /// </remarks>
        [NotNull]
        object SyncRoot { get; }
    }
}