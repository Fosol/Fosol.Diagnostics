using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Sends trace event messages to a specified TraceSource.
    /// </summary>
    public sealed class TraceWriter
        : IDisposable
    {
        #region Variables
        private readonly string _Source;
        private readonly TraceFactory _Factory;
        private volatile TraceFilter _Filter;
        private volatile List<WeakReference<TraceListener>> _Listeners;
        private volatile bool _AutoFlush;
        #endregion

        #region Properties
        public string Source
        {
            get { return _Source; }
        }

        internal TraceFactory Factory
        {
            get { return _Factory; }
        }

        public TraceFilter Filter
        {
            get { return _Filter; }
        }

        public List<WeakReference<TraceListener>> Listeners
        {
            get { return _Listeners; }
        }

        public bool AutoFlush
        {
            get { return _AutoFlush; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceWriter object.
        /// </summary>
        /// <param name="factory">TraceFactory object.</param>
        internal TraceWriter(TraceFactory factory)
            : this(factory, null)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceWriter object.
        /// </summary>
        /// <param name="factory">TraceFactory object.</param>
        /// <param name="sourceName">The name of the source.</param>
        internal TraceWriter(TraceFactory factory, string sourceName)
        {
            _Factory = factory;
            _Source = sourceName;
            _Filter = new TraceFilter();
        }
        #endregion

        #region Methods
        #region Write Methods
        public void Write(TraceEventType eventType, int id)
        {
            Write(eventType, id, null, null);
        }

        public void Write(TraceEventType eventType, int id, string message)
        {
            Write(eventType, id, message, null);
        }

        public void Write(TraceEventType eventType, string message)
        {
            Write(eventType, 0, message, null);
        }

        public void Write(TraceEventType eventType, string message, object data)
        {
            Write(eventType, 0, message, data);
        }

        public void Write(TraceEventType eventType, object data)
        {
            Write(eventType, 0, null, data);
        }

        /// <summary>
        /// Write the message to the TraceSource.
        /// </summary>
        /// <param name="eventType">TraceEventType value.</param>
        /// <param name="id">Id to identify the trace event.</param>
        /// <param name="message">Message to describe trace event.</param>
        /// <param name="data">Data to include in the trace event.</param>
        public void Write(TraceEventType eventType, int id, string message, object data)
        {
            // Check if this trace should be sent down the line before generating.
            var trace = new TraceEvent(eventType, this.Source, id, message, data);

            if (ShouldTrace(trace))
            {
                trace.Initialize();

                int i = 0;
                while (i < this.Listeners.Count)
                {
                    TraceListener listener;

                    if (this.Listeners[i].TryGetTarget(out listener))
                    {
                        if (!listener.IsThreadSafe)
                        {
                            lock (listener)
                            {
                                listener.Write(trace);

                                if (this.AutoFlush)
                                    listener.Flush();
                            }
                        }
                        else
                        {
                            listener.Write(trace);

                            if (this.AutoFlush)
                                listener.Flush();
                        }
                    }
                }
            }
        }
        #endregion

        #region Shortform Write
        /// <summary>
        /// Write a deug message to the TraceSource.
        /// </summary>
        /// <param name="message">Message to describe trace event.</param>
        public void Debug(string message)
        {
            Write(TraceEventType.Debug, message);
        }

        /// <summary>
        /// Write an information message to the TraceSource.
        /// </summary>
        /// <param name="message">Message to describe trace event.</param>
        public void Info(string message)
        {
            Write(TraceEventType.Information, message);
        }

        /// <summary>
        /// Write a warning message to the TraceSource.
        /// </summary>
        /// <param name="message">Message to describe trace event.</param>
        public void Warn(string message)
        {
            Write(TraceEventType.Warning, message);
        }

        /// <summary>
        /// Write an error message to the TraceSource.
        /// </summary>
        /// <param name="message">Message to describe trace event.</param>
        public void Error(string message)
        {
            Write(TraceEventType.Error, message);
        }

        /// <summary>
        /// Write a critical message to the TraceSource.
        /// </summary>
        /// <param name="message">Message to describe trace event.</param>
        public void Critical(string message)
        {
            Write(TraceEventType.Critical, message);
        }
        #endregion

        private bool ShouldTrace(TraceEvent trace)
        {
            if (this.Listeners != null && this.Listeners.Count > 0)
                return this.Filter.ShouldTrace(trace);
            return false;
        }

        /// <summary>
        /// Close the TraceSource.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Flush the TraceSource.
        /// </summary>
        public void Flush()
        {
        }
        
        /// <summary>
        /// Dispose the LogWriter.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
