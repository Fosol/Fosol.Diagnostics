using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceWriter provides a way to send message to all or a subset of TraceListeners.
    /// </summary>
    public sealed class TraceWriter
        : IDisposable
    {
        #region Variables
        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim();
        private string _Source;
        private Type _SourceType;
        private InstanceProcess _WriterProcess;
        private InstanceThread _WriterThread;
        #endregion

        #region Properties
        /// <summary>
        /// get - The source of message sent to this writer.
        /// </summary>
        public string Source
        {
            get { return _Source; }
            private set { _Source = value; }
        }

        /// <summary>
        /// get - The object type which is sending messages to this writer.
        /// </summary>
        public Type SourceType
        {
            get { return _SourceType; }
            private set { _SourceType = value; }
        }

        /// <summary>
        /// get - The configured listeners for this writer.
        /// </summary>
        internal Configuration.ListenerElementCollection Config
        {
            get 
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    var source = TraceManager.Manager.Sources[this.Source];
                    if (source != null)
                        return source.Listeners;
                    else
                        return null;
                }
                else
                    return TraceManager.Manager.Trace.Listeners;
            }
        }

        /// <summary>
        /// get - A collection of the listeners initialized from the configuration.
        /// </summary>
        public IEnumerable<Listeners.TraceListener> Listeners
        {
            get
            {
                if (this.Config == null)
                    yield break;

                foreach (var config in this.Config)
                {
                    yield return config.Listener;
                }
            }
        }

        public InstanceProcess WriterProcess
        {
            get { return _WriterProcess; }
            private set { _WriterProcess = value; }
        }

        public InstanceThread WriterThread
        {
            get { return _WriterThread; }
            private set { _WriterThread = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of TraceWriter.
        /// </summary>
        /// <param name="type">The Type of object submitting TraceEvents.</param>WWWWWWWWWWWW
        public TraceWriter(Type type)
        {
            Fosol.Common.Validation.Assert.IsNotNull(type, "type");
            this.SourceType = type;
        }

        /// <summary>
        /// Creates a new instance of TraceWriter.
        /// This will only send message to listeners configured for this source.
        /// </summary>
        /// <param name="source">The source used to filter these messages.</param>
        /// <param name="type">The Type of object submitting TraceEvents.</param>
        public TraceWriter(string source, Type type)
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(source, "source");
            Fosol.Common.Validation.Assert.IsNotNull(type, "type");
            this.Source = source;
            this.SourceType = type;
            this.WriterThread = new InstanceThread();
            this.WriterProcess = new InstanceProcess();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write an information message to the configured listeners.
        /// </summary>
        /// <param name="message">Message to send to listeners.</param>
        public void Write(string message)
        {
            Write(TraceEventType.Information, 0, message);
        }

        /// <summary>
        /// Write this message to the configured listeners.
        /// </summary>
        /// <param name="eventType">TraceEventType value.</param>
        /// <param name="message">Message to send to listeners.</param>
        public void Write(TraceEventType eventType, string message)
        {
            Write(eventType, 0, message);
        }

        /// <summary>
        /// Write this message to the configured listeners.
        /// </summary>
        /// <param name="eventType">TraceEventType value.</param>
        /// <param name="id">Unique id to identify this message.</param>
        /// <param name="message">Message to send to listeners.</param>
        public void Write(TraceEventType eventType, int id, string message)
        {
            Write(new TraceEvent(eventType, id, this.Source, this.SourceType, message));
        }

        /// <summary>
        /// Write an error message to the configured listeners.
        /// </summary>
        /// <param name="exception">Message to send to listeners.</param>
        public void Write(Exception exception)
        {
            Write(TraceEventType.Error, 0, exception);
        }

        /// <summary>
        /// Write this message to the configured listeners.
        /// </summary>
        /// <param name="eventType">TraceEventType value.</param>
        /// <param name="exception">Exception to send to listeners.</param>
        public void Write(TraceEventType eventType, Exception exception)
        {
            Write(eventType, 0, exception);
        }

        /// <summary>
        /// Write this message to the configured listeners.
        /// </summary>
        /// <param name="eventType">TraceEventType value.</param>
        /// <param name="id">Unique id to identify this message.</param>
        /// <param name="exception">Exception to send to listeners.</param>
        public void Write(TraceEventType eventType, int id, Exception exception)
        {
            Write(new TraceEvent(eventType, id, this.Source, this.SourceType, exception));
        }

        /// <summary>
        /// Write this TraceEvent to the configured listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        public void Write(TraceEvent traceEvent)
        {
            if (this.Listeners != null)
            {
                var auto_flush = TraceManager.Manager.AutoFlush;
                traceEvent.Writer = this;

                foreach (var listener in this.Listeners)
                {
                    if (listener.ShouldTrace(traceEvent))
                    {
                        listener.Write(traceEvent);
                        if (auto_flush)
                            listener.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Flush the listeners.
        /// </summary>
        public void Flush()
        {
            if (this.Listeners != null)
            {
                foreach (var listener in this.Listeners)
                {
                    listener.Flush();
                }
            }
        }

        /// <summary>
        /// Close the listeners.
        /// </summary>
        public void Close()
        {
            if (this.Listeners != null)
            {
                foreach (var listener in this.Listeners)
                {
                    listener.Close();
                }
            }
        }

        /// <summary>
        /// Close and then dispose the listeners.
        /// </summary>
        public void Dispose()
        {
            Close();

            if (this.Listeners != null)
            {
                foreach (var listener in this.Listeners)
                {
                    listener.Dispose();
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
