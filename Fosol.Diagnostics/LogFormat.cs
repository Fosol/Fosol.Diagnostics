﻿using Fosol.Common.Extensions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a way to create a dynamic message output to any given LogTarget.
    /// </summary>
    public sealed class LogFormat
        : IDisposable
    {
        #region Variables

        private static readonly Keywords.TraceKeywordCache _Cache = new Keywords.TraceKeywordCache();
        private readonly string _Format;
        private readonly List<Keywords.TraceKeywordBase> _Keywords = new List<Keywords.TraceKeywordBase>();
        private static readonly Common.Parsers.SimpleParser _Parser = new Common.Parsers.SimpleParser("{", "}");
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LogFormat object.
        /// </summary>
        /// <param name="format">Formatted string value containing keywords.</param>
        public LogFormat(string format)
        {
            _Format = format;
            var keywords = LogFormat.Compile(format);

            foreach (var key in keywords)
            {
                _Keywords.Add(key);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the dynamic text for this LogFormat.
        /// </summary>
        /// <param name="message">LogMessage object.</param>
        /// <returns>Dynamicly generated text.</returns>
        public string Render(LogMessage message)
        {
            var builder = new StringBuilder();

            foreach (var key in _Keywords)
            {
                var static_key = key as Keywords.LogStaticKeyword;
                var dynamic_key = key as Keywords.LogDynamicKeyword;

                if (static_key != null)
                    builder.Append(static_key.Text);
                else if (dynamic_key != null)
                    builder.Append(dynamic_key.Generate(message));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Parse the formatted string into a collection of Keywords.
        /// </summary>
        /// <param name="format">Formatting string value.</param>
        /// <returns>Collection of Keywords.</returns>
        private static IEnumerable<Keywords.TraceKeywordBase> Compile(string format)
        {
            return LogFormat.Compile(LogFormat._Parser, format);
        }

        /// <summary>
        /// Parse the formatted string into a collection of Keywords.
        /// Pulls keywords from cache if they are already added.
        /// Adds keywords to cache if they haven't been added.
        /// </summary>
        /// <param name="parser">Common.Parsers.SimpleParser object.</param>
        /// <param name="format">Formatting string value.</param>
        /// <returns>Collection of Keywords.</returns>
        private static IEnumerable<Keywords.TraceKeywordBase> Compile(Common.Parsers.SimpleParser parser, string format)
        {
            var phrases = parser.Parse(format);
            var is_cached = false;

            foreach (var phrase in phrases)
            {
                // Cached version found, return it.
                is_cached = _Cache.ContainsKey(phrase.Text);

                if (is_cached)
                    yield return _Cache.Get(phrase.Text);

                Keywords.TraceKeywordBase keyword = null;
                var key = phrase as Common.Parsers.Keyword;

                if (key == null)
                    // Return a new instance of the LiteralKeyword which will contain the text value.
                    keyword = new Keywords.LiteralKeyword(phrase.Text);
                else
                {
                    // Determine the appropraite Keyword to use.
                    var type = KeywordLibrary.Get(key.Name);

                    var is_static = typeof(Keywords.LogStaticKeyword).IsAssignableFrom(type);
                    var is_dynamic = typeof(Keywords.LogDynamicKeyword).IsAssignableFrom(type);
                    var is_empty_constructor = type.OnlyHasEmptyConstructor();

                    // Return a new instance of the Keyword.
                    if (type != null)
                    {
                        if (is_static)
                        {
                            // Use the correct constructor.
                            if (is_empty_constructor)
                                keyword = (Keywords.LogStaticKeyword)Activator.CreateInstance(type);
                            else if (type.HasTypeConstructor(typeof(string), typeof(System.Collections.Specialized.NameValueCollection)))
                                keyword = (Keywords.LogStaticKeyword)Activator.CreateInstance(type, phrase.Text, key.Params);
                            else if (type.HasTypeConstructor(typeof(string)))
                                keyword = (Keywords.LogStaticKeyword)Activator.CreateInstance(type, phrase.Text);
                            else if (type.HasTypeConstructor(typeof(System.Collections.Specialized.NameValueCollection)))
                                keyword = (Keywords.LogStaticKeyword)Activator.CreateInstance(type, key.Params);
                        }
                        else if (is_dynamic)
                        {
                            // Use the correct constructor.
                            if (is_empty_constructor)
                                keyword = (Keywords.LogDynamicKeyword)Activator.CreateInstance(type);
                            else
                                keyword = (Keywords.LogDynamicKeyword)Activator.CreateInstance(type, key.Params);
                        }
                        // If for some reason they've inherited directly from the LogKeyword abstract class instead of the normal ones.
                        else
                        {
                            // Use the correct constructor.
                            if (is_empty_constructor)
                                keyword = (Keywords.LogKeyword)Activator.CreateInstance(type);
                            else if (type.HasTypeConstructor(typeof(string), typeof(System.Collections.Specialized.NameValueCollection)))
                                keyword = (Keywords.LogKeyword)Activator.CreateInstance(type, phrase.Text, key.Params);
                            else if (type.HasTypeConstructor(typeof(string)))
                                keyword = (Keywords.LogKeyword)Activator.CreateInstance(type, phrase.Text);
                            else if (type.HasTypeConstructor(typeof(System.Collections.Specialized.NameValueCollection)))
                                keyword = (Keywords.LogKeyword)Activator.CreateInstance(type, key.Params);
                        }
                    }
                    else
                        throw new System.Configuration.ConfigurationErrorsException(string.Format(Resources.Strings.Exception_ConfigurationKeywordDoesNotExist, key.Name));
                }

                if (!is_cached)
                {
                    _Cache.Add(phrase.Text, keyword);
                    yield return keyword;
                }
            }
        }

        /// <summary>
        /// Returns the format string.
        /// </summary>
        /// <returns>Returns the format string.</returns>
        public override string ToString()
        {
            return _Format;
        }

        /// <summary>
        /// Dispose this LogFormat object.
        /// </summary>
        public void Dispose()
        {
            _Keywords.Clear();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}