﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianFoundMfnColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianFoundMfnColumn
        : SiberianColumn
    {
        #region Properties

        /// <summary>
        /// Use <see cref="FoundLine.SerialNumber"/> instead
        /// of <see cref="FoundLine.Mfn"/>.
        /// </summary>
        public bool UseSerialNumber { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFoundMfnColumn()
        {
            ReadOnly = true;

            Palette.BackColor = Color.LightGray;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianFoundMfnCell();
            result.Column = this;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
