﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DisconnectCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DisconnectCommand
        : ServerCommand
    {
        #region Properties

        #endregion


        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisconnectCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ClientRequest request = Data.Request.ThrowIfNull();
                ServerContext context = engine.RequireContext(Data);
                Data.Context = context;

                SendResponse();

                engine.DestroyContext(context);
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
