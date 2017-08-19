﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ServerUtility
    {
        #region Constants

        /// <summary>
        /// Inclusion begin sign.
        /// </summary>
        public const char InclusionStart = '\x1C';

        /// <summary>
        /// Inclusion end sign.
        /// </summary>
        public const char InclusionEnd = '\x1D';

        #endregion

        #region Public methods

        /// <summary>
        /// Expand inclusion.
        /// </summary>
        [NotNull]
        public static string ExpandInclusion
            (
                [NotNull] string text,
                [NotNull] string extension,
                [NotNull] string[] pathArray
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(extension, "extension");
            Code.NotNull(pathArray, "pathArray");

            if (pathArray.Length == 0)
            {
                throw new IrbisException();
            }

            if (text.Contains(InclusionStart))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length * 2);
            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                string prefix = navigator.ReadUntil(InclusionStart);
                result.Append(prefix);
                if (navigator.ReadChar() != InclusionStart)
                {
                    break;
                }
                string fileName = navigator.ReadUntil(InclusionEnd);
                if (string.IsNullOrEmpty(fileName)
                    || navigator.ReadChar() != InclusionEnd)
                {
                    break;
                }
                if (!fileName.Contains('.'))
                {
                    if (!extension.StartsWith("."))
                    {
                        fileName += '.';
                    }
                    fileName += extension;
                }

                bool found = false;
                foreach (string path in pathArray)
                {
                    string fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                    {
                        string fileContent = File.ReadAllText
                            (
                                fullPath,
                                IrbisEncoding.Ansi
                            );
                        fileContent = ExpandInclusion
                            (
                                fileContent,
                                extension,
                                pathArray
                            );
                        result.Append(fileContent);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new IrbisException();
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
