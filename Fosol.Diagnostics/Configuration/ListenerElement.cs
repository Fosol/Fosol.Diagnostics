using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal class ListenerElement
        : ConfigurationElement
    {
        #region Variables
        private const string NameKey = "name";
        private const string TypeNameKey = "type";
        private const string ArgsKey = "args";
        private const string FilterKey = "filter";
        private TraceListener _Listener;
        #endregion

        #region Properties
        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(TypeNameKey)]
        public string TypeName
        {
            get { return (string)base[TypeNameKey]; }
            set { base[TypeNameKey] = value; }
        }

        [ConfigurationProperty(ArgsKey)]
        public ArgumentElementCollection Args
        {
            get { return (ArgumentElementCollection)base[ArgsKey]; }
            set { base[ArgsKey] = value; }
        }

        [ConfigurationProperty(FilterKey)]
        public FilterElement Filter
        {
            get { return (FilterElement)base[FilterKey]; }
            set { base[FilterKey] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal TraceListener GetListener()
        {
            if (_Listener != null)
                return _Listener;

            var type_name = this.TypeName;
            var filter = this.Filter;
            var args = this.Args;
            // This is a reference to a SharedListener, so confirm that it exists.
            if (string.IsNullOrEmpty(type_name))
            {
                // When refrencing a SharedListener you cannot include other configuration options.
                if (filter != null || args != null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Invalid_Properties, this.Name));

                // The reference must exist in the shared listeners.
                if (TraceManager.Manager.SharedListeners == null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, this.Name));

                var listener = TraceManager.Manager.SharedListeners[this.Name];
                if (listener == null)
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Reference_Must_Exist, this.Name));

                // Reference the shared listener.
                _Listener = listener.GetListener();
                return _Listener;
            }
            // Try to create the listener as it has been defined in the configuration.
            else
            {
                try
                {
                    if (args != null && args.Count > 0)
                    {
                        // Initialize the listener with the arguments.
                        var largs = this.Args.GetArguments(this.Name).Select(a => a.Value).ToArray();
                        _Listener = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<TraceListener>(type_name, largs);
                        return _Listener;
                    }
                    else
                    {
                        // Initialize the listener without any arguments.
                        _Listener = Fosol.Common.Helpers.ReflectionHelper.ConstructObject<TraceListener>(type_name);
                        return _Listener;
                    }
                }
                catch
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.Strings.Configuration_Exception_Listener_Arguments_Invalid, this.Name));
                }
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
