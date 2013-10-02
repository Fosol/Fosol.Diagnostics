using Fosol.Common.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Similar to the ConsoleListener the ColoredConsoleListener provides a way to write TraceEvent messages to the console.
    /// It also provides a way to control the color of each message level.
    /// </summary>
    public class ColorConsoleListener
        : ConsoleListener
    {
        #region Variables
        private ConsoleColor _BackgroundColor = ConsoleColor.Black;
        private ConsoleColor _DebugColor = ConsoleColor.Cyan;
        private ConsoleColor _InformationColor = ConsoleColor.White;
        private ConsoleColor _StartColor = ConsoleColor.Green;
        private ConsoleColor _StopColor = ConsoleColor.Gray;
        private ConsoleColor _SuspendColor = ConsoleColor.Gray;
        private ConsoleColor _ResumeColor = ConsoleColor.Green;
        private ConsoleColor _WarningColor = ConsoleColor.Yellow;
        private ConsoleColor _ErrorColor = ConsoleColor.Red;
        private ConsoleColor _CriticalColor = ConsoleColor.Red;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The background color of the console.
        /// </summary>
        [TraceSetting("BackgroundColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        /// <summary>
        /// get/set - The color of debug messages.
        /// </summary>
        [TraceSetting("DebugColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor DebugColor
        {
            get { return _DebugColor; }
            set { _DebugColor = value; }
        }

        /// <summary>
        /// get/set - The color of information messages.
        /// </summary>
        [TraceSetting("InformationColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor InformationColor
        {
            get { return _InformationColor; }
            set { _InformationColor = value; }
        }

        /// <summary>
        /// get/set - The color of start messages.
        /// </summary>
        [TraceSetting("StartColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor StartColor
        {
            get { return _StartColor; }
            set { _StartColor = value; }
        }

        /// <summary>
        /// get/set - The color of stop messages.
        /// </summary>
        [TraceSetting("StopColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor StopColor
        {
            get { return _StopColor; }
            set { _StopColor = value; }
        }

        /// <summary>
        /// get/set - The color of suspend messages.
        /// </summary>
        [TraceSetting("SuspendColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor SuspendColor
        {
            get { return _SuspendColor; }
            set { _SuspendColor = value; }
        }

        /// <summary>
        /// get/set - The color of resume messages.
        /// </summary>
        [TraceSetting("ResumeColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor ResumeColor
        {
            get { return _ResumeColor; }
            set { _ResumeColor = value; }
        }

        /// <summary>
        /// get/set - The color of warning messages.
        /// </summary>
        [TraceSetting("WarningColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor WarningColor
        {
            get { return _WarningColor; }
            set { _WarningColor = value; }
        }

        /// <summary>
        /// get/set - The color of error messages.
        /// </summary>
        [TraceSetting("ErrorColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor ErrorColor
        {
            get { return _ErrorColor; }
            set { _ErrorColor = value; }
        }

        /// <summary>
        /// get/set - The color of critical messages.
        /// </summary>
        [TraceSetting("CriticalColor", ConverterType = typeof(EnumConverter<ConsoleColor>))]
        public ConsoleColor CriticalColor
        {
            get { return _CriticalColor; }
            set { _CriticalColor = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ColoredConsoleListener.
        /// </summary>
        public ColorConsoleListener()
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
        [TraceSetting("useErrorStream", ConverterType = typeof(System.ComponentModel.BooleanConverter))]
        public ColorConsoleListener(bool useErrorStream)
            : base(useErrorStream)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the background color of the console.
        /// </summary>
        protected internal override void Initialize()
        {
            base.Initialize();

            Console.BackgroundColor = this.BackgroundColor;
        }

        /// <summary>
        /// Write the TraceEvent message to the console.
        /// Modify the color of the text.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected override void OnWrite(TraceEvent trace)
        {
            switch (trace.Level)
            {
                case (TraceLevel.Debug):
                    Console.ForegroundColor = this.DebugColor;
                    break;
                case (TraceLevel.Information):
                    Console.ForegroundColor = this.InformationColor;
                    break;
                case (TraceLevel.Start):
                    Console.ForegroundColor = this.StartColor;
                    break;
                case (TraceLevel.Stop):
                    Console.ForegroundColor = this.StopColor;
                    break;
                case (TraceLevel.Suspend):
                    Console.ForegroundColor = this.SuspendColor;
                    break;
                case (TraceLevel.Resume):
                    Console.ForegroundColor = this.ResumeColor;
                    break;
                case (TraceLevel.Warning):
                    Console.ForegroundColor = this.WarningColor;
                    break;
                case (TraceLevel.Error):
                    Console.ForegroundColor = this.ErrorColor;
                    break;
                case (TraceLevel.Critical):
                    Console.ForegroundColor = this.CriticalColor;
                    break;
            }
            base.OnWrite(trace);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
