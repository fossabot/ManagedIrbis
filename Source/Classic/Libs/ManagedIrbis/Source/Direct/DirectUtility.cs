﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DirectUtility.cs -- direct reading IRBIS64 databases
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81 && !PORTABLE

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DirectUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static byte[] _l01Content32 = {};
        private static byte[] _l02Content32 = {};
        private static byte[] _n01Content32 = {};
        private static byte[] _n02Content32 = {};
        private static byte[] _cntContent32 = 
        {
            0x01, 0x00, 0x05, 0x00, 0x05, 0x00, 0x0F, 0x00,
            0x05, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x02, 0x00, 0x05, 0x00, 0x05, 0x00,
            0x0F, 0x00, 0x05, 0x00, 0xFF, 0xFF, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };
        private static byte[] _ifpContent32 =
        {
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };
        private static byte[] _mstContent32 =
        {
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x41, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x86, 0x04, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x96, 0x00, 0x16, 0x00,
            0x00, 0x00, 0x47, 0x00, 0x00, 0x00, 0x05, 0x00,
            0x1C, 0x00, 0x05, 0x00, 0x20, 0x00, 0x01, 0x00,
            0x25, 0x00, 0x03, 0x00, 0x02, 0x00, 0x28, 0x00,
            0x03, 0x00, 0x05, 0x00, 0x2B, 0x00, 0x03, 0x00,
            0x03, 0x00, 0x2E, 0x00, 0x03, 0x00, 0x0A, 0x00,
            0x31, 0x00, 0x17, 0x00, 0x0B, 0x00, 0x48, 0x00,
            0x0E, 0x00, 0x54, 0x00, 0x56, 0x00, 0x0B, 0x00,
            0x18, 0x00, 0x61, 0x00, 0x12, 0x00, 0x20, 0x00,
            0x73, 0x00, 0x11, 0x00, 0x4A, 0x00, 0x84, 0x00,
            0x05, 0x00, 0x35, 0x00, 0x89, 0x00, 0x0E, 0x00,
            0x36, 0x00, 0x97, 0x00, 0x0B, 0x00, 0x2E, 0x00,
            0xA2, 0x00, 0xE6, 0x02, 0x2D, 0x00, 0x88, 0x03,
            0x05, 0x00, 0x2D, 0x00, 0x8D, 0x03, 0x18, 0x00,
            0x2D, 0x00, 0xA5, 0x03, 0x0F, 0x00, 0x2D, 0x00,
            0xB4, 0x03, 0x1A, 0x00, 0x2D, 0x00, 0xCE, 0x03,
            0x0B, 0x00, 0x7E, 0x00, 0xD9, 0x03, 0x10, 0x00,
            0x5A, 0x00, 0xE9, 0x03, 0x06, 0x00, 0x34, 0x20,
            0xA8, 0xAB, 0x2E, 0x82, 0xA5, 0xA7, 0xA4, 0xA5,
            0xE5, 0xAE, 0xA4, 0xAD, 0xEB, 0xA9, 0x20, 0xAA,
            0xE0, 0xA0, 0xAD, 0x20, 0xAC, 0xAE, 0xA4, 0xA5,
            0xAB, 0xA8, 0x20, 0x8A, 0x8C, 0x8A, 0x20, 0x33,
            0x30, 0x34, 0x35, 0x32, 0x31, 0x34, 0x37, 0x32,
            0x32, 0x94, 0x90, 0x83, 0x8D, 0x85, 0x8C, 0x38,
            0x31, 0x30, 0x20, 0x20, 0x20, 0x20, 0x39, 0x30,
            0x91, 0x30, 0x30, 0x30, 0x36, 0x37, 0x35, 0x30,
            0x32, 0x34, 0x31, 0x31, 0x36, 0x36, 0x96, 0x8D,
            0x88, 0x88, 0x92, 0x9D, 0xE1, 0xE2, 0xE0, 0xAE,
            0xA9, 0xAC, 0xA0, 0xE8, 0x35, 0x35, 0x2E, 0x35,
            0x31, 0x2E, 0x33, 0x31, 0x2E, 0x33, 0x33, 0x4B,
            0x4D, 0x4B, 0x20, 0x33, 0x30, 0x34, 0x35, 0x20,
            0x4D, 0x6F, 0x62, 0x69, 0x6C, 0x6B, 0x72, 0x61,
            0x6E, 0x94, 0xA8, 0xE0, 0xAC, 0xA0, 0x20, 0x4B,
            0x52, 0x55, 0x50, 0x50, 0x20, 0x28, 0x94, 0x90,
            0x83, 0x29, 0x31, 0x32, 0x20, 0xE1, 0x2E, 0x96,
            0x8D, 0x88, 0x88, 0x92, 0x9D, 0xE1, 0xE2, 0xE0,
            0xAE, 0xA9, 0xAC, 0xA0, 0xE8, 0x91, 0x8C, 0x2D,
            0x34, 0x2F, 0x32, 0x32, 0x34, 0x2D, 0x39, 0x30,
            0x92, 0xE0, 0xA5, 0xE5, 0xAE, 0xE1, 0xAD, 0xEB,
            0xA9, 0x20, 0xAA, 0xE0, 0xA0, 0xAD, 0x20, 0xA3,
            0xE0, 0xE3, 0xA7, 0xAE, 0xAF, 0xAE, 0xA4, 0xEA,
            0xA5, 0xAC, 0xAD, 0xAE, 0xE1, 0xE2, 0xEC, 0xEE,
            0x20, 0x34, 0x35, 0x20, 0xE2, 0x20, 0xAF, 0xE0,
            0xA8, 0x20, 0xA2, 0xEB, 0xAB, 0xA5, 0xE2, 0xA5,
            0x20, 0xE1, 0xE2, 0xE0, 0xA5, 0xAB, 0xEB, 0x20,
            0x33, 0x20, 0xAC, 0x20, 0xA8, 0x20, 0x31, 0x2E,
            0x32, 0x20, 0xE2, 0x20, 0xAF, 0xE0, 0xA8, 0x20,
            0xA2, 0xEB, 0xAB, 0xA5, 0xE2, 0xA5, 0x20, 0xE1,
            0xE2, 0xE0, 0xA5, 0xAB, 0xEB, 0x20, 0x32, 0x38,
            0x20, 0xAC, 0x2E, 0x20, 0x82, 0xEB, 0xE1, 0xAE,
            0xE2, 0xA0, 0x20, 0xA3, 0xE0, 0xE3, 0xA7, 0xAE,
            0xA2, 0xAE, 0xA3, 0xAE, 0x20, 0xAA, 0xE0, 0xEE,
            0xAA, 0xA0, 0x20, 0x34, 0x36, 0x20, 0xAC, 0x2E,
            0x20, 0x91, 0xE2, 0xE0, 0xA5, 0xAB, 0xA0, 0x20,
            0xE2, 0xA5, 0xAB, 0xA5, 0xE1, 0xAA, 0xAE, 0xAF
        };

        private static byte[] _xrfContent32 =
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static byte[] _ifpContent64 = {};
        private static byte[] _l01Content64 = {};
        private static byte[] _n01Content64 = {};
        private static byte[] _xrfContent64 = {};
        private static byte[] _mstContent64 =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Create 8 database files for IRBIS32.
        /// </summary>
        public static void CreateDatabase32
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            string cntFile = path + ".cnt";
            File.WriteAllBytes(cntFile, _cntContent32);

            string ifpFile = path + ".ifp";
            File.WriteAllBytes(ifpFile, _ifpContent32);

            string l01File = path + ".l01";
            File.WriteAllBytes(l01File, _l01Content32);

            string l02File = path + ".l02";
            File.WriteAllBytes(l02File, _l02Content32);

            string mstFile = path + ".mst";
            File.WriteAllBytes(mstFile, _mstContent32);

            string n01File = path + ".n01";
            File.WriteAllBytes(n01File, _n01Content32);

            string n02File = path + ".n02";
            File.WriteAllBytes(n02File, _n02Content32);

            string xrfFile = path + ".xrf";
            File.WriteAllBytes(xrfFile, _xrfContent32);
        }

        /// <summary>
        /// Create 5 database files for IRBIS64.
        /// </summary>
        public static void CreateDatabase64
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            string ifpFile = path + ".ifp";
            File.WriteAllBytes(ifpFile, _ifpContent64);

            string l01File = path + ".l01";
            File.WriteAllBytes(l01File, _l01Content64);

            string mstFile = path + ".mst";
            File.WriteAllBytes(mstFile, _mstContent64);

            string n01File = path + ".n01";
            File.WriteAllBytes(n01File, _n01Content64);

            string xrfFile = path + ".xrf";
            File.WriteAllBytes(xrfFile, _xrfContent64);
        }

        #endregion
    }
}

#endif