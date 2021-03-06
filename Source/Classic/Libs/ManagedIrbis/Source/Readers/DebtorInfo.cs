﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DebtorInfo.cs -- информация о задолжнике.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о задолжнике
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Name")]
    public sealed class DebtorInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Фамилия, имя, отчество
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [CanBeNull]
        [XmlAttribute("dateOfBirth")]
        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Номер читательского билета
        /// </summary>
        [CanBeNull]
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [CanBeNull]
        [XmlAttribute("gender")]
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        [CanBeNull]
        [XmlAttribute("category")]
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [CanBeNull]
        [XmlAttribute("address")]
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        [CanBeNull]
        [XmlAttribute("workPlace")]
        [JsonProperty("workPlace")]
        public string WorkPlace { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [CanBeNull]
        [XmlAttribute("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Домашний телефон
        /// </summary>
        [CanBeNull]
        [XmlAttribute("homePhone")]
        [JsonProperty("homePhone")]
        public string HomePhone { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Age { get; set; }

        /// <summary>
        /// Примечания
        /// </summary>
        [CanBeNull]
        [XmlAttribute("remarks")]
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Mfn { get; set; }

        /// <summary>
        /// Расформатированное описание
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// Произвольные данные
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        /// <summary>
        /// Задолженные экземпляры
        /// </summary>
        [CanBeNull]
        [XmlArrayItem("exemplar")]
        [JsonProperty("exemplars")]
        public VisitInfo[] Debt { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Формирование задолжника из читателя
        /// </summary>
        [NotNull]
        public static DebtorInfo FromReader
            (
                [NotNull] ReaderInfo reader,
                [NotNull] VisitInfo[] debt
            )
        {
            Code.NotNull(reader, "reader");

            string address = null;
            if (!ReferenceEquals(reader.Address, null))
            {
                address = reader.Address.ToString();
            }
            DebtorInfo result = new DebtorInfo
            {
                Name = reader.FullName,
                DateOfBirth = reader.DateOfBirth,
                Ticket = reader.Ticket,
                Gender = reader.Gender,
                Category = reader.Category,
                Address = address,
                WorkPlace = reader.WorkPlace,
                Email = reader.Email,
                HomePhone = reader.HomePhone,
                Age = reader.Age,
                Remarks = reader.Remarks,
                Mfn = reader.Mfn,
                Debt = debt
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            DateOfBirth = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Gender = reader.ReadNullableString();
            Category = reader.ReadNullableString();
            Address = reader.ReadNullableString();
            WorkPlace = reader.ReadNullableString();
            Email = reader.ReadNullableString();
            HomePhone = reader.ReadNullableString();
            Age = reader.ReadPackedInt32();
            Remarks = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Description = reader.ReadNullableString();
            Debt = reader.ReadNullableArray<VisitInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Name)
                .WriteNullable(DateOfBirth)
                .WriteNullable(Ticket)
                .WriteNullable(Gender)
                .WriteNullable(Category)
                .WriteNullable(Address)
                .WriteNullable(WorkPlace)
                .WriteNullable(Email)
                .WriteNullable(HomePhone)
                .WritePackedInt32(Age)
                .WriteNullable(Remarks)
                .WritePackedInt32(Mfn)
                .WriteNullable(Description)
                .WriteNullableArray(Debt);
        }

        #endregion
    }
}
