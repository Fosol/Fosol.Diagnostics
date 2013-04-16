using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Writes trace event messages to a console window.
    /// Provides a way to color code messages.
    /// </summary>
    public class ColoredConsoleListener
        : ConsoleListener
    {
        #region Variables
        private ConsoleColor _BackgroundColor;
        private ConsoleColor _CriticalColor;
        private ConsoleColor _ErrorColor;
        private ConsoleColor _WarningColor;
        private ConsoleColor _InformationColor;
        private ConsoleColor _DebugColor;
        private ConsoleColor _StartColor;
        private ConsoleColor _StopColor;
        private ConsoleColor _SuspendColor;
        private ConsoleColor _ResumeColor;
        private ConsoleColor _TransferColor;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Background color of the console.
        /// </summary>
        [DefaultValue(ConsoleColor.Black)]
        [TraceListenerProperty("background", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor BackgroundColor
        {
            get 
            {
                return GetValue(ref _BackgroundColor);
            }
            set
            {
                SetValue(ref _BackgroundColor, value, "background");
            }
        }

        /// <summary>
        /// get/set - The foreground color for critical event type messages.
        /// </summary>
        [TraceListenerProperty("criticalColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor CriticalColor
        {
            get { return GetValue(ref _CriticalColor); }
            set { SetValue(ref _CriticalColor, value, "criticalColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for error event type messages.
        /// </summary>
        [TraceListenerProperty("errorColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor ErrorColor
        {
            get { return GetValue(ref _ErrorColor); }
            set { SetValue(ref _ErrorColor, value, "errorColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for warning event type messages.
        /// </summary>
        [TraceListenerProperty("warnColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor WarningColor
        {
            get { return GetValue(ref _WarningColor); }
            set { SetValue(ref _WarningColor, value, "warnColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for information event type messages.
        /// </summary>
        [TraceListenerProperty("infoColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor InformationColor
        {
            get { return GetValue(ref _InformationColor); }
            set { SetValue(ref _InformationColor, value, "infoColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for debug event type messages.
        /// </summary>
        [TraceListenerProperty("debugColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor DebugColor
        {
            get { return GetValue(ref _DebugColor); }
            set { SetValue(ref _DebugColor, value, "debugColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for start event type messages.
        /// </summary>
        [TraceListenerProperty("startColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor StartColor
        {
            get { return GetValue(ref _StartColor); }
            set { SetValue(ref _StartColor, value, "startColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for stop event type messages.
        /// </summary>
        [TraceListenerProperty("stopColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor StopColor
        {
            get { return GetValue(ref _StopColor); }
            set { SetValue(ref _StopColor, value, "stopColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for suspend event type messages.
        /// </summary>
        [TraceListenerProperty("suspendColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor SuspendColor
        {
            get { return GetValue(ref _SuspendColor); }
            set { SetValue(ref _SuspendColor, value, "suspendColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for resume event type messages.
        /// </summary>
        [TraceListenerProperty("resumeColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor ResumeColor
        {
            get { return GetValue(ref _ResumeColor); }
            set { SetValue(ref _ResumeColor, value, "resumeColor"); }
        }

        /// <summary>
        /// get/set - The foreground color for transfer event type messages.
        /// </summary>
        [TraceListenerProperty("transferColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor TransferColor
        {
            get { return GetValue(ref _TransferColor); }
            set { SetValue(ref _TransferColor, value, "transferColor"); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ColoredConsoleListener object.
        /// Send trace events to the Console.Out stream.
        /// </summary>
        public ColoredConsoleListener()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance of a ColoredConsoleListener object.
        /// Send trace events to the Console.Error or the Console.Out stream.
        /// </summary>
        /// <param name="useErrorStream">True if you want to send trace events to the Console.Error stream.</param>
        public ColoredConsoleListener(bool useErrorStream)
            : base(useErrorStream)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// This will only get called once.
        /// </summary>
        protected override void Intialize()
        {
            if (!this.UseErrorStream)
            {
                Console.BackgroundColor = this.BackgroundColor;
            }
        }

        /// <summary>
        /// Get the specific color for the event type.
        /// </summary>
        /// <param name="type">TraceEventType value.</param>
        /// <returns>ConsoleColor configured for the event type.</returns>
        protected ConsoleColor GetForegroundColor(TraceEventType type)
        {
            switch (type)
            {
                case (TraceEventType.Critical):
                    return this.CriticalColor;
                case (TraceEventType.Error):
                    return this.ErrorColor;
                case (TraceEventType.Information):
                    return this.InformationColor;
                case (TraceEventType.Resume):
                    return this.ResumeColor;
                case (TraceEventType.Start):
                    return this.StartColor;
                case (TraceEventType.Stop):
                    return this.StopColor;
                case (TraceEventType.Suspend):
                    return this.SuspendColor;
                case (TraceEventType.Warning):
                    return this.WarningColor;
                case (TraceEventType.Debug):
                default:
                    return this.DebugColor;
            }
        }

        /// <summary>
        /// Change the foreground color of the Console.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        public override void Write(TraceEvent traceEvent)
        {
            var current_color = Console.ForegroundColor;

            if (!this.UseErrorStream)
            {
                Console.ForegroundColor = GetForegroundColor(traceEvent.EventType);
            }

            base.Write(traceEvent);

            if (!this.UseErrorStream)
            {
                Console.ForegroundColor = current_color;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
