using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    internal sealed class ListenerElement
        : Fosol.Common.Configuration.TypeElement
    {
        #region Variables
        private const string FilterKey = "filter";
        private const string NameKey = "name";

        private static readonly ConfigurationProperty _NameProperty = new ConfigurationProperty(InitDataKey, typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
        private static readonly ConfigurationProperty _FilterProperty = new ConfigurationProperty(TypeNameKey, typeof(string), string.Empty, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _TypeNameProperty = new ConfigurationProperty(TypeNameKey, typeof(string), string.Empty, ConfigurationPropertyOptions.IsTypeStringTransformationRequired);

        private Hashtable _Attributes;
        #endregion

        #region Properties
        [ConfigurationProperty(FilterKey)]
        public FilterElement Filter
        {
            get { return (FilterElement)base[FilterKey]; }
            set { base[FilterKey] = value; }
        }

        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[NameKey]; }
            set { base[NameKey] = value; }
        }

        [ConfigurationProperty(TypeNameKey)]
        public override string TypeName
        {
            get { return (string)base[TypeNameKey]; }
            set { base[TypeNameKey] = value; }
        }

        public Hashtable Attributes
        {
            get 
            {
                if (_Attributes == null)
                    _Attributes = new Hashtable(StringComparer.OrdinalIgnoreCase);
                return _Attributes; 
            }
        }
        #endregion

        #region Constructors
        public ListenerElement()
            : base(typeof(TraceListener))
        {
            this.Properties.Remove(TypeNameKey);
            this.Properties.Add(_NameProperty);
            this.Properties.Add(_FilterProperty);
            this.Properties.Add(_TypeNameProperty);
        }
        #endregion

        #region Methods
        public TraceListener GetRuntimeObject()
        {
            if (_RuntimeObject != null)
                return (TraceListener)_RuntimeObject;

            TraceListener result;
            try
            {
                var type_name = this.TypeName;
                if (string.IsNullOrEmpty(type_name))
                {
                    // Reference cannot have properties.
                    if (_Attributes != null || this.InitData != null)
                        throw new ConfigurationErrorsException();
                    // Look for the listener in the SharedListener collection.
                    if (DiagnosticsConfiguration.SharedListeners == null)
                        throw new ConfigurationErrorsException();
                    var listener_element = DiagnosticsConfiguration.SharedListeners[this.Name];
                    if (listener_element == null)
                        throw new ConfigurationErrorsException();
                    _RuntimeObject = listener_element.GetRuntimeObject();
                    result = (TraceListener)_RuntimeObject;
                }
                else
                {
                    var trace_listener = (TraceListener)base.BaseConstructObject();
                    trace_listener.InitializeData = this.InitData;
                    trace_listener.Name = this.Name;
                    trace_listener.SetAttributes(this.Attributes);
                    if (this.Filter != null && !string.IsNullOrEmpty(this.Filter.TypeName))
                    {
                        trace_listener.Filter = this.Filter.GetRuntimeObject();
                    }
                    _RuntimeObject = trace_listener;
                    result = trace_listener;
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("", ex);
            }
            return result;
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            this.Attributes.Add(name, value);
            return true;
        }

        protected override void PreSerialize(System.Xml.XmlWriter writer)
        {
            if (_Attributes != null)
            {
                var enumerator = _Attributes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var text = (string)enumerator.Value;
                    var local_name = (string)enumerator.Key;
                    if (text != null && writer != null)
                        writer.WriteAttributeString(local_name, text);
                }
            }
        }

        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            return base.SerializeElement(writer, serializeCollectionKey) || (_Attributes != null && _Attributes.Count > 0);
        }

        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
            var listener_element = sourceElement as ListenerElement;
            if (listener_element != null && listener_element._Attributes != null)
                _Attributes = listener_element._Attributes;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
