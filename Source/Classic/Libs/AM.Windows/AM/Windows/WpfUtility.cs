﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WpfUtility.cs -- helper methods for WPF
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class WpfUtility
    {
        #region Private members

        private static void _DoNothing()
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Analog for WinForms Apllication.DoEvents.
        /// </summary>
        /// <remarks>
        /// Borrowed from https://stackoverflow.com/questions/4502037/where-is-the-application-doevents-in-wpf
        /// </remarks>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke
                (
                    DispatcherPriority.Background,
                    new Action(_DoNothing)
                );
        }

        /// <summary>
        /// List of (recursive) children of the given type.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static List<TChild> ListChildren<TChild>
            (
                [NotNull] DependencyObject element
            )
            where TChild : DependencyObject
        {
            Code.NotNull(element, "element");

            List<TChild> result = new List<TChild>();
            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(element, i);
                TChild item = obj as TChild;
                if (!ReferenceEquals(item, null))
                {
                    result.Add(item);
                }

                List<TChild> list = ListChildren<TChild>(obj);
                result.AddRange(list);
            }

            return result;
        }

        #endregion
    }
}
