﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TestContext.cs -- context of test execution
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Testing
{
    /// <summary>
    /// Context of <see cref="AbstractTest"/> execution.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TestContext
    {
        #region Properties

        /// <summary>
        /// Duration.
        /// </summary>
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Test failed?
        /// </summary>
        [JsonProperty]
        public bool Failed { get; set; }

        /// <summary>
        /// Finish time.
        /// </summary>
        [JsonProperty("finish")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Name of the test.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Output channel.
        /// </summary>
        [NotNull]
        [JsonIgnore]
        public AbstractOutput Output
        {
            get { return _output; }
        }

        /// <summary>
        /// Start time.
        /// </summary>
        [JsonProperty("start")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [NotNull]
        [JsonProperty("output")]
        public string Text
        {
            get { return _text.ToString(); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestContext
            (
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNull(output, "output");

            _text = new TextOutput();

            _output = new TeeOutput
                (
                    output,
                    _text
                );
        }

        #endregion

        #region Private members

        private readonly AbstractOutput _output;
        private readonly TextOutput _text;

        #endregion
    }
}
