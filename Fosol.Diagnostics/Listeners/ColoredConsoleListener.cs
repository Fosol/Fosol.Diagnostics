using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The ColoredConsoleListener writes messages to the command console and provides a way to use color.
    /// </summary>
    public class ColoredConsoleListener
        : ConsoleListener
    {
        #region Variables
        private ConsoleColor _BackgroundColor;
        private ConsoleColor _DebugColor;
        private ConsoleColor _InformationColor;
        private ConsoleColor _WarningColor;
        private ConsoleColor _ErrorColor;
        private ConsoleColor _CriticalColor;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The console background color.
        /// </summary>
        [DefaultValue(ConsoleColor.Black)]
        [TraceSetting("BackgroundColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor BackgroundColor
        {
            get 
            {
                return _BackgroundColor; 
            }
            set 
            {
                _BackgroundColor = value;
                Console.BackgroundColor = value;
            }
        }

        /// <summary>
        /// get/set - The color debug messages will be written in.
        /// </summary>
        [DefaultValue(ConsoleColor.Gray)]
        [TraceSetting("DebugColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor DebugColor
        {
            get
            {
                return _DebugColor;
            }
            set
            {
                _DebugColor = value;
            }
        }

        /// <summary>
        /// get/set - The color information messages will be written in.
        /// </summary>
        [DefaultValue(ConsoleColor.White)]
        [TraceSetting("InformationColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor InformationColor
        {
            get
            {
                return _InformationColor;
            }
            set
            {
                _InformationColor = value;
            }
        }

        /// <summary>
        /// get/set - The color warning messages will be written in.
        /// </summary>
        [DefaultValue(ConsoleColor.DarkRed)]
        [TraceSetting("WarningColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor WarningColor
        {
            get
            {
                return _WarningColor;
            }
            set
            {
                _WarningColor = value;
            }
        }

        /// <summary>
        /// get/set - The color error messages will be written in.
        /// </summary>
        [DefaultValue(ConsoleColor.Red)]
        [TraceSetting("ErrorColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor ErrorColor
        {
            get
            {
                return _ErrorColor;
            }
            set
            {
                _ErrorColor = value;
            }
        }

        /// <summary>
        /// get/set - The color critical messages will be written in.
        /// </summary>
        [DefaultValue(ConsoleColor.Red)]
        [TraceSetting("CriticalColor", typeof(EnumConverter), typeof(ConsoleColor))]
        public ConsoleColor CriticalColor
        {
            get
            {
                return _CriticalColor;
            }
            set
            {
                _CriticalColor = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ColoredConsoleListener.
        /// </summary>
        public ColoredConsoleListener()
            : base(false)
        {

        }

        /// <summary>
        /// Creates a new instance of a ColoredConsoleListener.
        /// </summary>
        /// <param name="useErrorStream">
        ///     If 'true' it will write messages to the Console.Error stream.
        ///     If 'false' it will write messages to the Console.Out stream.
        /// </param>
        public ColoredConsoleListener(bool useErrorStream)
            : base(useErrorStream)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the message to this listener.
        /// Updates the ForegroundColor based on the message TraceEventType.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override bool OnBeforeWrite(TraceEvent traceEvent)
        {
            if (traceEvent != null)
            {
                switch (traceEvent.EventType)
                {
                    case (TraceEventType.Critical):
                        Console.ForegroundColor = this.CriticalColor;
                        break;
                    case (TraceEventType.Debug):
                        Console.ForegroundColor = this.DebugColor;
                        break;
                    case (TraceEventType.Error):
                        Console.ForegroundColor = this.ErrorColor;
                        break;
                    case (TraceEventType.Warning):
                        Console.ForegroundColor = this.WarningColor;
                        break;
                    default:
                        Console.ForegroundColor = this.InformationColor;
                        break;
                }
            }

            return base.OnBeforeWrite(traceEvent);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
