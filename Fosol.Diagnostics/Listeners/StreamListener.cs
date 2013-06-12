using Fosol.Common.Extensions.Objects;
using Fosol.Common.Extensions.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Writes messages to the specified Stream.
    /// </summary>
    public class StreamListener
        : TraceListener
    {
        #region Variables
        private Stream _Writer;
        private bool _UseData;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The stream the listener will write to.
        /// </summary>
        public Stream Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }

        /// <summary>
        /// get/set - Controls what will be written to the stream.
        /// If UseData = 'true' it will write the TraceEvent.Data property to the stream instead of the rendered message.
        /// </summary>
        [DefaultValue(false)]
        [TraceSetting("UseData")]
        public bool UseData
        {
            get { return _UseData; }
            set { _UseData = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a StreamListener.
        /// </summary>
        public StreamListener()
        {

        }

        /// <summary>
        /// Creates a new instance of a StreamListener.
        /// Initializes the listener with the specified Stream.
        /// </summary>
        /// <param name="stream">The Stream that will be written to.</param>
        public StreamListener(Stream stream)
        {
            this.Writer = stream;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the data to the listener stream.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnWrite(TraceEvent traceEvent)
        {
            try
            {
                if (!this.UseData)
                {
                    var message = this.Render(traceEvent);
                    if (message != null)
                    {
                        var bytes = message.ToByteArray(this.Encoding);
                        this.Writer.Write(bytes, 0, bytes.Length);
                    }
                }
                else if (traceEvent.Data != null)
                {
                    if (traceEvent.Data is byte[])
                    {
                        var data = traceEvent.Data as byte[];
                        if (data != null)
                        {
                            this.Writer.Write(data, 0, data.Length);
                        }
                    }
                    else
                    {
                        var data = traceEvent.Data.ToByteArray();
                        if (data != null)
                        {
                            this.Writer.Write(data, 0, data.Length);
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        public override void Close()
        {
            if (this.Writer != null)
            {
                try
                {
                    this.Writer.Close();
                    this.Writer = null;
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        /// <summary>
        /// Flush the stream.
        /// </summary>
        public override void Flush()
        {
            if (this.Writer != null)
            {
                try
                {
                    this.Writer.Flush();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        /// <summary>
        /// Dispose the listener and the stream.
        /// </summary>
        public override void Dispose()
        {
            if (this.Writer != null)
            {
                Close();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
