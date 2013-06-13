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
        private const string _XmlnsKey = "xmlns";
        private const string _ThrowExceptionsKey = "throwExceptions";
        private const string _SharedListenersKey = "sharedListeners";
        private const string _SharedFiltersKey = "sharedFilters";
        private const string _SourcesKey = "sources";
        private const string _TraceKey = "trace";
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The default xml namespace.
        /// </summary>
        [ConfigurationProperty(_XmlnsKey)]
        public string Xmlns
        {
            get { return (string)this[_XmlnsKey]; }
            set { this[_XmlnsKey] = value; }
        }

        [ConfigurationProperty(_ThrowExceptionsKey, DefaultValue = false)]
        public bool ThrowExceptions
        {
            get { return (bool)base[_ThrowExceptionsKey]; }
            set { base[_ThrowExceptionsKey] = value; }
        }

        [ConfigurationProperty(_SharedListenersKey)]
        public ListenerElementCollection SharedListeners
        {
            get { return (ListenerElementCollection)base[_SharedListenersKey]; }
            set { base[_SharedListenersKey] = value; }
        }

        [ConfigurationProperty(_SharedFiltersKey)]
        public FilterElementCollection SharedFilters
        {
            get { return (FilterElementCollection)base[_SharedFiltersKey]; }
            set { base[_SharedFiltersKey] = value; }
        }

        [ConfigurationProperty(_SourcesKey)]
        public SourceElementCollection Sources
        {
            get { return (SourceElementCollection)base[_SourcesKey]; }
            set { base[_SourcesKey] = value; }
        }

        [ConfigurationProperty(_TraceKey)]
        public TraceElement Trace
        {
            get { return (TraceElement)base[_TraceKey]; }
            set { base[_TraceKey] = value; }
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
