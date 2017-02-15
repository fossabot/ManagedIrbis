﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TeeLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Collections;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TeeLogger
        : IAmLogger
    {
        #region Properties

        /// <summary>
        /// Loggers.
        /// </summary>
        [NotNull]
        public NonNullCollection<IAmLogger> Loggers
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TeeLogger()
        {
            Loggers = new NonNullCollection<IAmLogger>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IAmLogger members

        /// <inheritdoc/>
        public void Debug
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Debug(text);
            }
        }

        /// <inheritdoc/>
        public void Error
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Error(text);
            }
        }

        /// <inheritdoc/>
        public void Fatal
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Fatal(text);
            }
        }

        /// <inheritdoc/>
        public void Info
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Info(text);
            }
        }

        /// <inheritdoc/>
        public void Trace
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Trace(text);
            }
        }

        /// <inheritdoc/>
        public void Warn
            (
                string text
            )
        {
            foreach (IAmLogger logger in Loggers)
            {
                logger.Warn(text);
            }
        }

        #endregion
    }
}
