﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftV.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Ссылка на поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftV
        : PftField
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                Log.Error
                    (
                        "PftV::Constructor: "
                        + "text="
                        + text.ToVisibleString()
                    );

                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftV
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.V);

            FieldSpecification specification
                = ((FieldSpecification)token.UserData)
                .ThrowIfNull("token.UserData");
            Apply(specification);
        }

        #endregion

        #region Private members

        private int _count;

        private void _Execute
            (
                [NotNull] PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                context.Execute(LeftHand);

                string value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    if (context.UpperMode)
                    {
                        value = IrbisText.ToUpper(value);
                    }

                    context.Write(this, value);
                }
                if (HaveRepeat(context))
                {
                    context.OutputFlag = true;

                    if (!ReferenceEquals(context._vMonitor, null))
                    {
                        context._vMonitor.Output = true;
                    }
                }

                context.Execute(RightHand);
            }
            finally
            {
                context.CurrentField = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Число повторений _поля_ в записи.
        /// </summary>
        public int GetCount
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (record == null
                || string.IsNullOrEmpty(Tag))
            {
                return 0;
            }

            int result = record.Fields.GetField(Tag).Length;

            return result;
        }

        #endregion

        #region PftField members

        /// <inheritdoc cref="PftField.IsLastRepeat" />
        public override bool IsLastRepeat
            (
                PftContext context
            )
        {
            return context.Index >= _count - 1;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            FieldInfo info = compiler.CompileField(this);

            compiler.CompileNodes(LeftHand);
            compiler.CompileNodes(RightHand);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .Write("Action leftHand = ");
            if (LeftHand.Count == 0)
            {
                compiler.WriteLine("null;");
            }
            else if (LeftHand.Count == 1)
            {
                compiler
                    .RefNodeMethod(LeftHand[0])
                    .WriteLine(';');
            }
            else
            {
                compiler
                    .WriteLine("() =>")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .CallNodes(LeftHand)
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("};");
            }

            compiler
                .WriteIndent()
                .Write("Action rightHand = ");
            if (RightHand.Count == 0)
            {
                compiler.WriteLine("null;");
            }
            else if (RightHand.Count == 1)
            {
                compiler
                    .RefNodeMethod(RightHand[0])
                    .WriteLine(';');
            }
            else
            {
                compiler
                    .WriteLine("() =>")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .CallNodes(RightHand)
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("};");
            }

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "DoField({0}, leftHand, rightHand);",
                        info.Reference
                    );

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(context.CurrentField, null))
            {
                Log.Error
                    (
                        "PftV::Execute: "
                        + "nested field detected"
                    );

                throw new PftSemanticException("nested field");
            }

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                if (IsFirstRepeat(context))
                {
                    _count = GetCount(context);
                }

                _Execute(context);
            }
            else
            {
                _count = GetCount(context);

                context.DoRepeatableAction(_Execute);
            }

            OnAfterExecution(context);
        }

        ///// <inheritdoc cref="PftNode.PrettyPrint" />
        //public override void PrettyPrint
        //    (
        //        PftPrettyPrinter printer
        //    )
        //{
        //    printer.SingleSpace();
        //    printer.WriteNodes(LeftHand);
        //    printer.Write(ToString());
        //    printer.WriteNodes(RightHand);
        //}

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion
    }
}
