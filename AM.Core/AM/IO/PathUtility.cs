/* PathUtility.cs -- path manipulation routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Path manipulation routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PathUtility
    {
        #region Private members

        private static string _backslash
            = new string(Path.DirectorySeparatorChar, 1);

        #endregion

        #region Public methods

        /// <summary>
        /// Appends trailing backslash (if none exists)
        /// to given path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string AppendBackslash
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            string result = ConvertSlashes(path);
            if (!result.EndsWith(_backslash))
            {
                result = result + _backslash;
            }

            return result;
        }

        /// <summary>
        /// Converts ordinary slashes to backslashes.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string ConvertSlashes
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string result = path.Replace
                (
                    Path.AltDirectorySeparatorChar,
                    Path.DirectorySeparatorChar
                );

            return result;
        }

        /// <summary>
        /// Maps the path relative to the executable name.
        /// </summary>
        [NotNull]
        public static string MapPath
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string appDirectory = Path.GetDirectoryName
                (
                    RuntimeUtility.ExecutableFileName
                );
            string result = string.IsNullOrEmpty(appDirectory)
                ? path
                : Path.Combine
                    (
                        appDirectory,
                        path
                    );

            return result;
        }

        /// <summary>
        /// Strips extension from given path.
        /// </summary>
        [NotNull]
        public static string StripExtension
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string extension = Path.GetExtension(path);
            string result = path;
            if (!string.IsNullOrEmpty(extension))
            {
                result = result.Substring
                    (
                        0, 
                        result.Length - extension.Length
                    );
            }

            return result;
        }

        /// <summary>
        /// Removes trailing backslash (if exists) from the path.
        /// </summary>
        /// <param name="path">Path to convert.</param>
        /// <returns>Converted path.</returns>
        /// <remarks>Path need NOT to be existent.</remarks>
        [NotNull]
        public static string StripTrailingBackslash
            (
                [NotNull] string path
            )
        {
            Code.NotNull(path, "path");

            string result = ConvertSlashes(path);
            while (result.EndsWith(_backslash))
            {
                result = result.Substring
                    (
                        0, 
                        result.Length - _backslash.Length
                    );
            }

            return result;
        }

        #endregion
    }
}