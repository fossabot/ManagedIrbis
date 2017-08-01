﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPacket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PftPacket
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; private set; }

        /// <summary>
        /// Current field (if any).
        /// </summary>
        [CanBeNull]
        public FieldSpecification CurrentField { get; set; }

        /// <summary>
        /// In group?
        /// </summary>
        public bool InGroup { get; set; }

        /// <summary>
        /// Breakpoints.
        /// </summary>
        [NotNull]
        public Dictionary<int,object> Breakpoints { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftPacket
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
            Breakpoints = new Dictionary<int, object>();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Call the debugger.
        /// </summary>
        protected void CallDebugger()
        {
            PftDebugger debugger = Context.Debugger;
            if (!ReferenceEquals(debugger, null))
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        Context,
                        null
                    );
                debugger.Activate(eventArgs);
            }
        }

        /// <summary>
        /// Can output according given value.
        /// </summary>
        public bool CanOutput
            (
                [NotNull] FieldSpecification specification,
                [CanBeNull] string value
            )
        {
            return !string.IsNullOrEmpty(value);
        }


        /// <summary>
        /// Debugger hook.
        /// </summary>
        protected void DebuggerHook
            (
                int nodeId
            )
        {
            if (Breakpoints.ContainsKey(nodeId))
            {
                CallDebugger();
            }
        }

        /// <summary>
        /// Do the conditional literal.
        /// </summary>
        protected void DoConditionalLiteral
            (
                [CanBeNull] string literalText,
                FieldSpecification field,
                bool isSuffix
            )
        {
            if (string.IsNullOrEmpty(literalText))
            {
                return;
            }

            bool flag = isSuffix
                ? IsLastRepeat(field)
                : IsFirstRepeat(field);

            if (flag)
            {
                string value = GetValue(field);

                if (CanOutput(field, value))
                {
                    if (Context.UpperMode)
                    {
                        literalText = IrbisText.ToUpper(literalText);
                    }

                    Context.Write(null, literalText);
                    Context.OutputFlag = true;
                }
            }
        }

        /// <summary>
        /// Do field.
        /// </summary>
        protected void DoField
            (
                [NotNull] FieldSpecification field,
                [CanBeNull] Action leftHand,
                [CanBeNull] Action rightHand
            )
        {
            if (ReferenceEquals(Context.Record, null))
            {
                return;
            }

            CurrentField = field;

            char command = field.Command;

            if (command == 'v')
            {
                if (InGroup)
                {
                    DoFieldV(field, leftHand, rightHand);
                }
                else
                {
                    DoRepeatableAction
                        (
                            () => DoFieldV(field, leftHand, rightHand)
                        );
                }
            }
            else if (command == 'd')
            {
                
            }
            else if (command == 'n')
            {
                
            }

            CurrentField = null;
        }

        private void DoFieldV
            (
                [NotNull] FieldSpecification field,
                [CanBeNull] Action leftHand,
                [CanBeNull] Action rightHand
            )
        {
            if (!ReferenceEquals(leftHand, null))
            {
                leftHand();
            }

            string value = GetValue(field);
            if (!string.IsNullOrEmpty(value))
            {
                if (Context.UpperMode)
                {
                    value = IrbisText.ToUpper(value);
                }
                Context.Write(null, value);
            }

            if (HaveField(field))
            {
                Context.OutputFlag = true;
                if (!ReferenceEquals(Context._vMonitor, null))
                {
                    Context._vMonitor.Output = true;
                }
            }

            if (!ReferenceEquals(rightHand, null))
            {
                rightHand();
            }
        }

        /// <summary>
        /// Do group.
        /// </summary>
        protected void DoGroup
            (
                [NotNull] Action action
            )
        {
            InGroup = true;
            DoRepeatableAction(action);
            InGroup = false;
        }

        /// <summary>
        /// Do the repeatable literal.
        /// </summary>
        protected void DoRepeatableLiteral
            (
                [CanBeNull] string text,
                [NotNull] FieldSpecification field,
                bool isPrefix,
                bool plus
            )
        {
            bool flag = HaveField(field);

            if (flag)
            {
                string value = GetValue(field);

                flag = CanOutput(field, value);
            }

            if (flag && plus)
            {
                flag = isPrefix
                    ? !IsFirstRepeat(field)
                    : !IsLastRepeat(field);
            }

            if (flag)
            {
                if (Context.UpperMode
                    && !ReferenceEquals(text, null))
                {
                    text = IrbisText.ToUpper(text);
                }
                Context.Write(null, text);
                Context.OutputFlag = true;
            }
        }

        private void DoRepeatableAction
            (
                [NotNull] Action action
            )
        {
            for 
                (
                    Context.Index = 0; 
                    Context.Index < PftConfig.MaxRepeat; 
                    Context.Index++
                )
            {
                Context.OutputFlag = false;

                action();

                if (!Context.OutputFlag || Context.BreakFlag) //-V3022
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Evaluate as string.
        /// </summary>
        [CanBeNull]
        protected string Evaluate
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            using (PftContextGuard guard = new PftContextGuard(Context))
            {
                Context = guard.ChildContext;
                action();
                string result = Context.ToString();
                Context = guard.ParentContext;

                return result;
            }
        }

        [CanBeNull]
        private string GetValue
            (
                [NotNull] FieldSpecification spec
            )
        {
            // ReSharper disable once PossibleNullReferenceException

            RecordField field = Context.Record.Fields.GetField
                (
                    spec.Tag,
                    Context.Index
                );
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            string result = PftUtility.GetFieldValue
                (
                    Context,
                    field,
                    spec.SubField,
                    spec.SubFieldRepeat
                );

            return result;
        }

        /// <summary>
        /// Goto new position.
        /// </summary>
        public void Goto
            (
                int newPosition
            )
        {
            int current = Context.Output.GetCaretPosition();
            int delta = newPosition - current;
            if (delta > 0)
            {
                Context.Write
                    (
                        null,
                        new string(' ', delta)
                    );
            }
            else
            {
                Context.WriteLine(null);
                Context.Write
                    (
                        null,
                        new string(' ', newPosition - 1)
                    );
            }
        }

        /// <summary>
        /// Have the field?
        /// </summary>
        protected bool HaveField
            (
                [NotNull] FieldSpecification spec
            )
        {
            // ReSharper disable once PossibleNullReferenceException

            RecordField field = Context.Record.Fields.GetField
                (
                    spec.Tag,
                    Context.Index
                );

            return !ReferenceEquals(field, null);
        }

        /// <summary>
        /// First repeat?
        /// </summary>
        protected bool IsFirstRepeat
            (
                [NotNull] FieldSpecification field
            )
        {
            return Context.Index == 0;
        }

        /// <summary>
        /// Last repeat?
        /// </summary>
        protected bool IsLastRepeat
            (
                [NotNull] FieldSpecification field
            )
        {
            // ReSharper disable once PossibleNullReferenceException

            int count = Context.Record.Fields.GetFieldCount(field.Tag);

            return Context.Index >= count - 1;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the packet agains the record.
        /// </summary>
        [NotNull]
        public virtual string Execute
            (
                [NotNull] MarcRecord record
            )
        {
            Context.ClearAll();
            Context.Record = record;

            return String.Empty;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Context.ToString();
        }

        #endregion
    }
}