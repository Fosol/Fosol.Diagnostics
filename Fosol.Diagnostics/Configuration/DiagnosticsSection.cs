using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Configuration
{
    internal class DiagnosticsSection
        : ConfigurationSection
    {
        #region Variables
        private const string AutoFlushKey = "autoFlush";
        private const string SharedListenersKey = "sharedListeners";
        private const string FiltersKey = "filters";
        private const string SourcesKey = "sources";
        private const string ListenersKey = "listeners";
        #endregion

        #region Properties
        [ConfigurationProperty(AutoFlushKey)]
        public bool AutoFlush
        {
            get { return (bool)this[AutoFlushKey]; }
            set { this[AutoFlushKey] = value; }
        }

        [ConfigurationProperty(SharedListenersKey)]
        public ListenerElementCollection SharedListeners
        {
            get { return (ListenerElementCollection)this[SharedListenersKey]; }
            set { this[SharedListenersKey] = value; }
        }

        [ConfigurationProperty(FiltersKey)]
        public FilterElementCollection Filters
        {
            get { return (FilterElementCollection)this[FiltersKey]; }
            set { this[FiltersKey] = value; }
        }

        [ConfigurationProperty(SourcesKey)]
        public SourceElementCollection Sources
        {
            get { return (SourceElementCollection)this[SourcesKey]; }
            set { this[SourcesKey] = value; }
        }

        [ConfigurationProperty(ListenersKey)]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)this[ListenersKey]; }
            set { this[ListenersKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
