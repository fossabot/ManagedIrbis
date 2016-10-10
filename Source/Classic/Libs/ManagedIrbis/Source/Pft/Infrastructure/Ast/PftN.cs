﻿/* PftN.cs -- fake field reference
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Fake field reference.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftN
        : PftField
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        #endregion

        #region Private members

        private void _Execute
            (
                PftContext context
            )
        {
            if (!context.BreakFlag)
            {
                try
                {
                    context.CurrentField = this;

                    string value = GetValue(context);
                    if (string.IsNullOrEmpty(value))
                    {
                        foreach (PftNode node in LeftHand)
                        {
                            node.Execute(context);
                        }
                    }
                }
                finally
                {
                    context.CurrentField = null;
                }
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (context.CurrentField != null)
            {
                throw new PftSemanticException("nested field");
            }

            if (context.CurrentGroup != null)
            {
                if (!context.BreakFlag)
                {
                    if (IsFirstRepeat(context))
                    {
                        _Execute(context);
                    }

                }
            }
            else
            {
                context.Index = 0;

                while (true)
                {
                    context.OutputFlag = false;

                    _Execute(context);

                    if (!context.OutputFlag
                        || context.BreakFlag)
                    {
                        break;
                    }

                    context.Index++;
                }
            }

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion

    }
}
