using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Configuration
{
    internal class DiagnosticsSection
        : System.Configuration.ConfigurationSection
    {
        #region Variables
        public const string SectionName = "fosol.diagnostics";
        private const string XmlnsKey = "xmlns";
        private const string SharedListenersKey = "sharedListeners";
        private const string FiltersKey = "filters";
        private const string SourcesKey = "sources";
        private const string TraceKey = "trace";
        private const string ThrowExceptionsKey = "throwExceptions";
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The default xml namespace.
        /// </summary>
        [ConfigurationProperty(XmlnsKey)]
        public string Xmlns
        {
            get { return (string)this[XmlnsKey]; }
            set { this[XmlnsKey] = value; }
        }

        [ConfigurationProperty(ThrowExceptionsKey, DefaultValue = false)]
        public bool ThrowExceptions
        {
            get { return (bool)base[ThrowExceptionsKey]; }
            set { base[ThrowExceptionsKey] = value; }
        }

        [ConfigurationProperty(SharedListenersKey)]
        public ListenerElementCollection SharedListeners
        {
            get { return (ListenerElementCollection)base[SharedListenersKey]; }
            set { base[SharedListenersKey] = value; }
        }

        [ConfigurationProperty(FiltersKey)]
        public FilterElementCollection Filters
        {
            get { return (FilterElementCollection)base[FiltersKey]; }
            set { base[FiltersKey] = value; }
        }

        [ConfigurationProperty(SourcesKey)]
        public SourceElementCollection Sources
        {
            get { return (SourceElementCollection)base[SourcesKey]; }
            set { base[SourcesKey] = value; }
        }

        [ConfigurationProperty(TraceKey)]
        public TraceElement Trace
        {
            get { return (TraceElement)base[TraceKey]; }
            set { base[TraceKey] = value; }
        }
        #endregion

        #region Constructors
        public DiagnosticsSection()
        {
        }
        #endregion

        #region Methods
        protected override void InitializeDefault()
        {
            Trace.InitializeDefaultInternal();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
