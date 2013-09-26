using Fosol.Common.Extensions.Strings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Provides a way to write to the specified Stream.
    /// </summary>
    public sealed class StreamListener
        : TraceListener
    {
        #region Variables
        private Stream _Writer;
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
                var message = this.Render(traceEvent);
                if (message != null)
                {
                    var bytes = message.ToByteArray(this.Encoding);
                    this.Writer.Write(bytes, 0, bytes.Length);
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
