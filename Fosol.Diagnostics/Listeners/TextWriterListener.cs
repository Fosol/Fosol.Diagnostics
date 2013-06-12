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
    public class TextWriterListener
        : TraceListener
    {
        #region Variables
        private const int _DefaultBufferSize = 1024;
        private TextWriter _Writer;
        private AutoFlushOption _AutoFlush;
        private int _BufferSize;
        private int _BufferUsed;

        /// <summary>
        /// AutoFlushOption provides a way to control when auto flushing occurs.
        /// </summary>
        public enum AutoFlushOption
        {
            /// <summary>
            /// Never auto flush.
            /// </summary>
            Never = 0,
            /// <summary>
            /// Flush after every write.
            /// </summary>
            EveryWrite = 1,
            /// <summary>
            /// Flush when the buffer is full.
            /// </summary>
            BufferFull = 2
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The stream the listener will write to.
        /// </summary>
        public TextWriter Writer
        {
            get
            {
                return _Writer;
            }
            set
            {
                _Writer = value;
            }
        }

        /// <summary>
        /// get/set - Controls whether after every write it will flush.
        /// </summary>
        [DefaultValue(AutoFlushOption.Never)]
        [TraceSetting("AutoFlush", typeof(EnumConverter), typeof(AutoFlushOption))]
        public AutoFlushOption AutoFlush
        {
            get
            {
                return _AutoFlush;
            }
            set
            {
                _AutoFlush = value;
            }
        }

        /// <summary>
        /// get/set - The buffer size (in bytes) of the Stream.
        /// </summary>
        [DefaultValue(_DefaultBufferSize)]
        [TraceSetting("BufferSize")]
        public int BufferSize
        {
            get
            {
                return _BufferSize;
            }
            set
            {
                _BufferSize = value;
            }
        }

        /// <summary>
        /// get/set - How many bytes is currently in the Stream Buffer.
        /// </summary>
        protected int BufferUsed
        {
            get
            {
                return _BufferUsed;
            }
            set
            {
                _BufferUsed = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a TextWriterListener.
        /// </summary>
        public TextWriterListener()
        {

        }

        /// <summary>
        /// Creates a new instance of a TextWriterListener.
        /// Initializes the listener with the specified Stream.
        /// </summary>
        /// <param name="stream">The Stream that will be written to.</param>
        public TextWriterListener(Stream stream)
        {
            _Writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Creates a new instance f a TextWriterListener.
        /// Initializes the listener with the specified TextWriter stream.
        /// </summary>
        /// <param name="writer">The TextWriter that will be written to.</param>
        public TextWriterListener(TextWriter writer)
        {
            _Writer = writer;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the message to this listener.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnWrite(TraceEvent traceEvent)
        {
            if (this.Writer == null)
                return;

            var message = this.Render(traceEvent);
            try
            {
                var length = this.Encoding.GetByteCount(message);

                if (this.AutoFlush == AutoFlushOption.BufferFull
                    && this.BufferUsed + length >= this.BufferSize)
                    this.Flush();

                this.Writer.Write(message);

                this.BufferUsed += length;

                if (this.AutoFlush == AutoFlushOption.EveryWrite)
                    this.Flush();
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
                    this.BufferUsed = 0;
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
                    // Reset the buffer.
                    this.BufferUsed = 0;
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
