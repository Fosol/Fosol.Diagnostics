using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private TraceFormat _Format;
        private Encoding _Encoding;
        #endregion

        #region Properties
        [DefaultValue(_DefaultFormat)]
        [TraceSetting("format", typeof(Converters.LogFormatConverter))]
        public TraceFormat Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        [DefaultValue("default")]
        [TraceSetting("encoding", typeof(Common.Converters.EncodingConverter))]
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        #endregion

        #region Constructors
        public TraceListener()
        {
        }
        #endregion

        #region Methods
        public virtual void Initialize()
        {

        }

        public abstract void Write(string message);

        public virtual void Write(TraceEvent traceEvent)
        {
            Write(this.Format.Render(traceEvent));
        }

        public virtual void Close()
        {
        }

        public virtual void Flush()
        {
        }

        public virtual void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
