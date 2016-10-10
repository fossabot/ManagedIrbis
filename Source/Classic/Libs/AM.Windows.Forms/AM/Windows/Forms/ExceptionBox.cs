﻿/* ExceptionBox.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Windows.Forms;

using AM.Drawing.Printing;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public partial class ExceptionBox
        : ModalForm
    {
        #region Properties

        /// <summary>
        /// Exception.
        /// </summary>
        [CanBeNull]
        public Exception Exception
        {
            get { return _exception; }
            set
            {
                Code.NotNull(value, "value");

                _exception = value;
                _textBox.Text = value
                    .ThrowIfNull()
                    .ToString();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private Exception _exception;

        private void _abortButton_Click
            (
            object sender,
            EventArgs e
            )
        {
            Application.Exit();
        }

        private void _closeButton_Click
            (
            object sender,
            EventArgs e
            )
        {
            DialogResult = DialogResult.OK;
        }

        private void _copyButton_Click
            (
            object sender,
            EventArgs e
            )
        {
            Clipboard.SetText(_textBox.Text);
        }

        private void _printButton_Click
            (
            object sender,
            EventArgs e
            )
        {
            using (PlainTextPrinter printer
                = new PlainTextPrinter())
            {
                printer.Print(_textBox.Text);
            }

        }

        private void _saveButton_Click
            (
            object sender,
            EventArgs e
            )
        {
            if (_saveFileDialog.ShowDialog(this)
                == DialogResult.OK)
            {
                File.WriteAllText
                    (
                        _saveFileDialog.FileName,
                        _textBox.Text
                    );
            }

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        public static void Show
            (
                [CanBeNull] IWin32Window parent,
                [NotNull] Exception exception
            )
        {
            Code.NotNull(exception, "exception");

            using (ExceptionBox box = new ExceptionBox())
            {
                box._typeLabel.Text = exception.GetType().ToString();
                box._messageLabel.Text = exception.Message;
                box._textBox.Text = exception.ToString();
                box.ShowDialog(parent);
            }
        }

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        public static void Show
            (
                [NotNull] Exception exception
            )
        {
            Show(null, exception);
        }

        #endregion
    }
}