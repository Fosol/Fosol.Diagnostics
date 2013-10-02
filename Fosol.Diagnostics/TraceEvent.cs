using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// A TraceEvent captures all the relevant details for a single specific trace message.
    /// </summary>
    public sealed class TraceEvent
    {
        #region Variables
        private readonly DateTime _CreatedDate;
        private readonly TraceWriter _Writer;
        private readonly TraceLevel _Level;
        private readonly TraceSpot _Position;
        private readonly int _Id;
        private readonly string _Message;
        private readonly InstanceProcess _Process;
        private readonly InstanceThread _Thread;
        #endregion

        #region Properties
        /// <summary>
        /// get - The TraceWriter that submitted this trace.
        /// </summary>
        public TraceWriter Writer { get { return _Writer; } }

        /// <summary>
        /// get - A unique identity for this message.
        /// </summary>
        public int Id { get { return _Id; } }

        /// <summary>
        /// get - When this TraceEvent was created.
        /// </summary>
        public DateTime CreatedDate { get { return _CreatedDate; } }

        /// <summary>
        /// get - The TraceLevel of this message.
        /// </summary>
        public TraceLevel Level { get { return _Level; } }

        /// <summary>
        /// get - The TracePosition of this message.
        /// </summary>
        internal TraceSpot Position { get { return _Position; } }

        /// <summary>
        /// get - The message describing this trace.
        /// </summary>
        public string Message { get { return _Message; } }

        /// <summary>
        /// get - The Process this TraceEvent was created with.
        /// </summary>
        public InstanceProcess Process { get { return _Process; } }

        /// <summary>
        /// get - The Thread this TraceEvent was created with.
        /// </summary>
        public InstanceThread Thread { get { return _Thread; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceEvent object.
        /// This constructor is used for header and footer messages.
        /// </summary>
        /// <param name="writer">TraceWriter where this TraceEvent originated.</param>
        /// <param name="position">TracePosition of this message.</param>
        /// <param name="message">Message to describe this TraceEvent.</param>
        internal TraceEvent(TraceWriter writer, TraceSpot position, string message)
            : this(writer, TraceLevel.Information, position, -1, message)
        {

        }

        /// <summary>
        /// Creates a new instance of a TraceEvent object.
        /// </summary>
        /// <param name="writer">TraceWriter where this TraceEvent originated.</param>
        /// <param name="level">TraceLevel of this message.</param>
        /// <param name="message">Message to describe this TraceEvent.</param>
        public TraceEvent(TraceWriter writer, TraceLevel level, string message)
            : this(writer, level, TraceSpot.Message, -1, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceEvent object.
        /// </summary>
        /// <param name="writer">TraceWriter where this TraceEvent originated.</param>
        /// <param name="level">TraceLevel of this message.</param>
        /// <param name="id">Unique identity of this message.</param>
        /// <param name="message">Message to describe this TraceEvent.</param>
        public TraceEvent(TraceWriter writer, TraceLevel level, int id, string message)
            : this(writer, level, TraceSpot.Message, id, message)
        {
        }

        /// <summary>
        /// Creates a new instance of a TraceEvent object.
        /// </summary>
        /// <param name="writer">TraceWriter where this TraceEvent originated.</param>
        /// <param name="level">TraceLevel of this message.</param>
        /// <param name="position">TracePosition of this message.</param>
        /// <param name="id">Unique identity of this message.</param>
        /// <param name="message">Message to describe this TraceEvent.</param>
        private TraceEvent(TraceWriter writer, TraceLevel level, TraceSpot position, int id, string message)
        {
            _CreatedDate = Fosol.Common.Optimization.FastDateTime.UtcNow;
            _Process = new InstanceProcess();
            _Thread = new InstanceThread();
            _Writer = writer;
            _Level = level;
            _Position = position;
            _Id = id;
            _Message = message;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
