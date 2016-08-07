﻿/* CreateDatabaseCommand.cs -- create new database on the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO use Template property
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Create new database on the server.
    /// </summary>
    /// <remarks>For Administrator only.</remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CreateDatabaseCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Name for new database.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Description of the database.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// Will the database be visible to reader?
        /// </summary>
        public bool ReaderAccess { get; set; }

        /// <summary>
        /// Template database name.
        /// </summary>
        [CanBeNull]
        public string Template { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateDatabaseCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.CreateDatabase;

            // Layout is:
            // NEWDB          // database name
            // New database   // description
            // 0              // reader access

            result
                .Add(Database)
                .Add(Description)
                .Add(ReaderAccess);

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            // Response is (ANSI):
            // 0
            // NewDB NEWDB,New database,0 - Создана новая БД NEWDB
            // CloseDB - 
            // Exit C:\IRBIS64_2015\workdir\1126_0.ibf

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
            Verifier<CreateDatabaseCommand> verifier
                = new Verifier<CreateDatabaseCommand>(this, throwOnError);
            verifier
                .NotNullNorEmpty(Database, "Database")
                .NotNullNorEmpty(Description, "Description");

            return verifier.Result;
        }

        #endregion
    }
}