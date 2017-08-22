﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuChapter.cs -- 
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
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MenuChapter
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Leaf nodes only can contain records.
        /// </summary>
        [JsonProperty("leafOnly")]
        public bool LeafOnly { get; set; }

        /// <summary>
        /// Menu name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("menuName")]
        public string MenuName { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [CanBeNull]
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        /// <summary>
        /// Record selector.
        /// </summary>
        [CanBeNull]
        [JsonProperty("recordSelector")]
        public string RecordSelector { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("searchExpression")]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Title format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("titleFormat")]
        public string TitleFormat { get; set; }

        /// <summary>
        /// Special settings for some chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("specialSettings")]
        public List<SpecialSettings> SpecialSettings { get; private set; }

        /// <summary>
        /// Records.
        /// </summary>
        [CanBeNull]
        public RecordCollection Records { get; private set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuChapter()
        {
            SpecialSettings = new List<SpecialSettings>();
        }

        #endregion

        #region Private members

        private static char[] _lineDelimiters = { '\r', '\n' };

        private MenuSubChapter _CreateChapter
            (
                [NotNull] IPftFormatter formatter,
                [NotNull] IrbisTreeFile.Item item
            )
        {
            string key = item.Prefix.Trim();
            SpecialSettings settings = SpecialSettings.FirstOrDefault
                (
                    s => s.Name == key
                );
            string value = item.Suffix;

            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(1, key));
            record.Fields.Add(new RecordField(2, value));
            string title = formatter.FormatRecord(record);

            string className = null;
            if (!ReferenceEquals(settings, null))
            {
                className = settings.GetSetting("type");
            }

            MenuSubChapter result;
            if (string.IsNullOrEmpty(className))
            {
                result = new MenuSubChapter();
            }
            else
            {
                if (!className.Contains("."))
                {
                    className = "ManagedIrbis.Biblio." + className;
                }
                Type type = Type.GetType(className, true);
                result = (MenuSubChapter) Activator.CreateInstance(type);
            }
            result.Key = key;
            result.MainChapter = this;
            result.Title = title;
            result.Value = value;
            result.SpecialSettings = settings;

            foreach (IrbisTreeFile.Item child in item.Children)
            {
                MenuSubChapter subChapter
                    = _CreateChapter(formatter, child);
                result.Children.Add(subChapter);
            }

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather records {0}", this);
            RecordCollection badRecords = context.BadRecords;
            Records = new RecordCollection();
            MarcRecord record = null;

            try
            {
                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    IrbisProvider provider = context.Provider;
                    RecordCollection records = Records.ThrowIfNull();

                    string searchExpression = SearchExpression
                        .ThrowIfNull("SearchExpression");
                    formatter.ParseProgram(searchExpression);
                    record = new MarcRecord();
                    searchExpression = formatter.FormatRecord(record);

                    int[] found = provider.Search(searchExpression);
                    log.WriteLine("Found: {0} record(s)", found.Length);

                    log.Write("Reading records");
                    for (int i = 0; i < found.Length; i++)
                    {
                        log.Write(".");
                        record = provider.ReadRecord(found[i]);
                        records.Add(record);
                        context.Records.Add(record);
                    }
                    log.WriteLine(" done");

                    Dictionary<string, MenuSubChapter> dictionary
                        = new Dictionary<string, MenuSubChapter>();
                    Action<BiblioChapter> action = chapter =>
                    {
                        MenuSubChapter subChapter
                            = chapter as MenuSubChapter;
                        if (!ReferenceEquals(subChapter, null))
                        {
                            string key = subChapter.Key
                                .ThrowIfNull("subChapter.Key");
                            dictionary.Add(key, subChapter);
                        }
                    };
                    Walk(action);

                    string recordSelector = RecordSelector
                        .ThrowIfNull("RecordSelector");
                    formatter.ParseProgram(recordSelector);
                    log.Write("Distributing recors");

                    for (int i = 0; i < records.Count; i++)
                    {
                        log.Write(".");

                        record = records[i];
                        string key = formatter.FormatRecord(record);
                        if (string.IsNullOrEmpty(key))
                        {
                            badRecords.Add(record);
                        }
                        else
                        {
                            string[] keys = key.Trim()
                                .Split(_lineDelimiters)
                                .TrimLines()
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            key = keys.FirstOrDefault();
                            if (string.IsNullOrEmpty(key))
                            {
                                badRecords.Add(record);
                            }
                            else
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(key, out subChapter))
                                {
                                    subChapter.Records.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }

                            foreach (string nextKey in keys.Skip(1))
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(nextKey, out subChapter))
                                {
                                    subChapter.Duplicates.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }
                        }
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                log.WriteLine(" done");
                log.WriteLine("Bad records: {0}", badRecords.Count);

                // Do we really need this?

                foreach (BiblioChapter child in Children)
                {
                    child.GatherRecords(context);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format
                    (
                        "Exception: {0}",
                        exception
                    );

                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0}{1}{2}",
                            record.Mfn,
                            Environment.NewLine,
                            message
                        );
                }

                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End gather records {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.Initialize" />
        public override void Initialize
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
            try
            {
                string menuName = MenuName.ThrowIfNull("MenuName");

                IrbisProvider provider = context.Provider;

                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        provider.Database,
                        menuName
                    );
                MenuFile menu = provider.ReadMenuFile(specification);
                if (ReferenceEquals(menu, null))
                {
                    throw new IrbisException();
                }
                IrbisTreeFile tree = menu.ToTree();

                // Create Formatter

                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    string titleFormat = TitleFormat
                        .ThrowIfNull("TitleFormat");
                    formatter.ParseProgram(titleFormat);

                    foreach (IrbisTreeFile.Item root in tree.Roots)
                    {
                        MenuSubChapter chapter
                            = _CreateChapter(formatter, root);
                        Children.Add(chapter);
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.Initialize(context);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            context.Processor.Report.Body.Add(new NewPageBand());

            base.Render(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}