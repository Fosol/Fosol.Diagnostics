using Fosol.Common.Parsers;
using Fosol.Common.Parsers.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// The TraceProcessElement provides a way to output process information from the TraceEvent.
    /// </summary>
    [Element("process", true)]
    public sealed class TraceProcessElement
        : TraceElement
    {
        #region Variables
        /// <summary>
        /// Source property options.
        /// </summary>
        public enum SourceOption
        {
            /// <summary>
            /// Return process information of the current process.
            /// </summary>
            Current = 0,
            /// <summary>
            /// Return the process information of the TraceEvent.
            /// </summary>
            Trace,
            /// <summary>
            /// Return the process information of the TraceWriter.
            /// </summary>
            Writer
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The TraceEventProcess property name.
        /// </summary>
        [DefaultValue("Name")]
        [ElementProperty("Key", new[] { "k" })]
        public string Key { get; set; }

        /// <summary>
        /// get/set - The source of the process information.
        /// </summary>
        [DefaultValue(SourceOption.Trace)]
        [ElementProperty("Source", new[] { "s" })]
        public SourceOption Source { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceProcessElement object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public TraceProcessElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the process information of the specified source.
        /// </summary>
        /// <param name="trace">Information object containing data for the keyword.</param>
        /// <returns>A value from the process.</returns>
        public override string Render(TraceEvent trace)
        {
            InstanceProcess process;
            if (trace != null)
            {
                switch (this.Source)
                {
                    case (SourceOption.Trace):
                        process = trace.Process;
                        break;
                    case (SourceOption.Writer):
                        process = trace.Writer.Process;
                        break;
                    default:
                        process = new InstanceProcess();
                        break;
                }
            }
            else
                process = new InstanceProcess();

            var prop = (
                from p in process.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                select p).FirstOrDefault();

            if (prop != null)
                return string.Format("{0}", prop.GetValue(process));

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
