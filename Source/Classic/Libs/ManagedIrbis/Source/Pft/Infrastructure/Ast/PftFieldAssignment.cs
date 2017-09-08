﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFieldAssignment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFieldAssignment
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public PftField Field { get; set; }

        /// <summary>
        /// Expression.
        /// </summary>
        [NotNull]
        public PftNodeCollection Expression { get; private set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Field, null))
                    {
                        nodes.Add(Field);
                    }
                    nodes.AddRange(Expression);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftFieldAssignment::Children: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );
            }
        }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFieldAssignment()
        {
            Expression = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFieldAssignment
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Expression = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftFieldAssignment result
                = (PftFieldAssignment) base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField) Field.Clone();
            }
            result.Expression = Expression.CloneNodes(result)
                .ThrowIfNull();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode"/>
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftFieldAssignment otherAssignment
                = (PftFieldAssignment) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Field,
                    otherAssignment.Field
                );
            PftSerializationUtility.CompareLists
                (
                    Expression,
                    otherAssignment.Expression
                );
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Field = (PftField) PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Expression);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftField field = Field;
            if (ReferenceEquals(field, null))
            {
                Log.Error
                    (
                        "PftFieldAssignment::Execute: "
                        + "field not set"
                    );

                throw new IrbisException("Field is null");
            }
            string tag = field.Tag;
            if (string.IsNullOrEmpty(tag))
            {
                Log.Error
                    (
                        "PftFieldAssignment::Execute: "
                        + "field tag not set"
                    );

                throw new IrbisException("Field tag is null");
            }

            string value = context.Evaluate(Expression);
            if (field.SubField == SubField.NoCode)
            {
                PftUtility.AssignField
                    (
                        context,
                        tag.SafeToInt32(),
                        field.FieldRepeat,
                        value
                    );
            }
            else
            {
                PftUtility.AssignSubField
                    (
                        context,
                        tag.SafeToInt32(),
                        field.FieldRepeat,
                        field.SubField,
                        field.SubFieldRepeat,
                        value
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "FieldAssignment"
            };

            if (!ReferenceEquals(Field, null))
            {
                result.Children.Add(Field.GetNodeInfo());
            }

            if (Expression.Count != 0)
            {
                PftNodeInfo expression = new PftNodeInfo
                {
                    Name = "Expression"
                };
                result.Children.Add(expression);

                foreach (PftNode node in Expression)
                {
                    expression.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.WriteIndentIfNeeded();
            if (!ReferenceEquals(Field, null))
            {
                Field.PrettyPrint(printer);
            }
            printer.Write('=');
            foreach (PftNode node in Expression)
            {
                node.PrettyPrint(printer);
            }
            printer.Write(';');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Field);
            PftSerializer.Serialize(writer, Expression);
        }

        #endregion
    }
}
