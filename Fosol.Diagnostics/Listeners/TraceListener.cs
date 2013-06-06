using Fosol.Common.Extensions.Bytes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    public abstract class TraceListener
        : IDisposable
    {
        #region Variables
        protected readonly System.Threading.ReaderWriterLockSlim _ChildLock = new System.Threading.ReaderWriterLockSlim();
        private const string _DefaultFormat = "{source} {type}: {id}: {message}{newline}";
        private TraceFormatter _Format;
        private Encoding _Encoding;
        #endregion

        #region Properties
        [DefaultValue(_DefaultFormat)]
        [TraceSetting("Format", typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        [DefaultValue("default")]
        [TraceSetting("Encoding", typeof(Common.Converters.EncodingConverter))]
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

        /// <summary>
        /// Convert the data into a string with the configured Encoding and write to the listener.
        /// </summary>
        /// <param name="data">Data to write to the listener.</param>
        public virtual void Write(byte[] data)
        {
            this.Write(data.ToStringValue(this.Encoding));
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
