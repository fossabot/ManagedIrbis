﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalCatalogerIniFile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// INI-file for cataloger.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class LocalCatalogerIniFile
    {
        #region Constants

        /// <summary>
        /// Main section.
        /// </summary>
        public const string Main = "Main";

        #endregion

        #region Properties
        
        /// <summary>
        /// INI-file.
        /// </summary>
        [NotNull]
        public IniFile Ini { get; private set; }

        /// <summary>
        /// Main section.
        /// </summary>
        [NotNull]
        public IniFile.Section MainSection
        {
            get
            {
                IniFile ini = Ini;
                IniFile.Section result = ini.GetOrCreateSection(Main);

                return result; 
            }
        }

        /// <summary>
        /// Организация, на которую куплен ИРБИС.
        /// </summary>
        public string Organization
        {
            get { return MainSection["User"]; }
        }

        /// <summary>
        /// IP адрес ИРБИС сервера.
        /// </summary>
        [NotNull]
        public string ServerIP
        {
            get
            {
                // coverity[dereference]
                return MainSection["ServerIP"]
                    .ThrowIfNull("ServerIP");
            }
        }

        /// <summary>
        /// Port number of the IRBIS server.
        /// </summary>
        public int ServerPort
        {
            get
            {
                // coverity[dereference]
                int result = Convert.ToInt32
                    (
                        MainSection["ServerPort"]
                            .ThrowIfNull("ServerPort")
                    );

                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalCatalogerIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            Ini = iniFile;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public string GetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            string result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }


        /// <summary>
        /// Load from specified file.
        /// </summary>
        [NotNull]
        public static LocalCatalogerIniFile Load
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            IniFile iniFile = new IniFile();
            iniFile.Read(fileName, IrbisEncoding.Ansi);
            LocalCatalogerIniFile result = new LocalCatalogerIniFile(iniFile);

            return result;
        }

        #endregion
    }
}
