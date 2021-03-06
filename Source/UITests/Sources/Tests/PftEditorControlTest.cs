﻿/* PftEditorControlTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class PftEditorControlTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Text = "PFT editor";
                form.Size = new Size(800, 600);

                PftEditorControl editor = new PftEditorControl
                {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(editor);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
