﻿/* PftConditionAndOr.cs --
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
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftConditionAndOr
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Left operand.
        /// </summary>
        [CanBeNull]
        public PftCondition LeftOperand { get; set; }

        /// <summary>
        /// Operation.
        /// </summary>
        [CanBeNull]
        public string Operation { get; set; }

        /// <summary>
        /// Right operand.
        /// </summary>
        [CanBeNull]
        public PftCondition RightOperand { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionAndOr()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionAndOr
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

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

            if (ReferenceEquals(LeftOperand, null))
            {
                throw new PftSyntaxException();
            }

            LeftOperand.Execute(context);
            bool left = LeftOperand.Value;

            if (!ReferenceEquals(RightOperand, null))
            {
                if (string.IsNullOrEmpty(Operation))
                {
                    throw new PftSyntaxException();
                }

                RightOperand.Execute(context);
                bool right = RightOperand.Value;

                if (Operation.SameString("and"))
                {
                    left = left && right;
                }
                else if (Operation.SameString("or"))
                {
                    left = left || right;
                }
                else
                {
                    throw new PftSyntaxException();
                }
            }

            Value = left;

            OnAfterExecution(context);
        }

        #endregion
    }
}