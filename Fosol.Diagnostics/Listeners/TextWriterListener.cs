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
        private TextWriter _Writer;
        private bool _AutoFlush;
        private int _BufferSize;
        private int _FlushSize;
        private int _BufferUsed;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The stream the listener will write to.
        /// </summary>
        public TextWriter Writer
        {
            get 
            {
                // If already read locked just return the value.
                if (_ChildLock.IsUpgradeableReadLockHeld || _ChildLock.IsReadLockHeld)
                    return _Writer;

                _ChildLock.EnterReadLock();
                try
                {
                    return _Writer;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set 
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _Writer = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - Controls whether after every write it will flush.
        /// </summary>
        [DefaultValue(false)]
        [TraceSetting("AutoFlush")]
        public bool AutoFlush
        {
            get
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _AutoFlush;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _AutoFlush = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - The buffer size (in bytes) of the Stream.
        /// </summary>
        [DefaultValue(1024)]
        [TraceSetting("BufferSize")]
        public int BufferSize
        {
            get
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _BufferSize;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _BufferSize = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - Controls when the Stream is flushed, by default it will flush when the Buffer is full, but this means that the Stream's Buffer will grow before flushing.
        /// It's best to set this value to be less than the BufferSize so that the Buffer is flushed before the Stream needs to increase the Buffer size.
        /// Set it to '0' if you want to flush manually.
        /// </summary>
        [DefaultValue(1024)]
        [TraceSetting("FlushSize")]
        public int FlushSize
        {
            get
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _FlushSize;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _FlushSize = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - How many bytes is currently in the Stream Buffer.
        /// </summary>
        protected int BufferUsed
        {
            get
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _BufferUsed;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _BufferUsed = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
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
            this.Writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Creates a new instance f a TextWriterListener.
        /// Initializes the listener with the specified TextWriter stream.
        /// </summary>
        /// <param name="writer">The TextWriter that will be written to.</param>
        public TextWriterListener(TextWriter writer)
        {
            this.Writer = writer;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the message to this listener.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public override void Write(string message)
        {
            if (!EnsureWriter())
                return;

            try
            {
                this.Writer.Write(message);

                if (this.AutoFlush)
                    this.Flush();
                else if (this.FlushSize > 0)
                {
                    _ChildLock.EnterUpgradeableReadLock();
                    try
                    {
                        this.BufferUsed += this.Encoding.GetByteCount(message);

                        if (_BufferUsed >= _FlushSize)
                        {
                            this.Flush();
                        }
                    }
                    finally
                    {
                        _ChildLock.ExitUpgradeableReadLock();
                    }
                }
            }
            catch (ObjectDisposedException)
            {

            }
        }

        /// <summary>
        /// Ensure that the Writer has been initialized.
        /// If it hasn't this listener will not write to the stream.
        /// </summary>
        /// <returns>'True' if the Writer has been initialized.</returns>
        protected virtual bool EnsureWriter()
        {
            if (this.Writer == null)
            {
                return false;
            }

            return true;
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
                _ChildLock.EnterWriteLock();
                try
                {
                    _Writer.Flush();
                    // Reset the buffer.
                    _BufferUsed = 0;
                }
                catch (ObjectDisposedException)
                {
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
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
