/* AutoDisposeAttribute.cs -- mark instance field as auto-disposable 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Mark instance field as auto-disposable.
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class AutoDisposeAttribute
        : Attribute
    {
    }
}