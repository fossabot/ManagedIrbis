﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientLM.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Client LM.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ClientLM
    {
        #region Constants

        /// <summary>
        /// Default salt.
        /// </summary>
        public const string DefaultSalt = "Ассоциация ЭБНИТ";

        #endregion

        #region Properties

        /// <summary>
        /// Encoding.
        /// </summary>
        [NotNull]
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Salt.
        /// </summary>
        [CanBeNull]
        public string Salt { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientLM()
            : this
                (
                    IrbisEncoding.Ansi,
                    DefaultSalt
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="salt"></param>
        public ClientLM
            (
                [NotNull] Encoding encoding, 
                [CanBeNull] string salt
            )
        {
            Code.NotNull(encoding, "encoding");

            Encoding = encoding;
            Salt = salt;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Check IRBIS64-hash for the INI-file.
        /// </summary>
        public bool CheckHash64
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            string user = iniFile.GetValue("Main", "User", null);
            string common = iniFile.GetValue("Main", "Common", null);

            if (string.IsNullOrEmpty(user)
                || string.IsNullOrEmpty(common))
            {
                return false;
            }

            string hash = ComputeHash64(user);

            return hash == common;
        }

        /// <summary>
        /// Compute IRBIS32-hash for the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [NotNull]
        public string ComputeHash32
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            return string.Empty;
        }

        /// <summary>
        /// Compute IRBIS64-hash for the text.
        /// </summary>
        [NotNull]
        public string ComputeHash64
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            string salted = Salt + text;
            byte[] raw = Encoding.GetBytes(salted);
            unchecked
            {
                int sum = 0;
                foreach (byte one in raw)
                {
                    sum += one;
                }

                raw = Encoding.GetBytes
                    (
                        sum.ToString(CultureInfo.InvariantCulture)
                    )
                    .Reverse()
                    .ToArray();

                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] += 0x6D;
                }
            }

            string result = Encoding.GetString(raw);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}