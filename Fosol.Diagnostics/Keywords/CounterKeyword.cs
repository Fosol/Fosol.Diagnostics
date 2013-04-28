using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// A counter value (increases on each execution).
    /// </summary>
    [TraceKeyword("counter")]
    public sealed class CounterKeyword
        : DynamicKeyword
    {
        #region Variables
        private static Dictionary<string, int> _Counters = new Dictionary<string, int>();
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A sequence name provides a way to have multiple counters.
        /// </summary>
        [DefaultValue("default")]
        [TraceKeywordProperty("counter", new string[] { "c", "count", "name" })]
        public string CounterName { get; set; }

        /// <summary>
        /// get/set - The starting value of the sequence.
        /// </summary>
        [TraceKeywordProperty("value", new string[] { "v", "val" })]
        public int Value { get; set; }

        /// <summary>
        /// get/set - The value to increment each time.
        /// </summary>
        [DefaultValue(1)]
        [TraceKeywordProperty("increment", new string[] { "i", "inc" })]
        public int Increment { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CounterKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public CounterKeyword(StringDictionary attributes)
            : base(attributes)
        {
            lock (_Counters)
            {
                if (!_Counters.ContainsKey(this.CounterName))
                    _Counters.Add(this.CounterName, this.Value);
                else
                    _Counters[this.CounterName] = this.Value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// A counter value (increases on each execution).
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>A counter value (increases on each execution).</returns>
        public override string Render(TraceEvent traceEvent)
        {
            return GetAndSetNextSequenceValue(this.CounterName, this.Increment).ToString();
        }

        /// <summary>
        /// Gets the named sequence and increments it.
        /// </summary>
        /// <param name="name">Name of the sequence.</param>
        /// <param name="increment">Value to increment each time.</param>
        /// <returns>Next sequence counter value.</returns>
        private static int GetAndSetNextSequenceValue(string name, int increment)
        {
            lock (_Counters)
            {
                return (_Counters[name] = _Counters[name] + increment);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
