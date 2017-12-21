﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventUtility.cs -- Useful routines for event manipulations
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Reflection;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Useful routines for event manipulations.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class EventUtility
    {
        #region Delegates

        /// <summary>
        /// ???
        /// </summary>
        public delegate void RemoveDelegate(EventHandler handler);

        #endregion

        #region Public methods

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender,
                [CanBeNull] EventArgs args
            )
        {
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, args);
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler,
                [CanBeNull] object sender,
                [CanBeNull] T args
            )
            where T : EventArgs
        {
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, args);
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler,
                [CanBeNull] object sender
            )
            where T : EventArgs
        {
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, null);
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender
            )
        {
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                [CanBeNull] this EventHandler<T> handler
            )
            where T : EventArgs
        {
            if (!ReferenceEquals(handler, null))
            {
                handler(null, null);
            }
        }

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender,
                [CanBeNull] EventArgs args
            )
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        if (!ReferenceEquals(handler, null))
                        {
                            handler(sender, args);
                        }
                    }
                );

            return result;
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync
            (
                [CanBeNull] this EventHandler handler,
                [CanBeNull] object sender
            )
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        if (!ReferenceEquals(handler, null))
                        {
                            handler(sender, EventArgs.Empty);
                        }
                    }
                );

            return result;
        }

#endif

        /// <summary>
        /// Unsubscribe all handlers from the event.
        /// </summary>
        public static void UnsubscribeAll
            (
                [NotNull] object obj,
                [NotNull] string eventName
            )
        {
#if !PORTABLE && !WIN81

            Code.NotNull(obj, "obj");
            Code.NotNullNorEmpty(eventName, "eventName");

            //if (!ReferenceEquals(handler, null))
            {
                Type type = obj.GetType();
                EventInfo eventInfo = type.GetEvent(eventName);
                MethodInfo methodInfo = eventInfo.GetRemoveMethod();
                //Delegate method = Delegate.CreateDelegate(typeof(RemoveDelegate), methodInfo);

                //Delegate[] subscribers
                //    = handler.GetInvocationList();
                //foreach (Delegate subscriber in subscribers)
                //{
                //    // ReSharper disable DelegateSubtraction
                //    handler -= (EventHandler) subscriber;
                //    // ReSharper restore DelegateSubtraction
                //}
            }
#endif
        }

        #endregion
    }
}

