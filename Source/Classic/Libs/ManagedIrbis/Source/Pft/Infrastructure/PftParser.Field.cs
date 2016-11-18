﻿/* PftParser.Field.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    //
    // 
    //
    //
    //

    partial class PftParser
    {
        private PftNode ParseField()
        {
            PftToken leadToken = Tokens.Current;
            List<PftNode> leftHand = new List<PftNode>();
            PftField result = new PftV();
            PftNode node;
            PftRepeatableLiteral literal;
            PftToken token;

            // Gather left hand of the field: conditional literal and friends
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral
                    || token.Kind == PftTokenKind.V)
                {
                    break;
                }

                node = Get(FieldMap, LeftHandItems1);
                if (!ReferenceEquals(node, null))
                {
                    leftHand.Add(node);
                }
            } // Tokens.IsEof

            // Gather left hand of the field: repeatable literal
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral)
                {
                    literal = new PftRepeatableLiteral(token)
                    {
                        IsPrefix = true
                    };
                    leftHand.Add(literal);

                    if (Tokens.Peek() == PftTokenKind.Plus)
                    {
                        literal.Plus = true;
                        Tokens.MoveNext();
                    }

                    Tokens.MoveNext();
                }
                else
                {
                    node = Get(FieldMap, LeftHandItems2);
                    if (ReferenceEquals(node, null))
                    {
                        break;
                    }
                    leftHand.Add(node);
                }
            } // Tokens.IsEof

            // Orphaned left hand?
            if (Tokens.IsEof)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }

            // Parse field itself
            if (!Tokens.IsEof)
            {
                token = Tokens.Current;
                leadToken = token;

                // Orphaned?
                if (token.Kind != PftTokenKind.V)
                {
                    result = new PftOrphan();
                    result.LeftHand.AddRange(leftHand);
                    goto DONE;
                }
                if (string.IsNullOrEmpty(token.Text))
                {
                    throw new PftSyntaxException(token);
                }

                FieldSpecification specification
                    = (FieldSpecification)token.UserData;
                if (ReferenceEquals(specification, null))
                {
                    throw new PftSyntaxException(token);
                }

                // Check for command code
                switch (specification.Command)
                {
                    case 'v':
                    case 'V':
                        result = new PftV(leadToken);
                        break;

                    case 'd':
                    case 'D':
                        result = new PftD(leadToken);
                        break;

                    case 'n':
                    case 'N':
                        result = new PftN(leadToken);
                        break;

                    case 'g':
                    case 'G':
                        result = new PftG(leadToken);
                        break;

                    default:
                        throw new PftSyntaxException(token);
                }

                result.LeftHand.AddRange(leftHand);
                result.Apply(specification);
                Tokens.MoveNext();
            } // Tokens.IsEof

            // Gather right hand (for V command only)
            if (result is PftV)
            {
                if (!Tokens.IsEof)
                {
                    bool plus = false;
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.Plus)
                    {
                        plus = true;
                        Tokens.RequireNext();
                        token = Tokens.Current;
                    }
                    if (token.Kind == PftTokenKind.RepeatableLiteral)
                    {
                        literal = new PftRepeatableLiteral(token)
                        {
                            Plus = plus
                        };
                        result.RightHand.Add(literal);
                        Tokens.MoveNext();
                    }
                    else
                    {
                        if (plus)
                        {
                            throw new PftSyntaxException(token);
                        }

                        node = Get(FieldMap, RightHandItems);
                        if (!ReferenceEquals(node, null))
                        {
                            result.RightHand.Add(node);
                        }
                    }
                } // Tokens.IsEof

                if (!Tokens.IsEof)
                {
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.ConditionalLiteral)
                    {
                        node = new PftConditionalLiteral(token);
                        result.RightHand.Add(node);
                        Tokens.MoveNext();
                    }
                }
            } // result is PftV

            DONE: return result;
        }
    }
}
