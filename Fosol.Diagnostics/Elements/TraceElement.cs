using Fosol.Common.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Elements
{
    /// <summary>
    /// A keyword provides a way to insert dynamic data into a message format.
    /// Don't inherit directly from this abstract class, instead inherit from DynamicElement.
    /// </summary>
    public abstract class TraceElement
        : Fosol.Common.Parsers.DynamicElement
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceElement object.
        /// Initializes the Name property with the ElementAttribute.Name property.
        /// </summary>
        /// <exception cref="Fosol.Common.Exceptions.AttributeRequiredException">The TraceElementAttributeAttribute is required.</exception>
        public TraceElement()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceElement object.
        /// </summary>
        /// <param name="attributes">Dictionary of attributes to include with this keyword.</param>
        public TraceElement(StringDictionary attributes)
            : base(attributes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the dynamically generated keyword value.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Dynamically generated keyword value.</returns>
        public abstract string Render(TraceEvent traceEvent);

        /// <summary>
        /// Submit the object data to the Render method with the TraceEvent argument type.
        /// Returns the dynamically generated keyword value.
        /// </summary>
        /// <param name="data">Information to be used when generating the results.</param>
        /// <returns>Dynamically generated keyword value.</returns>
        public override string Render(object data)
        {
            return Render(data as TraceEvent);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
