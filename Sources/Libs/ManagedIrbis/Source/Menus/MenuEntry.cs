﻿/* MenuEntry.cs -- MNU file entry
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// MNU file entry. Represents two lines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("entry")]
    [DebuggerDisplay("{Code} = {Comment}")]
    public sealed class MenuEntry
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// First line -- the code.
        /// </summary>
        [NotNull]
        [XmlAttribute("code")]
        [JsonProperty("code")]
        // ReSharper disable NotNullMemberIsNotInitialized
        public string Code { get; set; }
        // ReSharper restore NotNullMemberIsNotInitialized

        /// <summary>
        /// Second line -- the comment.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Restore object state from given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Code = reader.ReadNullableString();
            Comment = reader.ReadNullableString();
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Save object state to the stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Code)
                .WriteNullable(Comment);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should JSON serialize the comment?
        /// </summary>
        public bool ShouldSerializeComment()
        {
            return !string.IsNullOrEmpty(Comment);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Code: {0}, Comment: {1}",
                    Code,
                    Comment
                );
        }

        #endregion
    }
}