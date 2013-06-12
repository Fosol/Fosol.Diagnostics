using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceWriter provides a way to send message to all or a subset of TraceListeners.
    /// </summary>
    public class TraceWriter
        : IDisposable
    {
        #region Variables
        private string _Source;
        private Type _SourceType;
        private Configuration.ListenerElementCollection _Listeners;
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
        internal Configuration.ListenerElementCollection Listeners
        {
            get { return _Listeners; }
            set { _Listeners = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of TraceWriter.
        /// </summary>
        public TraceWriter()
        {
            _Listeners = TraceManager.Manager.Trace.Listeners;
        }

        /// <summary>
        /// Creates a new instance of TraceWriter.
        /// </summary>
        /// <param name="type"></param>
        public TraceWriter(Type type)
            : this()
        {
            this.SourceType = type;
        }

        /// <summary>
        /// Creates a new instance of TraceWriter.
        /// This will only send message to listeners configured for this source.
        /// </summary>
        /// <param name="source">The source used to filter these messages.</param>
        public TraceWriter(string source)
        {
            this.Source = source;
            var config = TraceManager.Manager.Sources[source];

            if (config != null)
            {
                _Listeners = config.Listeners;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write this message to the configured listeners.
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
            TraceEvent trace_event = new TraceEvent(eventType, id, this.Source, this.SourceType, message);
            Write(trace_event);
        }

        /// <summary>
        /// Write this TraceEvent to the configured listeners.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        public void Write(TraceEvent traceEvent)
        {
            if (this.Listeners != null)
            {
                foreach (var config in this.Listeners)
                {
                    if (config.Filter.GetFilter().ShouldTrace(traceEvent))
                    {
                        var listener = config.GetListener();
                        listener.Write(traceEvent);
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
                foreach (var config in this.Listeners)
                {
                    var listener = config.GetListener();
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
                foreach (var config in this.Listeners)
                {
                    var listener = config.GetListener();
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
                foreach (var config in this.Listeners)
                {
                    var listener = config.GetListener();
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
