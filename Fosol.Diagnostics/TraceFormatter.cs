using Fosol.Common.Extensions.NameValueCollections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a way to create a dynamic message output to any given TraceListener.
    /// </summary>
    public sealed class TraceFormatter
        : IDisposable
    {
        #region Variables
        private static readonly Fosol.Common.Caching.SimpleCache<Fosol.Common.Formatters.Keywords.FormatKeyword> _Cache = new Fosol.Common.Caching.SimpleCache<Fosol.Common.Formatters.Keywords.FormatKeyword>();
        private readonly string _Format;
        private readonly List<Fosol.Common.Formatters.Keywords.FormatKeyword> _Keywords = new List<Fosol.Common.Formatters.Keywords.FormatKeyword>();
        private static readonly Common.Parsers.KeywordParser _Parser = new Common.Parsers.KeywordParser("{", "}");
        #endregion

        #region Properties
        /// <summary>
        /// get - The collection of FormatKeyword objects in this TraceFormatter.
        /// </summary>
        public List<Fosol.Common.Formatters.Keywords.FormatKeyword> Keywords
        {
            get { return _Keywords; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceFormatter object.
        /// </summary>
        /// <param name="format">Formatted string value containing keywords.</param>
        public TraceFormatter(string format)
        {
            _Format = format;
            var keywords = TraceFormatter.Compile(format);

            foreach (var key in keywords)
            {
                _Keywords.Add(key);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates the dynamic text for this TraceFormatter.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Dynamicly generated text.</returns>
        public string Render(TraceEvent traceEvent)
        {
            var builder = new StringBuilder();

            foreach (var key in _Keywords)
            {
                var static_key = key as Fosol.Common.Formatters.Keywords.FormatStaticKeyword;
                var dynamic_key = key as Fosol.Common.Formatters.Keywords.FormatDynamicKeyword;

                if (static_key != null)
                    builder.Append(static_key.Text);
                else if (dynamic_key != null)
                    builder.Append(dynamic_key.Render(traceEvent));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Parse the formatted string into a collection of Keywords.
        /// </summary>
        /// <param name="format">Formatting string value.</param>
        /// <returns>Collection of Keywords.</returns>
        private static IEnumerable<Fosol.Common.Formatters.Keywords.FormatKeyword> Compile(string format)
        {
            return TraceFormatter.Compile(TraceFormatter._Parser, format);
        }

        /// <summary>
        /// Parse the formatted string into a collection of Keywords.
        /// Pulls keywords from cache if they are already added.
        /// Adds keywords to cache if they haven't been added.
        /// </summary>
        /// <param name="parser">Common.Parsers.SimpleParser object.</param>
        /// <param name="format">Formatting string value.</param>
        /// <returns>Collection of Keywords.</returns>
        private static IEnumerable<Fosol.Common.Formatters.Keywords.FormatKeyword> Compile(Common.Parsers.KeywordParser parser, string format)
        {
            var phrases = parser.Parse(format);
            var is_cached = false;

            foreach (var phrase in phrases)
            {
                // Cached version found, return it.
                is_cached = _Cache.ContainsKey(phrase.Text);

                if (is_cached)
                    yield return _Cache.Get(phrase.Text);

                Fosol.Common.Formatters.Keywords.FormatKeyword keyword = null;
                var key = phrase as Common.Parsers.Keyword;

                if (key == null)
                    // Return a new instance of the LiteralKeyword which will contain the text value.
                    keyword = new Fosol.Common.Formatters.Keywords.TextKeyword(phrase.Text);
                else
                {
                    // Determine the appropriate Keyword to use.
                    var type = Fosol.Diagnostics.Keywords.FormatKeywordLibrary.Get(key.Name);

                    var is_static = typeof(Fosol.Common.Formatters.Keywords.FormatStaticKeyword).IsAssignableFrom(type);
                    var is_dynamic = typeof(Fosol.Common.Formatters.Keywords.FormatDynamicKeyword).IsAssignableFrom(type);

                    // Return a new instance of the Keyword.
                    if (type != null)
                    {
                        if (is_static)
                        {
                            if (type.GetConstructor(new Type[] { typeof(string), typeof(StringDictionary) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatStaticKeyword)Activator.CreateInstance(type, phrase.Text, key.Params.ToStringDictionary());
                            else if (type.GetConstructor(new Type[] { typeof(string) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatStaticKeyword)Activator.CreateInstance(type, phrase.Text);
                            else if (type.GetConstructor(new Type[] { typeof(StringDictionary) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatStaticKeyword)Activator.CreateInstance(type, key.Params.ToStringDictionary());
                            else if (type.GetConstructor(new Type[0]) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatStaticKeyword)Activator.CreateInstance(type);
                        }
                        else if (is_dynamic)
                        {
                            if (type.GetConstructor(new Type[] { typeof(StringDictionary) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatDynamicKeyword)Activator.CreateInstance(type, key.Params.ToStringDictionary());
                            else if (type.GetConstructor(new Type[0]) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatDynamicKeyword)Activator.CreateInstance(type);
                        }
                        // If for some reason they've inherited directly from the FormatKeyword abstract class instead of the normal ones.
                        else
                        {
                            if (type.GetConstructor(new Type[] { typeof(string), typeof(StringDictionary) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatKeyword)Activator.CreateInstance(type, phrase.Text, key.Params.ToStringDictionary());
                            else if (type.GetConstructor(new Type[] { typeof(string) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatKeyword)Activator.CreateInstance(type, phrase.Text);
                            else if (type.GetConstructor(new Type[] { typeof(StringDictionary) }) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatKeyword)Activator.CreateInstance(type, key.Params.ToStringDictionary());
                            else if (type.GetConstructor(new Type[0]) != null)
                                keyword = (Fosol.Common.Formatters.Keywords.FormatKeyword)Activator.CreateInstance(type);
                        }
                    }
                    else
                        throw new System.Configuration.ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_TraceKeyword_Does_Not_Exist, key.Name));
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
        /// Dispose this TraceFormatter object.
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
