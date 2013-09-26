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
    /// The TraceThreadElement provides a way to output thread information within the TraceEvent object.
    /// </summary>
    [Element("thread", true)]
    public sealed class TraceThreadElement
        : TraceElement
    {
        #region Variables
        /// <summary>
        /// Source property options.
        /// </summary>
        public enum SourceOption
        {
            /// <summary>
            /// Return thread information of the current process.
            /// </summary>
            Current = 0,
            /// <summary>
            /// Return the thread information of the TraceEvent.
            /// </summary>
            Trace,
            /// <summary>
            /// Return the thread information of the TraceWriter.
            /// </summary>
            Writer
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The property key name you want to display.
        /// </summary>
        [DefaultValue("Name")]
        [ElementProperty("Key", new[] { "k" })]
        public string Key { get; set; }

        /// <summary>
        /// get/set - The source of the thread information.
        /// </summary>
        [DefaultValue(SourceOption.Trace)]
        [ElementProperty("Source", new[] { "s" })]
        public SourceOption Source { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceThreadElement object.
        /// </summary>
        /// <param name="attributes">Attributes to include with this keyword.</param>
        public TraceThreadElement(StringDictionary attributes = null)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the thread information of the specified source.
        /// </summary>
        /// <param name="trace">Information object containing data for the keyword.</param>
        /// <returns>A value from the thread.</returns>
        public override string Render(TraceEvent trace)
        {
            InstanceThread thread;
            if (trace != null)
            {
                switch (this.Source)
                {
                    case (SourceOption.Trace):
                        thread = trace.Thread;
                        break;
                    case (SourceOption.Writer):
                        thread = trace.Writer.Thread;
                        break;
                    default:
                        thread = new InstanceThread();
                        break;
                }
            }
            else
                thread = new InstanceThread();

            var prop = (
                from p in thread.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where p.Name.Equals(this.Key, StringComparison.InvariantCulture)
                select p).FirstOrDefault();

            if (prop != null)
                return string.Format("{0}", prop.GetValue(thread));

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
