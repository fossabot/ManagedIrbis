﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XrfFile64.cs -- cross-reference file reading
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81 && !SILVERLIGHT && !PORTABLE

#region Using directives

using System;
using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006002000.htm
    //
    // Каждая ссылка состоит из 3-х полей:
    // Число бит Параметр
    // 32        XRF_LOW – младшее слово в 8 байтовом смещении на запись;
    // 32        XRF_HIGH– старшее слово в 8 байтовом смещении на запись;
    // 32        XRF_FLAGS – Индикатор записи в виде битовых флагов
    //           следующего содержания:
    //             BIT_LOG_DEL(1)  - логически удаленная запись;
    //             BIT_PHYS_DEL(2) - физически удаленная запись;
    //             BIT_ABSENT(4)  - несуществующая запись;
    //             BIT_NOTACT_REC(8)- неактуализированная запись;
    //             BIT_LOCK_REC(64)- заблокированная запись.
    //

    /// <summary>
    /// Файл перекрестных ссылок XRF представляет собой
    /// таблицу ссылок на записи файла документов.
    /// Первая ссылка соответствует записи файла документов
    /// с номером 1, вторая – 2  и тд.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class XrfFile64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        /// <summary>
        /// Access mode.
        /// </summary>
        public DirectAccessMode Mode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public XrfFile64
            (
                [NotNull] string fileName,
                DirectAccessMode mode
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileName = fileName;
            Mode = mode;

            _lockObject = new object();
            _stream = DirectUtility.OpenFile(fileName, mode);
        }

        #endregion

        #region Private members

        private object _lockObject;

        [NotNull]
        private readonly Stream _stream;

        private long _GetOffset
            (
                int mfn
            )
        {
            long result = unchecked(XrfRecord64.RecordSize * (mfn - 1));
            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record.
        /// </summary>
        [NotNull]
        public XrfRecord64 ReadRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            long ofs;
            int flags;
            lock (_lockObject)
            {
                long offset = _GetOffset(mfn);
                if (offset >= _stream.Length)
                {
                    throw new ArgumentOutOfRangeException("mfn");
                }
                if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
                {
                    throw new IOException();
                }

                _stream.Flush();
                ofs = _stream.ReadInt64Network();
                flags = _stream.ReadInt32Network();
            }

            XrfRecord64 result = new XrfRecord64
            {
                Mfn = mfn,
                Offset = ofs,
                Status = (RecordStatus)flags
            };

            return result;
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                [NotNull] XrfRecord64 record
            )
        {
            Code.NotNull(record, "record");

            long offset = _GetOffset(record.Mfn);
            lock (_lockObject)
            {
                _stream.Seek(offset, SeekOrigin.Begin);
                _stream.WriteInt64Network(record.Offset);
                _stream.WriteInt32Network((int)record.Status);
                _stream.Flush();
            }
        }

        /// <summary>
        /// Lock the record.
        /// </summary>
        public void LockRecord
            (
                int mfn,
                bool flag
            )
        {
            XrfRecord64 record = ReadRecord(mfn);
            if (flag != record.Locked)
            {
                record.Locked = flag;
                WriteRecord(record);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            lock (_lockObject)
            {
                _stream.Dispose();
            }
        }

        #endregion
    }
}

#endif



