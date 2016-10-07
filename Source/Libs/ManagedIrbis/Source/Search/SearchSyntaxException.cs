﻿/* SearchSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public sealed class SearchSyntaxException
        : SearchException
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException
            (
                string message,
                Exception innerException
            )
            : base
            (
                message,
                innerException
            )
        {
        }

        #endregion

        #region Public methods

        #endregion
    }
}
