﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RainbowItemList.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RainbowItemList
        : List<RainbowItem>
    {
        /// <summary>
        /// Adds the color at specified position.
        /// </summary>
        public void Add
            (
                Color color,
                float position
            )
        {
            Add(new RainbowItem(color, position));
        }
    }
}
