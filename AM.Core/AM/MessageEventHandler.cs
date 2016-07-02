/* MessageEventHandler.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public delegate void MessageEventHandler
        (
            object sender,
            MessageEventArgs args
        );
}
