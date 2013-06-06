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
    /// Writes messages to the specified Stream.
    /// </summary>
    public class StreamListener
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
        /// Write the message to this listener.
        /// First convert the message into a byte array using the Encoding.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public override void Write(string message)
        {
            this.Write(message.ToByteArray(this.Encoding));
        }

        /// <summary>
        /// Write the data to the listener stream.
        /// </summary>
        /// <param name="data">Data to write to the stream.</param>
        public override void Write(byte[] data)
        {
            this.Writer.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        public override void Close()
        {
            if (this.Writer != null)
                this.Writer.Close();
        }

        /// <summary>
        /// Flush the stream.
        /// </summary>
        public override void Flush()
        {
            if (this.Writer != null)
                this.Writer.Flush();
        }

        /// <summary>
        /// Dispose the listener and the stream.
        /// </summary>
        public override void Dispose()
        {
            if (this.Writer != null)
            {
                Close();
                this.Writer = null;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
