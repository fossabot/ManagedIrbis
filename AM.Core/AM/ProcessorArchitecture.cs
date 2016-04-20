/* ProcessorArchitecture.cs -- processor architecture enumeration
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Processor architecture enumeration.
    /// </summary>
    [PublicAPI]
    public enum ProcessorArchitecture
    {
        /// <summary>
        /// Can't determine processor architecture.
        /// </summary>
        Unknown,
        
        /// <summary>
        /// Intel x86 or compatible processor architecture.
        /// </summary>
        X86,
        
        /// <summary>
        /// AMD x64 or compatible processor architecture.
        /// </summary>
        X64,
        
        /// <summary>
        /// Intel IA64 or compatible processor architecture.
        /// </summary>
        IA64,

        /// <summary>
        /// ARM
        /// </summary>
        ARM,

        /// <summary>
        /// ARM64
        /// </summary>
        ARM64
    }
}
