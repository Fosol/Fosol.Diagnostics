using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
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
        [DefaultValue(ConsoleColor.Black)]
        [TraceSetting("backgroundColor", typeof(EnumConverter), typeof(ConsoleColor))]
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

        [DefaultValue(ConsoleColor.Gray)]
        [TraceSetting("debugColor", typeof(EnumConverter), typeof(ConsoleColor))]
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

        [DefaultValue(ConsoleColor.White)]
        [TraceSetting("informationColor", typeof(EnumConverter), typeof(ConsoleColor))]
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

        [DefaultValue(ConsoleColor.DarkRed)]
        [TraceSetting("warningColor", typeof(EnumConverter), typeof(ConsoleColor))]
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

        [DefaultValue(ConsoleColor.Red)]
        [TraceSetting("errorColor", typeof(EnumConverter), typeof(ConsoleColor))]
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

        [DefaultValue(ConsoleColor.Red)]
        [TraceSetting("criticalColor", typeof(EnumConverter), typeof(ConsoleColor))]
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
        public ColoredConsoleListener()
            : base(false)
        {

        }

        public ColoredConsoleListener(bool useErrorStream)
            : base(useErrorStream)
        {

        }
        #endregion

        #region Methods
        public override void Write(TraceEvent traceEvent)
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
            base.Write(traceEvent);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
