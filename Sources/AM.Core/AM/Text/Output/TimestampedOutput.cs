﻿/* TimestampedOutput.cs -- output that appends timestamp
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Output that appends timestamp.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TimestampedOutput
        : AbstractOutput
    {
        #region Constants

        /// <summary>
        /// Default format.
        /// </summary>
        public const string DefaultFormat = "G";

        #endregion

        #region Properties

        /// <summary>
        /// Inner output.
        /// </summary>
        [NotNull]
        public AbstractOutput InnerOutput { get; private set; }

        /// <summary>
        /// Format
        /// </summary>
        [NotNull]
        public string Format { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TimestampedOutput
            (
                [NotNull] AbstractOutput innerOutput
            )
        {
            Code.NotNull(innerOutput, "innerOutput");

            InnerOutput = innerOutput;
        }

        #endregion

        #region Private members

        private string _GetPrefix()
        {
            string result = DateTime.Now.ToString(Format) + ": ";

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }
        public override AbstractOutput Clear()
        {
            InnerOutput.Clear();

            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            InnerOutput.Configure(configuration);

            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            InnerOutput.Write(_GetPrefix());
            InnerOutput.Write(text);

            return this;
        }

        public override AbstractOutput WriteError
            (
                string text
            )
        {
            InnerOutput.WriteError(_GetPrefix());
            InnerOutput.WriteError(text);

            return this;
        }

        #endregion
    }
}