using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Exceptions
{
    public sealed class ConfigurationListenerException
        : ConfigurationErrorsException
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ConfigurationListenerException()
            : base()
        {
        }

        public ConfigurationListenerException(string message)
            : base(message)
        {
        }

        public ConfigurationListenerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConfigurationListenerException(string message, System.Xml.XmlNode node)
            : base(message, node)
        {
        }

        public ConfigurationListenerException(string message, System.Xml.XmlReader reader)
            : base(message, reader)
        {
        }

        public ConfigurationListenerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public ConfigurationListenerException(string message, Exception innerException, System.Xml.XmlNode node)
            : base(message, innerException, node)
        {
        }

        public ConfigurationListenerException(string message, Exception innerException, System.Xml.XmlReader reader)
            : base(message, innerException, reader)
        {
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
