﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus9.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Pft.Infrastructure.Ast;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus9
    {
        #region Private members

        #endregion

        #region Public methods


        /// <summary>
        /// Get character with given code.
        /// </summary>
        public static void GetCharacter
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                int code;
                if (NumericUtility.TryParseInt32(expression, out code))
                {
                    string output = ((char)code).ToString();
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Get directory name from full path.
        /// </summary>
        public static void GetDirectoryName
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
#if !WIN81 && !PORTABLE

            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetDirectoryName(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    if (!output.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        output += Path.DirectorySeparatorChar;
                    }
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }

#endif
        }

        /// <summary>
        /// Get drive name from full path.
        /// </summary>
        public static void GetDrive
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                Match match = Regex.Match(expression, "^[A-Za-z]:");
                string output = match.Value;
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Get extension from full path.
        /// </summary>
        public static void GetExtension
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetExtension(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Get file name from full path.
        /// </summary>
        public static void GetFileName
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetFileName(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GetFileSize
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                long fileSize = 0;
                try
                {
                    FileInfo fileInfo = new FileInfo(expression);
                    fileSize = fileInfo.Length;
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "UniforPlus9::GetFileSize",
                            exception
                        );
                }

                string output = fileSize.ToInvariantString();
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetIndex
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }
            string text = index.ToInvariantString();
            context.Write(node, text);
            context.OutputFlag = true;
        }

        /// <summary>
        /// Get IRBIS version (family): 32 or 64
        /// </summary>
        public static void GetVersion
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.Write(node, "64");
            context.OutputFlag = true;
        }

        /// <summary>
        /// Get string length.
        /// </summary>
        public static void Length
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string output = "0";
            if (!string.IsNullOrEmpty(expression))
            {
                output = expression.Length.ToInvariantString();
            }
            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Replace character in text.
        /// </summary>
        public static void ReplaceCharacter
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char first = navigator.ReadChar();
                char second = navigator.ReadChar();
                string text = navigator.GetRemainingText();
                if (!string.IsNullOrEmpty(text))
                {
                    string output = text.Replace(first, second);
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        /// <summary>
        /// Replace substring.
        /// </summary>
        public static void ReplaceString
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char firstDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }
                string first = navigator.ReadUntil(firstDelimiter);
                navigator.ReadChar();
                if (navigator.IsEOF
                    || string.IsNullOrEmpty(first))
                {
                    return;
                }
                char secondDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }
                string second = navigator.ReadUntil(secondDelimiter);
                if (navigator.ReadChar() != secondDelimiter)
                {
                    return;
                }
                string text = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                string output = text.Replace(first, second);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Split text to word array.
        /// </summary>
        public static void SplitWords
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                IrbisAlphabetTable table = context.Provider
                    .GetAlphabetTable();

                string[] words = table.SplitWords(expression);
                string output = string.Join
                    (
                        Environment.NewLine,
                        words.ToArray()
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Split text to word array.
        /// </summary>
        public static void Substring
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string text;

#if PocketPC || WINMOBILE || SILVERLIGHT

                string[] parts = expression.Split(new[]{'#'});

#else

                string[] parts = expression.Split(new[]{'#'}, 2);

#endif

                if (parts.Length == 2)
                {
                    string prefix = parts[0];
                    text = parts[1];

                    TextNavigator navigator = new TextNavigator(prefix);
                    char direction = navigator.ReadChar();
                    int offset = 0;
                    int length = text.Length;
                    string temp;
                    if (navigator.PeekChar() == '*')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger();
                        NumericUtility.TryParseInt32(temp, out offset);
                    }
                    if (navigator.PeekChar() == '.')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger();
                        NumericUtility.TryParseInt32(temp, out length);
                    }

                    if (direction != '0')
                    {
                        offset = text.Length - offset - length;
                    }

                    text = PftField.SafeSubString(text, offset, length);
                }
                else
                {
                    text = expression.Substring(1);
                }

                string output = text;
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        /// <summary>
        /// Convert text to upper case.
        /// </summary>
        public static void ToUpper
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
#if PocketPC

                string output = expression.ToUpper();

#else

                string output = expression.ToUpperInvariant();

#endif

                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
