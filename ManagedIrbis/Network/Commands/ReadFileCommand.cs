﻿/* ReadFileCommand.cs -- read text file(s) from the server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Read text file(s) from the server
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReadFileCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// File list.
        /// </summary>
        [NotNull]
        public NonNullCollection<FileSpecification> Files
        {
            get { return _files; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        public ReadFileCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _files = new NonNullCollection<FileSpecification>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<FileSpecification> _files;

        #endregion

        #region Public methods

        /// <summary>
        /// Get file text.
        /// </summary>
        [NotNull]
        public string[] GetFileText
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            int count = Files.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                string text = response.GetAnsiString();
                text = IrbisText.IrbisToWindows(text);
                result[i] = text;
            }

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Check the server response.
        /// </summary>
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            // Don't check: there's no return code
            response._returnCodeRetrieved = true;
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadDocument;

            foreach (FileSpecification fileName in Files)
            {
                string item = fileName.ToString();
                result.Arguments.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            return result;
        }

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReadFileCommand> verifier
                = new Verifier<ReadFileCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert(Files.Count != 0, "Files.Count")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
