﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCompiler.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.CSharp;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCompiler
    {
        #region Constants

        /// <summary>
        /// Prefix for node methods.
        /// </summary>
        public const string NodeMethodPrefix = "NodeMethod";

        #endregion

        #region Properties

        [NotNull]
        internal FieldDictionary Fields { get; private set; }

        [NotNull]
        internal NodeDictionary Nodes { get; private set; }

        [NotNull]
        internal TextWriter Output { get; private set; }

        /// <summary>
        /// Indentation level.
        /// </summary>
        public int Indent { get; private set; }

        /// <summary>
        /// Path to store compiled assemblies.
        /// </summary>
        public NonNullValue<string> OutputPath { get; set; }

        /// <summary>
        /// Keep source.
        /// </summary>
        public bool KeepSource { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCompiler()
        {
            Indent = 0;
            Fields = new FieldDictionary();
            Nodes = new NodeDictionary();
            Output = new StringWriter();
            OutputPath = Path.GetTempPath();
        }

        #endregion

        #region Private members

        private PftNode _currentNode;

        private void _RenumberNodes
            (
                [NotNull] PftNode rootNode
            )
        {
            NumberingVisitor visitor = new NumberingVisitor(Nodes);
            bool result = rootNode.AcceptVisitor(visitor);
            if (!result)
            {
                throw new PftCompilerException();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Call method for the node.
        /// </summary>
        [NotNull]
        public PftCompiler CallNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);
            WriteLine
                (
                    "{0}{1} ();",
                    NodeMethodPrefix,
                    info.Id
                );

            return this;
        }

        /// <summary>
        /// Call nodes.
        /// </summary>
        [NotNull]
        public PftCompiler CallNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                WriteIndent();
                CallNodeMethod(node);
            }

            return this;
        }

        /// <summary>
        /// Compile the field.
        /// </summary>
        [NotNull]
        internal FieldInfo CompileField
            (
                [NotNull] PftField field
            )
        {
            Code.NotNull(field, "field");

            FieldInfo result = Fields.Get(field);
            if (ReferenceEquals(result, null))
            {
                result = Fields.Create(field);
                FieldSpecification spec = result.Specification;
                WriteIndent();
                WriteLine
                    (
                        "// {0}",
                        result.Text
                    );
                WriteIndent();
                WriteLine
                    (
                        "FieldSpecification {0} = new FieldSpecification()",
                        result.Reference
                    );
                WriteIndent();
                WriteLine("{");
                IncreaseIndent();
                WriteIndent();
                WriteLine("Command = '{0}',", spec.Command);
                WriteIndent();
                WriteLine("Tag = \"{0}\",", spec.Tag);
                if (spec.SubField != SubField.NoCode)
                {
                    WriteIndent();
                    WriteLine("SubField = '{0}', ", spec.SubField);
                }
                DecreaseIndent();
                WriteIndent();
                WriteLine("};");
                WriteLine();
            }

            return result;
        }

        /// <summary>
        /// Compile nodes.
        /// </summary>
        public void CompileNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                node.Compile(this);
            }
        }

        /// <summary>
        /// Compile the program.
        /// </summary>
        [NotNull]
        public string CompileProgram
            (
                [NotNull] PftProgram program
            )
        {
            Code.NotNull(program, "program");

            _RenumberNodes(program);

            string result = StartClass();

            program.Compile(this);

            EndClass();

            return result;
        }

        /// <summary>
        /// Compile to DLL.
        /// </summary>
        [CanBeNull]
        public string CompileToDll
            (
                [NotNull] AbstractOutput output,
                [NotNull] string fileName
            )
        {
            Code.NotNull(output, "output");
            Code.NotNullNorEmpty(fileName, "fileName");

            string outputAssemblyPath = Path.Combine
                (
                    OutputPath,
                    fileName + ".dll"
                );

            List<string> references = new List<string>
            {
                "AM.Core.dll",
                "JetBrains.Annotations.dll",
                "ManagedIrbis.dll",
                "Microsoft.CSharp.dll",
                "MoonSharp.Interpreter.dll",
                "Newtonsoft.Json.dll",
                "System.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Data.DataSetExtensions.dll",
                "System.Xml.dll",
                "System.Xml.Linq.dll"
            };

            string sourceCode = GetSourceCode();
            if (KeepSource)
            {
                string fullName = Path.ChangeExtension
                    (
                        outputAssemblyPath,
                        ".cs"
                    );
                File.WriteAllText(fullName, sourceCode);
            }

            string[] sources =
            {
                sourceCode
            };

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters
                (
                    references.ToArray()
                )
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                // CompilerOptions = "/d:DEBUG",
                WarningLevel = 4,
                IncludeDebugInformation = true,
                OutputAssembly = outputAssemblyPath
            };
            CompilerResults results = provider.CompileAssemblyFromSource
                (
                    parameters,
                    sources
                );
            foreach (CompilerError error in results.Errors)
            {
                output.WriteLine(error.ToString());
            }
            if (results.Errors.Count != 0)
            {
                return null;
            }

            return outputAssemblyPath;
        }

        /// <summary>
        /// Decrease indent.
        /// </summary>
        [NotNull]
        public PftCompiler DecreaseIndent()
        {
            Indent--;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndClass()
        {
            if (!ReferenceEquals(_currentNode, null))
            {
                throw new PftCompilerException();
            }

            DecreaseIndent();
            WriteLine("} // end of class");
        }

        /// <summary>
        /// End method for the node.
        /// </summary>
        public void EndMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (!ReferenceEquals(_currentNode, node))
            {
                throw new PftCompilerException();
            }

            DecreaseIndent();
            WriteIndent();
            WriteLine("} // end of method");
            WriteLine();

            _currentNode = null;
        }

        /// <summary>
        /// Get source code.
        /// </summary>
        [NotNull]
        public string GetSourceCode()
        {
            string result = Output.ToString();

            return result;
        }

        /// <summary>
        /// Increase indent.
        /// </summary>
        [NotNull]
        public PftCompiler IncreaseIndent()
        {
            Indent++;

            return this;
        }

        /// <summary>
        /// Mark the node as ready.
        /// </summary>
        public void MarkReady
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);
            if (info.Ready)
            {
                throw new IrbisException();
            }
            info.Ready = true;
        }

        /// <summary>
        /// Referenct method for the node.
        /// </summary>
        [NotNull]
        public PftCompiler RefNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);
            Write
                (
                    "{0}{1}",
                    NodeMethodPrefix,
                    info.Id
                );

            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public string StartClass()
        {
            string result = StringUtility.Random(10);

            WriteLine("using System;");
            WriteLine("using System.CodeDom.Compiler;");
            WriteLine("using System.Collections.Generic;");
            WriteLine("using System.Diagnostics;");
            WriteLine("using System.IO;");
            WriteLine("using System.Linq;");
            WriteLine("using System.Text;");
            WriteLine("using System.Threading.Tasks;");
            WriteLine();
            WriteLine("using AM;");
            WriteLine("using AM.Logging;");
            WriteLine();
            WriteLine("using ManagedIrbis;");
            WriteLine("using ManagedIrbis.Pft;");
            WriteLine("using ManagedIrbis.Pft.Infrastructure;");
            WriteLine("using ManagedIrbis.Pft.Infrastructure.Ast;");
            WriteLine("using ManagedIrbis.Pft.Infrastructure.Compiler;");
            WriteLine();
            WriteLine("public class {0}", result);
            IncreaseIndent();
            WriteIndent();
            WriteLine(": PftPacket");
            DecreaseIndent();
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();
            WriteIndent();
            WriteLine("public {0} (PftContext context)", result);
            IncreaseIndent();
            WriteIndent();
            WriteLine(": base(context)");
            DecreaseIndent();
            WriteIndent();
            WriteLine("{");
            WriteIndent();
            WriteLine("}");
            WriteLine();

            return result;
        }

        /// <summary>
        /// Start method for the node.
        /// </summary>
        public void StartMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (!ReferenceEquals(_currentNode, null))
            {
                throw new IrbisException();
            }

            _currentNode = node;

            NodeInfo info = Nodes.Get(node);

            WriteIndent();
            WriteLine
                (
                    "// {0}: {1}",
                    PftNode.SimplifyTypeName(node.GetType().Name),
                    CompilerUtility.ShortenText(node.ToString())
                );
            string returnType = "void";
            if (node is PftBoolean)
            {
                returnType = "bool";
            }
            else if (node is PftNumeric)
            {
                returnType = "double";
            }
            WriteIndent();
            WriteLine
                (
                    "public {0} {1}{2}()",
                    returnType,
                    NodeMethodPrefix,
                    info.Id
                );
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();
        }

        #region Write

        /// <inheritdoc cref="TextWriter.Write(char)" />
        [NotNull]
        public PftCompiler Write
            (
                char value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(int)" />
        [NotNull]
        public PftCompiler Write
            (
                int value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(double)" />
        [NotNull]
        public PftCompiler Write
            (
                double value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(string)" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] string value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(object)" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] object value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(string,object[])" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] string format,
                params object[] arg
            )
        {
            Output.Write(format, arg);

            return this;
        }

        /// <summary>
        /// Write indentation.
        /// </summary>
        [NotNull]
        public PftCompiler WriteIndent()
        {
            for (int i = 0; i < Indent; i++)
            {
                Output.Write("    ");
            }

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine()" />
        [NotNull]
        public PftCompiler WriteLine()
        {
            Output.WriteLine();

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(char)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                char value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(int)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                int value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(double)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                double value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(string)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] string value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(object)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] object value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(string,object[])" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] string format,
                params object[] arg
            )
        {
            Output.WriteLine(format, arg);

            return this;
        }

        #endregion

        #endregion

        #region Object members

        #endregion
    }
}
